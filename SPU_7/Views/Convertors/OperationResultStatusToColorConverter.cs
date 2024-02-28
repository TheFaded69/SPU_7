using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SPU_7.Common.Scripts;

namespace SPU_7.Views.Convertors;

public class OperationResultStatusToColorConverter: MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        /*if (value is bool isEnabled)
            return isEnabled ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Gray);*/

        if (value is OperationResultType operationStatus)
        {
            return operationStatus switch
            {
                OperationResultType.Failed => new SolidColorBrush(Colors.IndianRed),
                OperationResultType.Success => new SolidColorBrush(Colors.LightGreen),
                OperationResultType.Error => new SolidColorBrush(Colors.IndianRed),
                OperationResultType.FatalError => new SolidColorBrush(Colors.Red),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}