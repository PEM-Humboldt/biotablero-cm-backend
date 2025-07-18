namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using IAVH.BioTablero.CM.Core.Interfaces.DTOs;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

public interface IMapper<E, DTO>
    where E : class, IAggregateRoot
    where DTO : class, IDto
{
    DTO Map(E entity);

    E Map(DTO entity);
}
