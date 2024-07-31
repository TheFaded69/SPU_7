using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.CommonDevice.Devices.Owen;
public class Owen110_224_8D4R : Owen110_Base, ISwitchController, IInputController
{
    public Owen110_224_8D4R(ICommunicationChannel? communicationChannel = null) : base(communicationChannel, ModbusExtensions.CreateRegisterMap<Owen110_224_8D4R_RegisterMap>())
    {
    }
    public int OutputsCount => 4;
    public int InputsCount => 8;

    public bool EnableAll() => SetParameterValue<ushort>(Owen110_224_8D4R_RegisterMap.OutputsState, 0x000F);
    public bool DisableAll() => SetParameterValue<ushort>(Owen110_224_8D4R_RegisterMap.OutputsState, 0x0000);

    public Task<bool> EnableAllAsync(CancellationToken cancellationToken = default) =>
        SetParameterValueAsync<ushort>(Owen110_224_8D4R_RegisterMap.OutputsState, 0x000F, cancellationToken);
    public Task<bool> DisableAllAsync(CancellationToken cancellationToken = default) =>
        SetParameterValueAsync<ushort>(Owen110_224_8D4R_RegisterMap.OutputsState, 0x0000, cancellationToken);

    public bool? GetInputState(int inputIndex)
    {
        var inputsMask = GetParameterValue<ushort?>(Owen110_224_8D4R_RegisterMap.InputsState);
        return inputsMask is ushort states ? (states >> inputIndex & 0x0001) > 0 : null;
    }

    public async Task<bool?> GetInputStateAsync(int inputIndex, CancellationToken cancellationToken = default)
    {
        var inputsMask = await GetParameterValueAsync<ushort?>(Owen110_224_8D4R_RegisterMap.InputsState, cancellationToken).ConfigureAwait(false);
        return inputsMask is ushort states ? (states >> inputIndex & 0x0001) > 0 : null;
    }

    public bool? GetOutputState(int outputIndex)
    {
        var outputsMask = GetParameterValue<ushort?>(Owen110_224_8D4R_RegisterMap.OutputsState);
        return outputsMask is ushort states ? (states >> outputIndex & 0x0001) > 0 : null;
    }

    public async Task<bool?> GetOutputStateAsync(int outputIndex, CancellationToken cancellationToken = default)
    {
        var outputsMask = await GetParameterValueAsync<ushort?>(Owen110_224_8D4R_RegisterMap.OutputsState, cancellationToken).ConfigureAwait(false);
        return outputsMask is ushort states ? (states >> outputIndex & 0x0001) > 0 : null;
    }

    public bool SetOutput(int outputIndex, bool value)
    {
        if (GetParameterValue<ushort?>(Owen110_224_8D4R_RegisterMap.OutputsState) is not ushort states) return false;
        return SetParameterValue(Owen110_224_8D4R_RegisterMap.OutputsState,
            (ushort)((states & (~(0x0001 << outputIndex) & 0xFFFF)) | (((value ? 0x0001 : 0x0000) << outputIndex) & 0xFFFF)));
    }

    public async Task<bool> SetOutputAsync(int outputIndex, bool value, CancellationToken cancellationToken = default)
    {
        if (await GetParameterValueAsync<ushort?>(Owen110_224_8D4R_RegisterMap.OutputsState, cancellationToken).ConfigureAwait(false) is not ushort states) return false;
        return await SetParameterValueAsync(Owen110_224_8D4R_RegisterMap.OutputsState,
            (ushort)((states & (~(0x0001 << outputIndex) & 0xFFFF)) | (((value ? 0x0001 : 0x0000) << outputIndex) & 0xFFFF)), cancellationToken).ConfigureAwait(false);
    }

    public T? GetInputsMask<T>() where T : struct => GetParameterValue<T?>(Owen110_224_8D4R_RegisterMap.InputsState);
    public Task<T?> GetInputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct =>
        GetParameterValueAsync<T?>(Owen110_224_8D4R_RegisterMap.InputsState, cancellationToken);

    public T? GetOutputsMask<T>() where T : struct => GetParameterValue<T?>(Owen110_224_8D4R_RegisterMap.OutputsState);
    public Task<T?> GetOutputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct =>
        GetParameterValueAsync<T?>(Owen110_224_8D4R_RegisterMap.OutputsState, cancellationToken);

    public ushort? GetOutputValue(int outputIndex) =>
        GetParameterValue<ushort?>(outputIndex switch
        {
            0 => Owen110_224_8D4R_RegisterMap.Output1,
            1 => Owen110_224_8D4R_RegisterMap.Output2,
            2 => Owen110_224_8D4R_RegisterMap.Output3,
            3 => Owen110_224_8D4R_RegisterMap.Output4,
            _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
        });

    public Task<ushort?> GetOutputValueAsync(int outputIndex, CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<ushort?>(outputIndex switch
        {
            0 => Owen110_224_8D4R_RegisterMap.Output1,
            1 => Owen110_224_8D4R_RegisterMap.Output2,
            2 => Owen110_224_8D4R_RegisterMap.Output3,
            3 => Owen110_224_8D4R_RegisterMap.Output4,
            _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
        }, cancellationToken);

    public bool SetOutputValue(int outputIndex, ushort value) =>
        SetParameterValue(outputIndex switch
        {
            0 => Owen110_224_8D4R_RegisterMap.Output1,
            1 => Owen110_224_8D4R_RegisterMap.Output2,
            2 => Owen110_224_8D4R_RegisterMap.Output3,
            3 => Owen110_224_8D4R_RegisterMap.Output4,
            _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
        }, value);

    public Task<bool> SetOutputValueAsync(int outputIndex, ushort value, CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(outputIndex switch
        {
            0 => Owen110_224_8D4R_RegisterMap.Output1,
            1 => Owen110_224_8D4R_RegisterMap.Output2,
            2 => Owen110_224_8D4R_RegisterMap.Output3,
            3 => Owen110_224_8D4R_RegisterMap.Output4,
            _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
        }, value, cancellationToken);
}