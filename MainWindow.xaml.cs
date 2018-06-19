﻿using SharePoint.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            new CaptureMouse(mouseEvent).Show();
        }
    }
}
