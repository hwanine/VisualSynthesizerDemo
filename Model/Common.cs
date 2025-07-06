using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSynthesizerDemo.Model
{
    public enum MediaType
    {
        Image,
        Video,
        CamVideo,
        None
    }

    public enum WebcamStatus
    {
        Loaded,
        NotLoaded,
        Started,
        Stopped,
        Error
    }
}
