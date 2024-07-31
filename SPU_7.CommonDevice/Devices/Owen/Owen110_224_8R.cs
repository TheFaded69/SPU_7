using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.CommonDevice.Devices.Owen;

public class Owen110_224_8R : Owen110_Base, ISwitchController, IAnalogOutputController
{
    public Owen110_224_8R(ICommunicationChannel? communicationChannel = null)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<Owen110_224_8R_RegisterMap>())
    {

    }

    public int OutputsCount => 8;
    public int AnalogOutputCount => OutputsCount;

    public bool EnableAll() => SetParameterValue<ushort>(Owen110_224_8R_RegisterMap.OutputsState, 0x000F);
    public bool DisableAll() => SetParameterValue<ushort>(Owen110_224_8R_RegisterMap.OutputsState, 0x0000);
    public Task<bool> EnableAllAsync(CancellationToken cancellationToken = default) => SetParameterValueAsync(Owen110_224_8R_RegisterMap.OutputsState, 0x000F, cancellationToken);
    public Task<bool> DisableAllAsync(CancellationToken cancellationToken = default) => SetParameterValueAsync(Owen110_224_8R_RegisterMap.OutputsState, 0x0000, cancellationToken);

    public bool? GetOutputState(int outputIndex)
    {
        var outputsMask = GetParameterValue<ushort?>(Owen110_224_8R_RegisterMap.OutputsState);
        return outputsMask is ushort states ? (states >> outputIndex & 0x0001) > 0 : null;
    }

    public async Task<bool?> GetOutputStateAsync(int outputIndex, CancellationToken cancellationToken = default)
    {
        var outputsMask = await GetParameterValueAsync<ushort?>(Owen110_224_8R_RegisterMap.OutputsState, cancellationToken).ConfigureAwait(false);
        return outputsMask is ushort states ? (states >> outputIndex & 0x0001) > 0 : null;
    }

    public bool SetOutput(int outputIndex, bool value)
    {
        if (GetParameterValue<ushort?>(Owen110_224_8R_RegisterMap.OutputsState) is not ushort states) return false;
        return SetParameterValue(Owen110_224_8R_RegisterMap.OutputsState,
            (ushort)((states & (~(0x0001 << outputIndex) & 0xFFFF)) | (((value ? 0x0001 : 0x0000) << outputIndex) & 0xFFFF)));
    }

    public async Task<bool> SetOutputAsync(int outputIndex, bool value, CancellationToken cancellationToken = default)
    {
        if (await GetParameterValueAsync<ushort?>(Owen110_224_8R_RegisterMap.OutputsState, cancellationToken).ConfigureAwait(false) is not ushort states) return false;
        return await SetParameterValueAsync(Owen110_224_8R_RegisterMap.OutputsState,
            (ushort)((states & (~(0x0001 << outputIndex) & 0xFFFF)) | (((value ? 0x0001 : 0x0000) << outputIndex) & 0xFFFF)), cancellationToken).ConfigureAwait(false);
    }

    public T? GetOutputsMask<T>() where T : struct => GetParameterValue<T?>(Owen110_224_8R_RegisterMap.OutputsState);
    public Task<T?> GetOutputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct =>
        GetParameterValueAsync<T?>(Owen110_224_8R_RegisterMap.OutputsState, cancellationToken);

    public float? GetOutputValue(int outputIndex) =>
        GetParameterValue<ushort?>(outputIndex switch
        {
            0 => Owen110_224_8R_RegisterMap.Output1,
            1 => Owen110_224_8R_RegisterMap.Output2,
            2 => Owen110_224_8R_RegisterMap.Output3,
            3 => Owen110_224_8R_RegisterMap.Output4,
            4 => Owen110_224_8R_RegisterMap.Output5,
            5 => Owen110_224_8R_RegisterMap.Output6,
            6 => Owen110_224_8R_RegisterMap.Output7,
            7 => Owen110_224_8R_RegisterMap.Output8,
            _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
        });

    public async Task<float?> GetOutputValueAsync(int outputIndex, CancellationToken cancellationToken = default) =>
         await GetParameterValueAsync<ushort?>(outputIndex switch
         {
             0 => Owen110_224_8R_RegisterMap.Output1,
             1 => Owen110_224_8R_RegisterMap.Output2,
             2 => Owen110_224_8R_RegisterMap.Output3,
             3 => Owen110_224_8R_RegisterMap.Output4,
             4 => Owen110_224_8R_RegisterMap.Output5,
             5 => Owen110_224_8R_RegisterMap.Output6,
             6 => Owen110_224_8R_RegisterMap.Output7,
             7 => Owen110_224_8R_RegisterMap.Output8,
             _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
         }, cancellationToken).ConfigureAwait(false);

    public bool SetOutputValue(int outputIndex, float value) =>
        SetParameterValue(outputIndex switch
        {
            0 => Owen110_224_8R_RegisterMap.Output1,
            1 => Owen110_224_8R_RegisterMap.Output2,
            2 => Owen110_224_8R_RegisterMap.Output3,
            3 => Owen110_224_8R_RegisterMap.Output4,
            4 => Owen110_224_8R_RegisterMap.Output5,
            5 => Owen110_224_8R_RegisterMap.Output6,
            6 => Owen110_224_8R_RegisterMap.Output7,
            7 => Owen110_224_8R_RegisterMap.Output8,
            _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
        }, value);

    public Task<bool> SetOutputValueAsync(int outputIndex, float value, CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(outputIndex switch
        {
            0 => Owen110_224_8R_RegisterMap.Output1,
            1 => Owen110_224_8R_RegisterMap.Output2,
            2 => Owen110_224_8R_RegisterMap.Output3,
            3 => Owen110_224_8R_RegisterMap.Output4,
            4 => Owen110_224_8R_RegisterMap.Output5,
            5 => Owen110_224_8R_RegisterMap.Output6,
            6 => Owen110_224_8R_RegisterMap.Output7,
            7 => Owen110_224_8R_RegisterMap.Output8,
            _ => throw new ArgumentOutOfRangeException(nameof(outputIndex), $"Выход с индексом {outputIndex} для данного типа прибора не существует"),
        }, value, cancellationToken);
}