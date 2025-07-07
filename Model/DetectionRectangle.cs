using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace VisualSynthesizerDemo.Model
{
    public class DetectionRectangle
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public DetectionRectangle(DlibDotNet.Rectangle rect)
        {
            Left = rect.Left;
            Top = rect.Top;
            Width = rect.Right - rect.Left + 1;
            Height = rect.Bottom - rect.Top + 1;

            if (Height > Width)
            {
                Top += Height - Width;
                Height = Width;
            }
            else
            {
                var dif = Width - Height;
                Left += dif / 2;
                Width = Height;
            }
        }
    }
}