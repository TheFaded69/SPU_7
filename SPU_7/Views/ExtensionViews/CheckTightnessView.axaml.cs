using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Material.Styles.Controls;
using Prism.Services.Dialogs;

namespace SPU_7.Views.ExtensionViews;

public partial class CheckTightnessView : UserControl
{
    public CheckTightnessView()
    {
        InitializeComponent();
    }
    
    private bool _mouseDownForWindowMoving = false;
    private PointerPoint _originalPoint;

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var window = ((UserControl)sender).Parent as DialogWindow;

        if (window == null) return;
        window.ExtendClientAreaToDecorationsHint = true;
        window.CanResize = false;
    }
    
    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var window = ((UserControl)((ContentControl)((DockPanel)((DockPanel)sender).Parent).Parent).Parent).Parent as DialogWindow;
        
        if (!_mouseDownForWindowMoving) return;

        PointerPoint currentPoint = e.GetCurrentPoint(this);
        window.Position = new PixelPoint(window.Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
            window.Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _mouseDownForWindowMoving = true;
        _originalPoint = e.GetCurrentPoint(this);
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _mouseDownForWindowMoving = false;
    }
}