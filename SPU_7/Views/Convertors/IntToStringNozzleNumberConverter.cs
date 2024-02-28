using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace SPU_7.Views.Convertors;

public class IntToStringNozzleNumberConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return "Линия №" + value;
        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}