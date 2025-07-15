namespace IAVH.BioTablero.CM.Core.Helpers.General;

using IAVH.BioTablero.CM.Core.interfaces;

/// <summary>
/// General entity for Enum values
/// </summary>
/// <typeparam name="TEnum">Enum</typeparam>
public class EnumEntityDto<TEnum> : IDto
    where TEnum : struct
{
    /// <summary>
    /// Constructor
    /// </summary>
    public EnumEntityDto() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Enum value</param>
    public EnumEntityDto(TEnum id)
    {
        Id = id;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Numeric enum value</param>
    public EnumEntityDto(int id)
    {
        if (!typeof(TEnum).IsEnumDefined(id))
        {
            return;
        }

        Id = (TEnum)(object)id;
    }

    /// <summary>
    /// Enum value
    /// </summary>
    public TEnum Id { get; set; }

    /// <summary>
    /// Enum value as string
    /// </summary>
    public string Name
    {
        get
        {
            if (!typeof(TEnum).IsEnum)
            {
                return null;
            }

            return Id.ToString();
        }
    }
}