using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Stand;
using SPU_7.Extensions;
using SPU_7.Models.Scripts;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

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

            NozzleItems = new ObservableCollection<NozzleItemViewModel>();
            foreach (var nozzleModel in settingsService.StandSettingsModel.NozzleViewModels)
            {
                NozzleItems.Add(new NozzleItemViewModel(notificationService, nozzleModel, _standController)
                {
                    NozzleNumber = NozzleItems.Count + 1
                });
            }

            ValveItems = new ObservableCollection<ValveItemViewModel>();
            foreach (var valveModel in settingsService.StandSettingsModel.ValveViewModels)
            {
                ValveItems.Add(new ValveItemViewModel(valveModel, _standController)
                {
                    ValveNumber = ValveItems.Count + 1
                });
            }
            
            LineViewModels = new ObservableCollection<LineItemViewModel>();
            foreach (var lineViewModel in settingsService.StandSettingsModel.LineViewModels)
            {
                LineViewModels.Add(new LineItemViewModel(notificationService, settingsService, standController, LineViewModels.Count)
                {
                    IsReverseLine = lineViewModel.IsReverseLine,
                    SelectedDeviceLineType = lineViewModel.SelectedDeviceLineType,
                });
            }
            
            SolenoidValveItems = new ObservableCollection<SolenoidValveItemViewModel>();
            foreach (var solenoidValveModel in settingsService.StandSettingsModel.SolenoidValveViewModels)
            {
                SolenoidValveItems.Add(new SolenoidValveItemViewModel(_standController, solenoidValveModel));
            }

            NozzleManualType = settingsService.StandSettingsModel.NozzleManualType;
            
            FirstSize = 80 * (NozzleItems.Count - 1) + 20;

            UsePressureDropSolenoidValveCommand = new DelegateCommand(UsePressureDropSolenoidValveCommandHandler);
            UseTubeSolenoidValveCommand = new DelegateCommand(UseTubeSolenoidValveCommandHandler);
        }


        private readonly IDialogService _dialogService;
        private readonly ILogger _logger;
        private readonly IStandSettingsService _settingsService;
        private readonly IStandController _standController;
        private readonly IScriptController _scriptController;

        private double _firstSize;
        private NozzleManualType _nozzleManualType;
        private bool _isSchemeEnabled = true;

        public bool IsSchemeEnabled
        {
            get => _isSchemeEnabled;
            set => SetProperty(ref _isSchemeEnabled, value);
        }
        
        public ObservableCollection<NozzleItemViewModel> NozzleItems { get; set; }
        
        public ObservableCollection<LineItemViewModel> LineViewModels { get; set; }
        
        public ObservableCollection<ValveItemViewModel> ValveItems { get; set; }

        public ObservableCollection<SolenoidValveItemViewModel> SolenoidValveItems { get; set; }

        public double FirstSize
        {
            get => _firstSize;
            set => SetProperty(ref _firstSize, value);
        }

        public NozzleManualType NozzleManualType
        {
            get => _nozzleManualType;
            set => SetProperty(ref _nozzleManualType, value);
        }
        
        public void Update(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateFromDataPair(DataPair dataPair)
        {
            if (dataPair.Data == null) return;

            switch (dataPair.DataType)
            {
                case DeviceInfoParameterType.NozzleState:
                {
                    if (dataPair.Data is StandInfoData standInfoData)
                    {
                        NozzleItems[standInfoData.Number].StateType = standInfoData.StateType;
                    }
                }
                    break;

                case DeviceInfoParameterType.LineState:
                {
                    if (dataPair.Data is LineInfoData lineInfoData)
                    {
                        if (lineInfoData.LineIndex == null)
                        {
                            foreach (var lineItemViewModel in LineViewModels)
                            {
                                lineItemViewModel.IsLineActive = false;
                                foreach (var deviceItemViewModel in lineItemViewModel.DeviceItemViewModels)
                                {
                                    deviceItemViewModel.Pressure = null;
                                }
                            }
                        }
                        else
                        {
                            foreach (var lineItemViewModel in LineViewModels.Where(lv => lv.LineNumber != (int)lineInfoData.LineIndex + 1))
                            {
                                lineItemViewModel.IsLineActive = false;
                                foreach (var deviceItemViewModel in lineItemViewModel.DeviceItemViewModels)
                                {
                                    deviceItemViewModel.Pressure = null;
                                }
                            }

                            LineViewModels[(int)lineInfoData.LineIndex].IsLineActive = lineInfoData.IsActive;
                            if (!lineInfoData.IsActive) 
                                foreach (var deviceItemViewModel in LineViewModels[(int)lineInfoData.LineIndex].DeviceItemViewModels)
                                {
                                    deviceItemViewModel.Pressure = null;
                                }
                        }
                    }
                    break;
                }
                
                case DeviceInfoParameterType.ReverseValveState:
                {
                    if (dataPair.Data is DirectFlowInfoData directFlowInfoData)
                    {
                        LineViewModels.FirstOrDefault(lw => lw.IsLineActive).LineDirectionFlowState = directFlowInfoData.DirectionFlowState;
                    }
                    break;
                }
                
                case DeviceInfoParameterType.LineValveState:
                {
                    if (dataPair.Data is StandInfoData standInfoData)
                    {
                        LineViewModels.FirstOrDefault(lw => lw.IsLineActive).DeviceItemViewModels[standInfoData.Number].ValveItemViewModel.StateType = standInfoData.StateType;
                    }
                    break;
                }
                
                case DeviceInfoParameterType.ValveState:
                {
                    if (dataPair.Data is StandInfoData standInfoData)
                    {
                        ValveItems[standInfoData.Number].StateType = standInfoData.StateType;
                    }

                    break;
                }

                case DeviceInfoParameterType.SolenoidValveState:
                {
                    if (dataPair.Data is StandInfoData standInfoData)
                    {
                        SolenoidValveItems[standInfoData.Number].StateType = standInfoData.StateType;
                    }
                }
                    break;
            }
        }
        
        public DelegateCommand UsePressureDropSolenoidValveCommand { get; }

        private async void UsePressureDropSolenoidValveCommandHandler()
        {
            var valve = SolenoidValveItems.FirstOrDefault(sol => sol.SolenoidValveType == SolenoidValveType.NormalClose);

            switch (valve.StateType)
            {
                case StateType.Open:
#if DEBUGGUI
                    valve.StateType = StateType.Close;
#else
                    await valve.CloseSolenoidValveAsync();
#endif
                    break;
                case StateType.Close:
#if DEBUGGUI
                    valve.StateType = StateType.Open;
#else
                    await valve.OpenSolenoidValveAsync();
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DelegateCommand UseTubeSolenoidValveCommand { get; }

        private async void UseTubeSolenoidValveCommandHandler()
        {
            var valve = SolenoidValveItems.FirstOrDefault(sol => sol.SolenoidValveType == SolenoidValveType.NormalOpen);

            switch (valve.StateType)
            {
                case StateType.Open:
#if DEBUGGUI
                    valve.StateType = StateType.Close;
#else
                    await valve.CloseSolenoidValveAsync();
#endif
                    break;
                case StateType.Close:
#if DEBUGGUI
                    valve.StateType = StateType.Open;
#else
                    await valve.OpenSolenoidValveAsync();
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        
    }
}