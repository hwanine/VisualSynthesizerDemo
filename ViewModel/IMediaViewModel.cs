using DlibDotNet;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.ViewModel
{
    public interface IMediaViewModel
    {
        ImageSource ImageSource { get; set; }
        ObservableCollection<DetectionRectangle> NoseRectangles { get; }
    }
}
