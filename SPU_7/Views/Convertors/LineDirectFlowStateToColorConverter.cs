using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SPU_7.Common.Line;

namespace SPU_7.Views.Convertors;

public class LineDirectFlowStateToColorConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is LineDirectionFlowState logLevel)
        {
            return logLevel switch
            {
                LineDirectionFlowState.AllOpen => new SolidColorBrush(Colors.LightGreen),
                LineDirectionFlowState.AllClose => new SolidColorBrush(Colors.IndianRed),
                LineDirectionFlowState.DirectDirection => new SolidColorBrush(Colors.LightGreen),
                LineDirectionFlowState.ReverseDirection => new SolidColorBrush(Colors.LightGreen),
                LineDirectionFlowState.Working => new SolidColorBrush(Colors.LightGray),
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