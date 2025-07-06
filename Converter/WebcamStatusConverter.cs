using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.Converter
{
    public class WebcamStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value == null || value.GetType() != typeof(WebcamStatus))
            {
                return string.Empty;
            }

            switch ((WebcamStatus)value)
            {
                case WebcamStatus.Loaded:
                    return Properties.Resources.WebcamLoaded;
                case WebcamStatus.NotLoaded:
                    return Properties.Resources.WebcamNotLoaded;
                case WebcamStatus.Started:
                    return Properties.Resources.WebcamStarted;
                case WebcamStatus.Stopped:
                    return Properties.Resources.WebcamStopped;
                case WebcamStatus.Error:
                    return Properties.Resources.WebcamError;
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
