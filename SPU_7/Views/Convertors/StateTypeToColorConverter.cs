using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;
using SPU_7.ViewModels;

namespace SPU_7.Views.Convertors;

public class StateTypeToColorConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is StateType st)
        {
            return st switch
            {
                StateType.None => new SolidColorBrush(Colors.Gray),
                StateType.Open => new SolidColorBrush(Colors.LightGreen),
                StateType.Close => new SolidColorBrush(Colors.IndianRed),
                StateType.Work => new SolidColorBrush(Colors.LightGray),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else throw new ArgumentOutOfRangeException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Enum
        .GetValues<DeviceType>()
        .FirstOrDefault(dt => dt.GetDescription() == (string)value);

}