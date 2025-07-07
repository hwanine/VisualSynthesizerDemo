using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.Service
{
    public class WebcamService : IWebcamService
    {
        private readonly IFaceDetectorService _faceDetectorService;
        private readonly INotificationService _notificationService;

        private VideoCapture _capture;
        private CancellationTokenSource _cts;

        public WebcamService(IFaceDetectorService faceDetectorService, INotificationService notificationService)
        {
            _faceDetectorService = faceDetectorService;
            _notificationService = notificationService;

            // OpenCV 초기화
            Cv2.UseOptimized();
        }

        public WebcamStatus StartWebcam()
        {
            if (_capture == null || !_capture.IsOpened())
                return WebcamStatus.NotLoaded;
            _cts = new CancellationTokenSource();

            try
            {
                Task.Run(async () =>
                {
                    var frame = new Mat();
                    while (_capture.IsOpened() && !_cts.Token.IsCancellationRequested)
                    {
                        if (_capture.Read(frame) && !frame.Empty())
                        {
                            await _faceDetectorService.WebcamDetectNoseAsync(frame);
                        }
                    }
                    frame.Dispose();
                }, _cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"웹캠 시작 중 오류 발생: {ex.Message}");
                return WebcamStatus.Error;
            }

            return WebcamStatus.Started;
        }

        public WebcamStatus LoadWebcam()
        {
            _cts = new CancellationTokenSource();
            _capture = new VideoCapture(0);

            if (!_capture.IsOpened())
            {
                Console.WriteLine("웹캠이 연결되어 있지 않거나 인식되지 않았습니다.");
                return WebcamStatus.NotLoaded;
            }
            return WebcamStatus.Loaded;
        }

        public WebcamStatus StopWebcam()
        {
            try
            {
                _cts?.Cancel();
            }catch (Exception ex)
            {
                Console.WriteLine($"웹캠 중지 중 오류 발생: {ex.Message}");
            }

            return WebcamStatus.Stopped;
        }
    }
}
