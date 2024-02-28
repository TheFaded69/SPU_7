using SPU_7.ViewModels;

namespace SPU_7.Extensions;

/// <summary>
/// Класс для передачи данных и их типа (если это что-то особенное)
/// </summary>
public class DataPair
{
    public DataPair(object data, DeviceInfoParameterType deviceInfoParameterType)
    {
        Data = data;
        DataType = deviceInfoParameterType;
    }
    public object Data { get; set; }
    
    public DeviceInfoParameterType DataType { get; set; } 
}