namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

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
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative service
/// </summary>
public class InitiativeService : ServiceRead<Initiative, InitiativeDto, int, InitiativeSpec>, IInitiativeService
{
    private const int MaxLeadersByInitiative = 3;
    private readonly IValidator<InitiativeDto> entityValidator;
    private readonly IRepository<InitiativeUser> initiativeUserRepository;
    private readonly IRepository<Location> locationRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entityRepository">Entity repository</param>
    /// <param name="mapper">Entity mapper</param>
    /// <param name="entityValidator">Entity validator</param>
    /// <param name="initiativeUserRepository">Initiative User repository</param>
    /// <param name="locationRepository">Initiative Location repository</param>
    public InitiativeService(
        IRepository<Initiative> entityRepository,
        IMapper<Initiative, InitiativeDto> mapper,
        IValidator<InitiativeDto> entityValidator,
        IRepository<InitiativeUser> initiativeUserRepository,
        IRepository<Location> locationRepository)
        : base(entityRepository, mapper)
    {
        this.entityValidator = entityValidator;
        this.initiativeUserRepository = initiativeUserRepository;
        this.locationRepository = locationRepository;
    }

    /// <summary>
    /// Add element
    /// </summary>
    /// <param name="entityData">Entity data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<CustomWebResponse> Add(InitiativeDto entityData, CancellationToken ct = default)
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
            .Select(l => l.Location.Id);

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

        // TODO: add external users data validation!

        // Build entity data
        var initiative = mapper.Map(entityData);

        // Save user
        initiative = await entityRepository.AddAsync(initiative, ct);

        return new CustomWebResponse()
        {
            ResponseBody = initiative,
        };
    }

    /// <summary>
    /// Disable or enable element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="disable">Disable flag</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public Task<CustomWebResponse> Disable(int id, bool disable, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Update element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="entityData">Entity data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public Task<CustomWebResponse> Update(int id, InitiativeDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();
}
