using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GroundWellDesign
{
    public class MyConvert : IValueConverter
    {
        public string sourceType
        {
            get;
            set;
        }
        public object Convert(object value, Type targetType, object param, CultureInfo c)
        {
            switch (sourceType)
            {
                case "login":
                    bool b1 = (bool)value;
                    return b1 ? Visibility.Visible : Visibility.Collapsed;

                case "key1":
                    bool b2 = (bool)value;
                    return b2 ? new SolidColorBrush(Colors.Coral) : new SolidColorBrush(Colors.White);
                case "key2":
                    bool b3 = (bool)value;
                    return b3 ? new SolidColorBrush(Colors.Coral) : new SolidColorBrush(Colors.Black);
                case "yanXing":
                    string s = value as string;
                    return Application.Current.FindResource(s);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object param, CultureInfo c)
        {
            throw new NotImplementedException();
        }
    }


    public class ItemHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            return values.Length == 2 ? (double)values[0] * (double)values[1] : 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
