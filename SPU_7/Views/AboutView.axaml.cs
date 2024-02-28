using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SPU_7.Views;

public partial class AboutView : UserControl
{
    public AboutView()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not UserControl userControlParent) return;
        if (userControlParent.Parent is Window window)
            window.Close();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var window = ((UserControl)sender).Parent as Window;

        if (window == null) return;
        window.ExtendClientAreaToDecorationsHint = true;
        window.CanResize = false;
    }
}