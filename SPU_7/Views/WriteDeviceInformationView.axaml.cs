﻿using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SPU_7.Views;

public partial class WriteDeviceInformationView : UserControl
{
    public WriteDeviceInformationView()
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