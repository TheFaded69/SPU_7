using System.Collections;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.UFG;

public class PrimaryConverterUFG : ModbusDevice, IModbusDevice
{
    public PrimaryConverterUFG(ICommunicationChannel? communicationChannel, DeviceEndianess endianess) :
        base(communicationChannel, ModbusExtensions.CreateRegisterMap<UFG_PrimaryConverterRegisterMap>(), endianess)
    {
        ModbusProtocol.Address = 16;
    }

    /// <summary>
    /// Получить количество лучей ПП
    /// </summary>
    public ushort? GetBeamsCount() => GetParameterValue<ushort?>(UFG_PrimaryConverterRegisterMap.BeamsCount);

    /// <summary>
    /// Получить количество лучей ПП асинхронно
    /// </summary>
    public Task<ushort?> GetBeamsCountAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<ushort?>(UFG_PrimaryConverterRegisterMap.BeamsCount, cancellationToken);

    /// <summary>
    /// Получить блок с значениями лучей
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="parameter">Значение из карты регистров UFG</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Список значений по количеству лучей</returns>
    private IList<T> GetBeamsBlock<T>(UFG_PrimaryConverterRegisterMap parameter, int registersOnItem = 1)
    {
        var defaultValue = new List<T>();
        if (parameter.GetRegisterConfiguration() is not RegisterConfiguration regConfig) return defaultValue;
        var beamsCountValue = GetBeamsCount();
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return defaultValue;
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem);
        return GetParameterValue<IList>(regConfig)?.Cast<T>()?.ToList() ?? defaultValue;
    }

    /// <summary>
    /// Получить блок с значениями лучей асинхронно
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="parameter">Значение из карты регистров UFG</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Задча по ожиданию списка значений по количеству лучей</returns>
    private async Task<IList<T>> GetBeamsBlockAsync<T>(UFG_PrimaryConverterRegisterMap parameter, int registersOnItem = 1, CancellationToken cancellationToken = default)
    {
        var defaultValue = new List<T>();
        if (parameter.GetRegisterConfiguration() is not RegisterConfiguration regConfig) return defaultValue;
        var beamsCountValue = await GetBeamsCountAsync(cancellationToken).ConfigureAwait(false);
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return defaultValue;
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem);
        return (await GetParameterValueAsync<IList>(regConfig, cancellationToken).ConfigureAwait(false))?.Cast<T>()?.ToList() ?? defaultValue;
    }

    /// <summary>
    /// Получить блок с значениями лучей
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="parameter">Значение из карты регистров ПП UFG</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Списки значений по количеству лучей</returns>
    private (IList<T>, IList<T>) GetBeamsBlockDirection<T>(UFG_PrimaryConverterRegisterMap parameter, int registersOnItem = 1)
    {
        var defaultValue = new List<T>();
        if (parameter.GetRegisterConfiguration() is not RegisterConfiguration regConfig) return (defaultValue, defaultValue);
        var beamsCountValue = GetBeamsCount();
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return (defaultValue, defaultValue);
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem * 2);
        var groups = GetParameterValue<IList>(regConfig)?.Cast<T>()?
            .Select((item, index) => new { Item = item, Index = index })
            .GroupBy(i => i.Index % 2 == 0)
            .ToDictionary(g => g.Key, g => g.Select(v => v.Item));
        return groups != null ? (groups[true].ToList(), groups[false].ToList()) : (defaultValue, defaultValue);
    }

    /// <summary>
    /// Получить блок с значениями лучей асинхронно
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="parameter">Значение из карты регистров ПП UFG</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Списки значений по количеству лучей</returns>
    private async Task<(IList<T>, IList<T>)> GetBeamsBlockDirectionAsync<T>(UFG_PrimaryConverterRegisterMap parameter,
        int registersOnItem = 1, CancellationToken cancellationToken = default)
    {
        var defaultValue = new List<T>();
        if (parameter.GetRegisterConfiguration() is not RegisterConfiguration regConfig) return (defaultValue, defaultValue);
        var beamsCountValue = await GetBeamsCountAsync(cancellationToken).ConfigureAwait(false);
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return (defaultValue, defaultValue);
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem * 2);
        var groups = (await GetParameterValueAsync<IList>(regConfig, cancellationToken).ConfigureAwait(false))?.Cast<T>()?
            .Select((item, index) => new { Item = item, Index = index })
            .GroupBy(i => i.Index % 2 == 0)
            .ToDictionary(g => g.Key, g => g.Select(v => v.Item));
        return groups != null ? (groups[true].ToList(), groups[false].ToList()) : (defaultValue, defaultValue);
    }

    /// <summary>
    /// Задать блок с значениями по потоку и против потока асинхронно
    /// </summary>
    /// <param name="parameter">Параметр для записи</param>
    /// <param name="flowAlong">Значения по потоку</param>
    /// <param name="flowAgainst">Значения против потока</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Удалось ли выполнить операцию записи блока</returns>
    private async Task<bool> SetBeamsBlockDirectionAsync<T>(UFG_PrimaryConverterRegisterMap parameter,
        IList<T> flowAlong, IList<T> flowAgainst, int registersOnItem = 1,
        CancellationToken cancellationToken = default)
    {
        if (parameter.GetRegisterConfiguration() is not RegisterConfiguration regConfig) return false;
        var beamsCountValue = await GetBeamsCountAsync(cancellationToken).ConfigureAwait(false);
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return false;
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem * 2);
        var listData = new List<T>();
        for (var i = 0; i < beamsCount; i++) {
            listData.Add(flowAlong[i]);
            listData.Add(flowAgainst[i]);
        }
        return await SetParameterValueAsync(regConfig, listData, cancellationToken);
    }


    /// <summary>
    /// Получить значения регистров
    /// </summary>
    /// <param name="registers">Список регистров для заполнения</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Удалось ли получить все значения регистров</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<bool> GetRegistersValueAsync(IList<Register> registers, CancellationToken cancellationToken = default)
    {
        if (registers.Count == 0) throw new InvalidOperationException("Пустой список регистров для чтения!");
        return ModbusProtocol.ReadRegistersBlocksAsync(registers, ModbusFunction.ReadHoldingRegisters, Logger, cancellationToken);
    }

    /// <summary>
    /// Запись не пустых значений регистров
    /// </summary>
    /// <param name="registers">Список регистров для записи</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Удалось ли записать все значения регистров</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<bool> SetRegistersValueAsync(IList<Register> registers, CancellationToken cancellationToken = default)
    {
        if (registers.Count == 0) throw new InvalidOperationException("Нельзя записать пустой список регистров!");
        return ModbusProtocol.WriteRegistersBlocksAsync(registers, Logger, cancellationToken);
    }

    /// <summary>
    /// Получить блок с информацией о индексе усиления подлучей асинхронно по потоку и против потока соответственно
    /// </summary>
    public (IList<ushort>, IList<ushort>) GetSubBeamsGainIndex() =>
        GetBeamsBlockDirection<ushort>(UFG_PrimaryConverterRegisterMap.SubBeamsGainFlowBlock);

    /// <summary>
    /// Получить блок с информацией о индексе усиления подлучей по потоку и против потока соответственно
    /// </summary>
    public Task<(IList<ushort>, IList<ushort>)> GetSubBeamsGainIndexAsync(CancellationToken cancellationToken = default) =>
        GetBeamsBlockDirectionAsync<ushort>(UFG_PrimaryConverterRegisterMap.SubBeamsGainFlowBlock, cancellationToken: cancellationToken);

    /// <summary>
    /// Получить блок с информацией о амплитуде сигнала подлучей по потоку и против потока соответственно
    /// </summary>
    public (IList<ushort>, IList<ushort>) GetSubBeamsSignalAmplitude() =>
        GetBeamsBlockDirection<ushort>(UFG_PrimaryConverterRegisterMap.SubBeamsSignalAmplitudeBlock);

    /// <summary>
    /// Получить блок с информацией о амплитуде сигнала подлучей асинхронно по потоку и против потока соответственно
    /// </summary>
    public Task<(IList<ushort>, IList<ushort>)> GetSubBeamsSignalAmplitudeAsync(CancellationToken cancellationToken = default) =>
        GetBeamsBlockDirectionAsync<ushort>(UFG_PrimaryConverterRegisterMap.SubBeamsSignalAmplitudeBlock, cancellationToken: cancellationToken);

    /// <summary>
    /// Получить блок с фильтрованной скоростью звука лучей
    /// </summary>
    public IList<float> GetBeamsFilteredSoundSpeed() =>
        GetBeamsBlock<float>(UFG_PrimaryConverterRegisterMap.BeamsFilteredSoundSpeedBlock,
                             UFG_PrimaryConverterRegisterMap.FilteredSoundSpeedBeam1.GetRegisterConfiguration()?.NumberOfRegisters ?? 2);

    /// <summary>
    /// Получить блок с фильтрованной скоростью звука лучей асинхронно
    /// </summary>
    public Task<IList<float>> GetBeamsFilteredSoundSpeedAsync(CancellationToken cancellationToken = default) =>
        GetBeamsBlockAsync<float>(UFG_PrimaryConverterRegisterMap.BeamsFilteredSoundSpeedBlock,
                                  UFG_PrimaryConverterRegisterMap.FilteredSoundSpeedBeam1.GetRegisterConfiguration()?.NumberOfRegisters ?? 2,
                                  cancellationToken);

    /// <summary>
    /// Получить блок с нефильтрованной скоростью звука лучей
    /// </summary>
    public Task<IList<float>> GetBeamsNotFilteredSoundSpeed(CancellationToken cancellationToken = default) =>
        GetBeamsBlockAsync<float>(UFG_PrimaryConverterRegisterMap.BeamsNotFilteredSoundSpeedBlock, 2, cancellationToken);

    /// <summary>
    /// Получить блок с нефильтрованной скоростью потока лучей
    /// </summary>
    public Task<IList<float>> GetBeamsNotFilteredFlowSpeed(CancellationToken cancellationToken) =>
        GetBeamsBlockAsync<float>(UFG_PrimaryConverterRegisterMap.BeamsNotFilteredFlowSpeedBlock, 2, cancellationToken);

    /// <summary>
    /// Получить блок с текущей разностью лучей
    /// </summary>
    public Task<IList<float>> GetBeamsCurrentDifference(CancellationToken cancellationToken) =>
        GetBeamsBlockAsync<float>(UFG_PrimaryConverterRegisterMap.BeamsCurrentDifferenceBlock, 2, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="flowAlong"></param>
    /// <param name="flowAgainst"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> SetSubBeamsCoefficientOffset(IList<float> flowAlong, IList<float> flowAgainst, CancellationToken cancellationToken = default) =>
        SetBeamsBlockDirectionAsync(UFG_PrimaryConverterRegisterMap.SubBeamsTimeOffsetBlock, flowAlong, flowAgainst, 2, cancellationToken);

    /// <summary>
    /// Записать точки эталонной осциллограммы
    /// </summary>
    /// <param name="points">Эталонные точки осциллограммы</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> SetStandardOscillogrammPoints(IList<int> points, CancellationToken cancellationToken = default)
    {
        var oscBlockConfiguration = UFG_PrimaryConverterRegisterMap.OscPointsBlock.GetRegisterConfiguration() ?? throw new InvalidOperationException("");
        var oscPointsRegisters = points.Select((p, i) =>
            new Register((ushort)(oscBlockConfiguration.Address + i), 1, RegisterDataType.Int16, oscBlockConfiguration.ReadFunction, oscBlockConfiguration.WriteFunction));
        if (await SetParameterValueAsync(UFG_PrimaryConverterRegisterMap.OscPointsCount, points.Count, cancellationToken) is false) return false;
        return points.Count > 0 ? await SetRegistersValueAsync(oscPointsRegisters.ToList(), cancellationToken) : true;
    }
}