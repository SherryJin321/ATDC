using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;



namespace CL500AClassLib
{
     public class clconditions
    {        
        //===========================
        // The definition value
        //===========================

        // Number of elements on colorimetric value
        public const int CL_RENDERING_LEN = 16;      //!< The buffer size of color rendering index
        public const int CL_USERCUSTOM_LEN = 4;      //!< The number of data which can specify on custom color mode

        /// <summary>
        /// The enumeration of the remote mode
        /// </summary>
        public enum CL_REMOTEMODE
        {
            CL_RMODE_OFF,       //!< A remote mode is OFF
            CL_RMODE_ON,        //!< A remote mode is ON
            CL_RMODE_NUM,

            CL_RMODE_FIRST = CL_RMODE_OFF,
            CL_RMODE_LAST = CL_RMODE_ON,
            CL_RMODE_DEFAULT = CL_RMODE_OFF
        }

        /// <summary>
        /// The enumeration of the implementation status of zero calibration
        /// </summary>
        public enum CL_CALIBMEASSTATUS
        {
            CL_CALIBMEAS_FREE,      //!< A zero calibration is not performed
            CL_CALIBMEAS_BUSY,      //!< A zero calibration is performing
            CL_CALIBMEAS_FINISH     //!< A zero calibration is completed
        }

        /// <summary>
        /// The enumeration of the measurement condition by the CL-SDK
        /// </summary>
        public enum CL_PROPERTIES
        {
            CL_PR_OBSERVER,         //!< Observer
            CL_PR_ILLUNIT,          //!< Illuminance units
            CL_PR_NUM,

            CL_PR_FIRST = CL_PR_OBSERVER,
            CL_PR_LAST = CL_PR_ILLUNIT
        }        

        /// <summary>
        /// The enumeration of the illuminance units
        /// </summary>
        public enum CL_ILLUMINANT_UNIT
        {
            CL_ILLUNIT_LX = 0,              //!< lux
            CL_ILLUNIT_FCD,                 //!< foot-candela
            CL_ILLUNIT_NUM,

            CL_ILLUNIT_DEFAULT = CL_ILLUNIT_LX,
            CL_ILLUNIT_FIRST = CL_ILLUNIT_LX,
            CL_ILLUNIT_LAST = CL_ILLUNIT_FCD
        }

        /// <summary>
        /// The enumeration of the implementation status of the measurement
        /// </summary>
        public enum CL_MEASSTATUS
        {
            CL_MEAS_FREE,           //!< A measurement is not performed
            CL_MEAS_BUSY,           //!< A measurement is performing
            CL_MEAS_FINISH          //!< A measurement is completed
        }

        /// <summary>
        /// The union of the measurement data
        /// </summary>        
        public struct CL_MEASDATA        
        {          
            public CL_EvxyDATA Evxy;                       //!< Ev/x/y data           
            public CL_EvuvDATA Evuv;                       //!< Ev/u乫/v            
            public CL_EvTduvDATA EvTduv;                   //!< Ev/Tcp/delta uv           
            public CL_EvDWPeDATA EvDWPe;                   //!< Ev/Dominant wavelength /Excitation purity            
            public CL_XYZDATA XYZ;                         //!< X/Y/Z           
            public CL_RenderingDATA Rendering;             //!< Color rendering index            
            public CL_PWDATA Pw;                           //!< Peak wavelength            
            public CL_SPCDATA Spc;                         //!< Illuminance spectral data           
            public CL_ScotopicDATA Scotopic;               //!< Ev/Ev'/SP
        }

        //--------------------
        // Measurement data
        //--------------------

        /// <summary>
        /// The structure of Evxy data
        /// </summary>
        public struct CL_EvxyDATA
        {
            public float Ev;           //!< Ev
            public float x;            //!< x
            public float y;            //!< y
        }

        /// <summary>
        /// instrument information structure
        /// </summary>
        public struct CL_EvuvDATA
        {
            public float Ev;           //!< Ev
            public float u;            //!< u乫
            public float v;            //!< v乫
        }

        /// <summary>
        /// The structure of Ev/Correlated color temperature/delta uv data
        /// </summary>
        public struct CL_EvTduvDATA
        {
            public float Ev;           //!< Ev data
            public float T;            //!< Correlated color temperature
            public float duv;          //!< delta uv
        }

        /// <summary>
        /// The structure of Ev/Dominant wavelength/Excitation purity data
        /// </summary>
        public struct CL_EvDWPeDATA
        {
            public float Ev;           //!< Ev data
            public float DW;           //!< Dominant wavelength
            public float Pe;           //!< Excitation purity
        }

        /// <summary>
        /// The structure of XYZ data
        /// </summary>
        public struct CL_XYZDATA
        {
            public float X;            //!< X
            public float Y;            //!< Y
            public float Z;            //!< Z
        }

        /// <summary>
        /// The structure of color rendering index
        /// </summary>

        public struct CL_RenderingDATA
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CL_RENDERING_LEN)]
            public float[] Data;           //!< The color rendering index
        }

        /// <summary>
        /// The structure of Peak wavelength
        /// </summary>
        public struct CL_PWDATA
        {
            public float PeakWave;     //!< Peak wavelength
        }

        /// <summary>
        /// The structure of illuminance spectral data
        /// </summary>
        public struct CL_SPCDATA
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]      //SizeConst的值与C++开发包提供的值不一致
            public float[] Data;          //!< Illuminance spectral data
        }

        /// <summary>
        ///  Th e structure of Scotopic lux data+
        ///  01
        /// </summary>
        public struct CL_ScotopicDATA
        {
            public float Ev;           //!< Ev  Photopic lux data
            public float Es;           //!< Ev' Scotopic lux data
            public float SP;           //!< S/P ratio
        }

        /// <summary>
        /// The structure of the measurement condition in standalone
        /// </summary>
        public struct CL_MEASSETTING
        {
            public CL_DISP_TYPES DispType;                             //!< Display type
            public clcolorconditions.CL_OBSERVER Obs;                  //!< Observer in standalone measurement
            public CL_COLOR_MODE ColorSpace;                           //!< Color mode in standalone measurement
            public CL_ILLUMINANT_UNIT IlluminantUnit;                  //!< Illuminance units in the standalone measurement
            public CL_MEASUREMENT_TIME ExposureTime;                   //!< The measurement time in standalone measurement
            public int UserCalibCh;                                    //!< The user calibration CH in standalone measurement
            public int TargetNo;                                       //!< The target data No. in standalone measurement
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CL_USERCUSTOM_LEN)]
            public CL_CUSTOMDATA_ITEM[] UserCustom;                    //!< The items which display in custom color mode
            public CL_MEASMODE MeasMode;                               //!< measurement mode ( SINGLE, MEAN, MULTI )
            public CL_TIMERCONF TimerConf;                             //!< timer configuration ( delay, interval, the num of measurements )
            public CL_USERWAVELENGTH UserWavelength;                   //!< arbitary wavelengths

        }

        /// <summary>
        /// The enumeration of the display type on the instrument
        /// </summary>
        public enum CL_DISP_TYPES
        {
            CL_DISP_ABS,        //!< Absolute
            CL_DISP_DIFF,       //!< Difference
            CL_DISP_RANK,       //!< Select Rank
            CL_DISP_NUM,

            CL_DISP_DEFAULT = CL_DISP_ABS,
            CL_DISP_FIRST = CL_DISP_ABS,
            CL_DISP_LAST = CL_DISP_RANK
        }        

        /// <summary>
        /// The enumeration of the color space which display on the instrument
        /// </summary>
        public enum CL_COLOR_MODE
        {
            CL_MODE_EVXY,                                   //!< Ev, x, y
            CL_MODE_EVUV,                                   //!< Ev, u乫, v乫
            CL_MODE_EVTCPDUV,                               //!< Ev, Correlated color temperature Tcp, duv
            CL_MODE_XYZ,                                    //!< X, Y, Z
            CL_MODE_EVDWPE,                                 //!< Ev, Dominant wavelength, Excitation purity Pe
            CL_MODE_RENDERING,                              //!< Color Rendering Index (Ra, R1 to R15)
            CL_MODE_SPECTRAL_GRAPH,                         //!< Spectral irradiance graph, Peak wavelength
            CL_MODE_CUSTOM,                                 //!< Custom
            CL_MODE_NUM,                                    //!< 
            CL_MODE_FIRST = CL_MODE_EVXY,
            CL_MODE_LAST = CL_MODE_CUSTOM,
            CL_MODE_DEFAULT = CL_MODE_EVXY
        }

        /// <summary>
        /// The enumeration of the measurement time
        /// </summary>
        public enum CL_MEASUREMENT_TIME
        {
            CL_MEAS_TIME_FAST = 0,          //!< FAST mode : The measurement time : 0.5 second
            CL_MEAS_TIME_SLOW,              //!< SLOW mode : The measurement time : 2.5 second
            CL_MEAS_TIME_AUTO,              //!< AUTO mode : Measures in accordance with the brightness of the light source. The measurement time : 0.5 to 27 seconds
            CL_MEAS_TIME_SUPER_FAST,        //!< SUPER FAST mode : The measurement time : 0.2second
            CL_MEAS_TIME_NUM,

            CL_MEAS_TIME_DEFAULT = CL_MEAS_TIME_AUTO,
            CL_MEAS_TIME_FIRST = CL_MEAS_TIME_FAST,
            CL_MEAS_TIME_LAST = CL_MEAS_TIME_SUPER_FAST
        }

        /// <summary>
        /// The enumeration of the display items for custom color mode
        /// </summary>
        public enum CL_CUSTOMDATA_ITEM
        {
            CL_CUSTOM_NONE,                                 //!< No display
            CL_CUSTOM_EV_DIFF,                              //!< Ev(Difference with target)
            CL_CUSTOM_EV_RATIO,                             //!< Ev(Percentage of target)
            CL_CUSTOM_SX,                                   //!< x
            CL_CUSTOM_SY,                                   //!< y
            CL_CUSTOM_U,                                    //!< u'
            CL_CUSTOM_V,                                    //!< v'
            CL_CUSTOM_TCP,                                  //!< Correlated color temperature Tcp
            CL_CUSTOM_DUV,                                  //!< duv
            CL_CUSTOM_LX_DIFF,                              //!< X(Difference with target)
            CL_CUSTOM_LX_RATIO,                             //!< X(Percentage of target)
            CL_CUSTOM_LY_DIFF,                              //!< Y(Difference with target)
            CL_CUSTOM_LY_RATIO,                             //!< Y(Percentage of target)
            CL_CUSTOM_LZ_DIFF,                              //!< Z(Difference with target)
            CL_CUSTOM_LZ_RATIO,                             //!< Z(Percentage of target)
            CL_CUSTOM_DW,                                   //!< Dominant wavelength
            CL_CUSTOM_PE,                                   //!< Excitation purity
            CL_CUSTOM_RANK,                                 //!< Rank
            CL_CUSTOM_RA,                                   //!< Ra
            CL_CUSTOM_R1,                                   //!< R1
            CL_CUSTOM_R2,                                   //!< R2
            CL_CUSTOM_R3,                                   //!< R3
            CL_CUSTOM_R4,                                   //!< R4
            CL_CUSTOM_R5,                                   //!< R5
            CL_CUSTOM_R6,                                   //!< R6
            CL_CUSTOM_R7,                                   //!< R7
            CL_CUSTOM_R8,                                   //!< R8
            CL_CUSTOM_R9,                                   //!< R9
            CL_CUSTOM_R10,                                  //!< R10
            CL_CUSTOM_R11,                                  //!< R11
            CL_CUSTOM_R12,                                  //!< R12
            CL_CUSTOM_R13,                                  //!< R13
            CL_CUSTOM_R14,                                  //!< R14
            CL_CUSTOM_R15,                                  //!< R15
                                                            //V1.2
            CL_CUSTOM_EV_SCOTOPIC_DIFF,                     //!< Scotopic Ev(Difference with target)
            CL_CUSTOM_EV_SCOTOPIC_RATIO,                    //!< Scotopic Ev(Percentage of target)
            CL_CUSTOM_SP,                                   //!< Scotopic/Photopic Ratio
            CL_CUSTOM_EV_LAMBDA1,                           //!< Wavelength
            CL_CUSTOM_EV_LAMBDA2,                           //!< Wavelength
            CL_CUSTOM_EV_LAMBDA3,                           //!< Wavelength
            CL_CUSTOM_EV_LAMBDA4,                           //!< Wavelength

            CL_CUSTOM_NUM,
            CL_CUSTOM_FIRST = CL_CUSTOM_NONE,
            CL_CUSTOM_LAST = CL_CUSTOM_EV_LAMBDA4,
            CL_CUSTOM_DEFAULT1 = CL_CUSTOM_EV_RATIO,
            CL_CUSTOM_DEFAULT2 = CL_CUSTOM_TCP,
            CL_CUSTOM_DEFAULT3 = CL_CUSTOM_DW,
            CL_CUSTOM_DEFAULT4 = CL_CUSTOM_RA

        }

        /// <summary>
        /// The enum of measurement modes
        /// </summary>
        public enum CL_MEASMODE
        {
            CL_MEASMODE_SINGLE,
            CL_MEASMODE_AVERAGED,
            CL_MEASMODE_CONTINUOUS,

            CL_MEASMODE_FIRST = CL_MEASMODE_SINGLE,
            CL_MEASMODE_LAST = CL_MEASMODE_CONTINUOUS,

        }

        /// <summary>
        /// The struct of timer settings
        /// </summary>
        public struct CL_TIMERCONF
        {
            public UInt16 DelaySec;
            public UInt16 Interval;
            public UInt16 MeasNum;

        }

        /// <summary>
        /// The struct of arbitary wavelengths
        /// </summary>
        public struct CL_USERWAVELENGTH
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] lambda;
        }

        /// <summary>
        /// The enumeration of the measurement condition type in standalone measurement
        /// </summary>
        public enum CL_MEASSETTYPE
        {
            CL_MEASSET_DISPTYPE,            //!< Display Type
            CL_MEASSET_OBS,                 //!< Observer
            CL_MEASSET_COLORSPACE,          //!< Color Space
            CL_MEASSET_ILLUNIT,             //!< Illuminance unit
            CL_MEASSET_EXPOSURETIME,        //!< Measurement Time
            CL_MEASSET_USERCALIBCH,         //!< User calibration CH
            CL_MEASSET_TARGETNO,            //!< Target data No.
            CL_MEASSET_USERCUSTOM,          //!< Custom color mode
            CL_MEASSET_MEASMODE,            //!< Measurement mode ( SINGLE, MEAN, MULTI )
            CL_MEASSET_TIMERCONF,           //!< timer configuration ( delay, interval, num of measurements )
            CL_MEASSET_USERWAVELENGTH,      //!< arbitary wavelengths

            CL_MEASSET_NUM,

            CL_MEASSET_FIRST = CL_MEASSET_DISPTYPE,
            CL_MEASSET_LAST = CL_MEASSET_USERWAVELENGTH
        }

        /// <summary>
        /// The structure of system setting for the instrument
        /// </summary>
        public struct CL_SYSTEMSETTING
        {
            public CL_DATETIME Datetime;               //!< The date and time setting for the instrument
            public CL_DISPLAYTYPE Display;             //!< Orientation of the display
            public CL_BEEP Beep;                   //!< The buzzer setting for the instrument
            public CL_LANGCODE Lang;                   //!< The language setting for the instrument
            public CL_TYPE_DATEFORMAT DateFormat;              //!< The date and time format for the instrument
            public CL_USERCAL_LIMIT UserCalLimit;          //!< Zero calibration expiry
            public CL_AUTOPOWEROFF AutoPowerOff;           //!< Auto power off setting
            public CL_PCALNOTIFY PCalNotify;               //!< Periodic calibration warning message
        }

        /// <summary>
        /// The structure of information on date and time
        /// </summary>
        public struct CL_DATETIME
        {
            public UInt32 Year;         //!< The year value
            public UInt32 Month;        //!< The month value
            public UInt32 Day;          //!< The day value 
            public UInt32 Hour;         //!< The hour value
            public UInt32 Minute;       //!< The minute value
            public UInt32 Second;       //!< The seconds value
        }

        /// <summary>
        /// The enumeration of the orientation of the display
        /// </summary>
        public enum CL_DISPLAYTYPE
        {
            DISPLAY_NORMAL,         //!< The LCD screen displays normal
            DISPLAY_INVERSE,        //!< The LCD screen displays invert
            DISPLAY_NUM,
            DISPLAY_DEFAULT = DISPLAY_NORMAL,
            DISPLAY_FIRST = DISPLAY_NORMAL,
            DISPLAY_LAST = DISPLAY_INVERSE
        }

        /// <summary>
        /// The enumeration of the buzzer setting
        /// </summary>
        public enum CL_BEEP
        {
            CL_BEEP_OFF,            //!< The buzzer does not sound
            CL_BEEP_ON,             //!< The buzzer sounds
            CL_BEEP_NUM,
            CL_BEEP_DEFAULT = CL_BEEP_ON,
            CL_BEEP_FIRST = CL_BEEP_OFF,
            CL_BEEP_LAST = CL_BEEP_ON
        }

        /// <summary>
        /// The enumeration of the display language
        /// </summary>
        public enum CL_LANGCODE
        {
            CL_LANG_ENG,                    //!< Engilish
            CL_LANG_JPN,                    //!< Japanese
            CL_LANG_CHN,                    //!< Chinese 
            CL_LANG_NUM,                    //!< the number of availabe languages
            CL_LANG_DEFAULT = CL_LANG_ENG,  //!< default 
            CL_LANG_FIRST = CL_LANG_ENG,
            CL_LANG_LAST = CL_LANG_CHN
        }

        /// <summary>
        /// The enumeration of the date display format
        /// </summary>
        public enum CL_TYPE_DATEFORMAT
        {
            CL_TYPE_YYMMDD,                 //!< Display the date in year/month/day order
            CL_TYPE_MMDDYY,                 //!< Display the date in month/day/year order
            CL_TYPE_DDMMYY,                 //!< Display the date in day/month/year order
            CL_TYPE_NUM,
            CL_DATEFORMAT_DEFAULT = CL_TYPE_YYMMDD, //!< default 
            CL_DATEFORMAT_FIRST = CL_TYPE_YYMMDD,
            CL_DATEFORMAT_LAST = CL_TYPE_DDMMYY,
        }

        /// <summary>
        /// The enumeration of the zero calibration expiry
        /// </summary>
        public enum CL_USERCAL_LIMIT
        {
            CL_USERCAL_LIMIT_3H,            //!< The calibration prompt screen is displayed when elapsing 3 hour from last calibration
            CL_USERCAL_LIMIT_6H,            //!< The calibration prompt screen is displayed when elapsing 6 hour from last calibration
            CL_USERCAL_LIMIT_12H,           //!< The calibration prompt screen is displayed when elapsing 12 hour from last calibration
            CL_USERCAL_LIMIT_24H,           //!< The calibration prompt screen is displayed when elapsing 24 hour from last calibration
            CL_USERCAL_LIMITLESS,           //!< The calibration prompt screen is not displayed
            CL_USERCAL_LIMIT_NUM,           //!< the number of availabe types
            CL_USERCAL_LIMIT_DEFAULT = CL_USERCAL_LIMIT_12H,    //!< default 
            CL_USERCAL_LIMIT_FIRST = CL_USERCAL_LIMIT_3H,
            CL_USERCAL_LIMIT_LAST = CL_USERCAL_LIMITLESS,
        }

        /// <summary>
        /// The enumeration of the auto power off setting
        /// </summary>
        public enum CL_AUTOPOWEROFF
        {
            CL_AUTOPOWEROFF_OFF = 0,        //!< Disable the configuration of the auto power off
            CL_AUTOPOWEROFF_ON,             //!< Enable the configuration of the auto power off
            CL_AUTOPOWEROFF_NUM,
            CL_AUTOPOWEROFF_DEFAULT = CL_AUTOPOWEROFF_ON,
            CL_AUTOPOWEROFF_FIRST = CL_AUTOPOWEROFF_OFF,
            CL_AUTOPOWEROFF_LAST = CL_AUTOPOWEROFF_ON
        }

        /// <summary>
        /// The enumeration of the periodic calibration warning message
        /// </summary>
        public enum CL_PCALNOTIFY
        {
            CL_PCALNOTIFY_OFF = 0,          //!< Disables display of the periodic calibration warning message
            CL_PCALNOTIFY_ON,               //!< Enables display of the periodic calibration warning message
            CL_PCALNOTIFY_NUM,
            CL_PCALNOTIFY_DEFAULT = CL_PCALNOTIFY_ON,
            CL_PCALNOTIFY_FIRST = CL_PCALNOTIFY_OFF,
            CL_PCALNOTIFY_LAST = CL_PCALNOTIFY_ON
        }

        /// <summary>
        /// The enumeration on the system type 
        /// </summary>
        public enum CL_SYSTEMTYPE
        {
            CL_SYSTEM_DATETIME,         //!< Date and time
            CL_SYSTEM_DISPLAY,          //!< Orientation of the display
            CL_SYSTEM_BEEP,             //!< Buzzer setting
            CL_SYSTEM_LANGUAGE,         //!< Display language
            CL_SYSTEM_DATEFORMAT,       //!< Date display format
            CL_SYSTEM_UCAL_LIMIT,       //!< Zero calibration expiry
            CL_SYSTEM_AUTOPOWEROFF,     //!< Auto power off
            CL_SYSTEM_PCALNOTIFY,       //!< Periodic calibration warning message
            CL_SYSTEM_NUM,
            CL_SYSTEM_FIRST = CL_SYSTEM_DATETIME,
            CL_SYSTEM_LAST = CL_SYSTEM_PCALNOTIFY
        }
        


    }
}
