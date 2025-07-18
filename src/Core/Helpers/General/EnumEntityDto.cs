namespace IAVH.BioTablero.CM.Core.Helpers.General;

using System;

using IAVH.BioTablero.CM.Core.Interfaces.DTOs;


/// <summary>
/// General entity for Enum values
/// </summary>
/// <typeparam name="TEnum">Enum</typeparam>
public class EnumEntityDto<TEnum> : IDto
    where TEnum : Enum
{
    /// <summary>
    /// Constructor
    /// </summary>
    public EnumEntityDto() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="typeEnum">Enum value</param>
    public EnumEntityDto(TEnum typeEnum)
    {
        TypeEnum = typeEnum;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Numeric enum value</param>
    public EnumEntityDto(int id)
    {
        if (!typeof(TEnum).IsEnumDefined(id))
        {
            throw new InvalidCastException($"{id} is not a valid value for {typeof(TEnum)} enum");
        }

        TypeEnum = (TEnum)(object)id;
    }

    /// <summary>
    /// Enum value
    /// </summary>
    public TEnum TypeEnum { private get; set; }

    /// <summary>
    /// Enum value as integer
    /// </summary>
    public int Id
    {
        get
        {
            var valueStr = TypeEnum.ToString("D");

            if (!int.TryParse(valueStr, out var value))
            {
                throw new InvalidCastException($"Invalid integer enum value: {valueStr}");
            }

            return value;
        }
    }

    /// <summary>
    /// Enum value as string
    /// </summary>
    public string Name =>
            TypeEnum.ToString("G");
}