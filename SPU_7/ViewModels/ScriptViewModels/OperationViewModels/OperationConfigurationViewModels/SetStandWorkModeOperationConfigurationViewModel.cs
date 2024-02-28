using SPU_7.Common.Device;
using SPU_7.Common.Extensions;
using SPU_7.Models.Scripts.Operations.Configurations;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public class SetStandWorkModeOperationConfigurationViewModel : ViewModelBase, IOperationConfiguration
{
    public SetStandWorkModeOperationConfigurationViewModel(BaseOperationConfigurationModel configurationModel)
    {
        if (configurationModel is SetStandWorkModeOperationConfigurationModel workModeOperationConfigurationModel)
        {
            
        }
    }
    public BaseOperationConfigurationModel CreateConfiguration(DeviceType deviceType) =>
        new SetStandWorkModeOperationConfigurationModel()
        {
            DeviceType = deviceType,
            GroupType = deviceType.GetDeviceGroupType()
        };
}