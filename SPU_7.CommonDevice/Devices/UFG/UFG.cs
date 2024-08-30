using System.Collections;
using System.ComponentModel;
using NLog;
using SPU_7.CommonDevice.Utils;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus;
using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;
using SPU_7.DeviceCommunication.Modbus.Response;
using SPU_7.DeviceCommunication.TurboDevices;

namespace SPU_7.CommonDevice.Devices.UFG;

public class UFG : ModbusDevice, IModbusDevice, IPressureSensor, ITemperatureSensor
{
    public UFG(ICommunicationChannel? communicationChannel = null, DeviceEndianess endianess = DeviceEndianess.ABCD)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<UFG_RegisterMap>(), endianess)
    {
        Logger = LogManager.GetLogger(nameof(UFG));
        PrimaryConverter = new PrimaryConverterUFG(communicationChannel, endianess); // По умолчанию имеет адрес 16
        PrimaryConverter.ModbusProtocol.PropertyChanged += ModbusProtocol_PropertyChanged;
        ModbusProtocol.PropertyChanged += ModbusProtocol_PropertyChanged;
    }

    private DateTime? _bridgeEnabledTime;
    private bool _isReceive;
    private bool _isTransmit;

    /// <summary>
    /// Идентификатор типа и тип прибора соответственно
    /// </summary>
    public static readonly IReadOnlyDictionary<int, string> DeviceIdType = new Dictionary<int, string>
    {
        {0x00, "Неизвестное устройство"},
        {0x01, "UFG"},
        {0x02, "GFG"},
        {0x03, "TFG"},
        {0x04, "LVG"},
        {0x05, "PS"},
        {0x06, "RS"},
        {0x09, "Гранд-СВ"},
        {0x0A, "SPI-Гранд"},
        {0x0B, "РС-2М"},
        {0x0C, "UFG2"},
        {0x0F, "UFL"}, // UFG для СУГ в корпусе БП20
	    {0x10, "BP20-TFG2"}, // Новый TFG в корпусе БП20
	    {0x11, "BP20-GFG2"}, // Новый GFG в корпусе БП20
	    {0x12, "BP20_PS2"}, // Новый PS в корпусе БП20
        {0x13, "PSI"}, // Датчик давления в корпусе БП20 с питанием от токовой петли
        {0x14, "PS10-RS485"}, // Преобразователь давления в корпусе BP10 + RS485
        {0x15, "PS10-I"}, // Преобразователь давления в корпусе BP10 токовый выход + RS485
        {0x16, "PS10-I-HART"}, // Преобразователь давления в корпусе BP10 токовый выход + HART
        {0x17, "PS10-U"}, // Преобразователь давления в корпусе BP10 потенциальный выход
        {0x18, "PS20-PP"}, // ПП давления в корпусе BP20, RS232 интефейс
	    {0x19, "UFG-H Grand"}, // UFG в корпусе SPI-GRAND
	    // Резерв для терминалов
        {0x20, "UFG BT"},
        {0x21, "UFL BT"},
        {0x22, "VT-TFG"},
        {0x23, "VT-GFG"},
        //
        {0x2A, "GRAND-LoRa"},
	    // Кориолисов расходомер
	    {0x2B, "CFM Sensor"},
        {0x2C, "CFM"},
        {0x2D, "UDM BP-20"}, // основной вариант, используется во Вьювере
        //
        {0x2E, "GRAND-NBIOT"}, // GRAND с NBIOT
        {0x2F, "UDM-BP-20"}, // Плотномер UDM
        {0x30, "DD-SWITCH"}, // Коммутатор стэнда ДД
        {0x31, "GRAND-TEST"}, // Контроллер проверки плат GRAND-TEST
        {0x32, "UFG-H"}, // вычислитель UFG-H
        {0x33, "UFG-H-PP"}, // ПП для UFG-H
    };

    /// <summary>
    /// Происходит ли получение данных из устройства
    /// </summary>
    public bool IsReceive
    {
        get => _isReceive;
        private set => SetProperty(ref _isReceive, value);
    }

    /// <summary>
    /// Происходит ли передача данных в устройство
    /// </summary>
    public bool IsTransmit
    {
        get => _isTransmit;
        private set => SetProperty(ref _isTransmit, value);
    }

    /// <summary>
    /// Первичный преобразователь UFG
    /// </summary>
    public PrimaryConverterUFG PrimaryConverter { get; }

    /// <summary>
    /// Дата/время включения моста
    /// </summary>
    public DateTime? BridgeEnabledTime
    {
        get => _bridgeEnabledTime;
        private set => SetProperty(ref _bridgeEnabledTime, value);
    }

    /// <summary>
    /// Обработка события изменения определённых свойтсв при взаимодействии по протоколу Modbus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Параметры события</param>
    private void ModbusProtocol_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName) {
            case nameof(ModbusDeviceProtocol.IsReceive):
                IsReceive = ModbusProtocol.IsReceive | PrimaryConverter.ModbusProtocol.IsReceive;
                break;
            case nameof(ModbusDeviceProtocol.IsTransmit):
                IsTransmit = ModbusProtocol.IsTransmit | PrimaryConverter.ModbusProtocol.IsTransmit;
                break;
        }
    }

    public float? GetPressure(PressureType pressureType = PressureType.DefaultPressure) => GetParameterValue<float?>(pressureType switch
    {
        PressureType.DefaultPressure => UFG_RegisterMap.AbsolutePressure,
        PressureType.AbsolutePressure => UFG_RegisterMap.AbsolutePressure,
        PressureType.ExcessPressure => UFG_RegisterMap.ExcessPressure,
        _ => throw new IndexOutOfRangeException()
    });

    public Task<float?> ReadPressureAsync(PressureType pressureType = PressureType.DefaultPressure, CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(pressureType switch
        {
            PressureType.DefaultPressure => UFG_RegisterMap.AbsolutePressure,
            PressureType.AbsolutePressure => UFG_RegisterMap.AbsolutePressure,
            PressureType.ExcessPressure => UFG_RegisterMap.ExcessPressure,
            _ => throw new IndexOutOfRangeException()
        }, cancellationToken);

    public float? GetTemperature() => GetParameterValue<float?>(UFG_RegisterMap.Temperature);

    public Task<float?> GetTemperatureAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(UFG_RegisterMap.Temperature, cancellationToken);

    /// <summary>
    /// Получить информацию о устройстве "Турбулентность-Дон"
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TDDeviceId?> GetDeviceIdAsync(CancellationToken cancellationToken = default)
    {
        var response = await ModbusProtocol.DoRequestAsync(new ReportSlaveIdRequest((byte)ModbusProtocol.Address, ModbusProtocol.Endianess), Logger, cancellationToken).ConfigureAwait(false);
        if (response is not ReportSlaveIdResponse slaveIdResponse) return null;
        var blockInfo = new TDDeviceBlockInfo(slaveIdResponse.Data.GetEnumerator(), ModbusProtocol.Endianess);
        return new TDDeviceId(blockInfo.InputDataEnumerator, ModbusProtocol.Endianess);
    }

    /// <summary>
    /// Получить маску НС
    /// </summary>
    public uint? GetAlarmBitsMask() => GetParameterValue<uint?>(UFG_RegisterMap.AlarmBitsMask);

    /// <summary>
    /// Получить маску НС асинхронно
    /// </summary>
    public Task<uint?> GetAlarmBitsMaskAsync(CancellationToken cancellationToken = default) => GetParameterValueAsync<uint?>(UFG_RegisterMap.AlarmBitsMask, cancellationToken);

    /// <summary>
    /// Получить сетевой адрес ПП
    /// </summary>
    public ushort? GetPrimaryConverterAddress() => GetParameterValue<ushort?>(UFG_RegisterMap.PrimaryConverterAddress);

    /// <summary>
    /// Получить сетевой адрес ПП асинхронно
    /// </summary>
    public Task<ushort?> GetPrimaryConverterAddressAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<ushort?>(UFG_RegisterMap.PrimaryConverterAddress, cancellationToken);

    /// <summary>
    /// Получить количество лучей ПП
    /// </summary>
    public ushort? GetBeamsCount() => GetParameterValue<ushort?>(UFG_RegisterMap.BeamsCount);

    /// <summary>
    /// Получить количество лучей ПП асинхронно
    /// </summary>
    public Task<ushort?> GetBeamsCountAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<ushort?>(UFG_RegisterMap.BeamsCount, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    public uint? GetSerialNumber() => GetParameterValue<uint?>(UFG_RegisterMap.SerialNumber);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<uint?> GetSerialNumberAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<uint?>(UFG_RegisterMap.SerialNumber, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    public uint? GetAuthorizationKey() => GetParameterValue<uint?>(UFG_RegisterMap.AuthorizationKey);

    /// <summary>
    /// 
    /// </summary>
    public Task<uint?> GetAuthorizationKeyAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<uint?>(UFG_RegisterMap.AuthorizationKey, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    public bool EnterPassword(uint hash) => SetParameterValue(UFG_RegisterMap.PasswordAccess, hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    public Task<bool> EnterPasswordAsync(uint hash, CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(UFG_RegisterMap.PasswordAccess, hash, cancellationToken);

    /// <summary>
    /// Ввести мастер пароль
    /// </summary>
    /// <returns></returns>
    public bool EnterMasterPassword()
    {
        var serialNumber = GetSerialNumber();
        if (serialNumber == null) return false;
        var passwordNumber = GetAuthorizationKey();
        if (passwordNumber == null) return false;
        return EnterPassword(MasterPasswordGenerator.Get((uint)serialNumber, (uint)passwordNumber));
    }

    /// <summary>
    /// Ввести мастер пароль асинхронно
    /// </summary>
    /// <returns></returns>
    public async Task<bool> EnterMasterPasswordAsync(CancellationToken cancellationToken = default)
    {
        var serialNumber = await GetSerialNumberAsync(cancellationToken).ConfigureAwait(false);
        if (serialNumber == null) return false;
        var passwordNumber = await GetAuthorizationKeyAsync(cancellationToken).ConfigureAwait(false);
        if (passwordNumber == null) return false;
        return await EnterPasswordAsync(MasterPasswordGenerator.Get((uint)serialNumber, (uint)passwordNumber), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Включить мост
    /// </summary>
    /// <returns></returns>
    public bool EnableBridgeMode()
    {
        if (EnterMasterPassword() is false) return false;
        if (SetParameterValue(UFG_RegisterMap.BridgeMode, BridgeStateUFG.Enabled)) {
            BridgeEnabledTime = DateTime.Now;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Включить мост асинхронно
    /// </summary>
    /// <returns></returns>
    public async Task<bool> EnableBridgeModeAsync(CancellationToken cancellationToken = default)
    {
        if (await EnterMasterPasswordAsync(cancellationToken).ConfigureAwait(false) is false) return false;
        if (await SetParameterValueAsync(UFG_RegisterMap.BridgeMode, BridgeStateUFG.Enabled, cancellationToken).ConfigureAwait(false)) {
            BridgeEnabledTime = DateTime.Now;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Включить мост если требуется
    /// </summary>
    /// <param name="thresholdTime">Минимальное время между записью регистра управления мостом</param>
    /// <returns>Мост включен или в актуальном состоянии</returns>
    public async Task<bool> EnableBridgeIfRequiredAsync(TimeSpan thresholdTime, CancellationToken cancellationToken = default)
    {
        if (await GetParameterValueAsync<BridgeStateUFG>(UFG_RegisterMap.BridgeMode, cancellationToken).ConfigureAwait(false) is BridgeStateUFG.Enabled
            && BridgeEnabledTime is DateTime bet && (DateTime.Now - bet) < thresholdTime) return true;
        return await EnableBridgeModeAsync(cancellationToken);
    }

    /// <summary>
    /// Отключить мост
    /// </summary>
    /// <returns></returns>
    public bool DisableBridgeMode()
    {
        if (EnterMasterPassword() is false) return false;
        if (SetParameterValue(UFG_RegisterMap.BridgeMode, BridgeStateUFG.Disabled)) {
            BridgeEnabledTime = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Отключить мост асинхронно
    /// </summary>
    /// <returns></returns>
    public async Task<bool> DisableBridgeModeAsync(CancellationToken cancellationToken = default)
    {
        if (await EnterMasterPasswordAsync(cancellationToken).ConfigureAwait(false) is false) return false;
        if (await SetParameterValueAsync(UFG_RegisterMap.BridgeMode, BridgeStateUFG.Disabled, cancellationToken).ConfigureAwait(false)) {
            BridgeEnabledTime = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Получить состояние моста
    /// </summary>
    public BridgeStateUFG? GetBridgeState() => (BridgeStateUFG?)GetParameterValue<ushort?>(UFG_RegisterMap.BridgeMode);

    /// <summary>
    /// Получить состояние моста асинхронно
    /// </summary>
    public async Task<BridgeStateUFG?> GetBridgeStateAsync(CancellationToken cancellationToken = default) =>
        (BridgeStateUFG?)await GetParameterValueAsync<ushort?>(UFG_RegisterMap.BridgeMode, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Получить блок с значениями лучей
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="registerMapEnum">Значение из карты регистров UFG</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Список значений по количеству лучей</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private IList<T> GetBeamsBlock<T>(UFG_RegisterMap registerMapEnum, int registersOnItem = 1)
    {
        var defaultValue = new List<T>();
        if (registerMapEnum.GetRegisterConfiguration() is not RegisterConfiguration regConfig) throw new InvalidOperationException("Не найдено настроек регистра!");
        var beamsCountValue = GetBeamsCount();
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return defaultValue;
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem);
        return GetParameterValue<IList>(regConfig)?.Cast<T>()?.ToList() ?? defaultValue;
    }

    /// <summary>
    /// Получить блок с значениями лучей асинхронно
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="registerMapEnum">Значение из карты регистров UFG</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Задача на получение списка значений по количеству лучей</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private async Task<IList<T>> GetBeamsBlockAsync<T>(UFG_RegisterMap registerMapEnum, int registersOnItem = 1, CancellationToken cancellationToken = default)
    {
        var defaultValue = new List<T>();
        if (registerMapEnum.GetRegisterConfiguration() is not RegisterConfiguration regConfig) throw new InvalidOperationException("Не найдено настроек регистра!");
        var beamsCountValue = await GetBeamsCountAsync(cancellationToken).ConfigureAwait(false);
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return defaultValue;
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem);
        return (await GetParameterValueAsync<IList>(regConfig, cancellationToken).ConfigureAwait(false))?.Cast<T>()?.ToList() ?? defaultValue;
    }

    /// <summary>
    /// Записать блок с значениями
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="registerMapEnum">Значение из карты регистров UFG</param>
    /// <param name="values">Значения для записи</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <returns>Удалось ли выполнить операцию</returns>
    private bool SetBeamsBlock<T>(UFG_RegisterMap registerMapEnum, IList values, int registersOnItem = 1)
    {
        if (values.Count == 0) throw new InvalidOperationException("Нельзя записать пустой список!");
        if (registerMapEnum.GetRegisterConfiguration() is not RegisterConfiguration regConfig) throw new InvalidOperationException("Не найдено настроек регистра!");
        var beamsCountValue = GetBeamsCount();
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return false;
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem);
        return SetParameterValue(regConfig, values);
    }

    /// <summary>
    /// Записать блок с значениями асинхронно
    /// </summary>
    /// <typeparam name="T">Тип значения в блоке</typeparam>
    /// <param name="registerMapEnum">Значение из карты регистров UFG</param>
    /// <param name="values">Значения для записи</param>
    /// <param name="registersOnItem">Количество регистров занимаемых одним значением</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Удалось ли выполнить операцию</returns>
    private async Task<bool> SetBeamsBlockAsync<T>(UFG_RegisterMap registerMapEnum, IList values, int registersOnItem = 1, CancellationToken cancellationToken = default)
    {
        if (values.Count == 0) throw new InvalidOperationException("Нельзя записать пустой список!");
        if (registerMapEnum.GetRegisterConfiguration() is not RegisterConfiguration regConfig) throw new InvalidOperationException("Не найдено настроек регистра!");
        var beamsCountValue = await GetBeamsCountAsync(cancellationToken).ConfigureAwait(false);
        if (beamsCountValue is not ushort beamsCount || beamsCount <= 0) return false;
        regConfig.NumberOfRegisters = (ushort)(beamsCount * registersOnItem);
        return await SetParameterValueAsync(regConfig, values, cancellationToken).ConfigureAwait(false);
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
        return ModbusProtocol.WriteRegistersBlocksAsync(registers, Logger/*, actionBefore: () => EnterMasterPasswordAsync()*/, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Получить индекс усиления луча под указанным индексом
    /// </summary>
    /// <param name="beamIndex"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public ushort? GetBeamGainIndex(int beamIndex) => GetParameterValue<ushort?>(beamIndex switch
    {
        0 => UFG_RegisterMap.BeamGainIndex1,
        1 => UFG_RegisterMap.BeamGainIndex2,
        2 => UFG_RegisterMap.BeamGainIndex3,
        3 => UFG_RegisterMap.BeamGainIndex4,
        4 => UFG_RegisterMap.BeamGainIndex5,
        5 => UFG_RegisterMap.BeamGainIndex6,
        6 => UFG_RegisterMap.BeamGainIndex7,
        7 => UFG_RegisterMap.BeamGainIndex8,
        _ => throw new IndexOutOfRangeException("Луч с таким индексом нельзя выбрать"),
    });

    /// <summary>
    /// Получить индекс усиления луча под указанным индексом асинхронно
    /// </summary>
    /// <param name="beamIndex"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public Task<ushort?> GetBeamGainIndexAsync(int beamIndex, CancellationToken cancellationToken = default) => GetParameterValueAsync<ushort?>(beamIndex switch
    {
        0 => UFG_RegisterMap.BeamGainIndex1,
        1 => UFG_RegisterMap.BeamGainIndex2,
        2 => UFG_RegisterMap.BeamGainIndex3,
        3 => UFG_RegisterMap.BeamGainIndex4,
        4 => UFG_RegisterMap.BeamGainIndex5,
        5 => UFG_RegisterMap.BeamGainIndex6,
        6 => UFG_RegisterMap.BeamGainIndex7,
        7 => UFG_RegisterMap.BeamGainIndex8,
        _ => throw new IndexOutOfRangeException("Луч с таким индексом нельзя выбрать"),
    }, cancellationToken);

    /// <summary>
    /// Получить блок с информацией о индексе усиления лучей
    /// </summary>
    public IList<ushort> GetBeamsGainIndex() => GetBeamsBlock<ushort>(UFG_RegisterMap.BeamGainIndexBlock);

    /// <summary>
    /// Получить блок с информацией о индексе усиления лучей асинхронно
    /// </summary>
    public Task<IList<ushort>> GetBeamsGainIndexAsync(CancellationToken cancellationToken = default) =>
        GetBeamsBlockAsync<ushort>(UFG_RegisterMap.BeamGainIndexBlock, cancellationToken: cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beamIndex"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public float? GetSignalToNoiseRatio(int beamIndex) => GetParameterValue<float?>(beamIndex switch
    {
        0 => UFG_RegisterMap.SignalToNoiseRatioBeam1,
        1 => UFG_RegisterMap.SignalToNoiseRatioBeam2,
        2 => UFG_RegisterMap.SignalToNoiseRatioBeam3,
        3 => UFG_RegisterMap.SignalToNoiseRatioBeam4,
        4 => UFG_RegisterMap.SignalToNoiseRatioBeam5,
        5 => UFG_RegisterMap.SignalToNoiseRatioBeam6,
        6 => UFG_RegisterMap.SignalToNoiseRatioBeam7,
        7 => UFG_RegisterMap.SignalToNoiseRatioBeam8,
        _ => throw new IndexOutOfRangeException("Луч с таким индексом не существует"),
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beamIndex"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public Task<float?> GetSignalToNoiseRatioAsync(int beamIndex, CancellationToken cancellationToken = default) => GetParameterValueAsync<float?>(beamIndex switch
    {
        0 => UFG_RegisterMap.SignalToNoiseRatioBeam1,
        1 => UFG_RegisterMap.SignalToNoiseRatioBeam2,
        2 => UFG_RegisterMap.SignalToNoiseRatioBeam3,
        3 => UFG_RegisterMap.SignalToNoiseRatioBeam4,
        4 => UFG_RegisterMap.SignalToNoiseRatioBeam5,
        5 => UFG_RegisterMap.SignalToNoiseRatioBeam6,
        6 => UFG_RegisterMap.SignalToNoiseRatioBeam7,
        7 => UFG_RegisterMap.SignalToNoiseRatioBeam8,
        _ => throw new IndexOutOfRangeException("Луч с таким индексом не существует"),
    }, cancellationToken);

    /// <summary>
    /// Получить блок с информацией о лучах
    /// </summary>
    public IList<float> GetBeamsSignalToNoiseRatio() => GetBeamsBlock<float>(UFG_RegisterMap.SignalToNoiseRatioBeamsBlock, 2);

    /// <summary>
    /// Получить блок с информацией о лучах асинхронно
    /// </summary>
    public Task<IList<float>> GetBeamsSignalToNoiseRatioAsync(CancellationToken cancellationToken = default) =>
        GetBeamsBlockAsync<float>(UFG_RegisterMap.SignalToNoiseRatioBeamsBlock, 2, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beamIndex"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public float? GetBeamSoundSpeedFiltered(int beamIndex) => GetParameterValue<float?>(beamIndex switch
    {
        0 => UFG_RegisterMap.SoundSpeedBeam1,
        1 => UFG_RegisterMap.SoundSpeedBeam2,
        2 => UFG_RegisterMap.SoundSpeedBeam3,
        3 => UFG_RegisterMap.SoundSpeedBeam4,
        4 => UFG_RegisterMap.SoundSpeedBeam5,
        5 => UFG_RegisterMap.SoundSpeedBeam6,
        6 => UFG_RegisterMap.SoundSpeedBeam7,
        7 => UFG_RegisterMap.SoundSpeedBeam8,
        _ => throw new IndexOutOfRangeException("Луч с таким индексом не существует"),
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beamIndex"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public Task<float?> GetBeamSoundSpeedFilteredAsync(int beamIndex, CancellationToken cancellationToken = default) => GetParameterValueAsync<float?>(beamIndex switch
    {
        0 => UFG_RegisterMap.SoundSpeedBeam1,
        1 => UFG_RegisterMap.SoundSpeedBeam2,
        2 => UFG_RegisterMap.SoundSpeedBeam3,
        3 => UFG_RegisterMap.SoundSpeedBeam4,
        4 => UFG_RegisterMap.SoundSpeedBeam5,
        5 => UFG_RegisterMap.SoundSpeedBeam6,
        6 => UFG_RegisterMap.SoundSpeedBeam7,
        7 => UFG_RegisterMap.SoundSpeedBeam8,
        _ => throw new IndexOutOfRangeException("Луч с таким индексом не существует"),
    }, cancellationToken);

    /// <summary>
    /// Получить блок с фильтрованной скоростью звука лучей
    /// </summary>
    public IList<float> GetBeamsSoundSpeedFiltered() => GetBeamsBlock<float>(UFG_RegisterMap.FilteredSoundSpeedBeamsBlock, 2);

    /// <summary>
    /// Получить блок с фильтрованной скоростью звука лучей асинхронно
    /// </summary>
    public Task<IList<float>> GetBeamsSoundSpeedFilteredAsync(CancellationToken cancellationToken = default) =>
        GetBeamsBlockAsync<float>(UFG_RegisterMap.FilteredSoundSpeedBeamsBlock, 2, cancellationToken);

    /// <summary>
    /// Получить технологический регистр управления
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<uint?> GetTechCR_Async(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<uint?>(UFG_RegisterMap.TechnicalControlRegister, cancellationToken);

    public Task<bool> SetTechControlRegisterAsync(uint value, CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(UFG_RegisterMap.TechnicalControlRegister, value, cancellationToken);

    public Task<bool> EnableTestModeAsync(CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(UFG_RegisterMap.DebugAccessPassword, 0x11775312u, cancellationToken);

    public Task<bool> DisableTestModeAsync(CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(UFG_RegisterMap.DebugAccessPassword, 0u, cancellationToken);

    /// <summary>
    /// Задать текущий режим модема
    /// </summary>
    /// <param name="state">Новое состояние</param>
    /// <param name="cancellationToken"></param>
    public async Task<bool> SetModemStateAsync(bool state, CancellationToken cancellationToken = default)
    {
        var regValue = await GetTechCR_Async(cancellationToken);
        if (regValue is uint tcr) {
            uint modemMask = 0x00000004u;
            return await SetTechControlRegisterAsync(state ? tcr | modemMask : tcr & ~modemMask, cancellationToken);
        }
        return false;
    }

    public Task<bool> SetModemTestStateAsync(UFG_ModemTestState state, CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(UFG_RegisterMap.ModemControl, state, cancellationToken);

    public async Task<bool?> GetBluetoothSupportAsync(CancellationToken cancellationToken = default)
    {
        var tcr = await GetTechCR_Async(cancellationToken);
        return tcr is uint cr ? (cr & 0x00000008u) > 0 : null;
    }

    public Task<string?> GetBluetoothAddressAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<string?>(UFG_RegisterMap.BluetoothAddress, cancellationToken);

    public Task<bool> SetBluetoothTestAsync(bool state, CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(UFG_RegisterMap.BluetoothControl, state ? 1u : 0u, cancellationToken);

    public Task<UFG_BluetoothState> GetBluetoothStateAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<UFG_BluetoothState>(UFG_RegisterMap.BluetoothStatus, cancellationToken);
}