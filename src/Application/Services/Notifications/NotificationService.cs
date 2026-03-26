namespace IAVH.BioTablero.CM.Application.Services.Notifications;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Notifications;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Notification service.
/// </summary>
public class NotificationService : ServiceRead<Notification, NotificationDto, int>, INotificationService
{
    private readonly IValidator<NotificationDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateAndRead<Notification, NotificationDto> mapper;
    private new readonly INotificationRepository entityRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    public NotificationService(
        INotificationRepository entityRepository,
        IMapperCreateAndRead<Notification, NotificationDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<NotificationDto> entityValidator,
        ILogger logger)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetTotalUnreadedByUserNameAsync(string userName, CancellationToken ct = default)
    {
        var total = await entityRepository.CountNotReadedByUserNameAsync(userName, ct);

        return new()
        {
            ResponseBody = new Dictionary<string, int>
            {
                { "Total", total },
            },
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetItemAsync(int id, string userName, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity != null)
        {
            if (entity.Receiver != userName)
            {
                return new(true)
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }

            if (!entity.Readed)
            {
                entity.Readed = true;

                if (entity.ReadingDate == null)
                {
                    entity.ReadingDate = DateTime.Now;
                }

                await entityRepository.UpdateAsync(entity, ct);

                var entityDto = mapper.Map(entity);
                logger.AddLog(LogType.Update, "Readed notification", "{@Entity}", entityDto);

                return new()
                {
                    ResponseBody = entityDto,
                };
            }
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.NotFound,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByUserNameAsync(string userName, ODataQueryOptions<Notification> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.GetQueryWithUserName(userName, query);
        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task SendNotificationAsync(NotificationDto entityData, bool sendEmail, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            logger.AddLog(LogType.System, "Invalid 'entityData' values", "{@ValidationResults}", validationResult.Errors, Serilog.Events.LogEventLevel.Error);
            throw new ArgumentException("Invalid 'entityData' values", nameof(entityData));
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.CreationDate = DateTime.Now;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);
        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added notification", "{@EntityData}", entityData);
    }
}
