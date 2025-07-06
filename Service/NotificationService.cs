using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSynthesizerDemo.Service
{
    public class NotificationService : INotificationService
    {
        public event EventHandler PlayRequestEvent;
        public event EventHandler PauseRequestEvent;
        public event EventHandler StopRequestEvent;

        public event EventHandler<OpenImageFileEventArgs> OpenImageFileEvent;
        public event EventHandler<OpenVideoFileEventArgs> OpenVideoFileEvent;
        public event EventHandler<FaceDectectionEventArgs> FaceDectectionEvent;

        public void SendMessage(object sender, NotificationType type, string message)
        {
            var args = new NotificationEventArgs { Message = message };
            switch (type)
            {
                case NotificationType.PlayRequest:
                    PlayRequestEvent?.Invoke(sender, args);
                    break;
                case NotificationType.PauseRequest:
                    PauseRequestEvent?.Invoke(sender, args);
                    break;
                case NotificationType.StopRequest:
                    StopRequestEvent?.Invoke(sender, args);
                    break;
                default:
                    return;
            }
        }

        public void SendMessage(object sender, OpenImageFileEventArgs args)
        {
            OpenImageFileEvent?.Invoke(sender, args);
        }

        public void SendMessage(object sender, OpenVideoFileEventArgs args)
        {
            OpenVideoFileEvent?.Invoke(sender, args);
        }
        public void SendMessage(object sender, FaceDectectionEventArgs args)
        {
            FaceDectectionEvent?.Invoke(sender, args);
        }

    }
}
