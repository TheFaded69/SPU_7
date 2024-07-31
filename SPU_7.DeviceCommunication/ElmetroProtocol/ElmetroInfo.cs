using SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroResponses;
using SPU_7.DeviceCommunication.ElmetroProtocol.Enums;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.ElmetroProtocol;

public class ElmetroInfo
{
    /// <summary>
    /// Конструктор информации о ElmetroDigitalDevice
    /// </summary>
    /// <param name="response">Ответ от устройства</param>
    /// <exception cref="ArgumentException">Тип ответа неправильный (должен быть ElmetroPSInfoResponse)</exception>
    public ElmetroInfo(DigitalElmetroResponse response)
    {
        if (response is not ElmetroPSInfoResponse) throw new ArgumentException("Неправильный тип ответа", nameof(response));
        SerialNumber = response.GetData(0, 9).GetElmetroString();
        SubRangeNumber = response.GetData(10)[0];
        PressureSensorType = (PressureSensorType)response.GetData(11)[0];
        var hardwareVersion = response.GetData(12)[0];
        MinorHardwareVersion = hardwareVersion & 0x0F;
        MajorHardwareVersion = hardwareVersion >> 4;
        var softwareVersion = response.GetData(13)[0];
        MinorSoftwareVersion = softwareVersion & 0x0F;
        MajorSoftwareVersion = softwareVersion >> 4;
        PrecisionClass = (float)response.GetData(14, sizeof(float)).ConvertRegisterData(RegisterDataType.Float, DeviceEndianess.ABCD);
        if (response.DataSize > 18) {
            PrecisionMeasureUnits = (PrecisionMeasureUnits)response.GetData(18)[0];
            Name = response.GetData(19, 9).GetElmetroString();
            var deviceType = response.GetData(29)[0];
            DeviceTypeFlags = (ElmetroDeviceTypeFlags)deviceType;
            DeviceType = (ElmetroDeviceType)((deviceType >> 4) & 0x07);
            MinMaxMeasureUnits = (ElmetroMinMaxMeasureUnits)response.GetData(30)[0];
            MaxRange = (float)response.GetData(31, sizeof(float)).ConvertRegisterData(RegisterDataType.Float, DeviceEndianess.ABCD);
            MinRange = (float)response.GetData(35, sizeof(float)).ConvertRegisterData(RegisterDataType.Float, DeviceEndianess.ABCD);
            VendorCalibrationDateTime = response.GetData(39, 6).GetElmetroDateTime();
            TemperatureClass = response.GetData(47, 9).GetElmetroString();
        }
    }

    /// <summary>
    /// Наименование модуля давления
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Температурный класс модуля давления
    /// </summary>
    public string? TemperatureClass { get; }

    /// <summary>
    /// Заводской номер модуля давления
    /// </summary>
    public string SerialNumber { get; }

    /// <summary>
    /// Нижний предел измерений
    /// </summary>
    public float? MinRange { get; }

    /// <summary>
    /// Верхний предел измерений
    /// </summary>
    public float? MaxRange { get; }

    /// <summary>
    /// Класс точности модуля давления
    /// </summary>
    public float PrecisionClass { get; }

    /// <summary>
    /// Число поддиапазонов модуля давления
    /// </summary>
    public int SubRangeNumber { get; }

    /// <summary>
    /// Доп. номер аппаратной версии модуля
    /// </summary>
    public int MinorHardwareVersion { get; }

    /// <summary>
    /// Главный номер аппаратной версии модуля
    /// </summary>
    public int MajorHardwareVersion { get; }

    /// <summary>
    /// Доп. номер программной версии модуля
    /// </summary>
    public int MinorSoftwareVersion { get; }

    /// <summary>
    /// Главный номер программной версии модуля
    /// </summary>
    public int MajorSoftwareVersion { get; }

    /// <summary>
    /// Тип модуля давления
    /// </summary>
    public PressureSensorType PressureSensorType { get; }

    /// <summary>
    /// Единицы измерения класса точности
    /// </summary>
    public PrecisionMeasureUnits? PrecisionMeasureUnits { get; }

    /// <summary>
    /// Флаги типа устройства
    /// </summary>
    public ElmetroDeviceTypeFlags? DeviceTypeFlags { get; }

    /// <summary>
    /// Код применения модуля давления
    /// </summary>
    public ElmetroDeviceType? DeviceType { get; }

    /// <summary>
    /// Единицы измерения ВПИ и НПИ модуля давления.
    /// </summary>
    public ElmetroMinMaxMeasureUnits? MinMaxMeasureUnits { get; }

    /// <summary>
    /// Дата-время заводской калибровки
    /// </summary>
    public DateTime? VendorCalibrationDateTime { get; }
}