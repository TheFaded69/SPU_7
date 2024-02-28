using System.ComponentModel;

namespace SPU_7.Common.Scripts;

public enum OperationType
{
    None = 0,
    
    /*[Description("Программирование")]
    Programming = 1,*/
    
    [Description("Поверка")]
    Validation = 2,

    /*[Description("Деактивация")]
    Deactivation = 3,
    
    [Description("Синхронизация времени")]
    TimeSynchronization = 4,
    
    [Description("Настройка диапазонов")]
    RangeSet = 5,
    
    [Description("Проверка платформы и связи")]
    CheckConnection = 6,
    
    [Description("Поверка клапанов")]
    CheckValve = 7,
    
    [Description("Настройка чувствительности")]
    SensitivitySet = 8,
    
    [Description("Калибровка")]
    Calibration = 9,
    
    [Description("Печать этикеток")]
    PrintTicket = 10,*/
    
    [Description("Проверка герметичности стенда")]
    CheckTightness = 11,
    
    [Description("Установка рабочего режима")]
    SetStandWorkMode = 12,
    
    
    
    
    
}