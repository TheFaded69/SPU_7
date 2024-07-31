using System.Buffers.Binary;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;

namespace SPU_7.DeviceCommunication.Modbus.Response;

public abstract class ModbusBaseResponse
{
    public ModbusBaseResponse(ModbusBaseRequest request, ArraySegment<byte> frameData, DeviceEndianess endianess)
    {
        Address = frameData[0];
        Function = frameData[1];
		FrameData = frameData;
        if (request.Address != Address) throw new ModbusException(frameData, $"Несовпадение адресов! В запросе: {request.Address}, в ответе: {Address}.");
        if (request.Function != Function) throw new ModbusException(frameData, $"Несовпадение кодов функций! В запросе: {request.Function}, в ответе: {Function}");
        var crc = FrameData.Slice(0, FrameData.Count - sizeof(ushort)).Crc16();
		var crcSegment = FrameData.Slice(FrameData.Count - sizeof(ushort), sizeof(ushort));
		if (crc != BinaryPrimitives.ReadUInt16LittleEndian(crcSegment))
			throw new ModbusException(frameData, "Ответ не прошёл проверку целостности!");
		//if (request.ResponseSize != frameData.Length) throw new ModbusException(frameData, $"Несоответствие размеров запроса: {request.ResponseSize} байт; и ответа: {frameData.Length} байт");
        Endianess = endianess;
    }

    protected ArraySegment<byte> FrameData;

    /// <summary>
    /// Адрес в сети Модбас
    /// </summary>
    public byte Address { get; }

    /// <summary>
    /// Код функции
    /// </summary>
    public byte Function { get; }

    /// <summary>
    /// 
    /// </summary>
    public DeviceEndianess Endianess { get; }
}