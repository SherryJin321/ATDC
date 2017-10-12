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
        public ResetRotatingPlatform()
        {
            InitializeComponent();
        }

        #region 关闭 转台复位 页面
        private void SetRotatingPlatformClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}
