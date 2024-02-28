using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Material.Icons;
using SPU_7.Common.Line;

namespace SPU_7.Views.Convertors;

public class LineDirectFlowStateToKindConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is LineDirectionFlowState logLevel)
        {
            return logLevel switch
            {
                LineDirectionFlowState.AllOpen => MaterialIconKind.Close,
                LineDirectionFlowState.AllClose => MaterialIconKind.Close,
                LineDirectionFlowState.DirectDirection => MaterialIconKind.ArrowRight,
                LineDirectionFlowState.ReverseDirection => MaterialIconKind.ArrowLeft,
                LineDirectionFlowState.Working => MaterialIconKind.Update,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException("Two way bindings are not supported with a string format");

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}