using IAVH.BioTablero.CM.Core.Interfaces.DTOs;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

namespace IAVH.BioTablero.CM.Application.Interfaces.General;

public interface IMapper<E, DTO>
    where E : class, IAggregateRoot
    where DTO : class, IDto
{
    DTO Map(E entity);
    E Map(DTO entity);
}
