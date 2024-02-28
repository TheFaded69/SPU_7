using System.Collections.Generic;
using SPU_7.Common.Device;

namespace SPU_7.Models.Scripts;

public class ScriptModel
{
    public ScriptModel()
    {
        OperationModels = new List<OperationModel>();
    }
    
    public ScriptModel(List<OperationModel> operationModels, string scriptName)
    {
        OperationModels = operationModels;
        ScriptName = scriptName;
    }
    
    /// <summary>
    /// Список операций в скрипте
    /// </summary>
    public List<OperationModel> OperationModels { get; set; }

    /// <summary>
    /// Имя скрипта
    /// </summary>
    public string ScriptName { get; set; }
    
    /// <summary>
    /// Описание сценария
    /// </summary>
    public string Description { get; set; }
    
    public DeviceType DeviceType { get; set; }
    
    public DeviceGroupType DeviceGroupType { get; set; }
    
    /// <summary>
    /// Номер выбранной линии для работы в сценарии
    /// </summary>
    public int LineNumber { get; set; }
}