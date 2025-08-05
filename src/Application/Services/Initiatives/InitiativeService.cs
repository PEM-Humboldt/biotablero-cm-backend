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
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Initiative service
/// </summary>
public class InitiativeService(IRepository<Initiative> entityRepository,
    IMapper<Initiative, InitiativeDto> mapper, IValidator<InitiativeDto> entityValidator) : ServiceRead<Initiative, InitiativeDto, int, InitiativeSpec>(entityRepository, mapper), IInitiativeService
{
    public async Task<CustomWebResponse> Add(InitiativeDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(error => error.ErrorMessage);

            return new CustomWebResponse(true)
            {
                Message = "Validation errors",
                ResponseBody = errors,
            };
        }

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    public Task<CustomWebResponse> Disable(int id, bool disable, CancellationToken ct = default) => throw new System.NotImplementedException();
    public Task<CustomWebResponse> Update(int id, InitiativeDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();
}
