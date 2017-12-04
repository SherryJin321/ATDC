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
        Timer SaveDataProtection = new Timer(100);
        SerialPort RotatingPlatformSerialPort = new SerialPort();
        SerialPort miniCCRSerialPort = new SerialPort();
        RotatingPlatform myRotatingPlatform = new RotatingPlatform();
        //缺少CL500A类对象的声明和创建
        MiniCCRWithoutCommunicationInterface myMiniCCRWithoutCommunicationInterface = new MiniCCRWithoutCommunicationInterface();
        //CCR含通讯协议，目前尚未开发

        int count = 0;
        double current = 0;
        double[] EVDataArray = new double[41];

        #region 中英文切换字符串
        string stringManualTestStart = (string)System.Windows.Application.Current.FindResource("LangsManualTestStart");
        string stringManualTestStop = (string)System.Windows.Application.Current.FindResource("LangsManualTestStop");
        string stringCL200ACL200 = (string)System.Windows.Application.Current.FindResource("LangsCL200ACL200");
        string stringCL500A = (string)System.Windows.Application.Current.FindResource("LangsCL500A");
        string stringMiniCCRWithoutCommunicationInterface = (string)System.Windows.Application.Current.FindResource("LangsMiniCCRWithoutCommunicationInterface");
        string stringMiniCCRWithCommunicationInterface = (string)System.Windows.Application.Current.FindResource("LangsMiniCCRWithCommunicationInterface");
        #endregion

        #region 实时刷新中英文字符
        public void LanguageRefresh()
        {
            stringManualTestStart = (string)System.Windows.Application.Current.FindResource("LangsManualTestStart");
            stringManualTestStop = (string)System.Windows.Application.Current.FindResource("LangsManualTestStop");
            stringCL200ACL200 = (string)System.Windows.Application.Current.FindResource("LangsCL200ACL200");
            stringCL500A = (string)System.Windows.Application.Current.FindResource("LangsCL500A");
            stringMiniCCRWithoutCommunicationInterface = (string)System.Windows.Application.Current.FindResource("LangsMiniCCRWithoutCommunicationInterface");
            stringMiniCCRWithCommunicationInterface = (string)System.Windows.Application.Current.FindResource("LangsMiniCCRWithCommunicationInterface");
        }
        #endregion

        public ManualTest()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            myRotatingPlatform.EVDataEvent += new EVDataHandler(DisplayEVData);
            RotatingPlatformSerialPort.DataReceived += new SerialDataReceivedEventHandler(PortDataReceived);
            miniCCRSerialPort.DataReceived += new SerialDataReceivedEventHandler(CCRPortDataReceived);
            ManualTimer.Elapsed += new ElapsedEventHandler(QueryEVData);
            SaveDataProtection.Elapsed += new ElapsedEventHandler(RestoreSetting);            

            if(ConfigurationParameters.sensorModelName==0)
            {
                ManualTestSensorModelDisplay.Text = stringCL500A;
            }
            else if(ConfigurationParameters.sensorModelName==1)
            {
                ManualTestSensorModelDisplay.Text = stringCL200ACL200;
            }

            if(ConfigurationParameters.miniCCRModelName==0)
            {
                ManualTestCCRModelDisplay.Text = stringMiniCCRWithoutCommunicationInterface;
            }
            else if(ConfigurationParameters.miniCCRModelName == 1)
            {
                ManualTestCCRModelDisplay.Text = stringMiniCCRWithCommunicationInterface;
            }

            ManualTestSensorStatusDisplay.Text = ConfigurationParameters.rotatingPlatformPortName;


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

        #region 手动测试下，定时器计时事件的处理函数，用于发送查询EV值指令
        private void QueryEVData(object source,ElapsedEventArgs e)
        {
            if(ConfigurationParameters.sensorModelName== 1)
            {
                OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;                
                result = myRotatingPlatform.OpenPort(RotatingPlatformSerialPort, ConfigurationParameters.rotatingPlatformPortName);
                this.Dispatcher.Invoke(new System.Action(() =>
                {
                    ManualTestSensorStatusDisplay.Text = result.ToString();
                }));

                //result = myRotatingPlatform.GetEVxy(RotatingPlatformSerialPort);
                //this.Dispatcher.Invoke(new System.Action(() =>
                //{
                //    ManualTestSensorStatusDisplay.Text = result.ToString();
                //}));
            }
            else if(ConfigurationParameters.sensorModelName== 0)
            {
                //待写
            }                        
        }
        #endregion

        #region 手动测试下，定时器计时事件的处理函数，用于恢复保存数据按钮始能
        private void RestoreSetting(object source, ElapsedEventArgs e)
        {
            ManualTestSaveData.IsEnabled = true;
            SaveDataProtection.Stop();
        }
        #endregion

        #region 进入 调光曲线 页面
        private void ManualTestGenerateCurve_Click(object sender, RoutedEventArgs e)
        {
            GenerateCurve GenerateCurveWindow = new GenerateCurve();
            GenerateCurveWindow.EVValue = EVDataArray;
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
                
                count = 0;
                current = 6.7;
                EVDataArray = new double[41];

                ManualTestCountDisplay.Text = count.ToString() + " / 41 ";
                ManualTestCurrentValueDisplay.Text = current.ToString();
                ManualTestEVValueDisplay.Text = "";

                if(ConfigurationParameters.miniCCRModelName== 0)
                {
                    OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;                    
                    result = myMiniCCRWithoutCommunicationInterface.OpenPort(miniCCRSerialPort, ConfigurationParameters.miniCCRPortName);
                    ManualTestCCRStatusDisplay.Text = result.ToString();

                    result = myMiniCCRWithoutCommunicationInterface.ConnectCCR(miniCCRSerialPort);
                    ManualTestCCRStatusDisplay.Text = result.ToString();
                }
                else if(ConfigurationParameters.miniCCRModelName== 1)
                {
                    //待开发
                }

                ManualTimer.Start();
            }
            else if(ManualTestStart.Content.ToString() == stringManualTestStop)
            {
                ManualTestStart.Content = stringManualTestStart;
                ManualTimer.Stop();

                if (ConfigurationParameters.miniCCRModelName == 0)
                {
                    OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;                   
                    result = myMiniCCRWithoutCommunicationInterface.DisconnectCCR(miniCCRSerialPort);
                    ManualTestCCRStatusDisplay.Text = result.ToString();

                    result = myMiniCCRWithoutCommunicationInterface.ClosePort(miniCCRSerialPort);
                    ManualTestCCRStatusDisplay.Text = result.ToString();
                }
                else if (ConfigurationParameters.miniCCRModelName == 1)
                {
                    //待开发
                }

                count = 0;
                current = 0;
                EVDataArray = new double[41];

                ManualTestCountDisplay.Text = "";
                ManualTestCurrentValueDisplay.Text = "";
                ManualTestEVValueDisplay.Text = "";
            }            
        }
        #endregion

        #region 手动测试下，转台串口接收指令函数
        private void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;
            result = myRotatingPlatform.GetFeedbackCommand(RotatingPlatformSerialPort);

            this.Dispatcher.Invoke(new System.Action(() =>
            {
                ManualTestSensorStatusDisplay.Text = result.ToString();
            }));
        }
        #endregion

        #region 手动测试下，CCR无通讯协议串口接收指令函数
        private void CCRPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;
            result = myMiniCCRWithoutCommunicationInterface.AnalysisFeedbackCommand(miniCCRSerialPort);

            this.Dispatcher.Invoke(new System.Action(() =>
            {
                ManualTestCCRStatusDisplay.Text = result.ToString();
            }));
        }
        #endregion
    
        #region 保存数据
        private void ManualTestSaveData_Click(object sender, RoutedEventArgs e)
        {
            EVDataArray[count] = Convert.ToDouble(ManualTestEVValueDisplay.Text);
            count++;
            current -= 0.1;

            ManualTestCountDisplay.Text = count.ToString() + " / 41 ";
            ManualTestCurrentValueDisplay.Text = current.ToString();

            if (count==41)
            {
                ManualTimer.Stop();
            }

            ManualTestSaveData.IsEnabled = false;
            SaveDataProtection.Start();
        }
        #endregion
    }
}
