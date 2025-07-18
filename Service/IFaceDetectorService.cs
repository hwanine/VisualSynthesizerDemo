﻿using DlibDotNet;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSynthesizerDemo.Model;

namespace VisualSynthesizerDemo.Service
{
    public interface IFaceDetectorService
    {
        Task<List<DetectionRectangle>> ImageDetectNoseAsync(string imagePath);
        List<DetectionRectangle> ImageDetectNose(string imagePath);
        Task VideoDetectNoseAsync(string videoPath);
        Task WebcamDetectNoseAsync(Mat frame);
        void StopDetection();
    }
}
