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
    /// ResetRotatingPlatform.xaml 的交互逻辑
    /// </summary>
    public partial class ResetRotatingPlatform : Window
    {
        SerialPort SerialPortRotatingPlatform = new SerialPort();
        RotatingPlatform myRotatingPlatform = new RotatingPlatform();
        OperationStatusRotatingPlatform myOperationStatusRotatingPlatform = OperationStatusRotatingPlatform.OriginalStatus;

        #region 后台代码，中英文切换字符串
        string tip1 = (string)System.Windows.Application.Current.FindResource("LangsSetRotatingPlatformTip1");
        string tip2 = (string)System.Windows.Application.Current.FindResource("LangsSetRotatingPlatformTip2");
        #endregion

        #region 刷新中英文字符
        public void RefreshLanguageString()
        {
            tip1 = (string)System.Windows.Application.Current.FindResource("LangsSetRotatingPlatformTip1");
            tip2 = (string)System.Windows.Application.Current.FindResource("LangsSetRotatingPlatformTip2");
        }
        #endregion


        public ResetRotatingPlatform()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myRotatingPlatform.xyDataEvent += new xyDataHandler(DisplayxyData);
            SerialPortRotatingPlatform.DataReceived += new SerialDataReceivedEventHandler(SerialPortRotatingPlatform_DateReceived);           
        }

        #region xyDataEvent事件处理函数
        public void DisplayxyData(Coordinates xy)
        {
            this.Dispatcher.Invoke(new System.Action(() =>
            {
                ResetRotatingPlatformMotorLocationContent.Text = "( " + xy.x.ToString() + " , " + xy.y.ToString() + " )";
            })); 
        }
        #endregion

        #region DataReceived事件处理函数
        private void SerialPortRotatingPlatform_DateReceived(object sender, SerialDataReceivedEventArgs e)
        {
            myOperationStatusRotatingPlatform = myRotatingPlatform.GetFeedbackCommand(SerialPortRotatingPlatform);
            Shows();
        }
        #endregion


        #region 关闭 转台复位 页面
        private void SetRotatingPlatformClose_Click(object sender, RoutedEventArgs e)
        {
            myRotatingPlatform.ClosePort(SerialPortRotatingPlatform);
            
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            myRotatingPlatform.ClosePort(SerialPortRotatingPlatform);
        }

        #endregion

        #region 转台复位
        private void ResetRotatingPlatformDo_Click(object sender, RoutedEventArgs e)
        {
            myOperationStatusRotatingPlatform = myRotatingPlatform.OpenPort(SerialPortRotatingPlatform, ConfigurationParameters.rotatingPlatformPortName);
            Shows();

            myOperationStatusRotatingPlatform = myRotatingPlatform.ResetRotatingPlatform(SerialPortRotatingPlatform);
            Shows();           
        }
        #endregion

        #region 显示通讯状态
        public void Shows()
        {
            this.Dispatcher.Invoke(new System.Action(() =>
            {
                ResetRotatingPlatformStatus.Text = myOperationStatusRotatingPlatform.ToString();
            }));
        }
        #endregion

        #region 获取电机位置
        private void ResetRotatingPlatformGetMotorLocation_Click(object sender, RoutedEventArgs e)
        {
            myOperationStatusRotatingPlatform = myRotatingPlatform.OpenPort(SerialPortRotatingPlatform, ConfigurationParameters.rotatingPlatformPortName);
            Shows();

            myOperationStatusRotatingPlatform = myRotatingPlatform.GetDegree(SerialPortRotatingPlatform);
            Shows();
        }

        #endregion

        #region 向上
        private void ResetRotatingPlatformUp_Click(object sender, RoutedEventArgs e)
        {
            RefreshLanguageString();

            try
            {
                Int16 degree =Convert.ToInt16(ResetRotatingPlatformUpContent.Text);

                if(degree>=-32000&&degree<=32000)
                {
                    byte degreeH = Convert.ToByte(degree >> 8);
                    byte degreeL = Convert.ToByte(degree&0x00ff);

                    myOperationStatusRotatingPlatform = myRotatingPlatform.OpenPort(SerialPortRotatingPlatform, ConfigurationParameters.rotatingPlatformPortName);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.SetMotorStatus(SerialPortRotatingPlatform,1,1,degreeH,degreeL);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.StartMotor(SerialPortRotatingPlatform, 1);
                    Shows();
                }
                else
                {
                    this.Dispatcher.Invoke(new System.Action(() =>
                    {
                        ResetRotatingPlatformStatus.Text = tip2;
                    }));
                }
            }
            catch
            {
                this.Dispatcher.Invoke(new System.Action(() =>
                {
                    ResetRotatingPlatformStatus.Text = tip1;
                }));
            }           
        }
        #endregion

        #region 向下                
        private void ResetRotatingPlatformDown_Click(object sender, RoutedEventArgs e)
        {
            RefreshLanguageString();

            try
            {
                Int16 degree = Convert.ToInt16(ResetRotatingPlatformUpContent.Text);

                if (degree >= -32000 && degree <= 32000)
                {
                    byte degreeH = Convert.ToByte(degree >> 8);
                    byte degreeL = Convert.ToByte(degree&0x00ff);

                    myOperationStatusRotatingPlatform = myRotatingPlatform.OpenPort(SerialPortRotatingPlatform, ConfigurationParameters.rotatingPlatformPortName);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.SetMotorStatus(SerialPortRotatingPlatform, 1, 0, degreeH, degreeL);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.StartMotor(SerialPortRotatingPlatform, 1);
                    Shows();
                }
                else
                {
                    this.Dispatcher.Invoke(new System.Action(() =>
                    {
                        ResetRotatingPlatformStatus.Text = tip2;
                    }));
                }
            }
            catch
            {
                this.Dispatcher.Invoke(new System.Action(() =>
                {
                    ResetRotatingPlatformStatus.Text = tip1;
                }));
            }
        }

        #endregion

        #region 向左
        private void ResetRotatingPlatformLeft_Click(object sender, RoutedEventArgs e)
        {
            RefreshLanguageString();

            try
            {
                Int16 degree = Convert.ToInt16(ResetRotatingPlatformUpContent.Text);

                if (degree >= -32000 && degree <= 32000)
                {
                    byte degreeH = Convert.ToByte(degree >> 8);
                    byte degreeL = Convert.ToByte(degree&0x00ff);

                    myOperationStatusRotatingPlatform = myRotatingPlatform.OpenPort(SerialPortRotatingPlatform, ConfigurationParameters.rotatingPlatformPortName);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.SetMotorStatus(SerialPortRotatingPlatform, 0, 0, degreeH, degreeL);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.StartMotor(SerialPortRotatingPlatform, 0);
                    Shows();
                }
                else
                {
                    this.Dispatcher.Invoke(new System.Action(() =>
                    {
                        ResetRotatingPlatformStatus.Text = tip2;
                    }));
                }
            }
            catch
            {
                this.Dispatcher.Invoke(new System.Action(() =>
                {
                    ResetRotatingPlatformStatus.Text = tip1;
                }));
            }
        }
        #endregion

        #region 向右
        private void ResetRotatingPlatformRight_Click(object sender, RoutedEventArgs e)
        {
            RefreshLanguageString();

            try
            {
                Int16 degree = Convert.ToInt16(ResetRotatingPlatformUpContent.Text);

                if (degree >= -32000 && degree <= 32000)
                {
                    byte degreeH = Convert.ToByte(degree >> 8);
                    byte degreeL = Convert.ToByte(degree&0x00ff);

                    myOperationStatusRotatingPlatform = myRotatingPlatform.OpenPort(SerialPortRotatingPlatform, ConfigurationParameters.rotatingPlatformPortName);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.SetMotorStatus(SerialPortRotatingPlatform, 0, 1, degreeH, degreeL);
                    Shows();

                    myOperationStatusRotatingPlatform = myRotatingPlatform.StartMotor(SerialPortRotatingPlatform, 0);
                    Shows();
                }
                else
                {
                    this.Dispatcher.Invoke(new System.Action(() =>
                    {
                        ResetRotatingPlatformStatus.Text = tip2;
                    }));
                }
            }
            catch
            {
                this.Dispatcher.Invoke(new System.Action(() =>
                {
                    ResetRotatingPlatformStatus.Text = tip1;
                }));
            }
        }
        #endregion   
    }
}
