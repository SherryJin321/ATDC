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
using CL500AClassLib;

using DEVICE_HANDLE = System.IntPtr;


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
            string[] portNames = SerialPort.GetPortNames();                   
            RotatingPlatformPortSelect.ItemsSource = portNames;               
            RotatingPlatformPortSelect.SelectedIndex = portNames.Length - 1;
            MiniCCRPortSelect.ItemsSource = portNames;    
            MiniCCRPortSelect.SelectedIndex = portNames.Length - 1;

            SerialPortMiniCCRWithout.DataReceived += new SerialDataReceivedEventHandler(SerialPortMiniCCRWithout_DateReceived);
            SerialPortRotatingPlatform.DataReceived += new SerialDataReceivedEventHandler(SerialPortRotatingPlatform_DateReceived);
            SerialPortCL200A.DataReceived += new SerialDataReceivedEventHandler(SerialPortCL200A_DateReceived);


        }

        #region 转台串口设置
        #region 刷新串口号
        private void RotatingPlatformPortSelect_DropDownOpened(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();      
            RotatingPlatformPortSelect.ItemsSource = portNames;    
            RotatingPlatformPortSelect.SelectedIndex = portNames.Length - 1;
        }
        #endregion

        #endregion

        #region CCR串口设置
        #region 刷新串口号
        private void MiniCCRPortSelect_DropDownOpened(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();      
            MiniCCRPortSelect.ItemsSource = portNames;    
            MiniCCRPortSelect.SelectedIndex = portNames.Length - 1;
        }
        #endregion

        #endregion

        #region 获取全局变量
        public void GetGlobalParameters()
        {
            if(ConnectStatusMiniCCR.Content.ToString()== DeviceConnectSuccess&& ConnectStatusRotatingPlatform.Content.ToString() == DeviceConnectSuccess&& ConnectStatusSensor.Content.ToString() == DeviceConnectSuccess)
            {
                ConfigurationParameters.miniCCRModelName = MiniCCRModelSelect.SelectedIndex + 1;
                ConfigurationParameters.miniCCRPortName = MiniCCRPortSelect.SelectedItem.ToString();
                ConfigurationParameters.rotatingPlatformPortName = RotatingPlatformPortSelect.SelectedItem.ToString();
                ConfigurationParameters.sensorModelName = SensorModelSelect.SelectedIndex + 1;
            }
            else
            {
                ConfigurationParameters.miniCCRModelName = 0;
                ConfigurationParameters.miniCCRPortName = "";
                ConfigurationParameters.rotatingPlatformPortName = "";
                ConfigurationParameters.sensorModelName = 0;
            }            
        }
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

            if (MiniCCRModelSelect.SelectedIndex == 0)
            {
                ConnectMiniCCRWithoutCommunicationInterface();
            }
            else
            {
                //待Mini CCR增加通讯功能后使用
            }

            if (SensorModelSelect.SelectedIndex == 0)
            {
                ConnectCL500A();
            }
            else
            {
                ConnectCL200A();
            }

            ConnectRotatingPlatform();

            GetGlobalParameters();
        }

      

        #region MiniCCR不含通讯，设备连接
        SerialPort SerialPortMiniCCRWithout = new SerialPort();
        MiniCCRWithoutCommunicationInterface myMiniCCRWithoutCommunicationInterface = new MiniCCRWithoutCommunicationInterface();
        OperationStatusMiniCCRWithout myOperationStatusMiniCCRWithout=OperationStatusMiniCCRWithout.OriginalStatus;

        public void ConnectMiniCCRWithoutCommunicationInterface()
        {
            myOperationStatusMiniCCRWithout = OperationStatusMiniCCRWithout.OriginalStatus;

            if (MiniCCRPortSelect.SelectedIndex==-1)
            {
                ConnectStatusMiniCCR.Content = DeviceConnectFailure;
            }
            else
            {
                string portName = MiniCCRPortSelect.SelectedItem.ToString();
                myOperationStatusMiniCCRWithout = myMiniCCRWithoutCommunicationInterface.OpenPort(SerialPortMiniCCRWithout, portName);
                myOperationStatusMiniCCRWithout = myMiniCCRWithoutCommunicationInterface.EnquiryStatus(SerialPortMiniCCRWithout);

                Thread.Sleep(1000);
                ResultAnalysisConnectMiniCCRWithout();
                myMiniCCRWithoutCommunicationInterface.ClosePort(SerialPortMiniCCRWithout);
            }
        }

        private void SerialPortMiniCCRWithout_DateReceived(object sender, SerialDataReceivedEventArgs e)
        {
            myOperationStatusMiniCCRWithout = myMiniCCRWithoutCommunicationInterface.AnalysisFeedbackCommand(SerialPortMiniCCRWithout);
        }

        public void ResultAnalysisConnectMiniCCRWithout()
        {
            if (myOperationStatusMiniCCRWithout == OperationStatusMiniCCRWithout.REnquiryStatusCCROffSuccess || myOperationStatusMiniCCRWithout == OperationStatusMiniCCRWithout.REnquiryStatusCCROnSuccess)
            {
                ConnectStatusMiniCCR.Content = DeviceConnectSuccess;
            }
            else
            {
                ConnectStatusMiniCCR.Content = DeviceConnectFailure;
            }
        }
        #endregion

        #region 转台，设备连接
        SerialPort SerialPortRotatingPlatform = new SerialPort();
        RotatingPlatform myRotatingPlatform = new RotatingPlatform();
        OperationStatusRotatingPlatform myOperationStatusRotatingPlatform=OperationStatusRotatingPlatform.OriginalStatus;

        public void ConnectRotatingPlatform()
        {
            myOperationStatusRotatingPlatform = OperationStatusRotatingPlatform.OriginalStatus;

            if (RotatingPlatformPortSelect.SelectedIndex==-1)
            {                              
                 ConnectStatusRotatingPlatform.Content = DeviceConnectFailure;               
            }
            else
            {
                string portName = RotatingPlatformPortSelect.SelectedItem.ToString();
                myOperationStatusRotatingPlatform = myRotatingPlatform.OpenPort(SerialPortRotatingPlatform, portName);
                myOperationStatusRotatingPlatform = myRotatingPlatform.SetMotorStatus(SerialPortRotatingPlatform, 0, 0, 0, 0);

                Thread.Sleep(1000);
                ResultAnalysisRotatingPlatform();
                myRotatingPlatform.ClosePort(SerialPortRotatingPlatform);
            }
        }

        private void SerialPortRotatingPlatform_DateReceived(object sender, SerialDataReceivedEventArgs e)
        {
            myOperationStatusRotatingPlatform = myRotatingPlatform.GetFeedbackCommand(SerialPortRotatingPlatform);
        }

        public void ResultAnalysisRotatingPlatform()
        {
            if (myOperationStatusRotatingPlatform == OperationStatusRotatingPlatform.RSetMotorStatusSuccess)
            {
                ConnectStatusRotatingPlatform.Content = DeviceConnectSuccess;
            }
            else
            {
                ConnectStatusRotatingPlatform.Content = DeviceConnectFailure;
            }
        }

        #endregion

        #region CL-200/CL-200A，设备连接
        SerialPort SerialPortCL200A = new SerialPort();
        RotatingPlatform myCL200A = new RotatingPlatform();
        OperationStatusRotatingPlatform myOperationStatusCL200A = OperationStatusRotatingPlatform.OriginalStatus;

        public void ConnectCL200A()
        {
            myOperationStatusCL200A = OperationStatusRotatingPlatform.OriginalStatus;

            if (RotatingPlatformPortSelect.SelectedIndex==-1)
            {                                
                ConnectStatusSensor.Content = DeviceConnectFailure;               
            }
            else
            {
                string portName = RotatingPlatformPortSelect.SelectedItem.ToString();
                myOperationStatusCL200A = myCL200A.OpenPort(SerialPortCL200A, portName);
                myOperationStatusCL200A = myCL200A.ConnectCL200ToPC(SerialPortCL200A);

                Thread.Sleep(1000);
                ResultAnalysisCL200A();
                myCL200A.ClosePort(SerialPortCL200A);
            }
        }

        private void SerialPortCL200A_DateReceived(object sender, SerialDataReceivedEventArgs e)
        {
            myOperationStatusCL200A = myCL200A.GetFeedbackCommand(SerialPortCL200A);
        }

        public void ResultAnalysisCL200A()
        {
            if (myOperationStatusCL200A == OperationStatusRotatingPlatform.RConnectCL200ToPCSuccess)
            {
                ConnectStatusSensor.Content = DeviceConnectSuccess;
            }
            else
            {
                ConnectStatusSensor.Content = DeviceConnectFailure;
            }
        }

        #endregion

        #region CL-500A，设备连接
        public void ConnectCL500A()
        {
            CL500A myCL500A = new CL500A();
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            DEVICE_HANDLE handle = new DEVICE_HANDLE();

            result = myCL500A.OpenDevice(ref handle);
            if (result == OperationStatusCL500A.OpenDeviceSuccess)
            {
                result = myCL500A.SetRemoteMode(handle);
                if (result == OperationStatusCL500A.SetRemoteModeSuccess)
                {
                    ConnectStatusSensor.Content = DeviceConnectSuccess;
                    myCL500A.RemoteOffClose(handle);
                }
                else
                {
                    ConnectStatusSensor.Content = DeviceConnectFailure;
                }
            }
            else
            {
                ConnectStatusSensor.Content = DeviceConnectFailure;
            }
        }
        #endregion

        #endregion
    }
}
