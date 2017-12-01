using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ATDC_V2._0
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        
    }

    public static class ConfigurationParameters
    {
        static public string rotatingPlatformPortName;
        static public string sensorModelName;
        static public string miniCCRModelName;
        static public string miniCCRPortName;
    }
}
