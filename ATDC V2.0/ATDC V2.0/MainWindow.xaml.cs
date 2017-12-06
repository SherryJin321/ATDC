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
using System.IO.Ports;

namespace ATDC_V2._0
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {               

        public MainWindow()
        {
            InitializeComponent();
        }


        #region 打开页面
        #region 打开 About 页面
        private void HelpSubMenuAbout_Click(object sender, RoutedEventArgs e)
        {
            About AboutWindow = new About();
            AboutWindow.ShowDialog();
        }
        #endregion

        #region 打开 语言设置（中文） 页面
        private void LanguageSubMenuChinese_Click(object sender, RoutedEventArgs e)
        {
            ChineseSetting ChineseSettingWindow = new ChineseSetting();
            ChineseSettingWindow.ShowDialog();
        }
        #endregion

        #region 打开 语言设置（英文） 页面
        private void LanguageSubMenuEnglish_Click(object sender, RoutedEventArgs e)
        {
            EnglishSetting EnglishSettingWindow = new EnglishSetting();
            EnglishSettingWindow.ShowDialog();
        }
        #endregion

        #region 打开 设备 页面
        private void ConnectSubMenuDevices_Click(object sender, RoutedEventArgs e)
        {
            Devices DevicesWindow = new Devices();
            DevicesWindow.ShowDialog();
        }
        #endregion

        #region 打开 零校正 页面
        private void FunctionsSubMenuZeroCalibration_Click(object sender, RoutedEventArgs e)
        {
            ZeroCalibration ZeroCalibrationWindow = new ZeroCalibration();
            ZeroCalibrationWindow.ShowDialog();
        }
        #endregion

        #region 打开 转台复位 页面
        private void FunctionsSubMenuSetRotatingPlatform_Click(object sender, RoutedEventArgs e)
        {
            ResetRotatingPlatform ResetRotatingPlatformWindow = new ResetRotatingPlatform();
            ResetRotatingPlatformWindow.ShowDialog();
        }
        #endregion

        #region 打开 手动测试 页面
        private void FunctionsSubMenuManualTest_Click(object sender, RoutedEventArgs e)
        {

            this.SwitchPage.Navigate(new Uri("ManualTest.xaml", UriKind.Relative));
        }
        #endregion

        #region 打开 自动测试 页面
        private void FunctionsSubMenuAutoTest_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchPage.Navigate(new Uri("AutoTest.xaml", UriKind.Relative));
        }

        #endregion

        #endregion

        
    }
}
