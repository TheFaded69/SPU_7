using System.Collections.ObjectModel;
using System.Linq;
using Prism.Services.Dialogs;
using SPU_7.Extensions;
using SPU_7.Models.Scripts;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels
{
    public class MnemonicSchemeViewModel : ViewModelBase, IStandObserver
    {
        public MnemonicSchemeViewModel(IDialogService dialogService,
            INotificationService notificationService,
            ILogger logger,
            IStandSettingsService settingsService,
            IStandController standController,
            IScriptController scriptController)
        {
            _dialogService = dialogService;
            _logger = logger;
            _settingsService = settingsService;
            _standController = standController;
            _scriptController = scriptController;


            standController.RegisterObserver(this);

            LineViewModels = [];
            foreach (var lineViewModel in settingsService.StandSettingsModel.LineViewModels)
            {
                LineViewModels.Add(new LineItemViewModel(dialogService, notificationService, settingsService, standController, LineViewModels.Count)
                {
                    IsReverseLine = lineViewModel.IsReverseLine,
                    SelectedDeviceLineType = lineViewModel.SelectedDeviceLineType,
                    SelectedLineType = lineViewModel.SelectedLineType,
                });
            }

            FirstSize = 80;
        }


        private readonly IDialogService _dialogService;
        private readonly ILogger _logger;
        private readonly IStandSettingsService _settingsService;
        private readonly IStandController _standController;
        private readonly IScriptController _scriptController;

        private double _firstSize;
        private bool _isSchemeEnabled = true;

        public bool IsSchemeEnabled
        {
            get => _isSchemeEnabled;
            set => SetProperty(ref _isSchemeEnabled, value);
        }

        public ObservableCollection<LineItemViewModel> LineViewModels { get; set; }


        public double FirstSize
        {
            get => _firstSize;
            set => SetProperty(ref _firstSize, value);
        }


        public void Update(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateFromDataPair(DataPair dataPair)
        {
            if (dataPair.Data is StandInfoData standInfoData)
            {
                switch (dataPair.DataType)
                {
                    case DeviceInfoParameterType.ValveState:
                        var lineItem = LineViewModels.FirstOrDefault(line =>
                            line.DeviceItemViewModels.Any(device => device.ValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel));
                        if (lineItem != null)
                        {
                            lineItem.DeviceItemViewModels.FirstOrDefault(fan => fan.ValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel)
                                .ValveItemViewModel.StateType = standInfoData.StateType;
                            return;
                        }
                        
                        lineItem = LineViewModels.FirstOrDefault(line =>
                            line.MasterDeviceItemViewModels.Any(device => device.ValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel));
                        if (lineItem != null)
                        {
                            lineItem.MasterDeviceItemViewModels.FirstOrDefault(fan => fan.ValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel)
                                .ValveItemViewModel.StateType = standInfoData.StateType;
                            return;
                        }
                        
                        lineItem = LineViewModels.FirstOrDefault(line =>
                            line.MasterDeviceItemViewModels.Any(device => device.PressureValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel));
                        if (lineItem != null)
                        {
                            lineItem.MasterDeviceItemViewModels.FirstOrDefault(fan => fan.PressureValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel)
                                .PressureValveItemViewModel.StateType = standInfoData.StateType;
                            return;
                        }
                        
                        lineItem = LineViewModels.FirstOrDefault(line =>
                            line.FanItemViewModels.Any(device => device.ValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel));
                        if (lineItem != null)
                        {
                            lineItem.FanItemViewModels.FirstOrDefault(fan => fan.ValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel)
                                .ValveItemViewModel.StateType = standInfoData.StateType;
                            return;
                        }
                        
                        lineItem = LineViewModels.FirstOrDefault(line =>
                            line.StartCommonValveViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel);
                        if (lineItem != null)
                        {
                            lineItem.StartCommonValveViewModel.StateType = standInfoData.StateType;
                            return;
                        }
                        
                        lineItem = LineViewModels.FirstOrDefault(line =>
                            line.EndCommonValveItemViewModel.StandSettingsValveModel == standInfoData.StandSettingsValveModel);
                        if (lineItem != null)
                        {
                            lineItem.EndCommonValveItemViewModel.StateType = standInfoData.StateType;
                            return;
                        }


                        break;
                    case DeviceInfoParameterType.FanState:
                    {
                        lineItem = LineViewModels.FirstOrDefault(line =>
                            line.FanItemViewModels.Any(fan => fan.FanIndex == standInfoData.Index));
                        if (lineItem != null)
                        {
                            lineItem.FanItemViewModels.FirstOrDefault(fan => fan.FanIndex == standInfoData.Index).IsFanWorking = standInfoData.StateType == StateType.Work;
                            return;
                        }
                    }
                        break;
                }
            }
        }
    }
}