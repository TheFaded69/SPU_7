using SPU_7.Domain.Extensions;

namespace SPU_7.Domain.Modbus;

/// <summary>
/// Карта регистров, созданная на основе перечисления
/// </summary>
/// <typeparam name="TRegisterMapEnum">тип перечисления, в котором содержатся регистры с описанием</typeparam>
public class RegisterMapEnum<TRegisterMapEnum> : IRegisterMapEnum<TRegisterMapEnum> where TRegisterMapEnum : struct, Enum
{
    public RegisterMapEnum()
    {
        // Создает словарь регистров
        Map = new Dictionary<TRegisterMapEnum, RegisterConfiguration>();
        foreach (var register in Enum.GetValues<TRegisterMapEnum>())
        {
            Map.Add(register, register.GetRegisterSetup());
        }
    }

    /// <summary>
    /// Словарь регистров
    /// </summary>
    public Dictionary<TRegisterMapEnum, RegisterConfiguration> Map { get; }
}