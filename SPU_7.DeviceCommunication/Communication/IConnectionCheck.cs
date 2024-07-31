namespace SPU_7.DeviceCommunication.Communication;

public interface IConnectionCheck
{
    /// <summary>
    /// Проверять находится ли устройство на связи
    /// </summary>
    public bool IsOnlineCheck { get; set; }

    /// <summary>
    /// На связи ли устройство
    /// </summary>
    public bool IsOnline { get; }

    /// <summary>
    /// Интервал опроса для проверки связи с устройством
    /// </summary>
    public int IsOnlineInterval { get; set; }
}