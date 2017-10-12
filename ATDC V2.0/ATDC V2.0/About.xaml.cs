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
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        //超链接至公司官网
        private void AirsafeWebsite_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.airsafe.com.cn/");
        }

        //关闭 About 页面
        private void CloseAboutPage_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
