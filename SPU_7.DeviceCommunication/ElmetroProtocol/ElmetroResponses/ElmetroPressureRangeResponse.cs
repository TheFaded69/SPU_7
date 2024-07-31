using SPU_7.DeviceCommunication.ElmetroProtocol.Enums;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroResponses;

public class ElmetroPressureRangeResponse : DigitalElmetroResponse
{
    public ElmetroPressureRangeResponse() : base(MinResponseSize + 10)
    {

    }

    /// <summary>
    /// Индекс поддиапазона модуля давления
    /// </summary>
    public int SubRangeIndex => ResponseData[MinResponseSize - 1];

    /// <summary>
    /// Используемые единицы измерения устройства
    /// </summary>
    public string MeasureUnitsString => ((ElmetroMinMaxMeasureUnits)ResponseData[MinResponseSize]).GetDescription();

    /// <summary>
    /// Используемые единицы измерения устройства
    /// </summary>
    public ElmetroMinMaxMeasureUnits MeasureUnits => (ElmetroMinMaxMeasureUnits)ResponseData[MinResponseSize];

    /// <summary>
    /// Нижний предел поддиапазона
    /// </summary>
    public float MinRange => (float)GetData(6, sizeof(float)).ConvertRegisterData(RegisterDataType.Float, DeviceEndianess.ABCD);

    /// <summary>
    /// Верхний предел поддиапазона
    /// </summary>
    public float MaxRange => (float)GetData(2, sizeof(float)).ConvertRegisterData(RegisterDataType.Float, DeviceEndianess.ABCD);
}