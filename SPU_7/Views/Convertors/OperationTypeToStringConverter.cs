using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Transformation;
using SPU_7.Common.Extensions;

namespace SPU_7.Views.Convertors;

public class OperationTypeToStringConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TransformOperation.OperationType dt)
            return dt.GetDescription();
        
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Enum
        .GetValues<TransformOperation.OperationType>()
        .FirstOrDefault(dt => dt.GetDescription() == (string)value);

}