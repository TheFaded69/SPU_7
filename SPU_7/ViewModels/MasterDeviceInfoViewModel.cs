using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using Prism.Commands;
using Prism.Services.Dialogs;
using SkiaSharp;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.Views;

namespace SPU_7.ViewModels;

public class MasterDeviceInfoViewModel : ViewModelBase, IDialogAware
{
    public MasterDeviceInfoViewModel()
    {
        Title = "Мастер-устройство";
        
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
        ChartUpdatedCommand = new DelegateCommand<ChartCommandArgs>(ChartUpdated);
        PointerUpCommand = new DelegateCommand<PointerCommandArgs>(PointerUp);
        PointerDownCommand = new DelegateCommand<PointerCommandArgs>(PointerDown);
        PointerMoveCommand = new DelegateCommand<PointerCommandArgs>(PointerMove);
        
        Series = new ISeries[]
        {
            new ColumnSeries<ObservablePoint>
            {
                Values = _values,
                Padding = 0,
                MaxBarWidth = double.PositiveInfinity,
                DataPadding = new LvcPoint(0, 1)
            }
        };

        ScrollbarSeries = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Values = _values,
                GeometryStroke = null,
                GeometryFill = null,
                DataPadding = new LvcPoint(0, 1)
            }
        };

        ScrollableAxes = new[] { new Axis() };

        Thumbs = new[]
        {
            new RectangularSection
            {
                Fill = new SolidColorPaint(new SKColor(255, 205, 210, 100))
            }
        };

        InvisibleX = new[] { new Axis { IsVisible = false } };
        InvisibleY = new[] { new Axis { IsVisible = false } };

        _ = AddData();
    }
    private bool _isAdding = true;
    private object _sync = new();
    
    private async Task AddData()
    {
        while (_isAdding)
        {
            await Task.Delay(1000);

            lock (_sync)
            {
                _values.Add(new ObservablePoint(_values.Count, new Random().Next(100, 200)));
            }
        }
    }
    
    private bool _isDown = false;
    private readonly ObservableCollection<ObservablePoint> _values = new();
    
    public ISeries[] Series { get; set; }
    public Axis[] ScrollableAxes { get; set; }
    public ISeries[] ScrollbarSeries { get; set; }
    public Axis[] InvisibleX { get; set; }
    public Axis[] InvisibleY { get; set; }
    public RectangularSection[] Thumbs { get; set; }

    public DelegateCommand<ChartCommandArgs> ChartUpdatedCommand { get; }
    public void ChartUpdated(ChartCommandArgs args)
    {
        var cartesianChart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;

        var x = cartesianChart.XAxes.First();

        // update the scroll bar thumb when the chart is updated (zoom/pan)
        // this will let the user know the current visible range
        var thumb = Thumbs[0];

        thumb.Xi = x.MinLimit;
        thumb.Xj = x.MaxLimit;
    }

    public DelegateCommand<PointerCommandArgs> PointerDownCommand { get; }
    public void PointerDown(PointerCommandArgs args)
    {
        _isDown = true;
    }

    public DelegateCommand<PointerCommandArgs> PointerMoveCommand { get; }
    public void PointerMove(PointerCommandArgs args)
    {
        if (!_isDown) return;

        var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
        var positionInData = chart.ScalePixelsToData(args.PointerPosition);

        var thumb = Thumbs[0];
        var currentRange = thumb.Xj - thumb.Xi;

        // update the scroll bar thumb when the user is dragging the chart
        thumb.Xi = positionInData.X - currentRange / 2;
        thumb.Xj = positionInData.X + currentRange / 2;

        // update the chart visible range
        ScrollableAxes[0].MinLimit = thumb.Xi;
        ScrollableAxes[0].MaxLimit = thumb.Xj;
    }

    public DelegateCommand<PointerCommandArgs> PointerUpCommand { get; }
    public void PointerUp(PointerCommandArgs args)
    {
        _isDown = false;
    }
    
    public DelegateCommand CloseWindowCommand { get; }
    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
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
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, StandSettingsMasterDeviceModel standSettingsMasterDeviceModel, Action positive, Action negative)
    {
        dialogService.ShowDialog(nameof(MasterDeviceInfoView),
            new DialogParameters() { { "StandSettingsMasterDeviceModel", standSettingsMasterDeviceModel } },
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