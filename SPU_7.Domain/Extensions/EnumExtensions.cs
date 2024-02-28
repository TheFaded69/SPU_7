using SPU_7.Domain.Attributes;
using SPU_7.Domain.Modbus;

namespace SPU_7.Domain.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Получить настройки регистра
    /// </summary>
    /// <param name="enumElement">элемент перечисления</param>
    public static RegisterConfiguration GetRegisterSetup(this Enum enumElement)
    {
        var type = enumElement.GetType();
        var memInfo = type.GetMember(enumElement.ToString());
        if (memInfo.Length <= 0) return null;
        var attrs = memInfo[0].GetCustomAttributes(typeof(RegisterSetupAttribute), false);
        return attrs.Length > 0 ? ((RegisterSetupAttribute)attrs[0]).RegisterConfiguration : null;
    }
}