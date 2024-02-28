using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using SPU_7.Common.Extensions;

namespace SPU_7.Views.Convertors;

public class EnumToDescriptionConverter: MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            return enumValue.GetDescription();
        }

        throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}