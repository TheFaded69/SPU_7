using SPU_7.DeviceCommunication.Communication;

namespace SPU_7.CommonDevice.Devices.Modems;

public class BT_AT_Modem : AT_Modem, ICommunicationChannel
{
    public BT_AT_Modem(ISerialPortCommunication? serialPortCommunication) : base(serialPortCommunication)
    {
        ErrorResponse = "FAIL";
    }

    public new string Connection
    {
        get => base.Connection;
        set
        {
            if (IsOpen) throw new InvalidOperationException("Канал связи уже открыт с существующим соединением!");
            base.Connection = value;
        }
    }

    public bool HasRSSI
    {
        get;
        set;
    }

    public int MaxDevicesResponses
    {
        get;
        set;
    }

    /// <summary>
    /// Базовая настройка модуля
    /// </summary>
    /// <param name="role">Роль модема</param>
    /// <param name="deviceClass">Класс устройства</param>
    /// <param name="disconnect">Отсоединить от текущих подключений</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<bool> InitAsync(BT_Role role, uint deviceClass, bool disconnect = false, CancellationToken token = default)
    {
        var result = await TryWriteCommandAsync("AT", token).ConfigureAwait(false);

        if (result && disconnect) {
            await SendCommandAsync("AT+STATE?", token).ConfigureAwait(false);
            await TryWriteCommandAsync("AT+DISC", token).ConfigureAwait(false);
        }

        result = result && await TryWriteCommandAsync($"AT+CLASS={deviceClass:x6}", cancellationToken: token).ConfigureAwait(false)
                        && await TryWriteCommandAsync($"AT+ROLE={(int)role}", cancellationToken: token).ConfigureAwait(false);
        return result;
    }

    public override async Task<bool> ConnectAsync(string connectionName, CancellationToken token = default)
    {
        var result = await TryWriteCommandAsync("AT+INQM=1,9,7", token).ConfigureAwait(false)
                  && await TryWriteCommandAsync($"AT+LINK={connectionName}", token).ConfigureAwait(false);
        if (result) {
            IsOpen = true;
            Connection = connectionName;
        }
        return result;
    }

    public override async Task<bool> CloseAsync(CancellationToken token = default)
    {
        var result = (await SendCommandAsync("AT+DISC", token))?.Contains("+DISC:SUCCESS") is true;
        if (result) {
            IsOpen = false;
        }
        return result;
    }
}
