using DeviceCommunication.Communication;
using NLog;

namespace DeviceCommunication.Devices;

/// <summary>
/// Устройство пустышка
/// </summary>
public class EmptyDevice : IDevice
{
    public Guid Id { get; set; }
    public DeviceCommunicationProtocol CurrentProtocol { get; set; }
    public ILogger Logger { get; set; }

    public T? GetParameterValue<T>(Enum parameterx) => throw new NotImplementedException();

    public Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public bool SetParameterValue<T>(Enum parameter, T value) where T : notnull => throw new NotImplementedException();

    public Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default) where T : notnull => throw new NotImplementedException();
}