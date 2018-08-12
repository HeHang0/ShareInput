using SharePoint.Input;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
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
        private static int HalfScreenWidth = (int)SystemParameters.PrimaryScreenWidth/2;
        private KeyboardHookLib _keyboardHook = null;
        public CaptureMouse(MouseEvent mouseEvent)
        {
            InitializeComponent();
            Top = 0;
            Left = HalfScreenWidth;
            MouseEventSingle = mouseEvent;
            InitPointer();
            StuckPointer();
            Thread th = new Thread(new ThreadStart(GetMouse));
            th.IsBackground = true;
            th.Start();
            FocusHelperButton.Focusable = true;
            FocusHelperButton.Focus();
            //安装勾子
            _keyboardHook = new KeyboardHookLib();
            _keyboardHook.InstallHook(OnKeyPress);
            CaptureMouse();
        }
        
        bool needEnd = false;
        private void GetMouse()
        {
            while (true)
            {
                var a = System.Windows.Forms.Cursor.Position;
                string p = $"{(int)a.X-960}{(int)a.Y}";
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
                        //needEnd = true;
                        Dispatcher.Invoke(() =>
                        {
                            StuckPointer();
                        });
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            MouseEventSingle.MouseEventStr = e.ChangedButton.ToString() + "按下";
            
            MouseEventSingle.MouseDown++;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            MouseEventSingle.MouseEventStr = e.ChangedButton.ToString() + "弹起";
        }
        int o = 0;
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            MouseEventSingle.MouseEventStr = "滑轮" + (e.Delta > 0 ? ++o : --o);
        }

        private void OnKeyPress(KeyboardHookLib.HookStruct hookStruct, out bool handle)
        {

            handle = true; //预设不拦截任何键
            Keys key = (Keys)hookStruct.vkCode;
            //  textBox1.Text = key.ToString();
            string keyName = key.ToString();
            MouseEventSingle.MouseEventStr = $"{keyName}{(hookStruct.flags < 96 ? "按下" : "弹起")}\n";
            MouseEventSingle.MouseEventStr += $"{hookStruct.dwExtraInfo}+{hookStruct.flags}+{hookStruct.scanCode}";
            if (key == Keys.Escape)
            {
                needEnd = true;
            }
        }

        private void StuckPointer()
        {
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(HalfScreenWidth, 0, HalfScreenWidth + 3, 3);
            ClipCursor(ref r);
            CaptureMouse();
            Keyboard.Focus(this);
            Mouse.OverrideCursor = System.Windows.Input.Cursors.None;
        }

        private void InitPointer()
        {
            System.Drawing.Point p = new System.Drawing.Point(HalfScreenWidth + 1, 1);
            System.Windows.Forms.Cursor.Position = p;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_keyboardHook != null) _keyboardHook.UninstallHook();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(0, 0, (int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
            ClipCursor(ref r);
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            InitPointer();
            StuckPointer();
            InputOutApi api = new InputOutApi();
            api.mouse_click();
            //Thread th = new Thread(new ThreadStart(AllFocus));
        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;
            FocusManager.SetFocusedElement(FocusHelperButton, FocusHelperButton);
            Keyboard.Focus(FocusHelperButton);
        }
    }
}
