using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LiveChartsCore;

namespace SPU_7.Views;

public partial class MasterDeviceInfoView : UserControl
{
    public MasterDeviceInfoView()
    {
        InitializeComponent();
    }
    
    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var window = ((UserControl)sender).Parent as Window;

        if (window != null)
        {
            window.ExtendClientAreaToDecorationsHint = true;
            window.CanResize = false;
        }
    }
}