using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Stand;
using SPU_7.Models.Scripts.Operations.Protocols;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Views.WorkReportViews.PdfViewerView;

namespace SPU_7.ViewModels.WorkReportViewModels;

public class PdfViewerViewModel : ViewModelBase, IDialogAware
{
    private readonly IStandSettingsService _standSettingsService;
    private readonly ILogger _logger;

    public PdfViewerViewModel(IStandSettingsService standSettingsService,
        ILogger logger)
    {
        _standSettingsService = standSettingsService;
        _logger = logger;
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
        SaveProtocolCommand = new DelegateCommand(SaveProtocolCommandHandler);
        PrintProtocolCommand = new DelegateCommand(PrintProtocolCommandHandler);
    }
    private string _currentAddress;

    public ObservableCollection<object> PagesOfProtocol { get; set; } = new();
    public string CurrentAddress
    {
        get => _currentAddress;
        set => SetProperty(ref _currentAddress, value);
    }
    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    public DelegateCommand SaveProtocolCommand { get; }

    private void SaveProtocolCommandHandler()
    {
        
    }
    
    public DelegateCommand PrintProtocolCommand { get; }



    private void PrintProtocolCommandHandler()
    {
    }
    
    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        try
        {
            var result = parameters.GetValue<ValidationOperationResult>("Result");
            var deviceNumber = parameters.GetValue<int>("DeviceNumber");
            var temperature = parameters.GetValue<float?>("Temperature");
            var humidity = parameters.GetValue<float?>("Humidity");
            var pressure = parameters.GetValue<float?>("Pressure");
        
            if (result == null || deviceNumber == null) return;

            var protocolCreator = new ValidationProtocolCreator();
            CurrentAddress = protocolCreator.CreateTempProtocol(result, deviceNumber, _standSettingsService, temperature, pressure, humidity);
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            if (e.InnerException != null) _logger.Logging(new LogMessage(e.InnerException.Message, LogLevel.Error));
        }
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, 
        ValidationOperationResult result, 
        int deviceNumber, 
        float? temperature = null,
        float? humidity = null, 
        float? pressure = null)
    {
        dialogService.ShowDialog(nameof(PdfViewerView),
            new DialogParameters() { 
                { "Result", result },
                {"DeviceNumber", deviceNumber},
                {"Temperature", temperature},
                {"Humidity", humidity},
                {"Pressure", pressure}
            },
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