using System.ComponentModel;

namespace SPU_7.Common.Settings;

public enum PlatformType
{
    None = 0,
    
    [Description("Протокол СоАР (МТС)")]
    CoAP = 1,
    
    [Description("Протокол TCP/IP (АПК \"Донтел\")")]
    TCP_IP = 2,
    
    [Description("Отключен")]
    Off = 3,
}