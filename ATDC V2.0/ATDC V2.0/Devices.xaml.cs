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
using System.Windows.Shapes;
using System.IO.Ports;

namespace ATDC_V2._0
{
    /// <summary>
    /// Devices.xaml 的交互逻辑
    /// </summary>
    public partial class Devices : Window
    {
        public Devices()
        {
            InitializeComponent();
        }

       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //刷新转台和CCR串口号
            string[] portNames = SerialPort.GetPortNames();                   //获取当前电脑所有串口号
            RotatingPlatformPortSelect.ItemsSource = portNames;               //将串口号显示在ComboBox
            RotatingPlatformPortSelect.SelectedIndex = portNames.Length - 1;
            MiniCCRPortSelect.ItemsSource = portNames;    //将串口号显示在ComboBox
            MiniCCRPortSelect.SelectedIndex = portNames.Length - 1;


        }

        #region 转台串口设置
        #region 刷新串口号
        private void RotatingPlatformPortSelect_DropDownOpened(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();      //获取当前电脑所有串口号
            RotatingPlatformPortSelect.ItemsSource = portNames;    //将串口号显示在ComboBox
            RotatingPlatformPortSelect.SelectedIndex = portNames.Length - 1;
        }
        #endregion

        #endregion

        #region CCR串口设置
        #region 刷新串口号
        private void MiniCCRPortSelect_DropDownOpened(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();      //获取当前电脑所有串口号
            MiniCCRPortSelect.ItemsSource = portNames;    //将串口号显示在ComboBox
            MiniCCRPortSelect.SelectedIndex = portNames.Length - 1;
        }
        #endregion

        #endregion

        #region 关闭 设备 页面
        private void CloseDevicesPage_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
