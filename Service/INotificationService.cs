using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.Service
{
    public enum NotificationType
    {
        OpenFileError,
        PlayRequest,
        PauseRequest,
        StopRequest,
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class OpenImageFileEventArgs : EventArgs
    {
        public ImageSource Image { get; set; }
        public List<DetectorRectangle> NoseRects{ get; set; }
    }

    public class OpenVideoFileEventArgs : EventArgs
    {
        public Uri Video{ get; set; }
        public string FilePath { get; set; }
    }

    public class FaceDectectionEventArgs: EventArgs
    {
        public List<DetectorRectangle> DetectionRectList { get; set; }
        public string FrameImagePath { get; set; }
        public MediaType Type { get; set; }
    }

     public interface INotificationService
    {
        event EventHandler PlayRequestEvent;
        event EventHandler PauseRequestEvent;
        event EventHandler StopRequestEvent;

        event EventHandler<OpenImageFileEventArgs> OpenImageFileEvent;
        event EventHandler<OpenVideoFileEventArgs> OpenVideoFileEvent;
        event EventHandler<FaceDectectionEventArgs> FaceDectectionEvent;

        void SendMessage(object sender, NotificationType type, string message);
        void SendMessage(object sender, OpenImageFileEventArgs args);
        void SendMessage(object sender, OpenVideoFileEventArgs args);
        void SendMessage(object sender, FaceDectectionEventArgs args);
    }
}

