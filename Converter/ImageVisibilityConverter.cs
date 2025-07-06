using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.Converter
{
    public class ImageVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || value.GetType() != typeof(MediaType))
            {
                return Visibility.Collapsed;
            }

            switch((MediaType)value)
            {
                case MediaType.Image:
                    return Visibility.Visible;
                case MediaType.Video:
                    return Visibility.Collapsed;
                case MediaType.CamVideo:
                    return Visibility.Collapsed;
                case MediaType.None:
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
