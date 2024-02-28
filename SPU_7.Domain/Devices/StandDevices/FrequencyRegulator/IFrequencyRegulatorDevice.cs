namespace SPU_7.Domain.Devices.StandDevices.FrequencyRegulator;

public interface IFrequencyRegulatorDevice
{
    /// <summary>
    /// Установить параметры ПИД регулятора
    /// </summary>
    /// <param name="pG">Пропорциональный</param>
    /// <param name="iG">Интегрирующая</param>
    /// <param name="dG">Дифференциальная</param>
    /// <param name="pMax">Макс значение</param>
    /// <param name="pMin">Мин значение</param>
    /// <param name="oMax">Макс выходное значение</param>
    /// <param name="oMin">Мин выходное значение</param>
    void SetPidParameters(double pG, double iG, double dG, double pMax, double pMin, double oMax, double oMin);
    
    /// <summary>
    /// Установить выходное значение
    /// </summary>
    /// <param name="value">Выходное значение</param>
    /// <returns>Получилось ли отправить запрос</returns>
    Task<bool> SetOutputValueAsync(double value);
    
    
    /// <summary>
    /// Получить текущее значение частоты
    /// </summary>
    /// <returns>Текущая частота</returns>
    Task<ushort?> GetCurrentFrequencyValueAsync();

    /// <summary>
    /// Start regulator
    /// </summary>
    /// <returns></returns>
    Task<bool> StartFrequencyWorkAsync();


    /// <summary>
    /// Stop regulator
    /// </summary>
    /// <returns></returns>
    Task<bool> StopFrequencyWorkAsync();

    void PidEnable();

    void PidDisable();

    

}