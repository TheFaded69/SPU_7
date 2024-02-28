using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Stand;
using SPU_7.ViewModels.Settings;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public class PointConfigurationViewModel : ViewModelBase
{
    private readonly IStandController _standController;
    private readonly IDialogService _dialogService;

    public PointConfigurationViewModel(IStandController standController, IDialogService dialogService)
    {
        _standController = standController;
        _dialogService = dialogService;

        ShowNozzleSelectorCommand = new DelegateCommand(ShowNozzleSelectorCommandHandler);
    }
    
    private int _number;
    private bool _isValveUse;
    private int? _delay;
    private double? _targetFlow;
    private int? _measureCount;
    private double? _targetVolume;
    private double? _inaccuracy;
    private ObservableCollection<StandSettingsNozzleViewModel> _selectedNozzles;

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
        set
        {
            SetProperty(ref _targetFlow, value);
        }
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

    public ObservableCollection<StandSettingsNozzleViewModel> SelectedNozzles
    {
        get => _selectedNozzles;
        set => SetProperty(ref _selectedNozzles, value);
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