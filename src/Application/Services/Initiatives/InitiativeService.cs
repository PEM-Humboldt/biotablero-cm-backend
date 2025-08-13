namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;
using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative service.
/// </summary>
public class InitiativeService : ServiceRead<Initiative, InitiativeDto, int, InitiativeSpec>, IInitiativeService
{
    private const int MaxLeadersByInitiative = 3;
    private const string StoragePrefix = "initiatives";
    private readonly IValidator<InitiativeDto> entityValidator;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly IRepository<Location> locationRepository;
    private readonly IIamService iamService;
    private readonly IStorageService storageService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Custom initiative repository.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    /// <param name="locationRepository">Initiative Location repository.</param>
    /// <param name="storageService">Storage service.</param>
    /// <param name="iamService">IAM service.</param>
    public InitiativeService(
        IRepository<Initiative> entityRepository,
        IMapper<Initiative, InitiativeDto> mapper,
        IValidator<InitiativeDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IRepository<Location> locationRepository,
        IStorageService storageService,
        IIamService iamService)
        : base(entityRepository, mapper)
    {
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.locationRepository = locationRepository;
        this.storageService = storageService;
        this.iamService = iamService;
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public override async Task<CustomWebResponse> GetList(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct = default)
    {
        // Add joins
        var query = initiativeRepository.GetQueryable();
        query = initiativeRepository.IncludeOdataEntities(query);

        return await GetOdataListByQuery(query, queryOptions, ct);
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Add(InitiativeDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, options => options.IncludeRuleSets("default", "Create"), ct);

        if (!validationResult.IsValid)
        {
            return new CustomWebResponse(true)
            {
                Message = "Validation errors",
                ResponseBody = validationResult.Errors
                    .Select(error => error.ErrorMessage),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(new InitiativeSpec(entityData.Name), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already an initiative with the same name",
            };
        }

        // Validate users data
        var leaderCount = entityData.InitiativeUsers
            .Select(u => u.Level.Id == (int)InitiativeUserLevelEnum.Leader)
            .Count();

        if (leaderCount > MaxLeadersByInitiative)
        {
            return new CustomWebResponse(true)
            {
                Message = $"The number of leaders per initiative should be between 1 and 3",
            };
        }

        // Validate locations data
        var locationsIds = entityData.InitiativeLocations
            .Select(l => l.LocationId);

        var initiativeLocationQuery = locationRepository
            .GetQueryable()
            .Where(l => locationsIds.Contains(l.Id));

        var locationsDb = await locationRepository.QueryToListAsync(initiativeLocationQuery, ct);

        if (locationsIds.Count() != locationsDb.Count)
        {
            return new CustomWebResponse(true)
            {
                Message = $"Invalid initiative locations data",
            };
        }

        // Validate users in external system
        var results = new Dictionary<string, bool>();
        var userTasks = entityData.InitiativeUsers.Select(async user =>
        {
            results[user.UserName] = await iamService.UserExists(user.UserName, ct);
        });

        await Task.WhenAll(userTasks);

        var invalidUsers = results.Any(r => !r.Value);

        if (invalidUsers)
        {
            return new CustomWebResponse(true)
            {
                Message = $"One or more users are invalid or do not exist",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.CreationDate = DateTime.Now;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Update(int id, InitiativeDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            return new CustomWebResponse(true)
            {
                Message = "Validation errors",
                ResponseBody = validationResult.Errors
                    .Select(error => error.ErrorMessage),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(InitiativeSpec.GetDuplicatesSpec(id, entityData.Name), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already an initiative with the same name",
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        // Update entity data
        entity.Name = entityData.Name;
        entity.Description = entityData.Description;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Upload image.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="imageType">Initiative image type.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UploadImage(int id, IInputFile formFile, InitiativeImageType imageType, CancellationToken ct)
    {
        if (formFile.IsEmpty())
        {
            return new CustomWebResponse(true)
            {
                Message = "The file is empty",
            };
        }

        if (!formFile.IsValidImage())
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid file format",
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        // Upload/Overwrite image
        var imageTypeStr = imageType.ToString("G").ToLower(CultureInfo.CurrentCulture);
        var fileName = $"{StoragePrefix}/{id}/{imageTypeStr}";
        var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
        var uploadSuccessful = await storageService.UploadFile(fileName, formFile, ct);

        if (uploadSuccessful)
        {
            switch (imageType)
            {
                case InitiativeImageType.Image:
                    entity.ImageUrl = fileUri;
                    break;
                case InitiativeImageType.Banner:
                    entity.BannerUrl = fileUri;
                    break;
                default:
                    return new CustomWebResponse(true)
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = $"Invalid image type: {imageType:G}",
                    };
            }

            await entityRepository.UpdateAsync(entity, ct);

            var entityData = mapper.Map(entity);

            logger.AddLog(LogType.Update, $"Updated initiative image (type: {imageTypeStr}): {{@entityData}}", entityData);

            return new CustomWebResponse()
            {
                ResponseBody = entityData,
            };
        }

        return new CustomWebResponse(true)
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = "Storage server error",
        };
    }

    /// <summary>
    /// Disable or enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="disable">Disable flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Disable(int id, bool disable, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        entity.Enabled = !disable;
        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, $"{(disable ? "Disabled" : "Enabled")} initiative: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }
}
