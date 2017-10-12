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
    /// ChineseSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ChineseSetting : Window
    {
        public ChineseSetting()
        {
            InitializeComponent();
        }

        //确认设置
        private void ChineseSettingConfirm_Click(object sender, RoutedEventArgs e)
        {
            LanguageHelper myLanguageHelper = new LanguageHelper();
            myLanguageHelper.LoadLanguageFile("/Resources/Langs/zh-CN.xaml");

            this.Close();
        }

        //取消设置
        private void ChineseSettingCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
