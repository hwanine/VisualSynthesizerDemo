using DlibDotNet;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using VisualSynthesizerDemo.Model;
using VisualSynthesizerDemo.Service;
using VisualSynthesizerDemo.View;

namespace VisualSynthesizerDemo.ViewModel
{
    public class MediaViewModel : ViewModelBase, IMediaViewModel
    {
        private readonly INotificationService _notificationService;
        private ImageSource _imageSource;
        private Uri _videoSource;

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }
        public ObservableCollection<DetectionRectangle> NoseRectangles { get; }

        public MediaViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            NoseRectangles = new ObservableCollection<DetectionRectangle>();
            NotificationServiceSubscribes(notificationService);
        }

        private void NotificationServiceSubscribes(INotificationService notificationService)
        {
            // Img 파일 직접 검출 후 출력
            notificationService.OpenImageFileEvent += async (sender, args) =>
            {
                ImageSource = args.Image;
                foreach(var rect in args.NoseRects)
                {
                    NoseRectangles.Add(rect);
                }          
            };

            // Video 검출 이벤트 수신, Webcam 검출 이미지 수신
            notificationService.FaceDectectionEvent += async (sender, args) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    BitmapImage image = new BitmapImage();
                    using (var stream = new System.IO.FileStream(args.FrameImagePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad; // 파일을 메모리로 모두 읽음
                        image.StreamSource = stream;
                        image.EndInit();
                        image.Freeze(); // 멀티스레드 안전
                    }
                    ImageSource = image;

                    NoseRectangles.Clear();
                    foreach (var rect in args.DetectionRectList)
                    {
                        NoseRectangles.Add(rect);
                    }

                    // 임시 파일 삭제
                    try
                    {
                        System.IO.File.Delete(args.FrameImagePath);
                    }
                    catch (Exception ex) 
                    { 
                        Console.WriteLine($"임시 파일 삭제 실패: {ex.Message}");
                    }                    
                });

            };
        }
    }
}
