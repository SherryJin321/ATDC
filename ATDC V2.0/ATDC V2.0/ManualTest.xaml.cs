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
using System.Timers;
using System.IO.Ports;

namespace ATDC_V2._0
{
    /// <summary>
    /// ManualTest.xaml 的交互逻辑
    /// </summary>
    public partial class ManualTest : Page
    {
        Timer ManualTimer = new Timer(1000);
        SerialPort RotatingPlatformSerialPort = new SerialPort();
        SerialPort miniCCRSerialPort = new SerialPort();
        RotatingPlatform myRotatingPlatform = new RotatingPlatform();
        //缺少CL500A类对象的声明和创建
        MiniCCRWithoutCommunicationInterface myMiniCCRWithoutCommunicationInterface = new MiniCCRWithoutCommunicationInterface();
        //CCR含通讯协议，目前尚未开发

        int count = 0;
        double current = 0;
        byte[] EVDataArray = new byte[41];

        #region 中英文切换字符串
        string stringManualTestStart = (string)System.Windows.Application.Current.FindResource("LangsManualTestStart");
        string stringManualTestStop = (string)System.Windows.Application.Current.FindResource("LangsManualTestStop");


        #endregion

        public ManualTest()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            myRotatingPlatform.EVDataEvent += new EVDataHandler(DisplayEVData);
            RotatingPlatformSerialPort.DataReceived += new SerialDataReceivedEventHandler(PortDataReceived);
            //miniCCRSerialPort.DataReceived += new SerialDataReceivedEventHandler(CCRPortDataReceived);
            ManualTimer.Elapsed += new ElapsedEventHandler(QueryEVData);

            

            ManualTestSensorModelDisplay.Text = ConfigurationParameters.sensorModelName;
            ManualTestCCRModelDisplay.Text = ConfigurationParameters.miniCCRModelName;
            
        }

        #region 手动测试下，EVDataEvent事件的处理函数
        public void DisplayEVData(double EVData)
        {
            this.Dispatcher.Invoke(new System.Action(() =>
            {
                ManualTestEVValueDisplay.Text = EVData.ToString();
            }));
        }
        #endregion

        #region 手动测试下，定时器计时事件的处理函数
        private void QueryEVData(object source,ElapsedEventArgs e)
        {            
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            if (RotatingPlatformSerialPort.IsOpen == false)
            {
                result = myRotatingPlatform.OpenPort(RotatingPlatformSerialPort, ConfigurationParameters.rotatingPlatformPortName);
            }

            result = myRotatingPlatform.GetEVxy(RotatingPlatformSerialPort);

            count++;
            if(count>41)
            {
                ManualTimer.Stop();
            }
        }
        #endregion

        #region 进入 调光曲线 页面
        private void ManualTestGenerateCurve_Click(object sender, RoutedEventArgs e)
        {
            GenerateCurve GenerateCurveWindow = new GenerateCurve();
            GenerateCurveWindow.ShowDialog();
        }

        #endregion

        #region 开始测试/停止测试
        private void ManualTestStart_Click(object sender, RoutedEventArgs e)
        {
            LanguageRefresh();

            if (ManualTestStart.Content.ToString()== stringManualTestStart)
            {
                ManualTestStart.Content = stringManualTestStop;
                ManualTimer.Start();
            }
            else if(ManualTestStart.Content.ToString() == stringManualTestStop)
            {
                ManualTestStart.Content = stringManualTestStart;
                ManualTimer.Stop();
            }

            count = 0;
            current = 6.7;
            EVDataArray = new byte[41];

            ManualTestCountDisplay.Text = count.ToString() + " / 41 ";
            ManualTestCurrentValueDisplay.Text = current.ToString();
            ManualTestEVValueDisplay.Text = "";
        }
        #endregion

        #region 手动测试下，转台串口接收指令函数
        private void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            myRotatingPlatform.GetFeedbackCommand(RotatingPlatformSerialPort);
        }
        #endregion

        #region 实时刷新中英文字符
        public void LanguageRefresh()
        {
            stringManualTestStart = (string)System.Windows.Application.Current.FindResource("LangsManualTestStart");
            stringManualTestStop = (string)System.Windows.Application.Current.FindResource("LangsManualTestStop");
        }
        #endregion

    }
}
