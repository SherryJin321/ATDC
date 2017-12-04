using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

using ER_CODE = System.Int32;
using DEVICE_HANDLE = System.IntPtr;



namespace CL_500ATest
{
    class CLAPI
    {
        /// <summary>
        /// Gets the object handle of the instrument which is connected on the PC
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]        
        public static extern ER_CODE CLOpenDevice(ref DEVICE_HANDLE hDevice);           
        
        /// <summary>
        /// Configure a remote mode ON/OFF
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="Mode">The configuration of remote mode</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLSetRemoteMode(DEVICE_HANDLE hDevice, clconditions.CL_REMOTEMODE Mode);

        /// <summary>
        /// Performs a zero calibration
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLDoCalibration(DEVICE_HANDLE hDevice);

        /// <summary>
        /// Gets the implementation status of a zero calibration
        /// </summary>
        /// <param name="hDevice"> The object handle of the instrument to be controlled</param>
        /// <param name="pStatus">The buffer to store the status of a zero calibration</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLPollingCalibration(DEVICE_HANDLE hDevice, ref clconditions.CL_CALIBMEASSTATUS pStatus);

        /// <summary>
        /// Configures the measurement condition when control the instrument by the CL-SDK
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="Type">The property type to be configured</param>
        /// <param name="Param">The property value to be configured</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLSetProperty(DEVICE_HANDLE hDevice, clconditions.CL_PROPERTIES Type, int Param);

        /// <summary>
        /// Performs a measurement
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="pTime">The buffer to store a measurement time</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLDoMeasurement(DEVICE_HANDLE hDevice, ref int pTime);

        /// <summary>
        ///  Confirms the implementation status of measurement
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="pStatus">The buffer to store the measurement status</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLPollingMeasure(DEVICE_HANDLE hDevice, ref clconditions.CL_MEASSTATUS pStatus);

        /// <summary>
        /// Gets the each colorimetric data at the latest measurement
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="Type">The colorimetric type to be obtained</param>
        /// <param name="pColor">The buffer to store a colorimetric data</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLGetMeasData(DEVICE_HANDLE hDevice, clcolorconditions.CL_COLORSPACE Type, ref clconditions.CL_MEASDATA pColor);

        /// <summary>
        /// Releases the object handle of the instrument which is stored in the CL-SDK
        /// </summary>
        /// <param name="hDevice">The object handle to be released</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLCloseDevice(DEVICE_HANDLE hDevice);

        /// <summary>
        /// Configures the measurement condition in standalone measurement
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="Type">The measurement condition type to be configured </param>
        /// <param name="pSetting"> The measurement condition to be configured</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLSetMeasSetting(DEVICE_HANDLE hDevice, clconditions.CL_MEASSETTYPE Type, ref clconditions.CL_MEASSETTING pSetting);

        /// <summary>
        /// Configures the system setting of the instrument
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="Type">The system setting type to be configured</param>
        /// <param name="pSetting">The system setting to be configured</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLSetSystemSetting(DEVICE_HANDLE hDevice, clconditions.CL_SYSTEMTYPE Type, ref clconditions.CL_SYSTEMSETTING pSetting);

        /// <summary>
        /// Gets the measurement condition in standalone measurement
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="Type">The measurement condition type to be obtained </param>
        /// <param name="pSetting">The buffer to store a measurement condition</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLGetMeasSetting(DEVICE_HANDLE hDevice, clconditions.CL_MEASSETTYPE Type, ref clconditions.CL_MEASSETTING pSetting);

        /// <summary>
        /// Gets the system setting of the instrument
        /// </summary>
        /// <param name="hDevice"> The object handle of the instrument to be controlled</param>
        /// <param name="Type">The system setting type to be obtained</param>
        /// <param name="pSetting">The buffer to store the system setting</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLGetSystemSetting(DEVICE_HANDLE hDevice, clconditions.CL_SYSTEMTYPE Type, ref clconditions.CL_SYSTEMSETTING pSetting);

        /// <summary>
        /// Performs a measurement by specifying exposure time and cumulative num of measurements
        /// </summary>
        /// <param name="hDevice">The object handle of the instrument to be controlled</param>
        /// <param name="ExposureTime">Exposure Time [x100us]</param>
        /// <param name="MeasTimes">cumltative number of measurements [times]</param>
        /// <returns>ER_CODE</returns>
        [DllImport("libclapi.dll")]
        public static extern ER_CODE CLDoManualMeasurement(DEVICE_HANDLE hDevice, int ExposureTime, int MeasTimes);
    }
}
