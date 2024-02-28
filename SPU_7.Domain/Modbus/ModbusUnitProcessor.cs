using SPU_7.Common.Modbus;
using SPU_7.Domain.Extensions;
using SPU_7.Modbus;
using SPU_7.Modbus.Extensions;
using SPU_7.Modbus.Processor;
using SPU_7.Modbus.Requests;
using SPU_7.Modbus.Responses;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Modbus;

/// <summary>
/// Класс является базовым модбасовским устройством. Описывает основные методы для чтения, записи.
/// </summary>
/// <typeparam name="TRegisterMapEnum">перечисление, в котором описаны основные регистры устройства</typeparam>
public abstract class ModbusUnitProcessor<TRegisterMapEnum> where TRegisterMapEnum : struct, Enum
{
    protected ModbusUnitProcessor(IModbusProcessor modbusProcessor, IRegisterMapEnum<TRegisterMapEnum> registerMap)
    {
        ModbusProcessor = modbusProcessor;
        _registerMap = registerMap;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public byte ModuleAddress { get; set; }

    protected IModbusProcessor ModbusProcessor { get; }
    private readonly IRegisterMapEnum<TRegisterMapEnum> _registerMap;

    protected RegisterConfiguration GetRegisterConfiguration(TRegisterMapEnum register) => _registerMap.Map[register];

    /// <summary>
    /// Формирует запрос на чтение регистра
    /// </summary>
    /// <param name="register">Стартовый регистр</param>
    /// <returns>запрос на чтение регистра</returns>
    private BaseRequest MakeReadRequest(TRegisterMapEnum register)
    {
        // формируем запрос
        var readRequest = _registerMap.Map[register].ReadFunction switch
        {
            ModbusFunction.ReadHoldingRegisters =>
                (BaseRequest)new ReadHoldingRegistersRequest(ModuleAddress, _registerMap.Map[register].Address, _registerMap.Map[register].RegisterQuantity),
            ModbusFunction.ReadInputRegisters =>
                new ReadInputRegistersRequest(ModuleAddress, _registerMap.Map[register].Address, _registerMap.Map[register].RegisterQuantity),
            _ => throw new ArgumentOutOfRangeException()
        };
        return readRequest;
    }

    /// <summary>
    /// Формирует запрос на чтение регистра
    /// </summary>
    /// <param name="register">Стартовый регистр</param>
    /// <returns>запрос на чтение регистра</returns>
    private BaseRequest MakeReadRequest(RegisterConfiguration register)
    {
        // формируем запрос
        var readRequest = register.ReadFunction switch
        {
            ModbusFunction.ReadHoldingRegisters =>
                (BaseRequest)new ReadHoldingRegistersRequest(ModuleAddress, register.Address, register.RegisterQuantity),
            ModbusFunction.ReadInputRegisters =>
                new ReadInputRegistersRequest(ModuleAddress, register.Address, register.RegisterQuantity),
            _ => throw new ArgumentOutOfRangeException()
        };
        return readRequest;
    }

    /// <summary>
    /// Обработка данных, полученных в результате запроса на чтение
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <param name="orderType">порядок следования байтов</param>
    /// <param name="dataType">тип данных</param>
    /// <returns>обработанные данные</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private object ProcessResponseData(byte[] data, ByteOrderType orderType, RegisterDataType dataType)
    {
        if (data == null) return null;
        object response = null;
        switch (orderType)
        {
            case ByteOrderType.BigEndian_ABCD:
                data = data.Reverse().ToArray();
                break;
            case ByteOrderType.MidBigEndian_BADC:
                data = data.SwapBytes();
                data = data.Reverse().ToArray();
                break;
            case ByteOrderType.LittleEndian_DCBA:
                break;
            case ByteOrderType.MidLittleEndian_CDAB:
                data = data.SwapBytes();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        switch (dataType)
        {
            case RegisterDataType.Flags16:
            case RegisterDataType.Enum16:
            case RegisterDataType.UInt16:
                response = BitConverter.ToUInt16(data);
                break;
            case RegisterDataType.Int16:
                response = BitConverter.ToInt16(data);
                break;
            case RegisterDataType.Flags32:
            case RegisterDataType.Enum32:
            case RegisterDataType.UInt32:
            case RegisterDataType.UnixTime:
                response = BitConverter.ToUInt32(data);
                break;
            case RegisterDataType.Flags64:
            case RegisterDataType.UInt64:
                response = BitConverter.ToUInt64(data);
                break;
            case RegisterDataType.Float:
                response = BitConverter.ToSingle(data);
                break;
            case RegisterDataType.Double:
                response = BitConverter.ToDouble(data);
                break;
            case RegisterDataType.CharArray:
                data = data.Reverse().ToArray();
                data.SwapBytes();
                response = data.ToUnicodeString();
                break;
            case RegisterDataType.ByteArray:
                response = data;
                break;
            case RegisterDataType.TDateTime:
                try
                {
                    var year = new byte[2];
                    Array.Copy(data, 4, year, 0, 2);
                    response = new DateTime(BitConverter.ToUInt16(year), data[6], data[7], data[3], data[2], data[1], data[0] * 4);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                break;
            case RegisterDataType.TConnection:
                break;
            case RegisterDataType.IpV4:
                response = $"{data[3]}.{data[2]}.{data[1]}.{data[0]}";
                break;
            case RegisterDataType.PhoneBCD:
            case RegisterDataType.TWaitConnect:
                response = data;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return response;
    }

    /// <summary>
    /// Формирует запрос на чтение "сырых" данных, т.е. просто данных в виде массива байтов без привязки к типу.
    /// Т.е. будут вычитаны данные, начиная с адреса стратового регистра и указанным количеством. Используется
    /// для создания группового запроса, кгода регистры следуют друг за другом, без разрывов.
    /// </summary>
    /// <param name="registerAddres">Адрес стартового регистра</param>
    /// <param name="registerQuantity">количество регистров</param>
    /// <param name="modbusFunction">функция чтения</param>
    /// <returns>запрос на чтение регистра</returns>
    private BaseRequest MakeReadRawDataRequest(ushort registerAddres, ushort registerQuantity, ModbusFunction modbusFunction)
    {
        if (registerQuantity > 125) throw new ArgumentOutOfRangeException();
        // формируем запрос
        var readRequest = modbusFunction switch
        {
            ModbusFunction.ReadHoldingRegisters => (BaseRequest)new ReadHoldingRegistersRequest(ModuleAddress, registerAddres, registerQuantity),
            ModbusFunction.ReadInputRegisters => new ReadInputRegistersRequest(ModuleAddress, registerAddres, registerQuantity),
            _ => throw new ArgumentOutOfRangeException()
        };
        return readRequest;
    }

    /// <summary>
    /// Считывает группу регистров
    /// </summary>
    /// <param name="startRegister">Первый регистр из группы</param>
    /// <param name="endRegister">Последний регистр из группы</param>
    /// <returns>запрос</returns>
    private BaseRequest MakeReadRequest(TRegisterMapEnum startRegister, TRegisterMapEnum endRegister)
    {
        var startAddress = _registerMap.Map[startRegister].Address;
        var endAddress = _registerMap.Map[endRegister].Address;
        if (startAddress >= endAddress) return null;
        var readFunction = _registerMap.Map[startRegister].ReadFunction;
        // у всех регистров указанного диапазона должна быть одна функция чтения
        if (_registerMap.Map.Any(kvp =>
                kvp.Value.Address >= startAddress && kvp.Value.Address <= endAddress && kvp.Value.ReadFunction != readFunction)) return null;
        var registerQuantity = startAddress - endAddress + _registerMap.Map[endRegister].RegisterQuantity;
        // формируем запрос
        var readRequest = readFunction switch
        {
            ModbusFunction.ReadHoldingRegisters => (BaseRequest)new ReadHoldingRegistersRequest(ModuleAddress, startAddress, (ushort)registerQuantity),
            ModbusFunction.ReadInputRegisters => new ReadInputRegistersRequest(ModuleAddress, startAddress, (ushort)registerQuantity),
            _ => throw new ArgumentOutOfRangeException()
        };
        return readRequest;

    }

    /// <summary>
    /// Формирует запрос на запись регистра/ов
    /// </summary>
    /// <param name="register">Стартовый регистр</param>
    /// <param name="data">массив байт для записи</param>
    /// <returns>запрос на запись регистра</returns>
    private BaseRequest MakeWriteRequest(TRegisterMapEnum register, byte[] data)
    {
        // формируем запрос
        var writeRequest = _registerMap.Map[register].WriteFunction switch
        {
            ModbusFunction.WriteSingleRegister => (BaseRequest)new WriteSingleRegisterRequest(ModuleAddress, _registerMap.Map[register].Address, data),
            ModbusFunction.WriteMultipleRegisters => new WriteMultipleRegistersRequest(ModuleAddress, _registerMap.Map[register].Address, data),
            _ => throw new ArgumentOutOfRangeException()
        };
        return writeRequest;
    }
    
    private BaseRequest MakeWriteRequest(RegisterConfiguration register, byte[] data)
    {
        // формируем запрос
        var writeRequest = register.WriteFunction switch
        {
            ModbusFunction.WriteSingleRegister => (BaseRequest)new WriteSingleRegisterRequest(ModuleAddress, register.Address, data),
            ModbusFunction.WriteMultipleRegisters => new WriteMultipleRegistersRequest(ModuleAddress, register.Address, data),
            _ => throw new ArgumentOutOfRangeException()
        };
        return writeRequest;
    }

    /// <summary>
    /// Обработка ответа на чтение из регистра
    /// </summary>
    private object ProcessReadResponse(ModbusRequestResponse requestResponse) =>
        requestResponse.RequestResult switch
        {
            ModbusRequestResult.Success when requestResponse.Response == null => throw new ModbusTimeoutException(requestResponse.Request),
            ModbusRequestResult.Success when requestResponse.Response != null => requestResponse.Response switch
            {
                ReadHoldingRegistersResponse readHoldingRegistersResponse => readHoldingRegistersResponse.RegisterValues,
                ReadInputRegistersResponse readInputRegistersResponse => readInputRegistersResponse.RegisterValues,
                ReadWriteMultipleRegistersResponse readWriteMultipleRegistersResponse => readWriteMultipleRegistersResponse.RegisterValues,
                DeviceIdentificatorResponse deviceIdentificatorResponse => deviceIdentificatorResponse,
                _ => null
            },
            ModbusRequestResult.SuccessError => throw new ModbusErrorException(requestResponse.Request, ((ErrorResponse)requestResponse.Response).ExceptionCode),
            ModbusRequestResult.FailedDataTransmission => throw new ModbusDataTransmissionException(requestResponse.Request),
            ModbusRequestResult.FailedDataReception => throw new ModbusTimeoutException(requestResponse.Request),
            ModbusRequestResult.WrongCRC => throw new ModbusCrcException(requestResponse.Request),
            _ => throw new ArgumentOutOfRangeException()
        };

    /// <summary>
    /// Обработка ответа на запись в регистр
    /// </summary>
    // private bool ProcessWriteResponse(ModbusRequestResponse modbusRequestResponse) =>
    //     modbusRequestResponse.Response switch
    //     {
    //         null => false,
    //         ErrorResponse => false, 
    //         _ => true
    //     };

    /// <summary>
    /// Обработка ответа на запись в регистр
    /// </summary>
    private bool ProcessWriteResponse(ModbusRequestResponse requestResponse) =>
        requestResponse.RequestResult switch
        {
            ModbusRequestResult.Success when requestResponse.Response == null => throw new ModbusTimeoutException(requestResponse.Request),
            ModbusRequestResult.Success when requestResponse.Response != null => true,
            ModbusRequestResult.SuccessError => throw new ModbusErrorException(requestResponse.Request, ((ErrorResponse)requestResponse.Response).ExceptionCode),
            ModbusRequestResult.FailedDataTransmission => throw new ModbusDataTransmissionException(requestResponse.Request),
            ModbusRequestResult.FailedDataReception => throw new ModbusTimeoutException(requestResponse.Request),
            ModbusRequestResult.WrongCRC => throw new ModbusCrcException(requestResponse.Request),
            _ => throw new ArgumentOutOfRangeException()
        };

    /// <summary>
    /// Прочесть регистр
    /// </summary>
    /// <param name="register">регистр из перечисления</param>
    /// <param name="attemts">количество попыток</param>
    /// <exception cref="ModbusException"></exception>
    /// <exception cref="ModbusCrcException">ошибка CRC при приеме данных</exception>
    /// <exception cref="ModbusTimeoutException">неудачный прием данных</exception>
    /// <exception cref="ModbusDataTransmissionException">при неудачной отправки данных</exception>
    /// <exception cref="ModbusErrorException">при ошибке Modbus</exception>
    /// <returns>значение регистра</returns>
    protected async Task<object> ReadRegisterAsync(TRegisterMapEnum register, int attemts = 3)
    {
        object response = null;
        while (attemts > 0)
        {
            attemts--;
            try
            {
                await Task.Run(() =>
                {
                    var requestResponse = new ModbusRequestResponse(MakeReadRequest(register));
                    ModbusProcessor.EnqueueRequest(requestResponse);
                    requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.ReadTimeout * 2);
                    var responseData = (byte[])ProcessReadResponse(requestResponse);
                    response = ProcessResponseData(responseData, register.GetRegisterSetup().ByteOrderType, register.GetRegisterSetup().DataType);
                });
            }
            catch (Exception e)
            {
                if (e is ModbusErrorException) break;
                if (attemts <= 0) break;
            }
            if (response != null) break;
        }
        return response;
    }

    /// <summary>
    /// Прочесть регистр
    /// </summary>
    /// <param name="register">регистр</param>
    /// <param name="attemts"></param>
    /// <exception cref="ModbusException"></exception>
    /// <exception cref="ModbusCrcException">ошибка CRC при приеме данных</exception>
    /// <exception cref="ModbusTimeoutException">неудачный прием данных</exception>
    /// <exception cref="ModbusDataTransmissionException">при неудачной отправки данных</exception>
    /// <exception cref="ModbusErrorException">при ошибке Modbus</exception>
    /// <returns>значение регистра</returns>
    private async Task<object> ReadRegisterAsync(RegisterConfiguration register, int attemts = 3)
    {
        object response = null;
        while (attemts > 0)
        {
            attemts--;
            try
            {
                await Task.Run(() =>
                {
                    var requestResponse = new ModbusRequestResponse(MakeReadRequest(register));
                    ModbusProcessor.EnqueueRequest(requestResponse);
                    requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.ReadTimeout * 2);
                    var responseData = (byte[])ProcessReadResponse(requestResponse);
                    response = ProcessResponseData(responseData, register.ByteOrderType, register.DataType);
                });
            }
            catch (Exception e)
            {
                if (e is ModbusErrorException) break;
                if (attemts <= 0) break;
            }
            if (response != null) break;
        }
        return response;
    }

    /// <summary>
    /// Прочесть регистр
    /// </summary>
    /// <param name="address">адрес</param>
    /// <param name="readFunction">функция чтения</param>
    /// <param name="dataType">тип данных</param>
    /// <param name="registerQuantity">количество регистров</param>
    /// <param name="byteOrderType">порядок байтов</param>
    /// <exception cref="ModbusException"></exception>
    /// <exception cref="ModbusCrcException">ошибка CRC при приеме данных</exception>
    /// <exception cref="ModbusTimeoutException">неудачный прием данных</exception>
    /// <exception cref="ModbusDataTransmissionException">при неудачной отправки данных</exception>
    /// <exception cref="ModbusErrorException">при ошибке Modbus</exception>
    /// <returns>значение регистра</returns>
    protected async Task<object> ReadRegisterAsync(ushort address, ModbusFunction readFunction, RegisterDataType dataType,
        ushort registerQuantity, ByteOrderType byteOrderType) =>
        await ReadRegisterAsync(new RegisterConfiguration(address, readFunction, ModbusFunction.WriteMultipleRegisters,
            registerQuantity, dataType, byteOrderType));

    /// <summary>
    /// Получаем запрос на группу регистров с одной функцией чтения и без разрывов адресов
    /// </summary>
    /// <param name="registerConfigurations">список регистров</param>
    /// <param name="index">индекс регистра, с которого начинать чтение</param>
    /// <returns>запрос и новый индекс</returns>
    private (BaseRequest request, int newIndex) GetNextReadRequest(List<RegisterConfiguration> registerConfigurations, int index)
    {
        if (registerConfigurations == null || registerConfigurations.Count == 0 || index < 0 || index >= registerConfigurations.Count) return (null, -1);
        var startRegisterAddress = registerConfigurations[index].Address;
        var readFunction = registerConfigurations[index].ReadFunction;
        var registerQuantity = registerConfigurations[index++].RegisterQuantity;
        var nextRegisterAddress = startRegisterAddress + registerQuantity;
        var byteCount = 0;
        // идем по списку пока не встретим разрыв адреса, другую функцию чтения или конец списка, также ограничиваем размером (не более 256 байтов)
        while (index < registerConfigurations.Count)
        {
            if (registerConfigurations[index].ReadFunction != readFunction || registerConfigurations[index].Address != nextRegisterAddress) break;
            byteCount += registerConfigurations[index].RegisterQuantity * 2;
            if (byteCount > 250) break;
            registerQuantity += registerConfigurations[index].RegisterQuantity;
            nextRegisterAddress += registerConfigurations[index++].RegisterQuantity;
        }
        if (index >= registerConfigurations.Count) index = -1;
        // формируем запрос
        var readRequest = readFunction switch
        {
            ModbusFunction.ReadHoldingRegisters => (BaseRequest)new ReadHoldingRegistersRequest(ModuleAddress, startRegisterAddress, registerQuantity),
            ModbusFunction.ReadInputRegisters => new ReadInputRegistersRequest(ModuleAddress, startRegisterAddress, registerQuantity),
            _ => throw new ArgumentOutOfRangeException()
        };
        return (readRequest, index);
    }

    /// <summary>
    /// Прочесть группу регистров
    /// </summary>
    /// <param name="registers">список регистров из перечисления</param>
    /// <returns>словарь значений регистров, key - регистр из перечисления</returns>
    /// <exception cref="ModbusException"></exception>
    /// <exception cref="ModbusCrcException">ошибка CRC при приеме данных</exception>
    /// <exception cref="ModbusTimeoutException">неудачный прием данных</exception>
    /// <exception cref="ModbusDataTransmissionException">при неудачной отправки данных</exception>
    /// <exception cref="ModbusErrorException">при ошибке Modbus</exception>
    protected async Task<Dictionary<TRegisterMapEnum, object>> ReadRegistersAsync(List<TRegisterMapEnum> registers)
    {
        var registerDictionary = new Dictionary<TRegisterMapEnum, object>();
        await Task.Run(() =>
        {
            // индекс, указывающий на положение в списке регистров
            var index = 0;
            // получаем список конфигураций регистров из списка регистров
            var registerConfigurations = registers.Select(rm => _registerMap.Map[rm]).ToList();
            while (true)
            {
                // получаем запрос и очередной индекс
                var (nextRequest, nextIndex) = GetNextReadRequest(registerConfigurations, index);
                if (nextRequest == null) throw new ModbusException();
                // делаем запрос
                var requestResponse = new ModbusRequestResponse(nextRequest);
                ModbusProcessor.EnqueueRequest(requestResponse);
                requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.ReadTimeout * 2);
                var responseData = (byte[])ProcessReadResponse(requestResponse);
                // получили ответ - обрабатываем "сырые"данные
                // считаем количество запрошенных регистров
                var count = (nextIndex == -1 ? registerConfigurations.Count : nextIndex) - index;
                // индекс, указывающий на положение в массиве считанных данных
                var rawDataIndex = 0;
                // добавляем результаты
                for (var i = 0; i < count; i++)
                {
                    var dataSize = registerConfigurations[i + index].RegisterQuantity * 2;
                    var data = new byte[dataSize];
                    Array.Copy(responseData, rawDataIndex, data, 0, dataSize);
                    rawDataIndex += dataSize;
                    var value = ProcessResponseData(data, registerConfigurations[i + index].ByteOrderType, registerConfigurations[i + index].DataType);
                    registerDictionary.Add(registers[i + index], value);
                }
                if (nextIndex == -1) break;
                index = nextIndex;
            }
        });
        return registerDictionary;
    }

    /// <summary>
    /// Прочесть группу регистров
    /// </summary>
    /// <param name="registers">список регистров</param>
    /// <param name="attemts"></param>
    /// <returns>словарь значений регистров, key - адрес регистра</returns>
    protected async Task<Dictionary<ushort, object>> ReadRegistersAsync(List<RegisterConfiguration> registers, int attemts = 3)
    {
        var registerDictionary = new Dictionary<ushort, object>();
        var attemtsCopy = attemts;
        var responseData = Array.Empty<byte>();
        await Task.Run(() =>
        {
            // индекс, указывающий на положение в списке регистров
            var index = 0;
            while (true)
            {
                // получаем запрос и очередной индекс
                var (nextRequest, nextIndex) = GetNextReadRequest(registers, index);
                if (nextRequest == null) break;
                while (attemtsCopy > 0)
                {
                    attemtsCopy--;
                    try
                    {
                        Task.Run(() =>
                        {
                            // делаем запрос
                            var requestResponse = new ModbusRequestResponse(nextRequest);
                            ModbusProcessor.EnqueueRequest(requestResponse);
                            requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.ReadTimeout * 2);
                            responseData = (byte[])ProcessReadResponse(requestResponse);
                        });
                    }
                    catch (Exception e)
                    {
                        if (e is ModbusErrorException) break;
                        if (attemtsCopy <= 0) break;
                    }
                }

                // получили ответ - обрабатываем "сырые"данные
                // считаем количество запрошенных регистров
                var count = (nextIndex == -1 ? registers.Count : nextIndex) - index;
                // индекс, указывающий на положение в массиве считанных данных
                var rawDataIndex = 0;
                // добавляем результаты
                for (var i = 0; i < count; i++)
                {
                    var dataSize = registers[i + index].RegisterQuantity * 2;
                    var data = new byte[dataSize];
                    Array.Copy(responseData, rawDataIndex, data, 0, dataSize);
                    rawDataIndex += dataSize;
                    var value = ProcessResponseData(data, registers[i + index].ByteOrderType, registers[i + index].DataType);
                    registerDictionary.Add(registers[i + index].Address, value);
                }
                if (nextIndex == -1) break;
                index = nextIndex;
            }
        });
        return registerDictionary;
    }

    /// <summary>
    /// Прочесть группу регистров. Результат в том же списке.
    /// </summary>
    /// <param name="registers">список регистров</param>
    /// <param name="attemts"></param>
    /// <exception cref="ModbusCrcException">ошибка CRC при приеме данных</exception>
    /// <exception cref="ModbusTimeoutException">неудачный прием данных</exception>
    /// <exception cref="ModbusDataTransmissionException">при неудачной отправки данных</exception>
    protected async Task ReadRegistersAsync(List<Register> registers, int attemts = 3)
    {
        ModbusRequestResponse requestResponse;
        // индекс, указывающий на положение в списке регистров
        var index = 0;
        while (true)
        {
            var attemtsCopy = attemts;
            var responseData = Array.Empty<byte>();
            // получаем запрос и очередной индекс
            var (nextRequest, nextIndex) = GetNextReadRequest(registers.Cast<RegisterConfiguration>().ToList(), index);
            if (nextRequest == null) throw new ModbusException();
            // считаем количество запрошенных регистров
            var requestRegistersCount = (nextIndex == -1 ? registers.Count : nextIndex) - index;
            // считаем количество запрошенных байтов
            var end = nextIndex == -1 ? registers.Count : nextIndex;
            var requestBytesCount = 0;
            for (var i = index; i < end; i++)
            {
                requestBytesCount += registers[i].RegisterQuantity * 2;
            }

            while (attemtsCopy > 0)
            {
                attemtsCopy--;
                try
                {
                    await Task.Run(() =>
                    {
                        // делаем запрос
                        requestResponse = new ModbusRequestResponse(nextRequest);
                        ModbusProcessor.EnqueueRequest(requestResponse);
                        requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.ReadTimeout * 2);
                        responseData = (byte[])ProcessReadResponse(requestResponse);
                    });
                }
                catch (Exception e)
                {
                    if (e is ModbusErrorException) break;
                    if (attemtsCopy <= 0) break;
                }
                if (responseData != null && responseData.Length == requestBytesCount) break;
            }

            if (responseData != null && responseData.Length == requestBytesCount)
            {
                // получили ответ - обрабатываем "сырые"данные
                // индекс, указывающий на положение в массиве считанных данных
                var rawDataIndex = 0;
                // добавляем результаты
                for (var i = 0; i < requestRegistersCount; i++)
                {
                    var dataSize = registers[i + index].RegisterQuantity * 2;
                    var data = new byte[dataSize];
                    Array.Copy(responseData, rawDataIndex, data, 0, dataSize);
                    rawDataIndex += dataSize;
                    var value = ProcessResponseData(data, registers[i + index].ByteOrderType, registers[i + index].DataType);
                    registers[i + index].Value = value;
                }
            }

            if (nextIndex == -1)
            {
                break;
            }
            index = nextIndex;
        }
    }

    /// <summary>
    /// Записать регистр
    /// </summary>
    /// <param name="register">Стартовый регистр</param>
    /// <param name="data">Массив байт для записи</param>
    /// <param name="attemts">Количество попыток</param>
    /// <returns>true если все ok</returns>
    protected async Task<bool> WriteRegisterAsync(TRegisterMapEnum register, byte[] data, int attemts = 3)
    {
        var response = false;
        while (attemts > 0)
        {   
            attemts--;
            try
            {
                await Task.Run(() =>
                {
                    var requestResponse = new ModbusRequestResponse(MakeWriteRequest(register, data));
                    ModbusProcessor.EnqueueRequest(requestResponse);
                    requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.WriteTimeout * 2);
                    response = ProcessWriteResponse(requestResponse);
                });
                if (response) break;
            }
            catch (Exception e)
            {
                if (e is ModbusErrorException) break;
                if (attemts <= 0) break;
            }
        }
        return response;
    }

    protected async Task<bool> WriteRegisterAsync(RegisterConfiguration registerConfiguration, byte[] data, int attemts = 3)
    {
        var response = false;
        while (attemts > 0)
        {
            attemts--;
            try
            {
                await Task.Run(() =>
                {
                    var requestResponse = new ModbusRequestResponse(MakeWriteRequest(registerConfiguration, data));
                    ModbusProcessor.EnqueueRequest(requestResponse);
                    requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.WriteTimeout * 2);
                    response = ProcessWriteResponse(requestResponse);
                });
                if (response) break;
            }
            catch (Exception e)
            {
                if (e is ModbusErrorException) break;
                if (attemts <= 0) break;
            }
        }
        return response;
    }

    /// <summary>
    /// Конвертирует строку в UInt16
    /// </summary>
    /// <param name="address">Строковый адрес в формате 0хFFFF</param>
    /// <returns></returns>
    protected ushort ConvertHexStringToUshort(string address) => Convert.ToUInt16(address, 16);
    
    /// <summary>
    /// Запись в регистр по числовому адресу
    /// </summary>
    /// <param name="address">Адрес в формате UInt16 (ushort)</param>
    /// <param name="writeFunction">Тип функции для записи</param>
    /// <param name="dataType">Тип данных</param>
    /// <param name="registerQuantity">Количество регистров</param>
    /// <param name="byteOrderType">Тип расстановки байтов </param>
    /// <param name="data">Массив данных</param>
    /// <returns>Получилось ли записать данные</returns>
    protected async Task<bool> WriteRegisterAsync(ushort address, 
        ModbusFunction writeFunction,
        RegisterDataType dataType,
        ushort registerQuantity, ByteOrderType byteOrderType, byte[] data) =>
        await WriteRegisterAsync(
            new RegisterConfiguration(address, ModbusFunction.ReadHoldingRegisters, writeFunction, registerQuantity,
                dataType, byteOrderType), data);
    
    /// <summary>
    /// Запись в регистр строковому адресу
    /// </summary>
    /// <param name="address">Адрес в строковом формате</param>
    /// <param name="writeFunction">Тип функции для записи</param>
    /// <param name="dataType">Тип данных</param>
    /// <param name="registerQuantity">Количество регистров</param>
    /// <param name="byteOrderType">Тип расстановки байтов </param>
    /// <param name="data">Массив данных</param>
    /// <returns>Получилось ли записать данные</returns>
    protected async Task<bool> WriteRegisterAsync(string address, 
        ModbusFunction writeFunction,
        RegisterDataType dataType,
        ushort registerQuantity, ByteOrderType byteOrderType, byte[] data) =>
        await WriteRegisterAsync(
            new RegisterConfiguration(ConvertHexStringToUshort(address), ModbusFunction.ReadHoldingRegisters, writeFunction, registerQuantity,
                dataType, byteOrderType), data);
    
    /// <summary>
    /// Запись и одновременное чтение регистров
    /// </summary>
    /// <param name="writeStartingAddress">стартовый адрес для записи</param>
    /// <param name="dataForWrite">данные для записи (должны быть кратны 2)</param>
    /// <param name="readStartingAddress">стартовый адрес для чтения</param>
    /// <param name="quantityRegistersToRead">количество регистров для чтения</param>
    /// <param name="attemts"></param>
    /// <returns>массив байтов</returns>
    /// <exception cref="ModbusException"></exception>
    /// <exception cref="ModbusCrcException">ошибка CRC при приеме данных</exception>
    /// <exception cref="ModbusTimeoutException">неудачный прием данных</exception>
    /// <exception cref="ModbusDataTransmissionException">при неудачной отправки данных</exception>
    /// <exception cref="ModbusErrorException">при ошибке Modbus</exception>
    protected async Task<object> WriteReadRegisters(ushort writeStartingAddress, byte[] dataForWrite,
        ushort readStartingAddress, ushort quantityRegistersToRead, int attemts = 5)
    {
        object response = null;
        while (attemts > 0)
        {
            attemts--;
            try
            {
                await Task.Run(() =>
                {
                    var request = new ReadWriteMultipleRegistersRequest(ModuleAddress, readStartingAddress, quantityRegistersToRead, writeStartingAddress,
                        dataForWrite);
                    var requestResponse = new ModbusRequestResponse(request);
                    ModbusProcessor.EnqueueRequest(requestResponse);
                    requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.ReadTimeout * 4);
                    response = ProcessReadResponse(requestResponse);
                });
                if (response != null) break;
            }
            catch (ModbusErrorException e)
            {
                response = e.ExceptionCode;
                break;
            }
            catch (Exception e)
            {
                if (e is ModbusErrorException) break;
                if (attemts <= 0) break;
            }
        }
        return response;
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Прочесть идентификатор прибора
    /// <param name="noRequest">без запроса или с запросом (без запроса в случае входящего соединения по TCP)</param>
    /// </summary>
    /// <returns><see cref="DeviceIdentificatorResponse"/>></returns>
    protected async Task<DeviceIdentificatorResponse> GetID(bool noRequest = false, int attemts = 10)
    {
        DeviceIdentificatorResponse response = null;
        while (attemts > 0)
        {
            attemts--;
            try
            {
                await Task.Run(() =>
                {
                    var request = new DeviceIdentificatorRequest(ModuleAddress) { IsNoRequest = noRequest };

                    var requestResponse = new ModbusRequestResponse(request);
                    ModbusProcessor.EnqueueRequest(requestResponse);
                    requestResponse.Event.WaitOne(ModbusProcessor.ProtocolSettings.ReadTimeout * 2);
                    response = (DeviceIdentificatorResponse)ProcessReadResponse(requestResponse);
                });
                if (response != null) break;
            }
            catch (Exception e)
            {
                if (e is ModbusErrorException) break;
                if (attemts <= 0) break;
            }
        }
        return response;
    }

}