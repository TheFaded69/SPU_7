using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Material.Icons;
using SPU_7.Common.Scripts;

namespace SPU_7.Views.Convertors;

public class OperationResultStatusToIconConverter: MarkupExtension, IValueConverter
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
                OperationResultType.Failed => MaterialIconKind.Close,
                OperationResultType.Success => MaterialIconKind.Done,
                OperationResultType.Error => MaterialIconKind.AlertCircleOutline,
                OperationResultType.FatalError => MaterialIconKind.AlertCircle,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}