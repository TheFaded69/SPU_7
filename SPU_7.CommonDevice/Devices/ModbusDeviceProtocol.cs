using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus;
using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;
using SPU_7.DeviceCommunication.Modbus.Response;

namespace SPU_7.CommonDevice.Devices;
public class ModbusDeviceProtocol : ModbusClient, IModbusDeviceProtocol
{
    public ModbusDeviceProtocol(ICommunicationChannel? deviceCommunication, DeviceEndianess endianess = DeviceEndianess.CDAB)
        : base(deviceCommunication, endianess)
    {
        RetryCount = 3;
    }

    private int _address;
    private int _retryCount;

    public int Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public int RetryCount
    {
        get => _retryCount;
        set => SetProperty(ref _retryCount, value);
    }

    public ModbusBaseResponse? DoRequest(ModbusBaseRequest request, ILogger logger)
    {
        var retries = RetryCount;
        do {
            try {
                IsRequesting = true;
                return ExecuteRequest(request);
            }
            catch (TimeoutException) {
                logger.Debug("Не удалось выполнить Modbus запрос за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Осталось попыток: {RetriesLeft}.",
                    CommunicationChannel?.Connection, Address, retries);
            }
            catch (ModbusException ex) {
                logger.Debug("Возникла ошибка при запросе Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                    CommunicationChannel?.Connection, Address, ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                if ((byte)ex.ExceptionCode == 0xFF) {
                    continue;
                }
                return default;
            }
            catch (Exception ex) {
                logger.Error(ex, "Возникла ошибка при выполнении запроса Modbus!");
                throw;
            }
            finally {
                IsRequesting = false;
            }
        }
        while (retries-- > 0);
        return default;
    }

    public async Task<ModbusBaseResponse?> DoRequestAsync(ModbusBaseRequest request, ILogger logger, CancellationToken cancellationToken = default)
    {
        var retries = RetryCount;
        do {
            try {
                IsRequesting = true;
                return await ExecuteRequestAsync(request, cancellationToken);//.ConfigureAwait(false);
            }
            catch (TimeoutException) {
                logger.Debug("Не удалось выполнить Modbus запрос за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Осталось попыток: {RetriesLeft}.",
                    CommunicationChannel?.Connection, Address, retries);
            }
            catch (ModbusException ex) {
                logger.Debug("Возникла ошибка при запросе Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                     CommunicationChannel?.Connection, Address, ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                if ((byte)ex.ExceptionCode == 0xFF) {
                    continue;
                }
                return default;
            }
            catch (Exception ex) {
                logger.Error(ex, "Возникла ошибка при выполнении запроса Modbus!");
                throw;
            }
            finally {
                IsRequesting = false;
            }
        }
        while (retries-- > 0);
        return default;
    }

    public T? ReadRegisterValue<T>(RegisterConfiguration rc, ILogger logger)
    {
        var retries = RetryCount;
        do {
            try {
                IsRequesting = true;
                return ReadRegisterValue<T>(rc);
            }
            catch (TimeoutException) {
                logger.Debug("Не удалось прочитать регистр Modbus за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Осталось попыток: {RetriesLeft}.",
                    CommunicationChannel?.Connection, Address, rc.Address, retries);
            }
            catch (ModbusException ex) {
                logger.Debug("Возникла ошибка при чтении регистра Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                    CommunicationChannel?.Connection, Address, rc.Address, ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                if ((byte)ex.ExceptionCode == 0xFF) {
                    continue;
                }
                return default;
            }
            catch (Exception ex) {
                logger.Error(ex, "Возникла ошибка при чтении регистра Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}.",
                    CommunicationChannel?.Connection, Address, rc.Address);
                throw;
            }
            finally {
                IsRequesting = false;
            }
        } while (retries-- > 0);
        return default;
    }

    public async Task<T?> ReadRegisterValueAsync<T>(RegisterConfiguration rc, ILogger logger, CancellationToken cancellationToken = default)
    {
        var retries = RetryCount;
        do {
            try {
                IsRequesting = true;
                return await ReadRegisterValueAsync<T>(rc, cancellationToken).ConfigureAwait(false);
            }
            catch (TimeoutException) {
                logger.Debug("Не удалось прочитать регистр Modbus за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Осталось попыток: {RetriesLeft}.",
                    CommunicationChannel?.Connection, Address, rc.Address, retries);
            }
            catch (ModbusException ex) {
                logger.Debug("Возникла ошибка при чтении регистра Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                    CommunicationChannel?.Connection, Address, rc.Address, ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                if ((byte)ex.ExceptionCode == 0xFF) {
                    continue;
                }
                return default;
            }
            catch (Exception ex) {
                logger.Error(ex, "Возникла ошибка при чтении регистра Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}.",
                    CommunicationChannel?.Connection, Address, rc.Address);
                throw;
            }
            finally {
                IsRequesting = false;
            }
        } while (retries-- > 0);
        return default;
    }

    public bool WriteRegisterValue(RegisterConfiguration rc, object value, ILogger logger)
    {
        var retries = RetryCount;
        do {
            try {
                IsRequesting = true;
                WriteRegisterValue(rc, value);
                return true;
            }
            catch (TimeoutException) {
                logger.Debug("Не удалось записать регистр Modbus за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Осталось попыток: {RetriesLeft}.",
                    CommunicationChannel?.Connection, Address, rc.Address, retries);
            }
            catch (ModbusException ex) {
                logger.Debug("Возникла ошибка при записи регистра Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                    CommunicationChannel?.Connection, Address, rc.Address, ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                if ((byte)ex.ExceptionCode == 0xFF) {
                    continue;
                }
                return false;
            }
            catch (Exception ex) {
                logger.Error(ex, "Возникла ошибка при записи регистра Modbus! Адрес: 0x{RegisterAddress:X4}.",
                    CommunicationChannel?.Connection, Address, rc.Address);
                throw;
            }
            finally {
                IsRequesting = false;
            }
        } while (retries-- > 0);
        return false;
    }

    public async Task<bool> WriteRegisterValueAsync(RegisterConfiguration rc, object value, ILogger logger, CancellationToken cancellationToken = default)
    {
        var retries = RetryCount;
        do {
            try {
                IsRequesting = true;
                await WriteRegisterValueAsync(rc, value, cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch (TimeoutException) {
                logger.Debug("Не удалось записать регистр Modbus за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Осталось попыток: {RetriesLeft}.",
                    CommunicationChannel?.Connection, Address, rc.Address, retries);
            }
            catch (ModbusException ex) {
                logger.Debug("Возникла ошибка при записи регистра Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}, Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                    CommunicationChannel?.Connection, Address, rc.Address, ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                /* TODO: Обработка определённых исключений Modbus
                 * 
                 * switch (ex.ExceptionCode) {
                    case ModbusExceptionCode.ServerDeviceFailure:
                        break;
                    case ModbusExceptionCode.Acknowledge: {
                        //if (waitAck) await Task.Delay();
                        //continue;
                        break;
                    }
                    case ModbusExceptionCode.ServerDeviceBusy: {
                        await Task.Delay(100, cancellationToken); // BusyDelay
                        continue;
                    }
                }*/
                if ((byte)ex.ExceptionCode == 0xFF) {
                    continue;
                }
                return false;
            }
            catch (Exception ex) {
                logger.Error(ex, "Возникла ошибка при записи регистра Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес регистра: 0x{RegisterAddress:X4}.",
                    CommunicationChannel?.Connection, Address, rc.Address);
                throw;
            }
            finally {
                IsRequesting = false;
            }
        } while (retries-- > 0);
        return false;
    }

    private T? ReadRegisterValue<T>(RegisterConfiguration rc) => rc.ReadFunction switch
    {
        ModbusFunction.ReadHoldingRegisters => ReadHoldingRegisters(Address, rc.Address, rc.NumberOfRegisters * 2)
        .ConvertRegisterData(rc.ValueDataType, Endianess).ConvertObjectTo<T>(),
        ModbusFunction.ReadInputRegisters => ReadInputRegisters(Address, rc.Address, rc.NumberOfRegisters * 2)
        .ConvertRegisterData(rc.ValueDataType, Endianess).ConvertObjectTo<T>(),
        ModbusFunction.WriteSingleRegister => throw new ArgumentException("Функция записи используется для чтения значения регистра", nameof(rc)),
        ModbusFunction.WriteMultipleRegisters => throw new ArgumentException("Функция записи используется для чтения значения регистра", nameof(rc)),
        _ => throw new NotImplementedException($"Функция чтения 0x{(int)rc.ReadFunction:X2} не поддерживается")
    };

    private async Task<T?> ReadRegisterValueAsync<T>(RegisterConfiguration rc, CancellationToken cancellationToken = default) => rc.ReadFunction switch
    {
        ModbusFunction.ReadHoldingRegisters => (await ReadHoldingRegistersAsync(Address, rc.Address, rc.NumberOfRegisters * 2, cancellationToken).ConfigureAwait(false))
        .ConvertRegisterData(rc.ValueDataType, Endianess).ConvertObjectTo<T>(),
        ModbusFunction.ReadInputRegisters => (await ReadInputRegistersAsync(Address, rc.Address, rc.NumberOfRegisters * 2, cancellationToken).ConfigureAwait(false))
        .ConvertRegisterData(rc.ValueDataType, Endianess).ConvertObjectTo<T>(),
        ModbusFunction.WriteSingleRegister => throw new ArgumentException("Функция записи используется для чтения значения регистра", nameof(rc)),
        ModbusFunction.WriteMultipleRegisters => throw new ArgumentException("Функция записи используется для чтения значения регистра", nameof(rc)),
        _ => throw new NotImplementedException($"Функция чтения 0x{(int)rc.ReadFunction:X2} не поддерживается")
    };

    private void WriteRegisterValue(RegisterConfiguration rc, object value)
    {
        switch (rc.WriteFunction)
        {
            case ModbusFunction.WriteSingleRegister:
                WriteSingleRegister(Address, rc.Address, value.ConvertRegisterData(rc.ValueDataType, Endianess));
                break;
            case ModbusFunction.WriteMultipleRegisters:
                WriteMultipleRegisters(Address, rc.Address, value.ConvertRegisterData(rc.ValueDataType, Endianess));
                break;
            case ModbusFunction.ReadHoldingRegisters:
            case ModbusFunction.ReadInputRegisters:
                throw new ArgumentException("Функция чтения используется для записи значения регистра", nameof(rc));
            default:
                throw new NotImplementedException($"Функция записи 0x{(int)rc.WriteFunction:X2} не поддерживается");
        }
    }

    private Task WriteRegisterValueAsync(RegisterConfiguration rc, object value, CancellationToken cancellationToken = default) => rc.WriteFunction switch
    {
        ModbusFunction.WriteSingleRegister =>
            WriteSingleRegisterAsync(Address, rc.Address, value.ConvertRegisterData(rc.ValueDataType, Endianess), cancellationToken),
        ModbusFunction.WriteMultipleRegisters =>
            WriteMultipleRegistersAsync(Address, rc.Address, value.ConvertRegisterData(rc.ValueDataType, Endianess), cancellationToken),
        ModbusFunction.ReadInputRegisters => throw new ArgumentException("Функция чтения используется для записи значения регистра", nameof(rc)),
        ModbusFunction.ReadHoldingRegisters => throw new ArgumentException("Функция чтения используется для записи значения регистра", nameof(rc)),
        _ => throw new NotImplementedException($"Функция записи 0x{(int)rc.WriteFunction:X2} не поддерживается")
    };

    public async Task<bool> ReadRegistersBlocksAsync(IList<Register> registers, ModbusFunction modbusFunction, ILogger logger, CancellationToken cancellationToken = default)
    {
        var sortedRegisters = registers.OrderBy(register => register.Configuration.Address).ToList();
        var registersBlocks = new RegistersBlocks(sortedRegisters);
        var lastStartAddress = 0;
        var result = sortedRegisters.Count > 0;
        foreach (var (startAddress, registersBlock) in registersBlocks) {
            if (registersBlock.Count == 0) continue;
            lastStartAddress = startAddress;
            var retries = RetryCount;
            ArraySegment<byte>? dataBlockResult = null;
            do {
                try {
                    IsRequesting = true;
                    dataBlockResult = await (modbusFunction switch
                    {
                        ModbusFunction.ReadHoldingRegisters =>
                            ReadHoldingRegistersAsync(Address, startAddress, registersBlock.Sum(rb => rb.Configuration.NumberOfRegisters) * 2, cancellationToken),
                        ModbusFunction.ReadInputRegisters =>
                            ReadInputRegistersAsync(Address, startAddress, registersBlock.Sum(rb => rb.Configuration.NumberOfRegisters) * 2, cancellationToken),
                        _ => Task.FromException<ArraySegment<byte>>(new InvalidOperationException("Нельзя выполнить чтение с выбранной функцией!"))
                    }).ConfigureAwait(false);
                    foreach (var register in registersBlock) {
                        register.LastRead = DateTime.Now;
                    }
                    if (dataBlockResult is not ArraySegment<byte> dataBlock) {
                        result &= false;
                        break;
                    }
                    var offset = 0;
                    foreach (var register in registersBlock) {
                        var registerSize = register.Configuration.ValueDataType.GetDataTypeDescription()?.Size ?? register.Configuration.NumberOfRegisters * 2;
                        register.Value = dataBlock.Slice(offset, registerSize).ConvertRegisterData(register.Configuration.ValueDataType, Endianess);
                        offset += registerSize;
                    }
                    break;
                }
                catch (TimeoutException) {
                    logger.Debug("Не удалось прочитать блок регистров Modbus за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес блока: 0x{RegisterBlockAddress:X4}, Осталось попыток: {RetriesLeft}.",
                        CommunicationChannel?.Connection, Address, lastStartAddress, retries);
                }
                catch (ModbusException ex) {
                    logger.Debug("Возникла ошибка при чтении блок регистров Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес блока: 0x{RegisterBlockAddress:X4}, Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                        CommunicationChannel?.Connection, Address, lastStartAddress, ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                    if ((byte)ex.ExceptionCode == 0xFF) {
                        continue;
                    }
                    result &= false;
                    break;
                }
                catch (Exception ex) {
                    logger.Error(ex, "Возникла ошибка при чтении блок регистров Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес блока: 0x{RegisterBlockAddress:X4}.",
                        CommunicationChannel?.Connection, Address, lastStartAddress);
                    throw;
                }
                finally {
                    IsRequesting = false;
                }
            }
            while (retries-- > 0);
            result &= retries >= 0;
        }
        return result;
    }

    public Task<bool> WriteRegistersBlocksAsync(IList<Register> registers, ILogger logger, CancellationToken cancellationToken = default) =>
        WriteRegistersBlocksAsync(registers, logger, null, null, cancellationToken);

    public async Task<bool> WriteRegistersBlocksAsync(IList<Register> registers, ILogger logger,
        Func<Task>? actionBefore, Func<Task>? actionAfter = null, CancellationToken cancellationToken = default)
    {
        var sortedRegisters = registers.Where(register => register.Value is not null).OrderBy(register => register.Configuration.Address).ToList();
        var registersBlocks = new RegistersBlocks(sortedRegisters);
        var lastStartAddress = 0;
        var result = sortedRegisters.Count > 0;
        foreach (var (startAddress, registersBlock) in registersBlocks) {
            if (registersBlock.Count == 0) continue;
            lastStartAddress = startAddress;
            var retries = RetryCount;
            do {
                try {
                    IsRequesting = true;
                    if (actionBefore != null) await actionBefore();
#pragma warning disable CS8604 // Possible null reference argument.
                    await WriteMultipleRegistersAsync(Address, startAddress,
                        registersBlock.SelectMany(register => register.Value.ConvertRegisterData(register.Configuration.ValueDataType, Endianess))
                        .ToArray(), cancellationToken).ConfigureAwait(false);
                    foreach (var register in registersBlock) {
                        register.LastWrite = DateTime.Now;
                    }
#pragma warning restore CS8604 // Possible null reference argument.
                    if (actionAfter != null) await actionAfter();
                    break;
                }
                catch (TimeoutException) {
                    logger.Debug("Не удалось записать блок регистров Modbus за отведённое время! Порт: {PortName}, Адрес устройства: {UnitId}, Осталось попыток: {RetriesLeft}.",
                        CommunicationChannel?.Connection, Address, retries);
                }
                catch (ModbusException ex) {
                    logger.Debug("Возникла ошибка при записи блока регистров Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес блока: 0x{RegisterBlockAddress:X4}," +
                        " Сообщение: {ErrorMessage}, Код ошибки: 0x{ModbusExceptionCode:X2}, Описание: {CodeDescription}, Данные: {FrameData}.",
                        CommunicationChannel?.Connection, Address, lastStartAddress,
                        ex.Message, (byte)ex.ExceptionCode, ex.ExceptionCode.GetDescription(), ex.DataFrameReceived.ToHexString());
                    if ((byte)ex.ExceptionCode == 0xFF) {
                        continue;
                    }
                    result = false;
                    break;
                }
                catch (Exception ex) {
                    logger.Error(ex, "Возникла ошибка при записи блока регистров Modbus! Порт: {PortName}, Адрес устройства: {UnitId}, Адрес блока: 0x{RegisterBlockAddress:X4}.",
                        CommunicationChannel?.Connection, Address, lastStartAddress);
                    throw;
                }
                finally {
                    IsRequesting = false;
                }
            }
            while (result && (retries-- > 0));
            result &= retries >= 0; // Проверка на кол-во попыток для текущей операции
        }
        return result;
    }
}