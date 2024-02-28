using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SPU_7.Models.Scripts;

namespace SPU_7.Views.Convertors;

public class OperationStatusToColorConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is OperationStatus operationStatus)
            return operationStatus switch
            {
                OperationStatus.ExecuteWaiting => new SolidColorBrush(Colors.Black),
                OperationStatus.Executing => new SolidColorBrush(Colors.Yellow),
                OperationStatus.CompletedPassed => new SolidColorBrush(Colors.Green),
                OperationStatus.CompletedFailed => new SolidColorBrush(Colors.Red),
                OperationStatus.CompletedError => new SolidColorBrush(Colors.MediumVioletRed),
                OperationStatus.Stop => new SolidColorBrush(Colors.Red),
                _ => throw new ArgumentOutOfRangeException()
            };

        throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

}