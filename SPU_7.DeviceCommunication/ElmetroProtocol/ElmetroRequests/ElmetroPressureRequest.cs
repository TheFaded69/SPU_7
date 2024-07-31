using SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroRequests;

public class ElmetroPressureRequest : DigitalElmetroRequest
{
    public ElmetroPressureRequest(ElmetroDeviceAddress deviceAddress = ElmetroDeviceAddress.DefaultAddress) : base(MinRequestSize)
    {
        RequestData[DeviceAddressIndex] = (byte)deviceAddress;
        RequestData[CommandIndex] = 1; // Данный запрос иммет код = 1
        RequestData[DataSizeIndex] = 0; // Запрос содержит 0 байт
    }
}