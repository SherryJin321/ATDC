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
using System.Collections.ObjectModel;

namespace ATDC_V2._0
{
    /// <summary>
    /// DimmingCurveDataAnalysis.xaml 的交互逻辑
    /// </summary>
    public partial class DimmingCurveDataAnalysis : Window
    {

        #region 需从调光曲线页面获取的参数值
        public int GenerateCurveLampColorSelectdIndex { get; set; }
        public double[] EVValue { get; set;}
        #endregion

        #region 调光曲线表中，使用的参数
        public double[] CurrentValue = new double[41] { 2.7, 2.8, 2.9, 3.0, 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7, 3.8, 3.9, 4.0, 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7, 4.8, 4.9, 5, 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7, 5.8, 5.9, 6.0, 6.1, 6.2, 6.3, 6.4, 6.5, 6.6, 6.7 };
        double[] MaxValue = new double[41];
        double[] MinValue = new double[41];
        double[] Percentage = new double[41];
        string[] IsFit = new string[41];
        #endregion

        #region 辅助参数
        double EVValueMax = 0;
        #endregion

        public DimmingCurveDataAnalysis()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Member> MemberData = new ObservableCollection<Member>();          

            if (GenerateCurveLampColorSelectdIndex == 0)
            {
                MaxValue = new double[41] { 0.7, 0.7, 0.92, 1.14, 1.35, 1.57, 1.78, 2.1, 2.75, 3.5, 4.25, 5, 5.75, 6.5, 7.35, 8.45, 9.76, 11.23, 12.89, 14.75, 16.83, 19.15, 21.73, 24.59, 27.76, 31.27, 35.15, 39.41, 44.11, 49.26, 54.9, 61.08, 67.83, 75.19, 83.21, 91.93, 100, 100, 100, 100, 100 };
                MinValue = new double[41] { 0.15, 0.15, 0.28, 0.41, 0.54, 0.67, 0.8, 1, 1.36, 1.79, 2.22, 2.65, 3.08, 3.51, 3.94, 4.57, 5.28, 6.08, 6.98, 7.99, 9.11, 10.37, 11.76, 13.31, 15.03, 16.93, 19.03, 21.34, 23.88, 26.67, 29.73, 33.07, 36.73, 40.71, 45.05, 49.77, 58.13, 71.58, 85.18, 100, 100 };
            }
            else
            {
                MaxValue = new double[41] { 1.6, 1.65, 1.8, 2.01, 2.23, 2.44, 2.65, 3, 3.7, 4.75, 5.8, 6.85, 7.9, 8.95, 10, 11.17, 12.33, 13.5, 14.67, 15.83, 17, 19, 24, 31.5, 36, 39.75, 43.5, 47.25, 51, 56, 62.29, 68.57, 74.86, 81.14, 87.43, 93.71, 100, 100, 100, 100, 100 };
                MinValue = new double[41] { 0.13, 0.15, 0.33, 0.5, 0.68, 0.85, 1.03, 1.2, 1.57, 1.93, 2.3, 2.9, 3.55, 4.28, 5, 5.71, 6.43, 7.14, 7.86, 8.57, 9.29, 10, 11.25, 12.5, 14, 16.8, 21.2, 25.6, 30, 33, 36, 39, 42, 45, 48, 53, 62, 74.67, 87.33, 100, 100 };
            }

            EVValueMax = EVValue[40];
            for(int i=0;i<40;i++)
            {
                if(EVValueMax<EVValue[i])
                {
                    EVValueMax = EVValue[i];
                }
            }

            for(int i=0;i<41;i++)
            {
                Percentage[i] = Math.Round(EVValue[i] / EVValueMax * 100,3);

                if(Percentage[i]>=MinValue[i]&&Percentage[i]<=MaxValue[i])
                {
                    IsFit[i] = "Y";
                }
                else
                {
                    IsFit[i] = "N";
                }
            }

            for(int i=0;i<41;i++)
            {
                MemberData.Add(new Member()
                {
                    MemberCurrentValue =CurrentValue[i],
                    MemberEVValue= EVValue[i],            
                    MemberPercentage=Percentage[i],         
                    MemberMaxValue=MaxValue[i],
                    MemberMinValue=MinValue[i],
                    MemberIsFit=IsFit[i]             
                });                   
            }

            DimmingCurveDataTable.DataContext = MemberData;
        }
    }

    public class Member
    {
        public double MemberCurrentValue { get; set; }
        public double MemberEVValue { get; set; }
        public double MemberPercentage { get; set; }
        public double MemberMaxValue { get; set; }
        public double MemberMinValue { get; set; }
        public string MemberIsFit { get; set; }
    }
}
