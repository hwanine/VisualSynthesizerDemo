using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VisualSynthesizerDemo.Locator;

namespace VisualSynthesizerDemo
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.Initialize();
            Locator.Locator.Init();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Locator.Locator.Cleanup();
        }
    }
}
