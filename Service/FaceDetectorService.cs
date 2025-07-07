using DlibDotNet;
using DlibDotNet.Extensions;
using DlibDotNet.ImageDatasetMetadata;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.Service
{
    public class FaceDetectorService : IFaceDetectorService
    {
        INotificationService _notificationService;
        private CancellationTokenSource _videoCts;
        private FrontalFaceDetector _detector;
        private ShapePredictor _sp;
        private readonly SemaphoreSlim _videoDetectLock = new SemaphoreSlim(1, 1);

        private const int MAX_WIDTH = 640;
        private const int MAX_HEIGHT = 480;

        public FaceDetectorService(INotificationService notificationService)
        {
            _notificationService = notificationService;
            _detector = Dlib.GetFrontalFaceDetector();
            _sp = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat");
            _videoCts = new CancellationTokenSource();
        }


        // Image 단일처리 Async
        public async Task<List<DetectionRectangle>> ImageDetectNoseAsync(string imagePath)
        {
            return await Task.Run(() =>
            {
                var noseRectangles = new List<DetectionRectangle>();
                try
                { 
                    using (var img = Dlib.LoadImage<RgbPixel>(imagePath))
                    {
                        var faces = _detector.Operator(img);
                        foreach (var rect in faces)
                        {
                            var shape = _sp.Detect(img, rect);
                            int noseStart = 27, noseEnd = 35;
                            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
                            for (int i = noseStart; i <= noseEnd; i++)
                            {
                                var point = shape.GetPart((uint)i);
                                if (point.X < minX) minX = point.X;
                                if (point.Y < minY) minY = point.Y;
                                if (point.X > maxX) maxX = point.X;
                                if (point.Y > maxY) maxY = point.Y;
                            }
                            int padding = 10;
                            int left = Math.Max(minX - padding, 0);
                            int top = Math.Max(minY - padding, 0);
                            int right = Math.Min(maxX + padding, img.Columns - 1);
                            int bottom = Math.Min(maxY + padding, img.Rows - 1);

                            if (right > left && bottom > top)
                            {
                                var noseRect = new Rectangle(left, top, right, bottom);
                                noseRectangles.Add(new DetectionRectangle(noseRect));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing image {imagePath}: {ex.Message}");
                    return new List<DetectionRectangle>();
                }
                return noseRectangles;
            });
        }

        // Image 단일처리
        public List<DetectionRectangle> ImageDetectNose(string imagePath)
        {
            var noseRectangles = new List<DetectionRectangle>();
            try
            {
                using (var img = Dlib.LoadImage<RgbPixel>(imagePath))
                {
                    var faces = _detector.Operator(img);
                    foreach (var rect in faces)
                    {
                        var shape = _sp.Detect(img, rect);
                        int noseStart = 27, noseEnd = 35;
                        int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
                        for (int i = noseStart; i <= noseEnd; i++)
                        {
                            var point = shape.GetPart((uint)i);
                            if (point.X < minX) minX = point.X;
                            if (point.Y < minY) minY = point.Y;
                            if (point.X > maxX) maxX = point.X;
                            if (point.Y > maxY) maxY = point.Y;
                        }
                        int padding = 10;
                        int left = Math.Max(minX - padding, 0);
                        int top = Math.Max(minY - padding, 0);
                        int right = Math.Min(maxX + padding, img.Columns - 1);
                        int bottom = Math.Min(maxY + padding, img.Rows - 1);

                        if (right > left && bottom > top)
                        {
                            var noseRect = new Rectangle(left, top, right, bottom);
                            noseRectangles.Add(new DetectionRectangle(noseRect));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing image {imagePath}: {ex.Message}");
                return new List<DetectionRectangle>();
            }
            return noseRectangles;
        }

        // Video 반복 처리
        public async Task VideoDetectNoseAsync(string videoPath)
        {
            // 동시 실행 제어: 이전 요청이 끝날 때까지 대기
            await _videoDetectLock.WaitAsync();
            try
            {
                using (var capture = new VideoCapture(videoPath))
                {
                    var frame = new Mat();
                    while (capture.Read(frame) && !_videoCts.Token.IsCancellationRequested)
                    {
                        ReSizeFrame(frame, MAX_WIDTH, MAX_HEIGHT);
                        string tempPath = System.IO.Path.GetTempFileName() + ".jpg";
                        Cv2.ImWrite(tempPath, frame);

                        var noseRects = await ImageDetectNoseAsync(tempPath);
                        Console.WriteLine($"Detected {noseRects.Count} nose rectangles in frame.");

                        _notificationService.SendMessage(this, new FaceDectectionEventArgs
                        {
                            DetectionRectList = noseRects,
                            FrameImagePath = tempPath,
                            Type = MediaType.Video
                        });
                    }
                    StopDetection();
                    // _videoCts 초기화
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Video detection error: {ex.Message}");
            }
            finally
            {
                _videoDetectLock.Release();
            }
        }

        // Webcam 실시간 처리
        public async Task WebcamDetectNoseAsync(Mat frame)
        {
            try
            {
                ReSizeFrame(frame, MAX_WIDTH, MAX_HEIGHT);
                string tempPath = System.IO.Path.GetTempFileName() + ".jpg";
                Cv2.ImWrite(tempPath, frame);

                var noseRects = ImageDetectNose(tempPath);
                Console.WriteLine($"Detected {noseRects.Count} nose rectangles in frame.");

                _notificationService.SendMessage(this, new FaceDectectionEventArgs
                {
                    DetectionRectList = noseRects,
                    FrameImagePath = tempPath,
                    Type = MediaType.CamVideo
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webcam detection error: {ex.Message}");
                return;
            }
        }


        // 프레임 크기 최적화
        private void ReSizeFrame(Mat frame, int maxWidth, int maxHeight)
        {
            // 원본 크기
            int originalWidth = frame.Width;
            int originalHeight = frame.Height;

            // 비율 계산
            double ratioX = (double)maxWidth / originalWidth;
            double ratioY = (double)maxHeight / originalHeight;
            double ratio = Math.Min(ratioX, ratioY);

            // 새 크기 계산 (비율 유지)
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);
            Cv2.Resize(frame, frame, new OpenCvSharp.Size(newWidth, newHeight));
        }


        public void StopDetection()
        {
            if(!_videoCts.Token.IsCancellationRequested)
            {
                _videoCts?.Cancel();
            }
            else
            {
                _videoCts = new CancellationTokenSource();
            }
        }
    }
}
