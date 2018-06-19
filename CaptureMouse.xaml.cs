using SharePoint.Input;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SharePoint
{
    /// <summary>
    /// CaptureMouse.xaml 的交互逻辑
    /// </summary>
    public partial class CaptureMouse : Window
    {
        [DllImport("user32.dll")]
        static extern bool ClipCursor(ref System.Drawing.Rectangle rect);
        private MouseEvent MouseEventSingle;
        public CaptureMouse(MouseEvent mouseEvent)
        {
            InitializeComponent();
            Top = 0;
            Left = 0;
            MouseEventSingle = mouseEvent;
            InitPointer();
            StuckPointer();
            Thread th = new Thread(new ThreadStart(GetMouse));
            th.IsBackground = true;
            th.Start();
        }

        private void GetMouse()
        {
            while (true)
            {
                var a = System.Windows.Forms.Cursor.Position;
                string p = $"{(int)a.X}{(int)a.Y}";
                bool needEnd = false;
                switch (p)
                {
                    case "00":
                        MouseEventSingle.MoveToLeftTop++;
                        InitPointer();
                        break;
                    case "02":
                        MouseEventSingle.MoveToLeftButtom++;
                        InitPointer();
                        break;
                    case "20":
                        MouseEventSingle.MoveToRightTop++;
                        InitPointer();
                        break;
                    case "22":
                        MouseEventSingle.MoveToRightButtom++;
                        InitPointer();
                        break;
                    case "01":
                        MouseEventSingle.MoveToLeft++;
                        InitPointer();
                        break;
                    case "10":
                        MouseEventSingle.MoveToTop++;
                        InitPointer();
                        break;
                    case "21":
                        MouseEventSingle.MoveToRight++;
                        InitPointer();
                        break;
                    case "12":
                        MouseEventSingle.MoveToButtom++;
                        InitPointer();
                        break;
                    case "11":
                        break;
                    default:
                        needEnd = true;
                        break;
                }
                if (needEnd)
                {
                    break;
                }
                Thread.Sleep(10);
            }
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseEventSingle.MouseDown++;
            var p = System.Windows.Forms.Cursor.Position;
            MessageBox.Show($"X: {p.X}, Y: {p.Y}");
        }

        private void StuckPointer()
        {
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(0, 0, 3, 3);
            ClipCursor(ref r);
        }

        private void InitPointer()
        {
            System.Drawing.Point p = new System.Drawing.Point(1, 1);
            System.Windows.Forms.Cursor.Position = p;
        }
    }
}
