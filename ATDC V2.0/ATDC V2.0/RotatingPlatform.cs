using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ATDC_V2._0
{
    class RotatingPlatform
    {
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform OpenPort(SerialPort serialPort, string portName)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            serialPort.PortName = portName;
            serialPort.BaudRate = 38400;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;

            try
            {
                if (serialPort.IsOpen == false)
                {
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
        /// 获取传感器EV、x、y
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

        /// <summary>
        /// 接收、解析反馈指令
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusRotatingPlatform AnalysisFeedbackCommand(SerialPort serialPort)
        {
            OperationStatusRotatingPlatform result = OperationStatusRotatingPlatform.OriginalStatus;

            byte[] receivedData = new byte[serialPort.BytesToRead];
            serialPort.Read(receivedData, 0, receivedData.Length);

            

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
        OriginalStatus
    }
}
