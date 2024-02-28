using System.ComponentModel;
using SPU_7.Common.Attributes;
using SPU_7.Common.Device;
using SPU_7.Common.Stand;

namespace SPU_7.Common.Extensions
{
    public static class EnumExtensions
    {
        private static readonly Dictionary<Tuple<string, Type, Enum>, object> _enumCache = new();

        /// <summary>
        /// Получает Description элемента перечисления
        /// </summary>
        /// <param name="enumElement">элемент перечисления</param>
        public static string GetDescription(this Enum enumElement)
        {
            var type = enumElement.GetType();
            var memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length <= 0) return enumElement.ToString();
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumElement.ToString();
        }

        public static StandType GetStandType(this Enum enumElement)
        {
            var type = enumElement.GetType();
            var memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length <= 0) return StandType.None;
            var attrs = memInfo[0].GetCustomAttributes(typeof(StandTypeAttribute), false);
            return attrs.Length > 0 ? ((StandTypeAttribute)attrs[0]).StandType : StandType.None;
        }

        public static DeviceGroupType GetDeviceGroupType(this Enum enumElement)
        {
            var type = enumElement.GetType();
            var memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length <= 0) return DeviceGroupType.None;
            var attrs = memInfo[0].GetCustomAttributes(typeof(DeviceGroupAttribute), false);
            return attrs.Length > 0 ? ((DeviceGroupAttribute)attrs[0]).DeviceGroupType : DeviceGroupType.None;
        }

        public static bool IsBKDevice(this Enum enumElement)
        {
            var type = enumElement.GetType();
            var memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length <= 0) return false;
            var attrs = memInfo[0].GetCustomAttributes(typeof(BKAttribute), false);
            return attrs.Length > 0 && ((BKAttribute)attrs[0]).IsBK;
        }
        
        public static bool IsTKDevice(this Enum enumElement)
        {
            var type = enumElement.GetType();
            var memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length <= 0) return false;
            var attrs = memInfo[0].GetCustomAttributes(typeof(TKAttribute), false);
            return attrs.Length > 0 && ((TKAttribute)attrs[0]).IsTK;
        }
        
        public static bool IsTKMDevice(this Enum enumElement)
        {
            var type = enumElement.GetType();
            var memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length <= 0) return false;
            var attrs = memInfo[0].GetCustomAttributes(typeof(TKMAttribute), false);
            return attrs.Length > 0 && ((TKMAttribute)attrs[0]).IsTKM;
        }

    }
}
