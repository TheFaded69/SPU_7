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
            
            LineViewModels = new ObservableCollection<LineItemViewModel>();
            foreach (var lineViewModel in settingsService.StandSettingsModel.LineViewModels)
            {
                LineViewModels.Add(new LineItemViewModel(notificationService, settingsService, standController, LineViewModels.Count)
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
           
        }
    }
}