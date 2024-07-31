using SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroRequests;

public class ElmetroPressureRangeRequest : DigitalElmetroRequest
{
    public ElmetroPressureRangeRequest(int subRangeIndex = 0, ElmetroDeviceAddress deviceAddress = ElmetroDeviceAddress.DefaultAddress) : base(MinRequestSize + 1)
    {
        RequestData[DeviceAddressIndex] = (byte)deviceAddress;
        RequestData[CommandIndex] = 14; // Данный запрос иммет код = 14
        RequestData[DataSizeIndex] = 1; // Запрос содержит 1 байт
        RequestData[DataSizeIndex + 1] = (byte)subRangeIndex; // Индекс поддиапазона модуля
    }
}