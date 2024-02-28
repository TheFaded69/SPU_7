using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SPU_7.Views.Settings;

public partial class StandSettingsView : UserControl
{
    public StandSettingsView()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var window = ((UserControl)sender).Parent as Window;

        if (window != null)
        {
            window.WindowState = WindowState.FullScreen;
        }
    }
}