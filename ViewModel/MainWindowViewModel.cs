using GalaSoft.MvvmLight;
using System.ComponentModel;
using VisualSynthesizerDemo.Model;
using VisualSynthesizerDemo.Service;

namespace VisualSynthesizerDemo.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private readonly INotificationService _notificationService;
        public IMediaViewModel MediaViewModel { get; }
        public IControlViewModel ControlViewModel { get; }
        public MainWindowViewModel(INotificationService notificationService, IMediaViewModel mediaViewModel, IControlViewModel controlViewModel)
        {
            this.MediaViewModel = mediaViewModel;
            this.ControlViewModel = controlViewModel;
            _notificationService = notificationService;
            NotificationServiceSubscribes(notificationService);
        }

        private void NotificationServiceSubscribes(INotificationService notificationService)
        {
        }
    }
}