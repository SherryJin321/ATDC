﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ATDC_V2._0
{     
    class MiniCCRWithoutCommunicationInterface
    {
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        public OperationStatusMiniCCRWithout OpenPort(SerialPort serialPort, string portName)
        {
            OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;

            serialPort.PortName = portName;
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;

            try
            {
                if (serialPort.IsOpen == false)
                {
                    serialPort.Open();
                }
                if(serialPort.IsOpen==true)
                {
                    result = OperationStatusMiniCCRWithout.OpenPortSuccess;
                }
            }
            catch
            {
                result = OperationStatusMiniCCRWithout.OpenPortFailure;
            }

            return result;            
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusMiniCCRWithout ClosePort(SerialPort serialPort)
        {
            OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;

            try
            {
                if (serialPort.IsOpen == true)
                {
                    serialPort.Close();
                }
                if(serialPort.IsOpen==false)
                {
                    result = OperationStatusMiniCCRWithout.ClosePortSuccess;
                }
            }
            catch
            {
                result = OperationStatusMiniCCRWithout.ClosePortFailure;
            }

            return result;            
        }

        /// <summary>
        /// 发送命令：连接CCR
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusMiniCCRWithout ConnectCCR(SerialPort serialPort)
        {
            OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;

            byte[] sendingCommand = new byte[8] { 0XFE, 0X05, 0X00, 0X00, 0XFF, 0X00, 0X98, 0X35 };

            if(serialPort.IsOpen==true)
            {
                serialPort.Write(sendingCommand, 0, 8);
                result = OperationStatusMiniCCRWithout.ConnectCCRSuccess;
            }
            else
            {
                result = OperationStatusMiniCCRWithout.ConnectCCRFailure;
            }

            return result;
        }

        /// <summary>
        /// 发送命令：断开CCR
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusMiniCCRWithout DisconnectCCR(SerialPort serialPort)
        {
            OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;

            byte[] sendingCommand = new byte[8] { 0XFE, 0X05, 0X00, 0X00, 0X00, 0X00, 0XD9, 0XC5 };                                 

            if(serialPort.IsOpen==true)
            {
                serialPort.Write(sendingCommand, 0, 8);
                result = OperationStatusMiniCCRWithout.DisconnectCCRSuccess;
            }
            else
            {
                result = OperationStatusMiniCCRWithout.DisconnectCCRFailure;
            }

            return result;
        }

        /// <summary>
        /// 发送命令：查询四路状态
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusMiniCCRWithout EnquiryStatus(SerialPort serialPort)
        {
            OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;

            byte[] sendingCommand = new byte[8] { 0XFE, 0X01, 0X00, 0X00, 0X00, 0X04, 0X29, 0XC6 };

            if(serialPort.IsOpen==true)
            {
                serialPort.Write(sendingCommand, 0, 8);
                result = OperationStatusMiniCCRWithout.EnquiryStatusSuccess;
            }
            else
            {
                result = OperationStatusMiniCCRWithout.EnquiryStatusFailure;
            }

            return result;
        }

        /// <summary>
        /// 接收、解析反馈指令
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        public OperationStatusMiniCCRWithout AnalysisFeedbackCommand(SerialPort serialPort)
        {
            OperationStatusMiniCCRWithout result = OperationStatusMiniCCRWithout.OriginalStatus;

            if(serialPort.IsOpen==true)
            {
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);
                
                if(receivedData.Length==6)
                {
                    if(receivedData[0] == 0xFE && receivedData[1] == 0x01 && receivedData[2] == 0x01 && receivedData[3] == 0x00 && receivedData[4] == 0x61 && receivedData[5] == 0x9C)
                    {
                        result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandEnquiryStatusCCROffSuccess;
                    }
                    else if(receivedData[0] == 0xFE && receivedData[1] == 0x01 && receivedData[2] == 0x01 && receivedData[3] == 0x01 && receivedData[4] == 0xA0 && receivedData[5] == 0x5C)
                    {
                        result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandEnquiryStatusCCROnSuccess;
                    }
                    else
                    {
                        result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandEnquiryStatusFailure;
                    }
                }
                else if(receivedData.Length==8)
                {
                    if (receivedData[0] == 0xFE && receivedData[1] == 0x05 && receivedData[2] == 0x00 && receivedData[3] == 0x00 && receivedData[4] == 0xFF && receivedData[5] == 0x00 && receivedData[6] == 0x98 && receivedData[7] == 0x35)
                    {
                        result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandConnectCCRSuccess;
                    }
                    else if (receivedData[0] == 0xFE && receivedData[1] == 0x05 && receivedData[2] == 0x00 && receivedData[3] == 0x00 && receivedData[4] == 0x00 && receivedData[5] == 0x00 && receivedData[6] == 0xD9 && receivedData[7] == 0xC5)
                    {
                        result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandDisconnectCCRSuccess;
                    }
                    else
                    {
                        result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandDisconnectOrConnectCCRFailure;
                    }
                }
                else
                {
                    result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandLengthFailure;
                }                
            }
            else
            {
                result = OperationStatusMiniCCRWithout.AnalysisFeedbackCommandSerialPortFailure;
            }

            return result;
        }

    }

    enum OperationStatusMiniCCRWithout
    {
        OpenPortSuccess,
        OpenPortFailure,
        ClosePortSuccess,
        ClosePortFailure,
        ConnectCCRSuccess,
        ConnectCCRFailure,
        DisconnectCCRSuccess,
        DisconnectCCRFailure,
        EnquiryStatusSuccess,
        EnquiryStatusFailure,
        AnalysisFeedbackCommandSerialPortFailure,
        AnalysisFeedbackCommandLengthFailure,
        AnalysisFeedbackCommandEnquiryStatusCCROffSuccess,
        AnalysisFeedbackCommandEnquiryStatusCCROnSuccess,
        AnalysisFeedbackCommandEnquiryStatusFailure,
        AnalysisFeedbackCommandConnectCCRSuccess,
        AnalysisFeedbackCommandDisconnectCCRSuccess,
        AnalysisFeedbackCommandDisconnectOrConnectCCRFailure,
        OriginalStatus
    }
}