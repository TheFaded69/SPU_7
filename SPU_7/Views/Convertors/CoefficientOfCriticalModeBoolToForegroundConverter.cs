using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Material.Icons;


namespace SPU_7.Views.Convertors;

public class CoefficientOfCriticalModeBoolToForegroundConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool dt)
            return dt 
                ? new SolidColorBrush(Colors.Black)
                : new SolidColorBrush(Colors.Red);

        throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}