
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;
using VisualSynthesizerDemo.Service;
using VisualSynthesizerDemo.ViewModel;

namespace VisualSynthesizerDemo.Locator
{
    public class Locator
    {
        public static void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            // VjewModels
            SimpleIoc.Default.Register<IMainWindowViewModel, MainWindowViewModel>();
            SimpleIoc.Default.Register<IMediaViewModel, MediaViewModel>();
            SimpleIoc.Default.Register<IControlViewModel, ControlViewModel>();

            // Services
            SimpleIoc.Default.Register<INotificationService, NotificationService>();
            SimpleIoc.Default.Register<IFaceDetectorService, FaceDetectorService>();
            SimpleIoc.Default.Register<IWebcamService, WebcamService>();
        }
        
        public static void Cleanup()
        {
            // VjewModels
            (ServiceLocator.Current.GetInstance<IMediaViewModel>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IControlViewModel>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IMainWindowViewModel>() as IDisposable)?.Dispose();

            // Services
            (ServiceLocator.Current.GetInstance<INotificationService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IFaceDetectorService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IWebcamService>() as IDisposable)?.Dispose();
        }

        // VjewModels
        public static IMainWindowViewModel MainWindowViewModel => ServiceLocator.Current.GetInstance<IMainWindowViewModel>();
        public static IMediaViewModel MediaViewModel => ServiceLocator.Current.GetInstance<IMediaViewModel>();
        public static IControlViewModel ControlViewModel => ServiceLocator.Current.GetInstance<IControlViewModel>();

        // Services
        public static INotificationService NotificationService => ServiceLocator.Current.GetInstance<INotificationService>();
        public static IFaceDetectorService FaceDetectorService => ServiceLocator.Current.GetInstance<IFaceDetectorService>();
        public static IWebcamService WebcamService => ServiceLocator.Current.GetInstance<IWebcamService>();
    }
}