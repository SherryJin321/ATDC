using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL500AClassLib
{
    class errordefine
    {        
        // Format:ERzxxxyy
        // z :  Module number, 0 and 1 are the main module. And  the numbers over 2 is the sub module.
        // xx : The decimal number (001 to 999) which gives for each API, No.0 is not used.
        // yy : The decimal number (01 to 99) which gives in the internal of API, No.0 is not used.
        public enum ERROR_CODE
        {
            // Common definition
            SUCCESS = 0,        //!< Succeeded to execute the processing
            WARNING = 1,        //!< Though succeeded to execute the processing, the necessary information to inform is exist

            ER_OVER_EXPOSURE = 6,   //!< exposure time specified in CLDoManualMeasurement()/CLDoManualMeasurementAll() was too long for the target illuminant
            ER_UNDER_EXPOSURE = 7,  //!< exposure time specified in CLDoManualMeasurement()/CLDoManualMeasurementAll() was too short for the target illuminant

            // Common error code
            ER_HANDLE_NULL = 10,//!< The object handle is not exist
            ER_INITIALIZE,      //!< (Not used)
            ER_VERSIONCHECK,    //!< (Not used)
            ER_OPENDEVICE,      //!< Failed to get the object handle
            ER_PARAM_NULL,      //!< Parameter is NULL value
            ER_INVALID,         //!< (Not used)
            ER_ALLMEASURING,    //!< The CLDoMeasureAll() is performing


            // The error code for each function

            ER00100 = 100,      //!< CLGetSDKVersion
            ER00101,            //!< Parameter SDKVersion is NULL value

            ER00300 = 300,      //!< CLSetRemoteMode()/CLGetRemoteMode()
            ER00301,            //!< Parameter Mode is NULL value
            ER00302,            //!< Configuration value is invalid value
            ER00303,            //!< Failed to get a remote mode setting
            ER00304,            //!< Failed to set a remote mode
            ER00305,            //!< Failed to get the calibration data
            ER00306,            //!< Failed to get the rank data
            ER00307,            //!< Failed to get the measurement time of the instrument
            ER00308,            //!< Failed to set a remote mode OFF
            ER00309,            //!< Failed to get the user calibration coefficient

            ER00400 = 400,      //!< CLGetProperty()
            ER00401,            //!< Parameter Param is NULL value
            ER00402,            //!< Specified property type is invalid value
            ER00403,            //!< The remote mode is OFF

            ER00500 = 500,      //!< CLSetProperty()
            ER00501,            //!< The observer value is invalid value
            ER00502,            //!< The illuminance units value is invalid value
            ER00503,            //!< The remote mode is OFF
            ER00504,            //!< The property type is invalid value

            ER00600 = 600,      //!< CLSetSystemSetting()
            ER00601,            //!< Parameter pSetting is NULL value
            ER00602,            //!< Specified system type is invalid value
            ER00603,            //!< The remote mode is OFF

            ER00800 = 800,      //!< CLGetButtonStatus()
            ER00801,            //!< pStatus is NULL value
            ER00802,            //!< The remote mode is OFF
            ER00803,            //!< Failed to get the key status

            ER00900 = 900,      //!< CLGetDeviceID()
            ER00901,            //!< Parameter pDevID is NULL value
            ER00902,            //!< The factory calibration is not completed
            ER00903,            //!< The remote mode is OFF

            ER01000 = 1000,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER01001,            //!< Parameter pSettings is NULL value
            ER01002,            //!< The remote mode is OFF
            ER01003,            //!< The specified type is invalid value

            ER01100 = 1100,     //!< CLGetCalibrateStatus()
            ER01101,            //!< Parameter Status is NULL 
            ER01102,            //!< The remote mode is OFF
            ER01103,            //!< Failed to get the calibration status

            ER01200 = 1200,     //!< CLPollingMeasure()
            ER01201,            //!< Parameter 乬pStatus乭 is NULL value
            ER01202,            //!< The remote mode is OFF
            ER01203,            //!< Failed to a measurement
            ER01204,            //!< Exceeds measurement range by the CL-500A
            ER01205,            //!< Failed to get the implementation of measurement status
            ER01206,            //!< Failed to a measurement

            ER01300 = 1300,     //!< CLDoMeasurement()
            ER01301,            //!< A zero calibration is not performed
            ER01302,            //!< The remote mode is OFF
            ER01303,            //!< Parameter pTime is NULL value
            ER01304,            //!< Failed to get a calibration status
            ER01305,            //!< Failed to perform a measurement
            ER01306,            //!< A zero calibration is not performed
            ER01307,            //!< The error was occurred in pre measurement

            ER01320 = 1320,     //!< CLDoManualMeasurement()
            ER01321,            //!< Calibrations are not performed
            ER01322,            //!< Remote mode is off
            ER01323,
            ER01324,            //!< Failed to get calibration status
            ER01325,            //!< Failed to perform a measurement
            ER01326,            //!< Firm version is old
            ER01327,
            ER01328,            //!< ExposureTime is invalid
            ER01329,            //!< Cumulative number is invalid
            ER01330,            //!< Combination of exposure time and cumulative number is invalid
            ER01331,            //!< Failed to set exposure time and cumulative number

            ER01400 = 1400,     //!< CLStopMeasurement()
            ER01401,            //!< The remote mode is OFF
            ER01402,            //!< Failed to stop a measurement

            ER01600 = 1600,     //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER01601,            //!< Buzzer value is invalid value
            ER01602,            //!< Failed to get the buzzer setting
            ER01603,            //!< Failed to configure the buzzer setting

            ER02000 = 2000,     //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER02001,            //!< Language value is invalid value
            ER02002,            //!< Failed to get language setting
            ER02003,            //!< Failed to configure the language

            ER02100 = 2100,     //!< CLSetUserCalibrationData()
            ER02101,            //!< The specified data No. is out of range
            ER02102,            //!< Some user calibration coefficient to be registered is not correct
            ER02103,            //!< The remote mode is OFF
            ER02104,            //!< The date to be registered is not correct
            ER02105,            //!< Parameter pCoef is NULL value
            ER02106,            //!< The data name to be registered is not correct
            ER02107,            //!< Failed to register the user calibration coefficient
            ER02108,            //!< Failed to update the user calibration coefficient

            ER02200 = 2200,     //!< CLGetUserCalibrationData()
            ER02201,            //!< The specified data No. is out of range
            ER02202,            //!< Parameter pCoef is NULL value
            ER02203,            //!< The remote mode is OFF
            ER02204,            //!< Failed to get the user calibration coefficient
            ER02205,            //!< Failed to confirm the data of the specified CH
            ER02206,            //!< The data is not exist in the specified CH

            ER02300 = 2300,     //!< CLDeleteUserCalibrationData()
            ER02301,            //!< The specified data No. is out of range
            ER02302,            //!< The remote mode is OFF
            ER02303,            //!< Failed to delete the user calibration coefficient
            ER02304,            //!< Failed to update the user calibration coefficient

            ER02400 = 2400,     //!< CLCalcUserCalibrationData()
            ER02401,            //!< The calculated coefficient is out of range 
            ER02402,            //!< Parameter output is NULL value
            ER02403,            //!< Parameter Target is NULL value
            ER02404,            //!< Parameter sample is NULL value

            ER03700 = 3700,     //!< CLGetMeasData()
            ER03701,            //!< (Not used)
            ER03702,            //!< Failed to calculate colorimetric value
            ER03703,            //!< A measurement is not performed
            ER03704,            //!< The remote mode is OFF
            ER03705,            //!< Parameter pColor is NULL value

            ER04700 = 4700,     //!< CLGetColorDifference()
            ER04701,            //!< Failed to get the target data
            ER04702,            //!< A measurement is not performed yet
            ER04703,            //!< Failed to set a target data
            ER04704,            //!< Failed to set a measurement data
            ER04705,            //!< Failed to calculate the color difference
            ER04706,            //!< The remote mode is OFF
            ER04707,            //!< Parameter pDiff is NULL value

            ER04900 = 4900,     //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER04901,            //!< Date format value is invalid value
            ER04902,            //!< Failed to get date and time format
            ER04903,            //!< Failed to configure date format

            ER05000 = 5000,     //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER05001,            //!< Orientation of the display value is invalid value
            ER05002,            //!< Failed to get orientation of the display
            ER05003,            //!< Failed to configure the orientation of the display

            ER05400 = 5400,     //!< CLSetTargetData()
            ER05401,            //!< Parameter pData is NULL value
            ER05402,            //!< The specified data No. is out of range
            ER05403,            //!< 
            ER05404,            //!< The remote mode is OFF
            ER05405,            //!< The date to be registered is not correct
            ER05406,            //!< The data name to be registered is not correct
            ER05407,            //!< Failed to register the target data

            ER05500 = 5500,     //!< CLGetTargetData()
            ER05501,            //!< Parameter pStatus is NULL value
            ER05502,            //!< The specified data No. is out of range
            ER05503,            //!< The remote mode is OFF
            ER05504,            //!< Failed to get a target data
            ER05505,            //!< Failed to get target data information 
            ER05506,            //!< The target data is not exist in the specified No.

            ER05600 = 5600,     //!< CLDeleteTargetData()
            ER05601,            //!< The specified data No. is out of range
            ER05602,            //!< Failed to delete the target data
            ER05603,            //!< The remote mode is OFF

            ER05800 = 5800,     //!< cIEigerSDK::GetDeviceStoredData
            ER05801,            //!< Parameter pData is NULL value
            ER05802,            //!< The remote mode is OFF
            ER05803,            //!< Failed to get the number of stored data
            ER05804,            //!< The buffer size is not enough
            ER05805,            //!< Failed to get the stored data in the instrument
            ER05806,            //!< Failed to get the stored data in the instrument
            ER05807,            //!< Failed to set measurement data
            ER05808,            //!< Failed to calculate the colorimetric data 
            ER05809,            //!< The stored data in the instrument is not exist

            ER05900 = 5900,     //!< CLGetDeviceStoredDataNum()
            ER05901,            //!< Parameter Num is NULL value
            ER05902,            //!< The remote mode is OFF
            ER05903,            //!< Failed to get the number of measurement data

            ER06600 = 6600,     //!< CLSetRankData()
            ER06601,            //!< Parameter RankData is NULL value
            ER06602,            //!< The remote mode is OFF
            ER06603,            //!< The specified data No. is invalid value
            ER06604,            //!< Failed to register the rank data
            ER06605,            //!< The rank area to register is not correct
            ER06606,            //!< Failed to convert to the xy data

            ER06700 = 6700,     //!< CLDeleteRankData()
            ER06701,            //!< The specified data No. is out of range
            ER06702,            //!< Failed to delete the rank data
            ER06703,            //!< Failed to delete the rank data
            ER06704,            //!< The remote mode is OFF

            ER07200 = 7200,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07201,            //!< Display type is invalid value
            ER07202,            //!< Failed to get the display type
            ER07203,            //!< Failed to configure the display 

            ER07300 = 7300,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07301,            //!< The observer value to be configured is invalid
            ER07302,            //!< Failed to get the observer
            ER07303,            //!< Failed to configure the observer

            ER07400 = 7400,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07401,            //!< Color space is invalid value
            ER07402,            //!< Failed to get color space
            ER07403,            //!< Failed to configure color space

            ER07500 = 7500,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07501,            //!< Illuminance units is invalid value
            ER07502,            //!< Failed to get the illuminance units 
            ER07503,            //!< Failed to configure the illuminance units

            ER07600 = 7600,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07601,            //!< Measurement time is invalid value
            ER07602,            //!< Failed to get the measurement time condition
            ER07603,            //!< Failed to configure the measurement time

            ER07700 = 7700,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07701,            //!< User calibration CH is invalid value
            ER07702,            //!< Failed to get user calibration CH
            ER07703,            //!< The calibration coefficient is not exist in specified calibration CH
            ER07704,            //!< Failed to configure user calibration CH
            ER07705,            //!< Failed to update the user calibration coefficient

            ER07800 = 7800,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07801,            //!< Target data No. is invalid value 
            ER07802,            //!< Failed to get target data No.
            ER07803,            //!< Failed to configure target data No.

            ER07900 = 7900,     //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER07901,            //!< Custom color mode is invalid value
            ER07902,            //!< 
            ER07903,            //!< Failed to configure the custom color mode

            ER15000 = 15000,    //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER15001,            //!< measurement mode is invalid
            ER15002,            //!< Failed to get measurement mode 
            ER15003,            //!< Failed to set measurement mode
            ER15004,            //!< firm version is old

            ER15100 = 15100,    //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER15101,
            ER15102,            //!< Failed to get timer cofigure
            ER15103,            //!< Failed to set timer configure
            ER15104,            //!< firm version is old

            ER15200 = 15200,    //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER15201,            //!< delay time in timer configure is invalid

            ER15300 = 15300,    //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER15301,            //!< interval time in timer configure is invalid

            ER15400 = 15400,    //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER15401,            //!< number of measurements in timer configure is invalid

            ER15500 = 15500,    //!< CLSetMeasSetting()/CLGetMeasSetting()
            ER15501,            //!< user wavelengths is invalid
            ER15502,            //!< Failed to get user wavelengths
            ER15503,            //!< Failed to set user wavelengths
            ER15504,            //!< firm version is old

            ER08000 = 8000,     //!< cIEigerSDK::CtrlUCalLimit
            ER08001,            //!< Zero calibration expiry value is invalid value
            ER08002,            //!< Failed to get zero calibration expiry
            ER08003,            //!< Failed to configure zero calibration expiry

            ER08100 = 8100,     //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER08101,            //!< Failed to get a measurement data
            ER08102,            //!< Failed to select rank
            ER08103,            //!< The remote mode is OFF
            ER08104,            //!< Parameter Rank is NULL value
            ER08105,            //!< The rank list is not exist

            ER08200 = 8200,     //!< CLGetRankData()
            ER08201,            //!< Failed to get the rank data
            ER08202,            //!< The remote mode is OFF
            ER08203,            //!< Parameter RankData is NULL value
            ER08204,            //!< The rank data is not exist
            ER08205,            //!< The specified data No. is out of range 

            ER08900 = 8900,     //!< CLDeleteDeviceStoredData()
            ER08901,            //!< The specified data No. is out of range
            ER08902,            //!< Failed to delete the stored data
            ER08903,            //!< Failed to delete the stored data
            ER08904,            //!< The remote mode is OFF
            ER08905,            //!< Failed to get the number of stored data in the instrument
            ER08906,            //!< The data is not exist in the specified No.

            ER09300 = 9300,     //!< CLGetPeriodicCalDate()
            ER09301,            //!< Failed to get the periodic calibration date
            ER09302,            //!< The remote mode is OFF
            ER09303,            //!< Parameter DateTime is NULL value
            ER09304,            //!< Specified type is invalid value

            ER09600 = 9600,     //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER09601,            //!< Failed to get date and time
            ER09602,            //!< Date and time value is invalid value
            ER09603,            //!< Failed to configure the date time

            ER10000 = 10000,    //!< CLGetWarning()
            ER10001,            //!< The remote mode is OFF

            ER10400 = 10400,    //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER10401,            //!< Failed to get periodic calibration
            ER10402,            //!< Periodic calibration value is invalid value
            ER10403,            //!< Failed to periodic calibration value

            ER10500 = 10500,    //!< CLSetSystemSetting()/CLGetSystemSetting()
            ER10501,            //!< Failed to get the auto power off setting
            ER10502,            //!< Auto power off value is invalid value
            ER10503,            //!< Failed to configure auto power off

            ER10800 = 10800,    //!< CLDoMeasurementAll()
            ER10801,            //!< The object handle is not exist
            ER10802,            //!< The instrument which a zero calibration is not performed exists
            ER10803,            //!< The instrument which a remote mode is OFF exists
            ER10804,            //!< Failed to get a calibration status
            ER10805,            //!< Failed to perform a measurement
            ER10806,            //!< Failed to get a measurement time
            ER10807,            //!< The instrument which a remote mode is OFF exists

            ER10820 = 10820,    //!< CLDoManualMeasurementAll()
            ER10821,            //!< The object haldle is not exist
            ER10822,            //!< The instrument which a zero calibration is not performed exists
            ER10823,            //!< The instrument which a remote mode is OFF exists
            ER10824,            //!< Failed to get a calibration status
            ER10825,            //!< Failed to perform a measurement
            ER10826,            //!< The instrument which has old version firm exists
            ER10827,            //!< The instrument which is remote off exists
            ER10828,            //!< ExposureTime is invalid
            ER10829,            //!< Cumulative number is invalid
            ER10830,            //!< Combination of exposure time and cumulative number is invalid
            ER10831,            //!< Failed to set exposure time and cumulative number

            ER10900 = 10900,    //!< CLDoPollingAll()
            ER10901,            //!< The number of instrument currently is different from the number of the instrument when starting a measurement
            ER10902,            //!< The instrument which a remote mode is OFF exists
            ER10903,            //!< Exceeds measurement range by the CL-500A
            ER10904,            //!< Failed to get the status of a measurement

            ER11100 = 11100,    //!< CLDoCalibration()
            ER11101,            //!< Failed to a zero calibration
            ER11102,            //!< The remote mode is OFF

            ER11200 = 11200,    //!< CLPollingCalibration()
            ER11201,            //!< Parameter pStatus is NULL value
            ER11202,            //!< Failed to a zero calibration
            ER11203,            //!< The abnormality of the device is occurred
            ER11204,            //!< The remote mode is OFF

            // The below error codes are not used by public API
            ERXXYY = 9,     //!< 
            ER_DUMMY = 9,       //!< 

            ER00700 = 700,      //!<
            ER00701,            //!< 
            ER00702,            //!< 

            ER03800 = 3800,     //!< 
            ER03801,            //!< 
            ER03802,

            ER03900 = 3900,     //!<
            ER03901,            //!< 
            ER03902,

            ER04000 = 4000,     //!< 
            ER04001,            //!< 
            ER04002,            //!< 
            ER04003,

            ER04100 = 4100,     //!< 
            ER04101,            //!< 
            ER04102,            //!< 

            ER04200 = 4200,     //!< 
            ER04201,            //!< 
            ER04202,
            ER05700 = 5700,     //!< 

            ER09400 = 9400,     //!< 
            ER09401,            //!< 
            ER09402,            //!< 
            ER09403,            //!< 
            ER09404,            //!< 

        };

        // The code for warning detail
        public enum WR_CODE
        {
            WRNONE = 0x0000,    //!< No warning
            WR001,              //!< It has passed a certain time from the last zero calibration
            WR002,              //!< Illuminance values is low value than the assured value
            WR003,              //!< The item which could not calculated is exist
            WR004,              //!< The corresponding rank is not exist 

            WR_OVER_EXPOSURE = 6,
            WR_UNDER_EXPOSURE = 7
        }        
    }
}
