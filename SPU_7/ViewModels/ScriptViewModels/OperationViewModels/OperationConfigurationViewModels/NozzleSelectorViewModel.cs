using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.StandSetting;
using SPU_7.ViewModels.Settings;
using SPU_7.Views.ScriptViews.OperationViews.OperationConfigurationViews;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public class NozzleSelectorViewModel : ViewModelBase, IDialogAware
{
    public NozzleSelectorViewModel(IStandSettingsService standSettingsService, IMapper mapper)
    {
        _standSettingsService = standSettingsService;
        _mapper = mapper;

        StandSettingsNozzleViewModels = _mapper.Map<ObservableCollection<StandSettingsNozzleViewModel>>(_standSettingsService.StandSettingsModel.NozzleViewModels);
        
        ChooseNozzlesCommand = new DelegateCommand(ChooseNozzlesCommandHandler);
    }
    private readonly IStandSettingsService _standSettingsService;
    private readonly IMapper _mapper;

    public ObservableCollection<StandSettingsNozzleViewModel> StandSettingsNozzleViewModels { get; set; }
    public ObservableCollection<StandSettingsNozzleViewModel> SelectedStandSettingsNozzleViewModels { get; set; }

    
    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        var selectedStandSettingsNozzleViewModels = parameters.GetValue<ObservableCollection<StandSettingsNozzleViewModel>>("SelectedNozzles");
        SelectedStandSettingsNozzleViewModels = selectedStandSettingsNozzleViewModels;
        
        foreach (var selectedStandSettingsNozzleViewModel in selectedStandSettingsNozzleViewModels)
        {
            StandSettingsNozzleViewModels
                .FirstOrDefault(ss => ss.Number == selectedStandSettingsNozzleViewModel.Number).IsChecked = true;
        }
    }

    public DelegateCommand ChooseNozzlesCommand { get; }

    private void ChooseNozzlesCommandHandler()
    {
        SelectedStandSettingsNozzleViewModels.Clear();
        
        foreach (var standSettingsNozzleViewModel in StandSettingsNozzleViewModels.Where(ss => ss.IsChecked))
        {
            SelectedStandSettingsNozzleViewModels.Add(standSettingsNozzleViewModel);
        }
        
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters(){{"SelectedStandSettingsNozzleViewModels", SelectedStandSettingsNozzleViewModels}}));
    }
    
    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, 
        ObservableCollection<StandSettingsNozzleViewModel> selectedNozzles,
        Action<ObservableCollection<StandSettingsNozzleViewModel>> positiveAction,
        Action negativeAction)
    {
        dialogService.Show(nameof(NozzleSelectorView),
            new DialogParameters(){{"SelectedNozzles", selectedNozzles}},
            result =>
            {
                switch (result.Result)
                {
                    case ButtonResult.Abort:
                        break;
                    case ButtonResult.Cancel:
                        break;
                    case ButtonResult.Ignore:
                        break;
                    case ButtonResult.No:
                        break;
                    case ButtonResult.None:
                        break;
                    case ButtonResult.OK:
                        positiveAction?.Invoke(selectedNozzles);
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