using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;

namespace SPU_7.Views.Convertors;

public class DeviceTypeToStringConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DeviceType dt)
            return dt.GetDescription();
        
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Enum
        .GetValues<DeviceType>()
        .FirstOrDefault(dt => dt.GetDescription() == (string)value);

}