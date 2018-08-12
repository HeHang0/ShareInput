using SharePoint.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharePoint
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MouseEvent mouseEvent = new MouseEvent();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = mouseEvent;
            //Thread th = new Thread(new ThreadStart(GetClipboard));
            //th.SetApartmentState(ApartmentState.STA);
            //th.IsBackground = true;
            //th.Start();
        }

        private void GetClipboard()
        {
            while (true)
            {
                mouseEvent.MouseEventStr = string.Join(",", Clipboard.GetDataObject().GetFormats().ToList());
                Thread.Sleep(1000);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            new CaptureMouse(mouseEvent).Show();
        }
    }
}
