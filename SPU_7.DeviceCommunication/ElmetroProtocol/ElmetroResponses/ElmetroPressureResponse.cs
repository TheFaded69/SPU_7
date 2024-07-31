using SPU_7.DeviceCommunication.ElmetroProtocol.Enums;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroResponses;

public class ElmetroPressureResponse : DigitalElmetroResponse
{
    public ElmetroPressureResponse() : base(MinResponseSize + 5)
    {
        
    }

    /// <summary>
    /// Используемые единицы измерения устройства
    /// </summary>
    public ElmetroMinMaxMeasureUnits MeasureUnits => (ElmetroMinMaxMeasureUnits)ResponseData[MinResponseSize - 1];

    /// <summary>
    /// Используемые единицы измерения устройства
    /// </summary>
    public string MeasureUnitsString => ((ElmetroMinMaxMeasureUnits)ResponseData[MinResponseSize - 1]).GetDescription();

    /// <summary>
    /// Значение давления из ответа
    /// </summary>
    public float Value => (float)GetData(1, sizeof(float)).ConvertRegisterData(RegisterDataType.Float, DeviceEndianess.ABCD);
}