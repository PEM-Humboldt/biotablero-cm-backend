namespace IAVH.BioTablero.CM.Core.Domain.Entities;

/// <summary>
/// Generic base entity.
/// </summary>
/// <typeparam name="TI">Entity identifier type.</typeparam>
public abstract class BaseEntity<TI>
    where TI : notnull
{
    /// <summary>
    /// Base entity identifier.
    /// </summary>
    public virtual TI Id { get; init; }
}
