using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SPU_7.Views;

public partial class AddUserView : UserControl
{
    public AddUserView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
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