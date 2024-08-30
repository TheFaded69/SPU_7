using NLog;
using SPU_7.CommonDevice.Utils;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.PS_UNI;

public class PS_UNI : ModbusDevice, IModbusDevice, IPressureSensor
{
    public PS_UNI(ICommunicationChannel? communicationChannel = null)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<PS_UNI_RegisterMap>(), DeviceEndianess.CDAB)
    {
        Logger = LogManager.GetLogger(nameof(PS_UNI));
        ModbusProtocol.UsePreamble = true; // По умолчанию необходимо посылать преамбулу
        ModbusProtocol.DelayAfterPreamble = 10; // Задерка после отсылки преамбулы
        ModbusProtocol.Preamble = new byte[] { 0xFF }; // Данные для преамбулы
    }
    
    #region Основные параметры

    /// <summary>
    /// Измеренное давление в Па
    /// </summary>
    public float? GetPressure(PressureType pressureType = PressureType.DefaultPressure) =>
        GetParameterValue<float?>(pressureType switch
        {
            PressureType.DefaultPressure => PS_UNI_RegisterMap.OutputPressure,
            PressureType.AbsolutePressure => PS_UNI_RegisterMap.OutputPressure,
            _ => throw new IndexOutOfRangeException(),
        });

    /// <summary>
    /// Измеренное давление в Па асинхронно
    /// </summary>
    public Task<float?> ReadPressureAsync(PressureType pressureType = PressureType.DefaultPressure, CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(pressureType switch
        {
            PressureType.DefaultPressure => PS_UNI_RegisterMap.OutputPressure,
            PressureType.AbsolutePressure => PS_UNI_RegisterMap.OutputPressure,
            _ => throw new IndexOutOfRangeException(),
        }, cancellationToken);

    #endregion

    #region Пароль

    #region Синхронные методы

    /// <summary>
    /// Получить заводской номер
    /// </summary>
    public ulong? GetPassportVendorNumber() => GetParameterValue<ulong?>(PS_UNI_RegisterMap.PassportVendorNumber);

    /// <summary>
    /// Получить номер для запроса одноразового пароля
    /// </summary>
    public uint? GetOneShotPasswordNumber() => GetParameterValue<uint?>(PS_UNI_RegisterMap.OneShotPasswordNumber);

    /// <summary>
    /// Ввести пароль
    /// </summary>
    /// <param name="hash">хэш пароля</param>
    public bool EnterPassword(uint hash) => SetParameterValue(PS_UNI_RegisterMap.PasswordAccess, hash);

    /// <summary>
    /// Ввести мастер пароль
    /// </summary>
    protected bool EnterMasterPassword()
    {
        var vendorNumber = GetPassportVendorNumber();
        if (vendorNumber == null) return false;
        var oneShotPasswordNumber = GetOneShotPasswordNumber();
        if (oneShotPasswordNumber == null) return false;
        var masterPasswordHash = MasterPasswordGenerator.Get((uint)vendorNumber, (uint)oneShotPasswordNumber);
        return EnterPassword(masterPasswordHash);
    }

    #endregion

    #region Асинхронные методы

    /// <summary>
    /// Получить заводской номер асинхронно
    /// </summary>
    public Task<ulong?> GetPassportVendorNumberAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<ulong?>(PS_UNI_RegisterMap.PassportVendorNumber, cancellationToken);

    /// <summary>
    /// Получить номер для запроса одноразового пароля асинхронно
    /// </summary>
    public Task<uint?> GetOneShotPasswordNumberAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<uint?>(PS_UNI_RegisterMap.OneShotPasswordNumber, cancellationToken);

    /// <summary>
    /// Ввести пароль асинхронно
    /// </summary>
    /// <param name="hash">хэш пароля</param>
    public Task<bool> EnterPasswordAsync(uint hash, CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(PS_UNI_RegisterMap.PasswordAccess, hash, cancellationToken);

    /// <summary>
    /// Ввести мастер пароль асинхронно
    /// </summary>
    protected async Task<bool> EnterMasterPasswordAsync(CancellationToken cancellationToken = default)
    {
        // генерируем хэш мастера-пароля
        var vendorNumber = await GetPassportVendorNumberAsync(cancellationToken).ConfigureAwait(false);
        if (vendorNumber == null) return false;
        var oneShotPasswordNumber = await GetOneShotPasswordNumberAsync(cancellationToken).ConfigureAwait(false);
        if (oneShotPasswordNumber == null) return false;
        var masterPasswordHash = MasterPasswordGenerator.Get((uint)vendorNumber, (uint)oneShotPasswordNumber);
        // вводим
        return await EnterPasswordAsync(masterPasswordHash, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #endregion
}