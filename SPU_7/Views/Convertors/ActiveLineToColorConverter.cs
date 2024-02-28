using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace SPU_7.Views.Convertors;

public class ActiveLineToColorConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool dt)
            return dt ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.IndianRed);
        
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}