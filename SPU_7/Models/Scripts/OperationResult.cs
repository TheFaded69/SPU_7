using System.Collections.Generic;
using Avalonia.Media.Transformation;
using SPU_7.Common.Scripts;
using SPU_7.Models.Scripts.Operations.Results;

namespace SPU_7.Models.Scripts;

public class OperationResult
{
    public OperationResult(OperationResultType resultStatus, string message, BaseOperationResult result)
    {
        ResultStatus = resultStatus;
        Message = message;
        Result = result;
    }

    public List<DeviceInformation> Device { get; set; } = new();
    public OperationType OperationType { get; set; }
    public OperationResultType ResultStatus { get; set; }
    public string Message { get; set; }
    public BaseOperationResult Result { get; set; }
}