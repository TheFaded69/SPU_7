using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SPU_7.Views.ExtensionViews;

public partial class CheckAverageQuadraticDifferenceView : UserControl
{
    public CheckAverageQuadraticDifferenceView()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var window = ((UserControl)sender).Parent as Window;

        if (window == null) return;
        window.ExtendClientAreaToDecorationsHint = true;
        window.CanResize = false;
    }
}