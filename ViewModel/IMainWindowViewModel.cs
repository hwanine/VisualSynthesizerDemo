using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSynthesizerDemo.ViewModel
{
    public interface IMainWindowViewModel
    {
        IControlViewModel ControlViewModel { get; }
        IMediaViewModel MediaViewModel { get; }
    }
}
