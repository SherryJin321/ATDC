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
        //Timer ManualTimer = new Timer();
        SerialPort RotatingPlatformSerialPort = new SerialPort();
        RotatingPlatform myRotatingPlatform = new RotatingPlatform();


        public ManualTest()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            myRotatingPlatform.EVDataEvent += new EVDataHandler(DisplayEVDate);
            RotatingPlatformSerialPort.DataReceived += new SerialDataReceivedEventHandler(PortDataReceived);

            //testArray[32] = myRotatingPlatform.GetCHK(testArray);
            //OperationStatusRotatingPlatform result = myRotatingPlatform.AnalysisFeedbackCommand(testArray);
            //ManualTestCCRModelDisplay.Text = result.ToString();
            //ManualTestEVValueDisplay.Text = myRotatingPlatform.GetEV(testArray).ToString();
            //Coordinates xy = new Coordinates();
            //xy = myRotatingPlatform.GetCoordinates(testArray1);

            //ManualTestEVValueDisplay.Text = "("+xy.x.ToString() + "," + xy.y.ToString()+")";
        }

        #region 手动测试下，EVDataEvent事件的处理函数
        public void DisplayEVDate(double EVData)
        {
            this.Dispatcher.Invoke(new System.Action(() =>
            {
                ManualTestEVValueDisplay.Text = EVData.ToString();
            }));
        }
        #endregion

        #region 进入 调光曲线 页面
        private void ManualTestGenerateCurve_Click(object sender, RoutedEventArgs e)
        {
            GenerateCurve GenerateCurveWindow = new GenerateCurve();
            GenerateCurveWindow.ShowDialog();
        }

        #endregion

        #region 开始测试
        private void ManualTestStart_Click(object sender, RoutedEventArgs e)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            if (RotatingPlatformSerialPort.IsOpen==false)
            {
                result = myRotatingPlatform.OpenPort(RotatingPlatformSerialPort, ConfigurationParameters.rotatingPlatformPortName);
            }

            result = myRotatingPlatform.GetEVxy(RotatingPlatformSerialPort);
        }
        #endregion

        #region 手动测试下，转台串口接收指令函数
        private void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            myRotatingPlatform.GetFeedbackCommand(RotatingPlatformSerialPort);
        }
        #endregion

    }
}
