using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.CommonDevice.Devices.Owen;
public class Owen110_32D : Owen110_Base, IInputController
{
    public Owen110_32D(ICommunicationChannel? communicationChannel = null) : base(communicationChannel, ModbusExtensions.CreateRegisterMap<Owen110_32D_RegisterMap>())
    {
    }

    public int InputsCount => 32;

    public T? GetInputsMask<T>() where T : struct =>
        GetParameterValue<T?>(Owen110_32D_RegisterMap.InputsState);

    public Task<T?> GetInputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct =>
        GetParameterValueAsync<T?>(Owen110_32D_RegisterMap.InputsState, cancellationToken);

    public bool? GetInputState(int inputIndex) =>
        GetParameterValue<uint?>(Owen110_32D_RegisterMap.InputsState) is uint outsMask ? (ushort)(outsMask >> inputIndex & 0x0001) > 0 : null;

    public async Task<bool?> GetInputStateAsync(int inputIndex, CancellationToken cancellationToken = default) =>
        await GetParameterValueAsync<uint?>(Owen110_32D_RegisterMap.InputsState, cancellationToken) is uint outsMask ? (ushort)(outsMask >> inputIndex & 0x0001) > 0 : null;
}