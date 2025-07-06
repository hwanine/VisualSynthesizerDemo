using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VisualSynthesizerDemo.Model;
using VisualSynthesizerDemo.Service;

namespace VisualSynthesizerDemo.ViewModel
{
    public class ControlViewModel : ViewModelBase, IControlViewModel
    {
        private readonly INotificationService _notificationService;
        private readonly IWebcamService _webcamService;
        private readonly IFaceDetectorService _faceDetectorService;
        private WebcamStatus _webcamStatus;
        private MediaType _mediaFileType;

        public RelayCommand OpenVisualFileCommand { get; }
        public RelayCommand WebcamLoadCommand { get; }
        public RelayCommand WebcamStartCommand { get; }
        public RelayCommand WebcamStopCommand { get; }

        public WebcamStatus WebcamStatus
        {
            get => _webcamStatus;
            set => Set(ref _webcamStatus, value);
        }
        public MediaType MediaFileType
        {
            get => _mediaFileType;
            set => Set(ref _mediaFileType, value);
        }

        public ControlViewModel(INotificationService notificationService, IWebcamService webcamService, IFaceDetectorService faceDetectorService)
        {
            _notificationService = notificationService;
            _webcamService = webcamService;
            _faceDetectorService = faceDetectorService;
            OpenVisualFileCommand = new RelayCommand(OpenMediaFile);
            WebcamStatus = WebcamStatus.NotLoaded;
            MediaFileType = MediaType.None;

            WebcamLoadCommand = new RelayCommand(() =>
            {
                WebcamStatus = _webcamService.LoadWebcam();
            });
            WebcamStartCommand = new RelayCommand(() =>
            {
                if (MediaFileType == MediaType.Video)
                {
                    _faceDetectorService.StopDetection();
                }
                WebcamStatus = _webcamService.StartWebcam();
                MediaFileType = MediaType.CamVideo;
            });
            WebcamStopCommand = new RelayCommand(() =>
            {
                WebcamStatus = _webcamService.StopWebcam();
            });

        }


        private async void OpenMediaFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = Properties.Resources.OpenFileFilter
            };

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                string ext = Path.GetExtension(filePath).ToLower();

                string[] imageExts = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                string[] videoExts = { ".mp4", ".avi", ".wmv", ".mov" };

                if(MediaFileType == MediaType.CamVideo)
                {
                    _webcamService.StopWebcam();
                }
                if (MediaFileType == MediaType.Video)
                {
                    _faceDetectorService.StopDetection();
                }

                if (Array.Exists(imageExts, e => e == ext))
                {
                    // 이미지 파일
                    var image = new BitmapImage(new Uri(filePath));
                    MediaFileType = MediaType.Image;
                    var noseRects = await _faceDetectorService.ImageDetectNoseAsync(filePath);
                    _notificationService.SendMessage(this, new OpenImageFileEventArgs { Image = image, NoseRects = noseRects });
                }
                else if (Array.Exists(videoExts, e => e == ext))
                {
                    // 비디오 파일
                    MediaFileType = MediaType.Video;
                    await _faceDetectorService.VideoDetectNoseAsync(filePath);
                }
                else
                {
                    // 지원하지 않는 파일
                    MediaFileType = MediaType.None;
                    _notificationService.SendMessage(this, NotificationType.OpenFileError, Properties.Resources.NotSupportFile);
                }
            }
        }
    }
}
