using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using DEVICE_HANDLE = System.IntPtr;
using ER_CODE = System.Int32;
     

namespace CL500AClassLib
{
    public class CL500A
    {
        
        /// <summary>
        /// Acquire the device handle
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A OpenDevice(ref DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;
            ret = CLAPI.CLOpenDevice(ref handle);
            if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
            {
                result = OperationStatusCL500A.OpenDeviceSuccess;
            }
            else
            {
                result = OperationStatusCL500A.OpenDeviceFailure;
            }
            return result;
        }

        /// <summary>
        /// Set remote mode on
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A SetRemoteMode(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;
            ret = CLAPI.CLSetRemoteMode(handle, clconditions.CL_REMOTEMODE.CL_RMODE_ON);
            if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
            {
                result = OperationStatusCL500A.SetRemoteModeSuccess;
            }
            else
            {
                result = OperationStatusCL500A.SetRemoteModeFailure;
                Close(handle);
            }
            return result;
        }

        /// <summary>
        /// Perform a zero calibration
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A DoCalibration(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;
            ret = CLAPI.CLDoCalibration(handle);
            if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
            {
                result = OperationStatusCL500A.DoCalibrationSuccess;
            }
            else
            {
                result = OperationStatusCL500A.DoCalibrationFailure;
                RemoteOffClose(handle);
            }

            return result;
        }

        /// <summary>
        /// Polling calibration
        /// </summary>
        /// <param name="handle"></param>        
        /// <returns></returns>
        public OperationStatusCL500A PollingCalibration(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            clconditions.CL_CALIBMEASSTATUS cstatus = clconditions.CL_CALIBMEASSTATUS.CL_CALIBMEAS_FREE;
            ER_CODE ret;

            while(cstatus!=clconditions.CL_CALIBMEASSTATUS.CL_CALIBMEAS_FINISH)
            {
                Thread.Sleep(1000);
                ret = CLAPI.CLPollingCalibration(handle, ref cstatus);

                if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
                {
                    result = OperationStatusCL500A.PollingCalibrationSuccess;
                }
                else
                {
                    result = OperationStatusCL500A.PollingCalibrationFailure;
                    RemoteOffClose(handle);
                    return result;
                }
            }

            return result;
        }
        
        /// <summary>
        /// Set the measurement conditions in the stand alone measurement
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A SetMeasSetting(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            clconditions.CL_MEASSETTING measset = new clconditions.CL_MEASSETTING();
            measset.DispType = clconditions.CL_DISP_TYPES.CL_DISP_DEFAULT;                 //显示类型为绝对值
            measset.Obs = clcolorconditions.CL_OBSERVER.CL_OBS_DEFAULT;                    //标准观察角为2度
            measset.ColorSpace = clconditions.CL_COLOR_MODE.CL_MODE_DEFAULT;               //色空间为EV/X/Y
            measset.IlluminantUnit = clconditions.CL_ILLUMINANT_UNIT.CL_ILLUNIT_DEFAULT;   //照度单位为lx
            measset.ExposureTime = clconditions.CL_MEASUREMENT_TIME.CL_MEAS_TIME_DEFAULT;  //测试速度为自动
            measset.UserCalibCh = 0;                                                       //用户校正为UC00
            measset.TargetNo = 1;                                                          //目标数据为T01

            measset.UserCustom = new clconditions.CL_CUSTOMDATA_ITEM[clconditions.CL_USERCUSTOM_LEN];
            measset.UserCustom[0] = clconditions.CL_CUSTOMDATA_ITEM.CL_CUSTOM_DEFAULT1;
            measset.UserCustom[1] = clconditions.CL_CUSTOMDATA_ITEM.CL_CUSTOM_DEFAULT2;
            measset.UserCustom[2] = clconditions.CL_CUSTOMDATA_ITEM.CL_CUSTOM_DEFAULT3;
            measset.UserCustom[3] = clconditions.CL_CUSTOMDATA_ITEM.CL_CUSTOM_DEFAULT4;
            measset.MeasMode = clconditions.CL_MEASMODE.CL_MEASMODE_SINGLE;                 //测量模式为单次测量
            measset.TimerConf.DelaySec = 0;                                                 //延迟时间为0秒

            measset.UserWavelength.lambda = new UInt16[4];
            measset.UserWavelength.lambda[0] = 400;
            measset.UserWavelength.lambda[1] = 500;
            measset.UserWavelength.lambda[2] = 600;
            measset.UserWavelength.lambda[3] = 700;

            for(int i=0;i<(Int32)clconditions.CL_MEASSETTYPE.CL_MEASSET_NUM;i++)
            {
                ret = CLAPI.CLSetMeasSetting(handle, (clconditions.CL_MEASSETTYPE)i, ref measset);
                if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
                {
                    result = OperationStatusCL500A.SetMeasSettingSuccess;
                }
                else
                {
                    result = OperationStatusCL500A.SetRemoteModeFailure;
                    RemoteOffClose(handle);
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Set the system condition in the device
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A SetSystemSetting(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            clconditions.CL_SYSTEMSETTING system;

            system.Datetime = globaldata.g_datedata;                                     //CL-500A具有计时功能
            system.Display = clconditions.CL_DISPLAYTYPE.DISPLAY_NORMAL;                 //设置为正常模式
            system.Beep = clconditions.CL_BEEP.CL_BEEP_OFF;                              //关闭蜂鸣器
            system.Lang = clconditions.CL_LANGCODE.CL_LANG_CHN;                          //言语设置为中文
            system.DateFormat = clconditions.CL_TYPE_DATEFORMAT.CL_TYPE_YYMMDD;          //日期格式设置为YYMMDD
            system.UserCalLimit = clconditions.CL_USERCAL_LIMIT.CL_USERCAL_LIMITLESS;    //不显示用户校准信息
            system.AutoPowerOff = clconditions.CL_AUTOPOWEROFF.CL_AUTOPOWEROFF_OFF;      //不自动断电
            system.PCalNotify = clconditions.CL_PCALNOTIFY.CL_PCALNOTIFY_OFF;            //不显示过期校准警告信息

            for(int i=0;i<(int)clconditions.CL_SYSTEMTYPE.CL_SYSTEM_NUM;i++)
            {
                ret = CLAPI.CLSetSystemSetting(handle, (clconditions.CL_SYSTEMTYPE)i, ref system);
                if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
                {
                    result = OperationStatusCL500A.SetSystemSettingSuccess;
                }
                else
                {
                    result = OperationStatusCL500A.SetSystemSettingFailure;
                    RemoteOffClose(handle);
                    return result;
                }
            }

            return result;
        }
        
        /// <summary>
        /// Get the measurement setting in the stand alone measurement
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="curMeasSet"></param>
        /// <returns></returns>
        public OperationStatusCL500A GetMeasSetting(DEVICE_HANDLE handle,ref clconditions.CL_MEASSETTING curMeasSet)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            for(int i=0;i<(int)clconditions.CL_MEASSETTYPE.CL_MEASSET_NUM;i++)
            {
                ret = CLAPI.CLGetMeasSetting(handle, (clconditions.CL_MEASSETTYPE)i, ref curMeasSet);
                if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
                {
                    result = OperationStatusCL500A.GetMeasSettingSuccess;
                }
                else
                {
                    result = OperationStatusCL500A.GetMeasSettingFailure;
                    RemoteOffClose(handle);
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the system setting in the device
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="curGetSystem"></param>
        /// <returns></returns>
        public OperationStatusCL500A GetSystemSetting(DEVICE_HANDLE handle,ref clconditions.CL_SYSTEMSETTING curGetSystem)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            for(int i=0;i<(int)clconditions.CL_SYSTEMTYPE.CL_SYSTEM_NUM;i++)
            {
                ret = CLAPI.CLGetSystemSetting(handle, (clconditions.CL_SYSTEMTYPE)i, ref curGetSystem);
                if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
                {
                    result = OperationStatusCL500A.GetSystemSettingSuccess;
                }
                else
                {
                    result = OperationStatusCL500A.GetSystemSettingFailure;
                    RemoteOffClose(handle);
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Property setting(Observer)
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A SetPropertyObserver(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            ret = CLAPI.CLSetProperty(handle, clconditions.CL_PROPERTIES.CL_PR_OBSERVER, (int)clcolorconditions.CL_OBSERVER.CL_OBS_02DEG);
            if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
            {
                result = OperationStatusCL500A.SetPropertyObserverSuccess;
            }
            else
            {
                result = OperationStatusCL500A.SetPropertyObserverFailure;
                RemoteOffClose(handle);
            }

            return result;
        }

        /// <summary>
        /// Property setting(Illuminant unit)
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A SetPropertyIlluminantUnit(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            ret = CLAPI.CLSetProperty(handle, clconditions.CL_PROPERTIES.CL_PR_ILLUNIT, (int)clconditions.CL_ILLUMINANT_UNIT.CL_ILLUNIT_LX);
            if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
            {
                result = OperationStatusCL500A.SetPropertyIlluminantUnitSuccess;
            }
            else
            {
                result = OperationStatusCL500A.SetPropertyIlluminantUnitFailure;
                RemoteOffClose(handle);
            }

            return result;
        }

        /// <summary>
        /// Perform a manual measurement
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="exposurement"></param>
        /// <param name="cumulativenum"></param>
        /// <returns></returns>
        public OperationStatusCL500A DoManualMeasurement(DEVICE_HANDLE handle,int exposurement,int cumulativenum)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            ret = CLAPI.CLDoManualMeasurement(handle, exposurement, cumulativenum);
            if(ret>(Int32)errordefine.ERROR_CODE.WARNING)
            {
                result = OperationStatusCL500A.DoManualMeasurementFailure;
                RemoteOffClose(handle);
            }
            else
            {
                result = OperationStatusCL500A.DoManualMeasurementSuccess;
            }

            return result;
        }

        /// <summary>
        /// Polling until completing a measurement
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A PollingMeasure(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            clconditions.CL_MEASSTATUS status = clconditions.CL_MEASSTATUS.CL_MEAS_FREE;
            while(status!=clconditions.CL_MEASSTATUS.CL_MEAS_FINISH)
            {
                Thread.Sleep(1000);
                ret = CLAPI.CLPollingMeasure(handle, ref status);
                if(ret==(Int32)errordefine.ERROR_CODE.ER01206)
                {
                    result = OperationStatusCL500A.PollingMeasureSensorSaturation;
                    RemoteOffClose(handle);
                    return result;
                }
                else if(ret!=(Int32)errordefine.ERROR_CODE.SUCCESS)
                {
                    result = OperationStatusCL500A.PollingMeasureFailure;
                    RemoteOffClose(handle);
                    return result;
                }
                else
                {
                    result = OperationStatusCL500A.PollingMeasureSuccess;
                }
            }

            return result;
        }

        /// <summary>
        /// Getting the measurement data(Ev/x/y)
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="Ev"></param>
        /// <returns></returns>
        public OperationStatusCL500A GetMeasData(DEVICE_HANDLE handle,ref float Ev)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;

            clconditions.CL_MEASDATA color = new clconditions.CL_MEASDATA();
            ret = CLAPI.CLGetMeasData(handle, clcolorconditions.CL_COLORSPACE.CL_COLORSPACE_EVXY, ref color);
            if(ret>(Int32)errordefine.ERROR_CODE.WARNING)
            {
                result = OperationStatusCL500A.GetMeasDataFailure;                
            }
            else
            {
                result = OperationStatusCL500A.GetMeasDataSuccess;
                Ev = color.Evxy.Ev;
            }

            RemoteOffClose(handle);

            return result;
        }

        /// <summary>
        /// Set remote mode off
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A RemoteOffClose(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;
            ret = CLAPI.CLSetRemoteMode(handle, clconditions.CL_REMOTEMODE.CL_RMODE_OFF);
            if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
            {
                result = OperationStatusCL500A.RemoteOffCloseSuccess;
                result = Close(handle);
            }
            else
            {
                result = OperationStatusCL500A.RemoteOffCloseFailure;
            }
            return result;
        }

        /// <summary>
        /// Release the device handle
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public OperationStatusCL500A Close(DEVICE_HANDLE handle)
        {
            OperationStatusCL500A result = OperationStatusCL500A.OriginalStatus;
            ER_CODE ret;
            ret = CLAPI.CLCloseDevice(handle);
            if(ret==(Int32)errordefine.ERROR_CODE.SUCCESS)
            {
                result = OperationStatusCL500A.CloseSuccess;
            }
            else
            {
                result = OperationStatusCL500A.CloseFailure;
            }
            return result;
        }
    }

    public enum OperationStatusCL500A
    {
        OriginalStatus,
        OpenDeviceSuccess,
        OpenDeviceFailure,
        SetRemoteModeSuccess,
        SetRemoteModeFailure,
        DoCalibrationSuccess,
        DoCalibrationFailure,
        PollingCalibrationSuccess,
        PollingCalibrationFailure,
        SetMeasSettingSuccess,
        SetMeasSettingFailure,
        SetSystemSettingSuccess,
        SetSystemSettingFailure,
        GetMeasSettingSuccess,
        GetMeasSettingFailure,
        GetSystemSettingSuccess,
        GetSystemSettingFailure,
        SetPropertyObserverSuccess,
        SetPropertyObserverFailure,
        SetPropertyIlluminantUnitSuccess,
        SetPropertyIlluminantUnitFailure,
        DoManualMeasurementSuccess,
        DoManualMeasurementFailure,
        PollingMeasureSensorSaturation,
        PollingMeasureSuccess,
        PollingMeasureFailure,
        GetMeasDataSuccess,
        GetMeasDataFailure,
        RemoteOffCloseSuccess,
        RemoteOffCloseFailure,
        CloseSuccess,
        CloseFailure
    }
}
