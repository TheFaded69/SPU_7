using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using SPU_7.Common.Extensions;
using SPU_7.Common.Stand;

namespace SPU_7.Views.Convertors;

public class StandTypeToStringConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is StandType dt)
            return dt.GetDescription();
        
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Enum
        .GetValues<StandType>()
        .FirstOrDefault(dt => dt.GetDescription() == (string)value);

}