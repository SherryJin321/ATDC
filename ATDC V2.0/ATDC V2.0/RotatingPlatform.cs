using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections;

namespace ATDC_V2._0
{
    public delegate void EVDataHandler(double EVData);
    public delegate void xyDataHandler(Coordinates xyData);

    class RotatingPlatform
    {
        public event EVDataHandler EVDataEvent;
        public event xyDataHandler xyDataEvent;

        #region 发送指令
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform OpenPort(SerialPort serialPort, string portName)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;  
            

            try
            {
                if (serialPort.IsOpen == false)
                {
                    serialPort.PortName = portName;
                    serialPort.BaudRate = 38400;
                    serialPort.Parity = Parity.None;
                    serialPort.DataBits = 8;
                    serialPort.StopBits = StopBits.One;

                    serialPort.Open();
                }
                if (serialPort.IsOpen == true)
                {
                    result = OperationStatusRotatingPlatform.OpenPortSuccess;
                }
            }
            catch
            {
                result = OperationStatusRotatingPlatform.OpenPortFailure;
            }

            return result;
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform ClosePort(SerialPort serialPort)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            try
            {
                if (serialPort.IsOpen == true)
                {
                    serialPort.Close();
                }
                if (serialPort.IsOpen == false)
                {
                    result = OperationStatusRotatingPlatform.ClosePortSuccess;
                }
            }
            catch
            {
                result = OperationStatusRotatingPlatform.ClosePortFailure;
            }

            return result;
        }

        /// <summary>
        /// 获取当前角度
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform GetDegree(SerialPort serialPort)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte[] sendingCommand = new byte[5] { 0X02, 0X55, 0X45, 0X00, 0X00};
            sendingCommand[sendingCommand.Length - 1] = GetCHK(sendingCommand);

            if (serialPort.IsOpen == true)
            {
                serialPort.Write(sendingCommand, 0, 5);
                result = OperationStatusRotatingPlatform.GetDegreeSuccess;
            }
            else
            {
                result = OperationStatusRotatingPlatform.GetDegreeFailure;
            }

            return result;
        }

        /// <summary>
        /// 转台复位
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform ResetRotatingPlatform(SerialPort serialPort)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte[] sendingCommand = new byte[5] { 0X02, 0X55, 0X51, 0X00, 0X00 };
            sendingCommand[sendingCommand.Length - 1] = GetCHK(sendingCommand);

            if (serialPort.IsOpen == true)
            {
                serialPort.Write(sendingCommand, 0, 5);
                result = OperationStatusRotatingPlatform.ResetRotatingPlatformSuccess;
            }
            else
            {
                result = OperationStatusRotatingPlatform.ResetRotatingPlatformFailure;
            }

            return result;
        }

       /// <summary>
       /// 设置电机状态
       /// </summary>
       /// <param name="serialPort"></param>
       /// <param name="channel"></param>
       /// <param name="direction"></param>
       /// <param name="degreesH"></param>
       /// <param name="degreesL"></param>
       /// <returns></returns>
        public OperationStatusRotatingPlatform SetMotorStatus(SerialPort serialPort,byte channel,byte direction,byte degreesH,byte degreesL)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte[] sendingCommand = new byte[11] { 0X02, 0X55, 0X40, 0X06, 0X00, 0X00, 0X64, 0X00, 0X00, 0X00, 0X00};

            sendingCommand[4] = channel;
            sendingCommand[7] = direction;
            sendingCommand[8] = degreesH;
            sendingCommand[9] = degreesL;
            sendingCommand[sendingCommand.Length - 1] = GetCHK(sendingCommand);

            if (serialPort.IsOpen == true)
            {
                serialPort.Write(sendingCommand, 0, 11);
                result = OperationStatusRotatingPlatform.SetMotorStatusSuccess;
            }
            else
            {
                result = OperationStatusRotatingPlatform.SetMotorStatusFailure;
            }

            return result;
        }

        /// <summary>
        /// 启动电机
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform StartMotor(SerialPort serialPort,byte channel)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte[] sendingCommand = new byte[6] { 0X02, 0X55, 0X42, 0X01, 0X00, 0X00 };
            sendingCommand[4] = channel;
            sendingCommand[sendingCommand.Length - 1] = GetCHK(sendingCommand);

            if (serialPort.IsOpen == true)
            {
                serialPort.Write(sendingCommand, 0, 6);
                result = OperationStatusRotatingPlatform.StartMotorSuccess;
            }
            else
            {
                result = OperationStatusRotatingPlatform.StartMotorFailure;
            }

            return result;
        }

        /// <summary>
        /// 连接CL200至PC
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform ConnectCL200ToPC(SerialPort serialPort)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte[] sendingCommand = new byte[5] { 0X02, 0X55, 0X34, 0X00, 0X00};            
            sendingCommand[sendingCommand.Length - 1] = GetCHK(sendingCommand);

            if (serialPort.IsOpen == true)
            {
                serialPort.Write(sendingCommand, 0, 5);
                result = OperationStatusRotatingPlatform.ConnectCL200ToPCSuccess;
            }
            else
            {
                result = OperationStatusRotatingPlatform.ConnectCL200ToPCFailure;
            }

            return result;
        }

        /// <summary>
        /// 获取传感器EV
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform GetEVxy(SerialPort serialPort)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte[] sendingCommand = new byte[5] { 0X02, 0X55, 0X33, 0X00, 0X00 };
            sendingCommand[sendingCommand.Length - 1] = GetCHK(sendingCommand);

            if (serialPort.IsOpen == true)
            {
                serialPort.Write(sendingCommand, 0, 5);
                result = OperationStatusRotatingPlatform.GetEVxySuccess;
            }
            else
            {
                result = OperationStatusRotatingPlatform.GetEVxyFailure;
            }

            return result;
        }

        #endregion

        #region 接收指令
        /// <summary>
        /// 接收、解析反馈指令
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        byte[] integrityData = new byte[2];
        ArrayList notIntegrityData = new ArrayList();
        public OperationStatusRotatingPlatform GetFeedbackCommand(SerialPort serialPort)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            if(serialPort.IsOpen==true)
            {
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                if(receivedData.Length!=0)
                {
                    bool IsCommandIntegrity = false;
                    IsCommandIntegrity = JudgeCommandIntegrity(receivedData);

                    if(IsCommandIntegrity==false)
                    {
                        notIntegrityData.AddRange(receivedData);

                        byte[] notIntegrityCommand = new byte[notIntegrityData.Count];
                        for(int i=0;i<notIntegrityCommand.Length;i++)
                        {
                            notIntegrityCommand[i] = (byte)notIntegrityData[i];
                        }
                        IsCommandIntegrity = JudgeCommandIntegrity(notIntegrityCommand);

                        if(IsCommandIntegrity==true)
                        {
                            integrityData = new byte[notIntegrityCommand.Length];
                            for (int i = 0; i < integrityData.Length; i++)
                            {
                                integrityData[i] = notIntegrityCommand[i];
                            }
                        }
                    }
                    else
                    {
                        integrityData = new byte[receivedData.Length];
                        for(int i=0;i<integrityData.Length;i++)
                        {
                            integrityData[i] = receivedData[i];
                        }
                    }
                    if(IsCommandIntegrity==true)
                    {
                        notIntegrityData.Clear();
                        result = AnalysisFeedbackCommand(integrityData);
                    }

                }
                else
                {
                    result = OperationStatusRotatingPlatform.FeedbackCommandLengthIsNull;
                }
            }
            else
            {
                result = OperationStatusRotatingPlatform.FeedbackCommandSerialPortFailure;
            }

            return result;
        }

        /// <summary>
        /// 判断接收指令是否完整
        /// </summary>
        /// <param name="receivedData"></param>
        /// <returns></returns>
        public bool JudgeCommandIntegrity(byte[] receivedData)
        {
            bool isIntegrated = false;

            if(receivedData.Length>=5)
            {
                if(receivedData[0]==0x02&&receivedData[1]==0xAA)
                {
                    if(receivedData.Length==(receivedData[3]+5))
                    {
                        isIntegrated = true;
                    }
                    else
                    {
                        isIntegrated = false;
                    }
                }
                else
                {
                    isIntegrated = false;
                }
            }
            else
            {
                isIntegrated = false;
            }

            return isIntegrated;
        }         

        /// <summary>
        /// 解析反馈指令
        /// </summary>
        /// <param name="receivedData"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform AnalysisFeedbackCommand(byte[] receivedData)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte CHK = GetCHK(receivedData);
            if(CHK==receivedData[receivedData.Length-1])
            {
                if(receivedData[2]==0x45)
                {
                    Coordinates xy = new Coordinates();
                    xy = GetCoordinates(receivedData);
                    xyDataEvent(xy);
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunction45Success;
                }
                else if(receivedData[2]==0x51)
                {
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunction51Success;
                }
                else if(receivedData[2]==0xA1)
                {
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunctionA1Success;
                }
                else if(receivedData[2]==0x40)
                {
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunction40Success;
                }
                else if(receivedData[2]==0x42)
                {
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunction42Success;
                }
                else if(receivedData[2]==0x34)
                {
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunction34Success;
                }
                else if(receivedData[2]==0xA3)
                {
                    double EV = GetEV(receivedData);
                    EVDataEvent(EV);
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunctionA3Success;

                }
                else
                {
                    result = OperationStatusRotatingPlatform.FeedbackCommandFunctionCodeError;
                }
            }
            else
            {
                result = OperationStatusRotatingPlatform.FeedbackCommandCHKError;
            }

            return result;
        }

        /// <summary>
        /// 获得功能码为A3的转台反馈指令的EV值
        /// </summary>
        /// <param name="receivedData"></param>
        /// <returns></returns>
        public double GetEV(byte[] receivedData)
        {
            double result = 0;

            for(int i=0;i<4;i++)
            {
                if(receivedData[11+i]!=0x20)
                {
                    result += (receivedData[11 + i] - 0x30) * Math.Pow(10, 3 - i);
                }
            }

            result *= Math.Pow(10, (receivedData[15] - 0x30 - 4));

            if(receivedData[10]==0x2B)
            {
                result = result * 1;
            }
            else if(receivedData[10]==0x2D)
            {
                result = result * (-1);
            }
            else if(receivedData[10]==0x3D)
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// 获得功能码为45的转台反馈指令的坐标值
        /// </summary>
        /// <param name="receivedData"></param>
        /// <returns></returns>
        public Coordinates GetCoordinates(byte[] receivedData)
        {
            Coordinates result = new Coordinates();

            short xShort = 0;
            short yShort = 0;

            xShort = receivedData[4];
            xShort <<= 8;
            xShort |= receivedData[5];

            yShort = receivedData[6];
            yShort <<= 8;
            yShort |= receivedData[7];

            result.x = xShort/100.0;
            result.y = yShort/100.0;

            return result;
        }

        #endregion

        /// <summary>
        /// 获取指令校验码
        /// </summary>
        /// <param name="OriginalArray"></param>
        /// <returns></returns>
        public byte GetCHK(byte[] OriginalArray)
        {
            byte result = 0x00;

            for (int i = 0; i < (OriginalArray.Length - 1); i++)
            {
                result += OriginalArray[i];
            }

            return result;
        }

    }

    public struct Coordinates
    {
        public double x;
        public double y;
    }

    enum OperationStatusRotatingPlatform
    {
        OpenPortSuccess,
        OpenPortFailure,
        ClosePortSuccess,
        ClosePortFailure,
        GetDegreeSuccess,
        GetDegreeFailure,
        ResetRotatingPlatformSuccess,
        ResetRotatingPlatformFailure,
        SetMotorStatusSuccess,
        SetMotorStatusFailure,
        StartMotorSuccess,
        StartMotorFailure,
        ConnectCL200ToPCSuccess,
        ConnectCL200ToPCFailure,
        GetEVxySuccess,
        GetEVxyFailure,
        FeedbackCommandCHKError,
        FeedbackCommandFunctionCodeError,
        FeedbackCommandFunction45Success,
        FeedbackCommandFunction51Success,
        FeedbackCommandFunctionA1Success,
        FeedbackCommandFunction40Success,
        FeedbackCommandFunction42Success,
        FeedbackCommandFunction34Success,
        FeedbackCommandFunctionA3Success,
        FeedbackCommandSerialPortFailure,
        FeedbackCommandLengthIsNull,
        OriginalStatus
    }
}
