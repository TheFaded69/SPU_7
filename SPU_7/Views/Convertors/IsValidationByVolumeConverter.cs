using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using SPU_7.Common.Settings;

namespace SPU_7.Views.Convertors;

public class IsValidationByVolumeConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ValidationType validationType)
            return validationType is ValidationType.ValidationByVolume or ValidationType.AutoValidationByVolume;

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}