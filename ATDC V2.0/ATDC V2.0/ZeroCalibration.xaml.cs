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
using CL500AClassLib;
using DEVICE_HANDLE = System.IntPtr;
using System.Threading;

namespace ATDC_V2._0
{
    /// <summary>
    /// ZeroCalibration.xaml 的交互逻辑
    /// </summary>
    public partial class ZeroCalibration : Window
    {
        CL500A myCL500A = new CL500A();

        #region 中英文切换字符串
        string stringCL500AZeroCalibrationDoing = (string)System.Windows.Application.Current.FindResource("LangsCL500AZeroCalibrationDoing");
        string stringCL500AZeroCalibrationSuccess = (string)System.Windows.Application.Current.FindResource("LangsCL500AZeroCalibrationSuccess");
        string stringCL500AZeroCalibrationFailure = (string)System.Windows.Application.Current.FindResource("LangsCL500AZeroCalibrationFailure");
        
        #endregion

        #region 实时刷新中英文字符
        public void LanguageRefresh()
        {
            stringCL500AZeroCalibrationDoing = (string)System.Windows.Application.Current.FindResource("LangsCL500AZeroCalibrationDoing");
            stringCL500AZeroCalibrationSuccess = (string)System.Windows.Application.Current.FindResource("LangsCL500AZeroCalibrationSuccess");
            stringCL500AZeroCalibrationFailure = (string)System.Windows.Application.Current.FindResource("LangsCL500AZeroCalibrationFailure");

        }
        #endregion

        public ZeroCalibration()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(ConfigurationParameters.sensorModelName == 1)
            {
                this.IsEnabled = true;
            }
            else
            {
                this.IsEnabled = false;
            }
        }

        #region 关闭 零校正 页面
        private void ZeroCalibrationCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region CL-500A 零校正操作
        Thread ZeroCalibrationThread;
        private void ZeroCalibrationDo_Click(object sender, RoutedEventArgs e)
        {
            LanguageRefresh();

            ZeroCalibrationThread = new Thread(new ThreadStart(CL500AZeroCalibration));

            ZeroCalibrationThread.Start();
            
        }

        public void CL500AZeroCalibration()
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            DEVICE_HANDLE handle = new DEVICE_HANDLE();

            result = myCL500A.OpenDevice(ref handle);
            if (result == OperationStatusCL500A.OpenDeviceSuccess)
            {
                ShowCL500AStatusDisplay(stringCL500AZeroCalibrationDoing);
                result = myCL500A.SetRemoteMode(handle);
                if (result == OperationStatusCL500A.SetRemoteModeSuccess)
                {
                    ShowCL500AStatusDisplay(stringCL500AZeroCalibrationDoing);
                    result = myCL500A.DoCalibration(handle);
                    if(result==OperationStatusCL500A.DoCalibrationSuccess)
                    {
                        ShowCL500AStatusDisplay(stringCL500AZeroCalibrationDoing);
                        result = myCL500A.PollingCalibration(handle);
                        if(result==OperationStatusCL500A.PollingCalibrationSuccess)
                        {
                            ShowCL500AStatusDisplay(stringCL500AZeroCalibrationSuccess);
                            myCL500A.RemoteOffClose(handle);
                            return;
                        }
                        else
                        {
                            ShowCL500AStatusDisplay(stringCL500AZeroCalibrationFailure);
                            return;
                        }
                    }
                    else
                    {
                        ShowCL500AStatusDisplay(stringCL500AZeroCalibrationFailure);
                        return;
                    }                                        
                }
                else
                {
                    ShowCL500AStatusDisplay(stringCL500AZeroCalibrationFailure);
                    return;
                }
            }
            else
            {
                ShowCL500AStatusDisplay(stringCL500AZeroCalibrationFailure);
                return;
            }

            ZeroCalibrationThread.Abort();
        }


        public void ShowCL500AStatusDisplay(string status)
        {
            this.Dispatcher.Invoke(new System.Action(() =>
            {
                ZeroCalibrationStatusContent.Text = status;
            }));            
        }
        #endregion
    }
}
