namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Serilog;

/// <summary>
/// Resource file service.
/// </summary>
public class ResourceFileService : ServiceRead<ResourceFile, ResourceFileDto, int>, IResourceFileService
{
    private new readonly IResourceFileRepository entityRepository;
    private readonly IValidator<ResourceFileDto> entityValidator;
    private readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator"></param>
    /// <param name="logger"></param>
    public ResourceFileService(
        IResourceFileRepository entityRepository,
        IMapper<ResourceFile, ResourceFileDto> mapper,
        IValidator<ResourceFileDto> entityValidator,
        ILogger logger)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByResourceAsync(int resourceId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByResourceAsync(resourceId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public Task<CustomWebResponse> AddAsync(string userName, ResourceFileDto entityData, IInputFile formFile, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <inheritdoc/>
    public Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceFileDto entityData, IInputFile formFile, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <inheritdoc/>
    public Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default) => throw new System.NotImplementedException();
}
