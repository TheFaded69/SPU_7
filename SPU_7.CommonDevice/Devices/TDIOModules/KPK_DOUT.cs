using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.TDIOModules;
public class KPK_DOUT : ModbusDevice, IModbusDevice, ISwitchController
{
    public KPK_DOUT(ICommunicationChannel? communicationChannel = null)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<TDM_RegisterMap>(), DeviceEndianess.ABCD)
    {
        Logger = LogManager.GetLogger(nameof(KPK_DOUT));
    }

    public int OutputsCount => 10;

    public bool EnableAll() => SetParameterValue(TDM_RegisterMap.Outputs, 0xFFFF);
    public bool DisableAll() => SetParameterValue(TDM_RegisterMap.Outputs, 0x0000);

    public Task<bool> EnableAllAsync(CancellationToken cancellationToken = default) => SetParameterValueAsync(TDM_RegisterMap.Outputs, 0xFFFF, cancellationToken);
    public Task<bool> DisableAllAsync(CancellationToken cancellationToken = default) => SetParameterValueAsync(TDM_RegisterMap.Outputs, 0x0000, cancellationToken);

    public T? GetOutputsMask<T>() where T : struct =>
        GetParameterValue<T?>(TDM_RegisterMap.Outputs);
    public Task<T?> GetOutputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct =>
        GetParameterValueAsync<T?>(TDM_RegisterMap.Outputs, cancellationToken);

    public bool? GetOutputState(int outputIndex)
    {
        var outputsMask = GetParameterValue<ushort?>(TDM_RegisterMap.Outputs);
        return outputsMask is ushort states ? (states >> outputIndex & 0x0001) > 0 : null;
    }

    public async Task<bool?> GetOutputStateAsync(int outputIndex, CancellationToken cancellationToken = default)
    {
        var outputsMask = await GetParameterValueAsync<ushort?>(TDM_RegisterMap.Outputs, cancellationToken).ConfigureAwait(false);
        return outputsMask is ushort states ? (states >> outputIndex & 0x0001) > 0 : null;
    }

    public bool SetOutput(int outputIndex, bool value)
    {
        if (GetParameterValue<ushort?>(TDM_RegisterMap.Outputs) is not ushort states) return false;
        return SetParameterValue(TDM_RegisterMap.Outputs,
            (ushort)((states & (~(0x0001 << outputIndex) & 0xFFFF)) | (((value ? 0x0001 : 0x0000) << outputIndex) & 0xFFFF)));
    }

    public async Task<bool> SetOutputAsync(int outputIndex, bool value, CancellationToken cancellationToken = default)
    {
        if (await GetParameterValueAsync<ushort?>(TDM_RegisterMap.Outputs, cancellationToken).ConfigureAwait(false) is not ushort states) return false;
        return await SetParameterValueAsync(TDM_RegisterMap.Outputs,
            (ushort)((states & (~(0x0001 << outputIndex) & 0xFFFF)) | (((value ? 0x0001 : 0x0000) << outputIndex) & 0xFFFF)), cancellationToken).ConfigureAwait(false);
    }
}