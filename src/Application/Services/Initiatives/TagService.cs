namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Tag service.
/// </summary>
public class TagService : ServiceRead<Tag, TagDto, int, TagSpec>, ITagService
{
    private readonly IValidator<TagDto> entityValidator;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    public TagService(
        IRepository<Tag> entityRepository,
        IMapper<Tag, TagDto> mapper,
        IValidator<TagDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository)
        : base(entityRepository, mapper)
    {
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(TagDto entityData, CancellationToken ct = default)
    {
        // Validate data
        entityData.Name = entityData.Name.Capitalize();
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
        var hasDuplicatedEntities = await entityRepository.AnyAsync(new TagSpec(entityData.Name), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "Duplicated tag",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added tag: {@entityData}", entityData);

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
    public async Task<CustomWebResponse> UpdateAsync(int id, TagDto entityData, CancellationToken ct = default)
    {
        // Validate data
        entityData.Name = entityData.Name.Capitalize();
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

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(new TagSpec(id, entityData.Name), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "Duplicated tag",
            };
        }

        // Update entity data
        entity.Name = entityData.Name;
        entity.Url = new Uri(entityData.Url);
        entity.CategoryId = entityData.Category.Id;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated tag: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> DeleteAsync(int id, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        // Validate relationships
        var hasRelationships = await initiativeRepository.AnyAsync(InitiativeSpec.GetByTagSpec(id), ct);

        if (hasRelationships)
        {
            return new CustomWebResponse(true)
            {
                Message = "The tag has relationships with one or more initiatives",
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted tag: {@entityData}", entityData);

        return new CustomWebResponse();
    }
}
