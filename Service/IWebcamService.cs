using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.Service
{
    public interface IWebcamService
    {
        WebcamStatus LoadWebcam();
        WebcamStatus StartWebcam();
        WebcamStatus StopWebcam();
    }
}
