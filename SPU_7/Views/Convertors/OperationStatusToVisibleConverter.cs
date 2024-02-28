using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using SPU_7.Models.Scripts;

namespace SPU_7.Views.Convertors;

public class OperationStatusToVisibleConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is OperationStatus operationStatus)
            return operationStatus != OperationStatus.Executing;

        throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

}