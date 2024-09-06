using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.ViewModels.Settings;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public class ValidationPointConfigurationViewModel : ViewModelBase
{
    private readonly IStandController _standController;
    private readonly IDialogService _dialogService;
    private readonly IStandSettingsService _standSettingsService;

    public ValidationPointConfigurationViewModel(IStandController standController,
        IDialogService dialogService,
        IStandSettingsService standSettingsService)
    {
        _standController = standController;
        _dialogService = dialogService;
        _standSettingsService = standSettingsService;

        LineNumbers = [];
        foreach (var line in _standSettingsService.StandSettingsModel.LineViewModels)
        {
            LineNumbers.Add(line.LineNumber);
        }

        ShowNozzleSelectorCommand = new DelegateCommand(ShowNozzleSelectorCommandHandler);
    }

    private int _number;
    private bool _isValveUse;
    private int? _delay;
    private double? _targetFlow;
    private int? _measureCount;
    private double? _targetVolume;
    private double? _inaccuracy;
    private int _selectedLineNumber;
    private string _selectedMasterDeviceName;
    private ObservableCollection<StandSettingsNozzleViewModel> _selectedNozzles;
    private bool _isNeedleValveUse;
    private int? _needleValveValue;
    private bool _isReplaceNozzle;
    private bool _isMasterDeviceEnable;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public bool IsValveUse
    {
        get => _isValveUse;
        set => SetProperty(ref _isValveUse, value);
    }

    public int? Delay
    {
        get => _delay;
        set => SetProperty(ref _delay, value);
    }

    public double? TargetFlow
    {
        get => _targetFlow;
        set { SetProperty(ref _targetFlow, value); }
    }

    public int? MeasureCount
    {
        get => _measureCount;
        set => SetProperty(ref _measureCount, value);
    }

    public double? TargetVolume
    {
        get => _targetVolume;
        set => SetProperty(ref _targetVolume, value);
    }

    public double? Inaccuracy
    {
        get => _inaccuracy;
        set => SetProperty(ref _inaccuracy, value);
    }

    public ObservableCollection<int> LineNumbers { get; set; }

    public int SelectedLineNumber
    {
        get => _selectedLineNumber;
        set
        {
            SetProperty(ref _selectedLineNumber, value);

            if (value == 0) return;

            MasterDeviceNames.Clear();

            if (_standSettingsService.StandSettingsModel.LineViewModels[value - 1]
                    .MasterDeviceViewModels.Count > 0)
            {
                IsMasterDeviceEnable = true;
                foreach (var masterDeviceModel in _standSettingsService.StandSettingsModel.LineViewModels[value - 1]
                             .MasterDeviceViewModels)
                {
                    MasterDeviceNames.Add(masterDeviceModel.MasterDeviceName);
                }
            }
            else
            {
                IsMasterDeviceEnable = false;
            }
        }
    }

    public bool IsNeedleValveUse
    {
        get => _isNeedleValveUse;
        set => SetProperty(ref _isNeedleValveUse, value);
    }

    public int? NeedleValveValue
    {
        get => _needleValveValue;
        set => SetProperty(ref _needleValveValue, value);
    }

    public bool IsMasterDeviceEnable
    {
        get => _isMasterDeviceEnable;
        set => SetProperty(ref _isMasterDeviceEnable, value);
    }

    public ObservableCollection<string> MasterDeviceNames { get; set; } = [];

    public string SelectedMasterDeviceName
    {
        get => _selectedMasterDeviceName;
        set => SetProperty(ref _selectedMasterDeviceName, value);
    }

    public ObservableCollection<StandSettingsNozzleViewModel> SelectedNozzles
    {
        get => _selectedNozzles;
        set => SetProperty(ref _selectedNozzles, value);
    }

    public bool IsReplaceNozzle
    {
        get => _isReplaceNozzle;
        set => SetProperty(ref _isReplaceNozzle, value);
    }


    public DelegateCommand ShowNozzleSelectorCommand { get; }

    private void ShowNozzleSelectorCommandHandler()
    {
        NozzleSelectorViewModel.Show(_dialogService, SelectedNozzles, UpdateSelectedNozzles, null);
    }

    private void UpdateSelectedNozzles(ObservableCollection<StandSettingsNozzleViewModel> selectedNozzles)
    {
        SelectedNozzles = selectedNozzles;

        TargetFlow = 0;

        foreach (var standSettingsNozzleViewModel in SelectedNozzles)
        {
            TargetFlow += standSettingsNozzleViewModel.NozzleFactValue;
        }
    }
}