using System.ComponentModel;
using SPU_7.DeviceCommunication.Base;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;
using SPU_7.DeviceCommunication.Modbus.Response;

namespace SPU_7.DeviceCommunication.Modbus;

public abstract class ModbusClient : BindableBase
{
    public ModbusClient(ICommunicationChannel? deviceCommunication, DeviceEndianess deviceEndianess, ModbusClientMode clientMode = ModbusClientMode.RTU)
    {
        CommunicationChannel = deviceCommunication;
        ClientMode = clientMode;
        Endianess = deviceEndianess;
    }

    private ModbusClientMode _clientMode;
    private ICommunicationChannel? _communicationChannel;
    private bool _isRequesting;
    private bool _usePreamble;
    private int _delayAfterPreamble;
    private byte[] _preamble = Array.Empty<byte>();

    /// <summary>
    /// Использовать ли преамбулу
    /// </summary>
    public bool UsePreamble
    {
        get => _usePreamble;
        set => SetProperty(ref _usePreamble, value);
    }

    /// <summary>
    /// Данные для преамбулы
    /// </summary>
    public byte[] Preamble
    {
        get => _preamble;
        set => SetProperty(ref _preamble, value);
    }

    /// <summary>
    /// Задержка после преамбулы, в мс
    /// </summary>
    public int DelayAfterPreamble
    {
        get => _delayAfterPreamble;
        set => SetProperty(ref _delayAfterPreamble, value);
    }

    /// <summary>
    /// Тип модели связи и памяти устройства
    /// </summary>
    public DeviceEndianess Endianess { get; }

    /// <summary>
    /// Происходит ли запрос данных
    /// </summary>
    public bool IsRequesting
    {
        get => _isRequesting;
        protected set => SetProperty(ref _isRequesting, value, () =>
        {
            RaisePropertyChanged(nameof(IsReceive));
            RaisePropertyChanged(nameof(IsTransmit));
        });
    }

    /// <summary>
    /// Происходит ли сейчас получение 
    /// </summary>
    public bool IsReceive => CommunicationChannel?.IsReceive is true && IsRequesting;

    /// <summary>
    /// Происходит ли сейчас отправка
    /// </summary>
    public bool IsTransmit => CommunicationChannel?.IsTransmit is true && IsRequesting;

    /// <summary>
    /// Интерфейс связи с устройством
    /// </summary>
    public ICommunicationChannel? CommunicationChannel
    {
        get => _communicationChannel;
        set => SetProperty(ref _communicationChannel, value, () =>
        {
            if (CommunicationChannel is null) return; // Если был null, можно ничего не делать
            CommunicationChannel.PropertyChanged -= OnCommunicationChannelPropertyChanged;
        }, () =>
        {
            if (CommunicationChannel is null) return; // Если стал null, можно ничего не делать
            CommunicationChannel.PropertyChanged += OnCommunicationChannelPropertyChanged;
        });
    }

    /// <summary>
    /// Режим передачи данных клиенту
    /// </summary>
    public ModbusClientMode ClientMode
    {
        get => _clientMode;
        set => SetProperty(ref _clientMode, value);
    }

    /// <summary>
    /// Перенаправление события изменения свойства
    /// </summary>
    /// <param name="s">От кого получено событие</param>
    /// <param name="e">Параметры события</param>
    private void OnCommunicationChannelPropertyChanged(object? s, PropertyChangedEventArgs e)
    {
        if (IsRequesting is false) return; // Если не было запроса с этой стороны, не создавать событие изменения свойств
        switch (e.PropertyName) {
            case nameof(CommunicationChannel.IsTransmit):
                RaisePropertyChanged(nameof(IsTransmit));
                break;
            case nameof(CommunicationChannel.IsReceive):
                RaisePropertyChanged(nameof(IsReceive));
                break;
        }
    }

    #region Синхронные методы

    /// <summary>
    /// Выполнить запрос Modbus
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns>Ответ на запрос</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected ModbusBaseResponse ExecuteRequest(ModbusBaseRequest request)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        CheckUnitIdentifier(request.Address);
        return CommunicationChannel.TryProcessRequestAndResponse<ModbusBaseResponse>(delegate
        {
            if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
            if (CommunicationChannel is SerialPortCommunication serialCommunication) {
                serialCommunication.DiscardInBuffer();
                serialCommunication.DiscardOutBuffer();
            }
            var frameData = new byte[5];
            if (UsePreamble && Preamble.Length != 0) {
                CommunicationChannel.Write(Preamble);
                if (DelayAfterPreamble != 0) Thread.Sleep(DelayAfterPreamble);
            }
            CommunicationChannel.Write(request.Data);
            CommunicationChannel.Read(frameData, 0, frameData.Length);
            CheckErrorFrame(frameData);
            var byteCount = frameData[2];
            if ((byteCount - 2) > 0) {
                Array.Resize(ref frameData, frameData.Length + byteCount - 2);
                CommunicationChannel.Read(frameData, 5, byteCount - 2);
            }
            return new ReportSlaveIdResponse(request, frameData, Endianess);
        });
    }

    /// <summary>
    /// Прочитать регистры хранения
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="count"></param>
    /// <returns>Массив сырых данных (байты)</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected ArraySegment<byte> ReadHoldingRegisters(int unitId, int startingAddress, int count)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        var registerCount = count / 2;
        CheckUnitIdentifier(unitId);
        CheckReadRegistersCount(registerCount);
        return CommunicationChannel.TryProcessRequestAndResponse(
            () => ReadRegisters((byte)unitId, (ushort)startingAddress, (ushort)registerCount, ModbusFunction.ReadHoldingRegisters));
    }

    /// <summary>
    /// Прочитать регистры ввода
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="count"></param>
    /// <returns>Массив сырых данных (байты)</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected ArraySegment<byte> ReadInputRegisters(int unitId, int startingAddress, int count)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        var registerCount = count / 2;
        CheckUnitIdentifier(unitId);
        CheckReadRegistersCount(registerCount);
        return CommunicationChannel.TryProcessRequestAndResponse(
            () => ReadRegisters((byte)unitId, (ushort)startingAddress, (ushort)registerCount, ModbusFunction.ReadInputRegisters));
    }

    /// <summary>
    /// Прочитать регистры
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="registerCount">Количество регистров</param>
    /// <param name="function">Номер функции чтения</param>
    /// <returns>Данные регистров в сыром виде</returns>
    /// <exception cref="ModbusException"></exception>
    private ArraySegment<byte> ReadRegisters(byte unitId, ushort startingAddress, ushort registerCount, ModbusFunction function)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        if (CommunicationChannel is SerialPortCommunication serialCommunication) {
            serialCommunication.DiscardInBuffer();
            serialCommunication.DiscardOutBuffer();
        }
        var request = new ReadRegistersRequest(unitId, startingAddress, registerCount, (byte)function, Endianess);
        if (UsePreamble && Preamble.Length != 0) {
            CommunicationChannel.Write(Preamble);
            if (DelayAfterPreamble != 0) Thread.Sleep(DelayAfterPreamble);
        }
        CommunicationChannel.Write(request.Data);
        var frameData = new byte[request.ResponseSize];
        CommunicationChannel.Read(frameData, 0, 5); // Как минимум можно получить 5 байт
        CheckErrorFrame(frameData);
        var byteCount = frameData[2];
        if (byteCount > (frameData.Length - 5) && byteCount == 0) throw new ModbusException(frameData, "Размер входных данных слишком большой!");
        CommunicationChannel.Read(frameData, 5, request.ResponseSize - 5);
        return new ReadRegistersResponse(request, frameData, Endianess).Data;
    }

    /// <summary>
    /// Записать один регистр
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="data">Данные регистра</param>
    protected void WriteSingleRegister(int unitId, int startingAddress, ArraySegment<byte> data)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        CheckUnitIdentifier(unitId);
        CommunicationChannel.TryProcessRequestAndResponse(() =>
        {
            if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
            var request = new WriteSingleRegisterRequest((byte)unitId, (ushort)startingAddress, data, Endianess);
            if (UsePreamble && Preamble.Length != 0) {
                CommunicationChannel.Write(Preamble);
                if (DelayAfterPreamble != 0) Thread.Sleep(DelayAfterPreamble);
            }
            CommunicationChannel.Write(request.Data);
            var frameData = new byte[request.ResponseSize];
            CommunicationChannel.Read(frameData, 0, 5); // Как минимум можно получить 5 байт
            //CheckFrameAddress(unitId, frameData);
            CheckErrorFrame(frameData);
            CommunicationChannel.Read(frameData, 5, request.ResponseSize - 5);
            var response = new WriteSingleRegisterResponse(request, frameData, Endianess);
            return true;
        });
    }

    /// <summary>
    /// Записать множество регистров
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистров</param>
    /// <param name="data">Данные регистров</param>
    protected void WriteMultipleRegisters(int unitId, int startingAddress, ArraySegment<byte> data)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        var registerCount = data.Count / 2;
        CheckUnitIdentifier(unitId);
        CheckWriteRegistersCount(registerCount);
        CommunicationChannel.TryProcessRequestAndResponse(() =>
        {
            if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
            if (CommunicationChannel is SerialPortCommunication serialCommunication) {
                serialCommunication.DiscardInBuffer();
                serialCommunication.DiscardOutBuffer();
            }
            var request = new WriteMultipleRegistersRequest((byte)unitId, (ushort)startingAddress, (ushort)registerCount, data, Endianess);
            if (UsePreamble && Preamble.Length != 0) {
                CommunicationChannel.Write(Preamble);
                if (DelayAfterPreamble != 0) Thread.Sleep(DelayAfterPreamble);
            }
            CommunicationChannel.Write(request.Data);
            var frameData = new byte[request.ResponseSize];
            CommunicationChannel.Read(frameData, 0, 5); // Как минимум можно получить 5 байт
            //CheckFrameAddress(unitId, frameData);
            CheckErrorFrame(frameData);
            CommunicationChannel.Read(frameData, 5, request.ResponseSize - 5);
            var response = new WriteMultipleRegistersResponse(request, frameData, Endianess);
            return true;
        });
    }

    #endregion

    #region Асинхронные методы

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected Task<ModbusBaseResponse> ExecuteRequestAsync(ModbusBaseRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        CheckUnitIdentifier(request.Address);
        return CommunicationChannel.TryProcessRequestAndResponseAsync<ModbusBaseResponse>(async() =>
        {
            if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
            if (CommunicationChannel.IsOpen is false) CommunicationChannel.Connect();
            if (CommunicationChannel.BytesToWrite > 0) {
                await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
                CommunicationChannel.DiscardOutBuffer();
                await Task.Delay(CommunicationChannel.RequestDelay).ConfigureAwait(false);
            }
            //if (CommunicationChannel.BytesToRead > 0) {
            CommunicationChannel.DiscardInBuffer();
            //}
            if (UsePreamble && Preamble.Length != 0) {
                await CommunicationChannel.WriteAsync(Preamble, cancellationToken).ConfigureAwait(false);
                if (DelayAfterPreamble != 0) await Task.Delay(DelayAfterPreamble, cancellationToken).ConfigureAwait(false);
            }
            await CommunicationChannel.WriteAsync(request.Data, cancellationToken).ConfigureAwait(false);
            var frameData = new byte[4];
            var readCount = await CommunicationChannel.ReadAsync(frameData, cancellationToken).ConfigureAwait(false);
            CheckErrorFrame(frameData);
            var byteCount = frameData[2];
            if (byteCount > readCount) {
                Array.Resize(ref frameData, frameData.Length + byteCount - 1);
                await CommunicationChannel.ReadAsync(new ArraySegment<byte>(frameData, 4, byteCount - 1), cancellationToken).ConfigureAwait(false);
            }
            return new ReportSlaveIdResponse(request, frameData, Endianess);
        });
    }

    /// <summary>
    /// Прочитать регистры хранения асинхронно
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="count"></param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Задача по ожиданию массива сырых данных (байты)</returns>
    protected Task<ArraySegment<byte>> ReadHoldingRegistersAsync(int unitId, int startingAddress, int count, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        var registerCount = count / 2;
        CheckUnitIdentifier(unitId);
        CheckReadRegistersCount(registerCount);
        return CommunicationChannel.TryProcessRequestAndResponseAsync(
            () => ReadRegistersAsync((byte)unitId, (ushort)startingAddress, (ushort)registerCount, ModbusFunction.ReadHoldingRegisters, cancellationToken));
    }

    /// <summary>
    /// Прочитать регистры ввода асинхронно
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="count"></param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Задача по ожиданию массива сырых данных (байты)</returns>
    protected Task<ArraySegment<byte>> ReadInputRegistersAsync(int unitId, int startingAddress, int count, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        var registerCount = count / 2;
        CheckUnitIdentifier(unitId);
        CheckReadRegistersCount(registerCount);
        return CommunicationChannel.TryProcessRequestAndResponseAsync(
            () => ReadRegistersAsync((byte)unitId, (ushort)startingAddress, (ushort)registerCount, ModbusFunction.ReadInputRegisters, cancellationToken));
    }

    /// <summary>
    /// Прочитать регистры асинхронно
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="registerCount">Количество регистров</param>
    /// <param name="function">Номер функции чтения</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Данные регистров в сыром виде</returns>
    /// <exception cref="ModbusException"></exception>
    private async Task<ArraySegment<byte>> ReadRegistersAsync(byte unitId, ushort startingAddress, ushort registerCount, ModbusFunction function, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        if (CommunicationChannel.IsOpen is false) CommunicationChannel.Connect();
        if (CommunicationChannel.BytesToWrite > 0) {
            await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
            CommunicationChannel.DiscardOutBuffer();
        }
        /*if (CommunicationChannel.BytesToRead > 0)*/ CommunicationChannel.DiscardInBuffer();
        var request = new ReadRegistersRequest(unitId, startingAddress, registerCount, (byte)function, Endianess);
        if (UsePreamble && Preamble.Length != 0) {
            await CommunicationChannel.WriteAsync(Preamble, cancellationToken).ConfigureAwait(false);
            if (DelayAfterPreamble != 0) await Task.Delay(DelayAfterPreamble, cancellationToken).ConfigureAwait(false);
        }
        await CommunicationChannel.WriteAsync(request.Data, cancellationToken).ConfigureAwait(false);
        var frameData = new byte[request.ResponseSize];
        await CommunicationChannel.ReadAsync(new ArraySegment<byte>(frameData, 0, 5), cancellationToken).ConfigureAwait(false); // Как минимум можно получить 5 байт
        CheckErrorFrame(frameData);
        var byteCount = frameData[2];
        if (byteCount > (frameData.Length - 5)) {
            throw new ModbusException(frameData, "Размер входных данных слишком большой!");
        }
        await CommunicationChannel.ReadAsync(new ArraySegment<byte>(frameData, 5, request.ResponseSize - 5), cancellationToken).ConfigureAwait(false);
        return new ReadRegistersResponse(request, frameData, Endianess).Data;
    }

    /// <summary>
    /// Записать один регистр асинхронно
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="data">Данные регистра</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns></returns>
    protected Task WriteSingleRegisterAsync(int unitId, ushort startingAddress, ArraySegment<byte> data, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        CheckUnitIdentifier(unitId);
        return CommunicationChannel.TryProcessRequestAndResponseAsync(async () =>
        {
            if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
            if (CommunicationChannel.IsOpen is false) CommunicationChannel.Connect();
            if (CommunicationChannel.BytesToWrite > 0) {
                await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
                CommunicationChannel.DiscardOutBuffer();
            }
            /*if (CommunicationChannel.BytesToRead > 0)*/
            CommunicationChannel.DiscardInBuffer();
            var request = new WriteSingleRegisterRequest((byte)unitId, startingAddress, data, Endianess);
            if (UsePreamble && Preamble.Length != 0) {
                await CommunicationChannel.WriteAsync(Preamble, cancellationToken).ConfigureAwait(false);
                if (DelayAfterPreamble != 0) await Task.Delay(DelayAfterPreamble, cancellationToken).ConfigureAwait(false);
            }
            var frameData = new byte[request.ResponseSize];
            await CommunicationChannel.WriteAsync(request.Data, cancellationToken).ConfigureAwait(false);
            await CommunicationChannel.ReadAsync(new ArraySegment<byte>(frameData, 0, 5), cancellationToken).ConfigureAwait(false); // Как минимум можно получить 5 байт
            CheckErrorFrame(frameData);
            await CommunicationChannel.ReadAsync(new ArraySegment<byte>(frameData, 5, request.ResponseSize - 5), cancellationToken).ConfigureAwait(false);
            var response = new WriteSingleRegisterResponse(request, frameData, Endianess);
            return true;
        });
    }

    /// <summary>
    /// Записать множество регистров асинхронно
    /// </summary>
    /// <param name="unitId">Идентификатор устройства (его Адрес в сети Modbus)</param>
    /// <param name="startingAddress">Начальный адрес регистра</param>
    /// <param name="data">Данные регистров</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns></returns>
    protected Task WriteMultipleRegistersAsync(int unitId, ushort startingAddress, ArraySegment<byte> data, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
        var registerCount = data.Count / 2;
        CheckUnitIdentifier(unitId);
        CheckWriteRegistersCount(registerCount);
        return CommunicationChannel.TryProcessRequestAndResponseAsync(async() =>
        {
            if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для Modbus клиента!");
            if (CommunicationChannel.IsOpen is false) CommunicationChannel.Connect();
            if (CommunicationChannel.BytesToWrite > 0) {
                await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
                CommunicationChannel.DiscardOutBuffer();
            }
            /*if (CommunicationChannel.BytesToRead > 0)*/
            CommunicationChannel.DiscardInBuffer();
            var request = new WriteMultipleRegistersRequest((byte)unitId, startingAddress, (ushort)registerCount, data, Endianess);
            if (UsePreamble && Preamble.Length != 0) {
                await CommunicationChannel.WriteAsync(Preamble, cancellationToken).ConfigureAwait(false);
                if (DelayAfterPreamble != 0) await Task.Delay(DelayAfterPreamble, cancellationToken).ConfigureAwait(false);
            }
            await CommunicationChannel.WriteAsync(request.Data, cancellationToken).ConfigureAwait(false);
            var frameData = new byte[request.ResponseSize];
            await CommunicationChannel.ReadAsync(new ArraySegment<byte>(frameData, 0, 5), cancellationToken).ConfigureAwait(false); // Как минимум можно получить 5 байт
            CheckErrorFrame(frameData);
            await CommunicationChannel.ReadAsync(new ArraySegment<byte>(frameData, 5, request.ResponseSize - 5), cancellationToken).ConfigureAwait(false);
            var response = new WriteMultipleRegistersResponse(request, frameData, Endianess);
            return true;
        });
    }

    #endregion

    // TODO: По хорошему надо считывать до тех пор пока не обнаружим нужный адрес и проверить следующий кусок данных,
    // тогда не надо будет очищать входной буфер каждый раз перед запросами
    /// <summary>
    /// Проверка адреса посылки
    /// </summary>
    /// <param name="frameData">Данные кадра</param>
    /// <returns>Совпадает ли текущий адрес с полученным</returns>
    private static bool CheckFrameAddress(int unitId, IList<byte> frameData) => frameData[0] == unitId;

    /// <summary>
    /// Проверка являются ли входные данные ответом об ошибке
    /// </summary>
    /// <param name="frameData">Данные принятого кадра</param>
    /// <exception cref="ModbusException"></exception>
    private static void CheckErrorFrame(byte[] frameData)
    {
        if ((frameData[1] & 0x80) > 0) {
            throw new ModbusException(frameData, (ModbusExceptionCode)frameData[2], $"Код операции: 0x{frameData[1]:X2}"); // Если возникло исклчение на стороне сервера
        }
    }

    /// <summary>
    /// Проверка адреса на соответствие диапазону
    /// </summary>
    /// <param name="unitId">Адрес устройства</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void CheckUnitIdentifier(int unitId)
    {
        if (unitId > 0xFF) throw new ArgumentOutOfRangeException(nameof(unitId), $"Неверный адрес устройства! Текущее значение: {unitId}");
    }

    /// <summary>
    /// Проверка количества входных регистров на соответствие диапазону
    /// </summary>
    /// <param name="registerCount">Количество регистров</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void CheckReadRegistersCount(int registerCount)
    {
        if (registerCount is not (> 0 and <= 125))
            throw new ArgumentOutOfRangeException(nameof(registerCount), $"Неверное количество регистров для команды (необходимо от 1 до 125)! Текущее значение: {registerCount}");
    }

    /// <summary>
    /// Проверка количества записываемых регистров на соответствие диапазону
    /// </summary>
    /// <param name="registerCount">Количество регистров</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void CheckWriteRegistersCount(int registerCount)
    {
        if (registerCount is not (> 0 and <= 123))
            throw new ArgumentOutOfRangeException(nameof(registerCount), $"Неверное количество регистров для команды (необходимо от 1 до 123)! Текущее значение: {registerCount}");
    }
}