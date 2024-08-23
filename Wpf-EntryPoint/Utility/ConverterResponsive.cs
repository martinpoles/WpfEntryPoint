using System.Globalization;
using System.Windows.Data;
using System;
using System.Reflection.Metadata;

namespace Wpf_EntryPoint.Utility
{
    class ConverterResponsive
    {
    }


    public class IsLessThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsLessThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




    public class IsGraterThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsGraterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = System.Convert.ToDouble(value);
            double compareToValuew = System.Convert.ToDouble(parameter);

            return doubleValue > compareToValuew;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
