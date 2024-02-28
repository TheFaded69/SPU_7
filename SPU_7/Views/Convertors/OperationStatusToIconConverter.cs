using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Material.Icons;
using SPU_7.Models.Scripts;

namespace SPU_7.Views.Convertors;

public class OperationStatusToIconConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is OperationStatus operationStatus)
            return operationStatus switch
            {
                OperationStatus.ExecuteWaiting => MaterialIconKind.Timer,
                OperationStatus.Executing => MaterialIconKind.Abacus,
                OperationStatus.CompletedPassed => MaterialIconKind.Done,
                OperationStatus.CompletedFailed => MaterialIconKind.Close,
                OperationStatus.CompletedError => MaterialIconKind.Error,
                OperationStatus.Stop => MaterialIconKind.Stop,
                _ => throw new ArgumentOutOfRangeException()
            };

        throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

}