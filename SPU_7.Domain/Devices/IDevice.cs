using SPU_7.Common.Device;
using SPU_7.Domain.Devices.StandDevices.PulseMeter;

namespace SPU_7.Domain.Devices
{
    /// <summary>
    /// Интерфейс устройств (общие свойства и методы для всех видов устройств)
    /// </summary>
    public interface IDevice
    {
      
        /// <summary>
        /// Доступно ли усройство для использования
        /// </summary>
        bool IsManualEnabled { get; set; }
        
        /// <summary>
        /// Заводской номер
        /// </summary>
        public string VendorNumberString { get; set; }
        
        /// <summary>
        /// Device name
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// Device information
        /// </summary>
        public string DeviceTypeInfo { get; set; }


        /// <summary>
        /// Сброс ДД на ноль
        /// </summary>
        /// <returns></returns>
        Task<bool> ResetToZeroAsync();

        /// <summary>
        /// Установить количество импульсов для БИПЧ
        /// </summary>
        /// <param name="pulseCount">Количество импульсов</param>
        /// <returns>Получилось ли отправить запрос</returns>
        Task<bool> SetPulseCountAsync(int pulseCount);

        /// <summary>
        /// Считать количество импульсов БИПЧ
        /// </summary>
        /// <param name="pulseCount">Количество импульсов</param>
        /// <returns>Количество импульсов</returns>
        Task<int?> ReadPulseCountAsync();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pulseCount"></param>
        /// <returns></returns>
        Task<bool> StartPeriodMeasureAsync(int pulseCount);

        /// <summary>
        /// Считать статус БИПЧ
        /// </summary>
        /// <returns></returns>
        Task<PulseMeter2ChannelState> ReadChannelStatusAsync();

        Task<uint?> GetStartMeasureTimeAsync();
        
        Task<uint?> GetEndMeasureTimeAsync();

        Task<bool> SetPulseTimeOutAsync(int i);
    }
}
