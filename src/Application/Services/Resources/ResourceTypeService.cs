namespace IAVH.BioTablero.CM.Application.Services.Resources;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Resource type service.
/// </summary>
public class ResourceTypeService : ServiceRead<ResourceType, ResourceTypeDto, int>, IResourceTypeService
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    public ResourceTypeService(
        IRepository<ResourceType, int> entityRepository,
        IMapperRead<ResourceType, ResourceTypeDto> mapper,
        IValidationErrorTranslator errorTranslator)
        : base(entityRepository, mapper, errorTranslator)
    {
    }
}
