using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.ViewModel
{
    public interface IControlViewModel
    {
        WebcamStatus WebcamStatus { get; set; }
        RelayCommand OpenVisualFileCommand { get; }
        RelayCommand WebcamLoadCommand { get; }
        RelayCommand WebcamStartCommand { get; }
        RelayCommand WebcamStopCommand { get; }
    }
}
