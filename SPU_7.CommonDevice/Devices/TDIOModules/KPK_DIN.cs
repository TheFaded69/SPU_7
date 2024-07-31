using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.TDIOModules;
public class KPK_DIN : ModbusDevice, IModbusDevice, IInputController
{
    public KPK_DIN(ICommunicationChannel? communicationChannel = null)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<TDM_RegisterMap>(), DeviceEndianess.ABCD)
    {
        Logger = LogManager.GetLogger(nameof(KPK_DIN));
    }

    public int InputsCount => 10;

    public T? GetInputsMask<T>() where T : struct => GetParameterValue<T?>(TDM_RegisterMap.Inputs);
    public Task<T?> GetInputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct =>
        GetParameterValueAsync<T?>(TDM_RegisterMap.Inputs, cancellationToken);

    public bool? GetInputState(int inputIndex)
    {
        var inputsMask = GetParameterValue<ushort?>(TDM_RegisterMap.Inputs);
        return inputsMask is ushort states ? (states >> inputIndex & 0x0001) > 0 : null;
    }

    public async Task<bool?> GetInputStateAsync(int inputIndex, CancellationToken cancellationToken = default)
    {
        var inputsMask = await GetParameterValueAsync<ushort?>(TDM_RegisterMap.Inputs, cancellationToken).ConfigureAwait(false);
        return inputsMask is ushort states ? (states >> inputIndex & 0x0001) > 0 : null;
    }
}