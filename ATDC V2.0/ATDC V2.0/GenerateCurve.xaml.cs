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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;


namespace ATDC_V2._0
{
    /// <summary>
    /// GenerateCurve.xaml 的交互逻辑
    /// </summary>
    public partial class GenerateCurve : Window
    {

        #region 需从自动测试页面 或 手动测试页面获取的参数
        public double[] EVValue { get; set; }
        #endregion

        #region 绘制调光曲线用到的参数
        public double EVValueMax = 0;
        public double[] Percentage = new double[41];
        public double[] MaxValue = new double[41];
        public double[] MinValue = new double[41];
        public string[] IsFit = new string[41];
        public bool FinalResult=true;
        #endregion

        #region 绘制调光曲线模板参数
        private ObservableDataSource<Point> DataSourceWhiteLightMinimum = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> DataSourceWhiteLightMaximum = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> DataSourceNotWhiteLightMinimum = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> DataSourceNotWhiteLightMaximum = new ObservableDataSource<Point>();

        private LineGraph graphWhiteLightMinimum = new LineGraph();
        private LineGraph graphWhiteLightMaximum = new LineGraph();
        private LineGraph graphNotWhiteLightMinimum = new LineGraph();
        private LineGraph graphNotWhiteLightMaximum = new LineGraph();

        public double[] CurrentValue = new double[41] {2.7,2.8,2.9,3.0,3.1,3.2,3.3,3.4,3.5,3.6,3.7,3.8,3.9,4.0,4.1,4.2,4.3,4.4,4.5,4.6,4.7,4.8,4.9,5,5.1,5.2,5.3,5.4,5.5,5.6,5.7,5.8,5.9,6.0,6.1,6.2,6.3,6.4,6.5,6.6,6.7 };
        double[] DataWhiteLightMinimum = new double[41] { 0.15, 0.15, 0.28, 0.41, 0.54, 0.67, 0.8, 1, 1.36, 1.79, 2.22, 2.65, 3.08, 3.51, 3.94, 4.57, 5.28, 6.08, 6.98, 7.99, 9.11, 10.37, 11.76, 13.31, 15.03, 16.93, 19.03, 21.34, 23.88, 26.67, 29.73, 33.07, 36.73, 40.71, 45.05, 49.77, 58.13, 71.58, 85.18, 100, 100 };
        double[] DataWhiteLightMaximum = new double[41] { 0.7, 0.7, 0.92, 1.14, 1.35, 1.57, 1.78, 2.1, 2.75, 3.5, 4.25, 5, 5.75, 6.5, 7.35, 8.45, 9.76, 11.23, 12.89, 14.75, 16.83, 19.15, 21.73, 24.59, 27.76, 31.27, 35.15, 39.41, 44.11, 49.26, 54.9, 61.08, 67.83, 75.19, 83.21, 91.93, 100, 100, 100, 100, 100 };
        double[] DataNotWhiteLightMinimum = new double[41] { 0.13, 0.15, 0.33, 0.5, 0.68, 0.85, 1.03, 1.2, 1.57, 1.93, 2.3, 2.9, 3.55, 4.28, 5, 5.71, 6.43, 7.14, 7.86, 8.57, 9.29, 10, 11.25, 12.5, 14, 16.8, 21.2, 25.6, 30, 33, 36, 39, 42, 45, 48, 53, 62, 74.67, 87.33, 100, 100 };
        double[] DataNotWhiteLightMaximum = new double[41] { 1.6, 1.65, 1.8, 2.01, 2.23, 2.44, 2.65, 3, 3.7, 4.75, 5.8, 6.85, 7.9, 8.95, 10, 11.17, 12.33, 13.5, 14.67, 15.83, 17, 19, 24, 31.5, 36, 39.75, 43.5, 47.25, 51, 56, 62.29, 68.57, 74.86, 81.14, 87.43, 93.71, 100, 100, 100, 100, 100 };
        #endregion

        #region 绘制调光曲线参数
        private ObservableDataSource<Point> DataSourceDimmingCurveEVValue = new ObservableDataSource<Point>();
        private LineGraph graphDimmingCurveEVValue = new LineGraph();
        #endregion

        #region 后台代码，中英文切换字符串
        string DimmingCurveFinalResultFit=(string)System.Windows.Application.Current.FindResource("LangsDimmingCurveFinalResultFit");
        string DimmingCurveFinalResultNotFit = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveFinalResultNotFit");
        string DimmingCurveLabelMax = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveLabelMax");
        string DimmingCurveLabelMin = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveLabelMin");
        string DimmingCurveLabelTestValue = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveLabelTestValue");
        string DimmingCurveLabelTestHeaderWhite = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveChartHeaderWhite");
        string DimmingCurveLabelTestHeaderNotWhite = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveChartHeaderNotWhite");

        #endregion

        public GenerateCurve()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            DataSourceWhiteLightMinimumFunction();
            DataSourceWhiteLightMaximumFunction();            

            DrawingDimmingCurve();
        }

        #region 绘制调光曲线
        public void DrawingDimmingCurve()
        {
            EVValueMax = EVValue[40];
            for (int i = 0; i < 40; i++)
            {
                if (EVValueMax < EVValue[i])
                {
                    EVValueMax = EVValue[i];
                }
            }

            for(int i=0;i<41;i++)
            {
                MinValue[i] = DataWhiteLightMinimum[i];
                MaxValue[i] = DataWhiteLightMaximum[i];
            }

            for (int i = 0; i < 41; i++)
            {
                Percentage[i] = Math.Round(EVValue[i] / EVValueMax * 100, 3);
                if (Percentage[i] >= MinValue[i] && Percentage[i] <= MaxValue[i])
                {
                    IsFit[i] = "Y";
                }
                else
                {
                    IsFit[i] = "N";
                    FinalResult = false;
                }
            }

            if (FinalResult == true)
            {
                GenerateCurveResultContent.Text = DimmingCurveFinalResultFit;
            }
            else
            {
                GenerateCurveResultContent.Text = DimmingCurveFinalResultNotFit;
            }
            DimmingCurveChartHeader.Content = DimmingCurveLabelTestHeaderWhite;

            graphDimmingCurveEVValue = DimmingCurveChart.AddLineGraph(DataSourceDimmingCurveEVValue, Colors.Black, 2, DimmingCurveLabelTestValue);
            for (int i = 0; i < 41; i++)
            {
                Point point = new Point(CurrentValue[i], Percentage[i]);
                DataSourceDimmingCurveEVValue.AppendAsync(base.Dispatcher, point);
            }
        }
        #endregion

        #region 选择调光曲线模板
        private void GenerateCurveLampColorSelectConfirm_Click(object sender, RoutedEventArgs e)
        {
            LanguageRefresh();

            DimmingCurveChart.Children.Remove(graphWhiteLightMinimum);
            DimmingCurveChart.Children.Remove(graphWhiteLightMaximum);
            DimmingCurveChart.Children.Remove(graphNotWhiteLightMinimum);
            DimmingCurveChart.Children.Remove(graphNotWhiteLightMaximum);

            DataSourceWhiteLightMinimum = new ObservableDataSource<Point>();
            DataSourceWhiteLightMaximum = new ObservableDataSource<Point>();
            DataSourceNotWhiteLightMinimum = new ObservableDataSource<Point>();
            DataSourceNotWhiteLightMaximum = new ObservableDataSource<Point>();

            if (GenerateCurveLampColorSelect.SelectedIndex==0)
            {                
                DataSourceWhiteLightMaximumFunction();
                DataSourceWhiteLightMinimumFunction();
                MaxValue = new double[41] { 0.7, 0.7, 0.92, 1.14, 1.35, 1.57, 1.78, 2.1, 2.75, 3.5, 4.25, 5, 5.75, 6.5, 7.35, 8.45, 9.76, 11.23, 12.89, 14.75, 16.83, 19.15, 21.73, 24.59, 27.76, 31.27, 35.15, 39.41, 44.11, 49.26, 54.9, 61.08, 67.83, 75.19, 83.21, 91.93, 100, 100, 100, 100, 100 };
                MinValue = new double[41] { 0.15, 0.15, 0.28, 0.41, 0.54, 0.67, 0.8, 1, 1.36, 1.79, 2.22, 2.65, 3.08, 3.51, 3.94, 4.57, 5.28, 6.08, 6.98, 7.99, 9.11, 10.37, 11.76, 13.31, 15.03, 16.93, 19.03, 21.34, 23.88, 26.67, 29.73, 33.07, 36.73, 40.71, 45.05, 49.77, 58.13, 71.58, 85.18, 100, 100 };
                DimmingCurveChartHeader.Content = DimmingCurveLabelTestHeaderWhite;
            }
            else
            {                
                DataSourceNotWhiteLightMaximumFunction();
                DataSourceNotWhiteLightMinimumFunction();
                MaxValue = new double[41] { 1.6, 1.65, 1.8, 2.01, 2.23, 2.44, 2.65, 3, 3.7, 4.75, 5.8, 6.85, 7.9, 8.95, 10, 11.17, 12.33, 13.5, 14.67, 15.83, 17, 19, 24, 31.5, 36, 39.75, 43.5, 47.25, 51, 56, 62.29, 68.57, 74.86, 81.14, 87.43, 93.71, 100, 100, 100, 100, 100 };
                MinValue = new double[41] { 0.13, 0.15, 0.33, 0.5, 0.68, 0.85, 1.03, 1.2, 1.57, 1.93, 2.3, 2.9, 3.55, 4.28, 5, 5.71, 6.43, 7.14, 7.86, 8.57, 9.29, 10, 11.25, 12.5, 14, 16.8, 21.2, 25.6, 30, 33, 36, 39, 42, 45, 48, 53, 62, 74.67, 87.33, 100, 100 };
                DimmingCurveChartHeader.Content = DimmingCurveLabelTestHeaderNotWhite;
            }

            CaculatingDimmingCurveResult();
        } 

        #region 调光曲线模板
        //白色，最小曲线
        public void DataSourceWhiteLightMinimumFunction()
        {
            graphWhiteLightMinimum=DimmingCurveChart.AddLineGraph(DataSourceWhiteLightMinimum, Colors.Blue, 2, DimmingCurveLabelMin);

            for (int i = 0; i < 41; i++)
            {
                Point point = new Point(CurrentValue[i], DataWhiteLightMinimum[i]);
                DataSourceWhiteLightMinimum.AppendAsync(base.Dispatcher, point);
            }
        }

        //白色，最大曲线
        public void DataSourceWhiteLightMaximumFunction()
        {
            graphWhiteLightMaximum=DimmingCurveChart.AddLineGraph(DataSourceWhiteLightMaximum, Colors.Red, 2, DimmingCurveLabelMax);

            for (int i = 0; i < 41; i++)
            {
                Point point = new Point(CurrentValue[i], DataWhiteLightMaximum[i]);
                DataSourceWhiteLightMaximum.AppendAsync(base.Dispatcher, point);
            }
        }

        //非白色，最小曲线
        public void DataSourceNotWhiteLightMinimumFunction()
        {
            graphNotWhiteLightMinimum=DimmingCurveChart.AddLineGraph(DataSourceNotWhiteLightMinimum, Colors.Blue, 2, DimmingCurveLabelMin);

            for (int i = 0; i < 41; i++)
            {
                Point point = new Point(CurrentValue[i], DataNotWhiteLightMinimum[i]);
                DataSourceNotWhiteLightMinimum.AppendAsync(base.Dispatcher, point);
            }
        }

        //非白色，最大曲线
        public void DataSourceNotWhiteLightMaximumFunction()
        {
            graphNotWhiteLightMaximum=DimmingCurveChart.AddLineGraph(DataSourceNotWhiteLightMaximum, Colors.Red, 2, DimmingCurveLabelMax);

            for (int i = 0; i < 41; i++)
            {
                Point point = new Point(CurrentValue[i], DataNotWhiteLightMaximum[i]);
                DataSourceNotWhiteLightMaximum.AppendAsync(base.Dispatcher, point);
            }
        }
        #endregion

        #region 计算调光曲线结果        
        public void CaculatingDimmingCurveResult()
        {
            IsFit = new string[41];
            FinalResult = true;
            for (int i = 0; i < 41; i++)
            {
                if (Percentage[i] >= MinValue[i] && Percentage[i] <= MaxValue[i])
                {
                    IsFit[i] = "Y";
                }
                else
                {
                    IsFit[i] = "N";
                    FinalResult = false;
                }
            }

            if (FinalResult == true)
            {
                GenerateCurveResultContent.Text = DimmingCurveFinalResultFit;
            }
            else
            {
                GenerateCurveResultContent.Text = DimmingCurveFinalResultNotFit;
            }
        }
        #endregion

        #endregion

        #region 打开 数据分析 页面，并传递灯具颜色参数、测得的EV值参数
        private void GenerateCurveCheckDetailResult_Click(object sender, RoutedEventArgs e)
        {            
            DimmingCurveDataAnalysis DimmingCurveDataAnalysisWindow = new DimmingCurveDataAnalysis();
            DimmingCurveDataAnalysisWindow.GenerateCurveLampColorSelectedIndex = GenerateCurveLampColorSelect.SelectedIndex;
            DimmingCurveDataAnalysisWindow.EVValue = new double[41];
            DimmingCurveDataAnalysisWindow.CurrentValue = new double[41];
            DimmingCurveDataAnalysisWindow.Percentage = new double[41];
            DimmingCurveDataAnalysisWindow.MaxValue = new double[41];
            DimmingCurveDataAnalysisWindow.MinValue = new double[41];
            DimmingCurveDataAnalysisWindow.IsFit = new string[41];

            for (int i = 0; i < 41; i++)
            {
                DimmingCurveDataAnalysisWindow.EVValue[i] = EVValue[i];
                DimmingCurveDataAnalysisWindow.CurrentValue[i] = CurrentValue[i];
                DimmingCurveDataAnalysisWindow.Percentage[i] = Percentage[i];
                DimmingCurveDataAnalysisWindow.MaxValue[i] = MaxValue[i];
                DimmingCurveDataAnalysisWindow.MinValue[i] = MinValue[i];
                DimmingCurveDataAnalysisWindow.IsFit[i] = IsFit[i];
            }

            DimmingCurveDataAnalysisWindow.ShowDialog();
        }
        #endregion

        #region 实时刷新中英文字符
        public void LanguageRefresh()
        {
            DimmingCurveFinalResultFit = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveFinalResultFit");
            DimmingCurveFinalResultNotFit = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveFinalResultNotFit");
            DimmingCurveLabelMax = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveLabelMax");
            DimmingCurveLabelMin = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveLabelMin");
            DimmingCurveLabelTestValue = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveLabelTestValue");
            DimmingCurveLabelTestHeaderWhite = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveChartHeaderWhite");
            DimmingCurveLabelTestHeaderNotWhite = (string)System.Windows.Application.Current.FindResource("LangsDimmingCurveChartHeaderNotWhite");
        }
        #endregion

    }
}
