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
using System.Threading;

namespace ATDC_V2._0
{
    /// <summary>
    /// Devices.xaml 的交互逻辑
    /// </summary>
    public partial class Devices : Window
    {
        #region 后台代码，中英文切换字符串
        string DeviceConnectSuccess = (string)System.Windows.Application.Current.FindResource("LangsDeviceConnectSuccess");
        string DeviceConnectFailure = (string)System.Windows.Application.Current.FindResource("LangsDeviceConnectFailure");

        #endregion

        #region 刷新中英文字符
        public void RefreshLanguageString()
        {
            DeviceConnectSuccess = (string)System.Windows.Application.Current.FindResource("LangsDeviceConnectSuccess");
            DeviceConnectFailure = (string)System.Windows.Application.Current.FindResource("LangsDeviceConnectFailure");
        }
        #endregion

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

        #region 连接设备
        private void ConnectDevicesPage_Click(object sender, RoutedEventArgs e)
        {
            RefreshLanguageString();

            if (MiniCCRModelSelect.SelectedIndex==0)
            {
                ConnectMiniCCRWithoutCommunicationInterface();
            }
            else
            {
                //待Mini CCR增加通讯功能后使用
            }

            if(SensorModelSelect.SelectedIndex==0)
            {
                //CL-500A
            }
            else
            {
                //CL-200A
            }

            //转台

            ConfigurationParameters.rotatingPlatformPortName = RotatingPlatformPortSelect.SelectedItem.ToString();

        }

        #region MiniCCR不含通讯，设备连接
        SerialPort SerialPortMiniCCRWithout = new SerialPort();
        MiniCCRWithoutCommunicationInterface myMiniCCRWithoutCommunicationInterface = new MiniCCRWithoutCommunicationInterface();
        OperationStatusMiniCCRWithout myOperationStatusMiniCCRWithout;

        public void ConnectMiniCCRWithoutCommunicationInterface()
        {
            SerialPortMiniCCRWithout.DataReceived += new SerialDataReceivedEventHandler(SerialPortMiniCCRWithout_DateReceived);   

            string portName = MiniCCRPortSelect.SelectedItem.ToString();
            myOperationStatusMiniCCRWithout = myMiniCCRWithoutCommunicationInterface.OpenPort(SerialPortMiniCCRWithout, portName);            
            myOperationStatusMiniCCRWithout = myMiniCCRWithoutCommunicationInterface.EnquiryStatus(SerialPortMiniCCRWithout);

            Thread.Sleep(50);
            ResultAnalysisConnectMiniCCRWithout();
            myOperationStatusMiniCCRWithout = myMiniCCRWithoutCommunicationInterface.ClosePort(SerialPortMiniCCRWithout);
        }

        private void SerialPortMiniCCRWithout_DateReceived(object sender, SerialDataReceivedEventArgs e)
        {
            myOperationStatusMiniCCRWithout = myMiniCCRWithoutCommunicationInterface.AnalysisFeedbackCommand(SerialPortMiniCCRWithout);
        }

        public void ResultAnalysisConnectMiniCCRWithout()
        {
            if(myOperationStatusMiniCCRWithout==OperationStatusMiniCCRWithout.AnalysisFeedbackCommandEnquiryStatusCCROffSuccess||myOperationStatusMiniCCRWithout==OperationStatusMiniCCRWithout.AnalysisFeedbackCommandEnquiryStatusCCROnSuccess)
            {
                ConnectStatusMiniCCR.Content = DeviceConnectSuccess;
            }
            else
            {
                ConnectStatusMiniCCR.Content = DeviceConnectFailure;
            }
        }
        #endregion

        #endregion
    }
}
