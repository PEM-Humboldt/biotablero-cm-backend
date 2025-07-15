using IAVH.BioTablero.CM.Core.interfaces;

namespace IAVH.BioTablero.CM.Application.Interfaces.General;

public interface IMapper<E, DTO>
    where E : class, IAggregateRoot
    where DTO : class, IDto
{
    public DTO Map(E entity);
    public E Map(DTO entity);
}
