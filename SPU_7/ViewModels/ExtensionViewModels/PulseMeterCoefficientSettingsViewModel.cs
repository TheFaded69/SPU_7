using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class PulseMeterCoefficientSettingsViewModel : ViewModelBase, IDialogAware
{
    public PulseMeterCoefficientSettingsViewModel(IStandController standController, IStandSettingsService standSettingsService)
    {
        _standController = standController;
        _standSettingsService = standSettingsService;

        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
        ReadCoefficientCommand = new DelegateCommand(ReadCoefficientCommandHandler);
        WriteCoefficientCommand = new DelegateCommand(WriteCoefficientCommandHandler);

        Title = "Коэффициенты БИПЧей";
    }
    private readonly IStandController _standController;
    private readonly IStandSettingsService _standSettingsService;

    public ObservableCollection<PulseMeterCoefficientViewModel> PulseMeterCoefficientViewModels { get; set; } = [];
    
    
    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }
    
    public DelegateCommand ReadCoefficientCommand { get; }

    public async void ReadCoefficientCommandHandler()
    {
        var coefficientList = await _standController.ReadPulseCoefficientsAsync();

        for (var i = 0; i < coefficientList.Count; i++)
        {
            var coefficientTuple = coefficientList[i];

            PulseMeterCoefficientViewModels[i].FirstCoefficientRead = coefficientTuple.Item1;
            PulseMeterCoefficientViewModels[i].SecondCoefficientRead = coefficientTuple.Item2;
        }
    }
    
    public DelegateCommand WriteCoefficientCommand { get; }
    
    public async void WriteCoefficientCommandHandler()
    {
        
    }
    
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        foreach (var pulseMeterViewModel in _standSettingsService.StandSettingsModel.PulseMeterViewModels)
        {
            PulseMeterCoefficientViewModels.Add(new PulseMeterCoefficientViewModel()
            {
                Number = PulseMeterCoefficientViewModels.Count + 1,
                FirstCoefficientWrite = (float?)pulseMeterViewModel.FirstCalibrateCoefficient,
                SecondCoefficientWrite = (float?)pulseMeterViewModel.SecondCalibrateCoefficient,
            });
        }
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, Action positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(PulseMeterCoefficientSettingsView), null, result =>
        {
            switch (result.Result)
            {
                case ButtonResult.Abort:
                    break;
                case ButtonResult.Cancel:
                    negativeAction?.Invoke();
                    break;
                case ButtonResult.Ignore:
                    break;
                case ButtonResult.No:
                    break;
                case ButtonResult.None:
                    break;
                case ButtonResult.OK:
                    positiveAction?.Invoke();
                    break;
                case ButtonResult.Retry:
                    break;
                case ButtonResult.Yes:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }
}