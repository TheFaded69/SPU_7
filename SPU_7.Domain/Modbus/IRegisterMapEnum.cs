namespace SPU_7.Domain.Modbus;

public interface IRegisterMapEnum<TRegisterMapEnum> where TRegisterMapEnum : struct, Enum
{
    /// <summary>
    /// Словарь регистров
    /// </summary>
    Dictionary<TRegisterMapEnum, RegisterConfiguration> Map { get; }
}