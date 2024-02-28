using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SPU_7.Views.WorkReportViews.PdfViewerView;

public partial class PdfViewerView : UserControl
{
    public PdfViewerView()
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