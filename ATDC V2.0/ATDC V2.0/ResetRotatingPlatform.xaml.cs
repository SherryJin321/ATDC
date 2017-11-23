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

namespace ATDC_V2._0
{
    /// <summary>
    /// ResetRotatingPlatform.xaml 的交互逻辑
    /// </summary>
    public partial class ResetRotatingPlatform : Window
    {
        byte[] testArray = new byte[9] { 0, 0, 0x45, 0, 0x83, 0x00, 0x00, 0x01, 0 };


        public ResetRotatingPlatform()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RotatingPlatform myRatatingPlatform = new RotatingPlatform();
            myRatatingPlatform.xyDataEvent += new xyDataHandler(DisplayxyData);
            testArray[8] = myRatatingPlatform.GetCHK(testArray);
            myRatatingPlatform.AnalysisFeedbackCommand(testArray);
        }

        #region xyDataEvent事件处理函数
        public void DisplayxyData(Coordinates xy)
        {
            ResetRotatingPlatformMotorLocationContent.Text = "( " + xy.x.ToString() + " , " + xy.y.ToString() + " )";
        }
        #endregion

        #region 关闭 转台复位 页面
        private void SetRotatingPlatformClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

       
    }
}
