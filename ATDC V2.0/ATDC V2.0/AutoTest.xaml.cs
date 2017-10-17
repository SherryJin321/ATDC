﻿using System;
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

namespace ATDC_V2._0
{
    /// <summary>
    /// AutoTest.xaml 的交互逻辑
    /// </summary>
    public partial class AutoTest : Page
    {
        double[] EVValue = new double[41];

        public AutoTest()
        {
            InitializeComponent();
        }

        #region 进入 调光曲线 页面
        private void AutoTestGenerateCurve_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0;i<41;i++)
            {
                EVValue[i] = i+1;
            }

            GenerateCurve GenerateCurveWindow = new GenerateCurve();
            GenerateCurveWindow.EVValue = new double[41];
            for (int i=0;i<41;i++)
            {
                GenerateCurveWindow.EVValue[i] = EVValue[i];
            }

            GenerateCurveWindow.ShowDialog();
        }
        #endregion
    }
}
