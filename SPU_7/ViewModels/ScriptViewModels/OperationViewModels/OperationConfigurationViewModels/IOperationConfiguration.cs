using SPU_7.Common.Device;
using SPU_7.Models.Scripts.Operations.Configurations;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public interface IOperationConfiguration
{
    public BaseOperationConfigurationModel CreateConfiguration(DeviceType deviceType);
}