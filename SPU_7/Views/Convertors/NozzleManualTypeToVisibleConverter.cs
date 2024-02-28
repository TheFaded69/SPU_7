using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using SPU_7.Common.Stand;

namespace SPU_7.Views.Convertors;

public class NozzleManualTypeToVisibleConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is NozzleManualType nozzleManualType)
            return nozzleManualType switch
            {
                NozzleManualType.StandardManual => true,
                NozzleManualType.FrequencyManual => false,
                _ => throw new ArgumentOutOfRangeException()
            };

        return null;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}