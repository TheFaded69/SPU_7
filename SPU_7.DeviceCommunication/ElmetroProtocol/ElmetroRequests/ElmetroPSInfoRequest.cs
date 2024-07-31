using SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroRequests;

public class ElmetroPSInfoRequest : DigitalElmetroRequest
{
    public ElmetroPSInfoRequest(ElmetroDeviceAddress deviceAddress = ElmetroDeviceAddress.DefaultAddress) : base(MinRequestSize + 1)
    {
        RequestData[DeviceAddressIndex] = (byte)deviceAddress;
        RequestData[CommandIndex] = 0; // Данный запрос иммет код = 0
        RequestData[DataSizeIndex] = 1; // Запрос содержит 1 байт
        RequestData[DataSizeIndex + 1] = 0; // Номер карты выходных параметров устройства (Всегда 0 для текущих устройств)
    }
}
