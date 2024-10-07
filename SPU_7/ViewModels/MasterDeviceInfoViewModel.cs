using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using Prism.Commands;
using Prism.Services.Dialogs;
using SkiaSharp;
using SPU_7.Extensions.Interface;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.Views;

namespace SPU_7.ViewModels;

public class MasterDeviceInfoViewModel : ViewModelBase, IDialogAware, IFlowDataObserver
{
    public MasterDeviceInfoViewModel(IStandController standController, IFlowDataService flowDataService)
    {
        Title = "Мастер-устройство";

        _standController = standController;
        _flowDataService = flowDataService;

        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
        AddValueCommand = new DelegateCommand(AddValueCommandHandler);

        _flowValues = new ObservableCollection<double>();
        _frequencyValues = new ObservableCollection<double>();
        Series =
        [
            new LineSeries<double>
            {
                LineSmoothness = 1,
                Name = "Расход",
                Values = _flowValues,
                Stroke = new SolidColorPaint(s_blue, 2),
                GeometrySize = 10,
                GeometryStroke = new SolidColorPaint(s_blue, 2),
                Fill = null,
                ScalesYAt = 0 // it will be scaled at the YAxis[0] instance 
            },
            new LineSeries<double>
            {
                Name = "Частота",
                Values = _frequencyValues,
                Stroke = new SolidColorPaint(s_red, 2),
                GeometrySize = 10,
                GeometryStroke = new SolidColorPaint(s_red, 2),
                Fill = null,
                ScalesYAt = 1 // it will be scaled at the YAxis[1] instance 
            }
        ];

        YAxes =
        [
            new Axis
            {
                Name = "Расход, м³/ч",
                NameTextSize = 14,
                NamePaint = new SolidColorPaint(s_blue),
                NamePadding = new Padding(0, 20),
                Padding = new Padding(0, 0, 20, 0),
                TextSize = 12,
                LabelsPaint = new SolidColorPaint(s_blue),
                TicksPaint = new SolidColorPaint(s_blue),
                SubticksPaint = new SolidColorPaint(s_blue),
                DrawTicksPath = true
            },
            new Axis
            {
                Name = "Частота, гц",
                NameTextSize = 14,
                NamePaint = new SolidColorPaint(s_red),
                NamePadding = new Padding(0, 20),
                Padding = new Padding(20, 0, 0, 0),
                TextSize = 12,
                LabelsPaint = new SolidColorPaint(s_red),
                TicksPaint = new SolidColorPaint(s_red),
                SubticksPaint = new SolidColorPaint(s_red),
                DrawTicksPath = true,
                ShowSeparatorLines = false,
                Position = LiveChartsCore.Measure.AxisPosition.End
            }
        ];

        VerticalLines = [new RectangularSection
        {
            Yi = 0,
            Yj = 0,
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Green,
                StrokeThickness = 3,
                PathEffect = new DashEffect(new float[]{6,6})
            }
        },];
    }

    private readonly IStandController _standController;
    private readonly IFlowDataService _flowDataService;

    private static readonly SKColor s_blue = new(25, 118, 210);
    private static readonly SKColor s_red = new(229, 57, 53);
    
    private readonly ObservableCollection<double> _flowValues;
    private readonly ObservableCollection<double> _frequencyValues;
    
    private double _targetFlow;
    public ISeries[] Series { get; set; } = [];
    public ICartesianAxis[] YAxes { get; set; } = [];

    public RectangularSection[] VerticalLines { get; set; } = [];

    public double TargetFlow
    {
        get => _targetFlow;
        set => SetProperty(ref _targetFlow, value);
    }
    
    public SolidColorPaint LegendTextPaint { get; set; } =
        new()
        {
            Color = new SKColor(50, 50, 50),
            SKTypeface = SKTypeface.FromFamilyName("Courier New")
        };

    public SolidColorPaint LegendBackgroundPaint { get; set; } = new(new SKColor(240, 240, 240));

    public void AcceptFlowData(double currentFlow, double currentFrequency)
    {
        _flowValues.Add(currentFlow);
        _frequencyValues.Add(currentFrequency);
    }


    public DelegateCommand CloseWindowCommand { get; }


    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    public DelegateCommand AddValueCommand { get; }

    private void AddValueCommandHandler()
    {
        _flowValues.Add(new Random().Next(0, 1000));
        _frequencyValues.Add(new Random().Next(0, 50));
    }

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        _flowDataService.UnsubscribeDataObserver(this);
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        TargetFlow = parameters.GetValue<double>("TargetFlow");
        
        VerticalLines[0].Yi = TargetFlow;
        VerticalLines[0].Yj = TargetFlow;

        
        _flowDataService.SubscribeDataObserver(this);
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, StandSettingsMasterDeviceModel standSettingsMasterDeviceModel, Action positive, Action negative, double targetFlow)
    {
        dialogService.ShowDialog(nameof(MasterDeviceInfoView),
            new DialogParameters() { { "StandSettingsMasterDeviceModel", standSettingsMasterDeviceModel }, {"TargetFlow", targetFlow} },
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
                        negative?.Invoke();
                        break;
                    case ButtonResult.None:
                        break;
                    case ButtonResult.OK:
                        positive?.Invoke();
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