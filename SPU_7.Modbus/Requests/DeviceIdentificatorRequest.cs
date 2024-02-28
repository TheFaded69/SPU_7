using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Requests;

/// <summary>
/// Команда на чтение идентификатора устройства (<b>0x11</b>)
/// </summary>
public class DeviceIdentificatorRequest : BaseRequest
{
    public DeviceIdentificatorRequest(byte deviceAddress)
    {
        DeviceAddress = deviceAddress;
        FunctiomalCode = (byte)FunctiomalCodeEnum.ReportSlaveId;
    }

    /// <summary>
    /// размер ответа
    /// </summary>
    /// <returns></returns>
    public override int GetResponseSize()
    {
        return 86;
    }
}