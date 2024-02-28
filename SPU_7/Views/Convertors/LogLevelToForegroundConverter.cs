using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SPU_7.Common.Stand;

namespace SPU_7.Views.Convertors
{
    public class LogLevelToForegroundConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LogLevel logLevel)
            {
                return logLevel switch
                {
                    LogLevel.Info => new SolidColorBrush(Colors.Black),
                    LogLevel.Warning => new SolidColorBrush(Colors.Goldenrod),
                    LogLevel.Error => new SolidColorBrush(Colors.Orange),
                    LogLevel.Fatal => new SolidColorBrush(Colors.Red),
                    LogLevel.Success => new SolidColorBrush(Colors.Green),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException("Two way bindings are not supported with a string format");

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
