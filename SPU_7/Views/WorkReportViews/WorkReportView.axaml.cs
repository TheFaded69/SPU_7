using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SPU_7.Views.WorkReportViews;

public partial class WorkReportView : UserControl
{
    public WorkReportView()
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