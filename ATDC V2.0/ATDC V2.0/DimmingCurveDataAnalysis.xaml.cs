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

        #region 需从调光曲线页面获取的参数
        public double[] EVValue { get; set;}
        public double[] CurrentValue { get; set; }
        public double[] MaxValue { get; set; }
        public double[] MinValue { get; set; }
        public double[] Percentage { get; set; }
        public string[] IsFit { get; set; }
        public int GenerateCurveLampColorSelectedIndex { get; set; }
        #endregion

        #region 后台代码，中英文切换字符串
        string DimmingCurveDataAnalysisTitleWhite= (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveDataAnalysisTitleWhite");
        string DimmingCurveDataAnalysisTitleNotWhite = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveDataAnalysisTitleNotWhite");
        #endregion

        public DimmingCurveDataAnalysis()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LanguageSwitch();

            if(GenerateCurveLampColorSelectedIndex==0)
            {
                DimmingCurveTableTitle.Content = DimmingCurveDataAnalysisTitleWhite;
            }
            else
            {
                DimmingCurveTableTitle.Content = DimmingCurveDataAnalysisTitleNotWhite;
            }

            ObservableCollection<Member> MemberData = new ObservableCollection<Member>();                      

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

        public void LanguageSwitch()
        {
            DimmingCurveDataAnalysisTitleWhite = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveDataAnalysisTitleWhite");
            DimmingCurveDataAnalysisTitleNotWhite = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveDataAnalysisTitleNotWhite");
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
