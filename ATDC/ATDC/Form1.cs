using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.IO.Ports;
using System.Reflection;
using System.Diagnostics;


namespace ATDC
{
    public partial class Form1 : Form
    {
        /****************************************设置全局变量*********************************************************************************/

        double d_time;                                                                                  //创建定时时间
        double d_current;                                                                               //创建电流值   
        double d_ev = 0;                                                                                //初始化EV值变量        

        double[] db_time = new double[1000];                                                            //存储定时时间数组
        double[] db_current = new double[1000];                                                         //存储电流数组
        double[] db_lightintensity = new double[1000];                                                  //存储光强数组
        double[] db_EV1 = new double[1000];                                                             //存储第一组EV值
        double[] db_EV2 = new double[1000];                                                             //存储第二组EV值
                                  
        int i_time = 0;                                                                                 //初始化倒计时
        int i_light;                                                                                    //创建光强数值计数
        int i_add;                                                                                      //创建时间、电流数值计数
        int i_timeinterview = 4;                                                                        //创建时间间隔变量
        int i_sumtime;                                                                                  //创建总时间
        int i_count = 0;                                                                                //初始化测试次数    
       

        Boolean bl_reset = false;                                                                       //指令接收标识符      
        Boolean AidShutDown = false;                                                                    //自动测试内部参数标识符
        Boolean bl_ManualClick = false;                                                                 //“手动测试”复合按钮标识符

        SerialPort sp_sensor = new SerialPort();                                                        //声明传感器串口
        SerialPort sp_USB = new SerialPort();                                                           //声明USB继电器串口      

        //定义发送指令字节数组
        byte[] bt_USB_open = new byte[8] { 0XFE, 0X05, 0X00, 0X00, 0XFF, 0X00, 0X98, 0X35 };                                    //USB继电器开启指令
        byte[] bt_USB_close = new byte[8] { 0XFE, 0X05, 0X00, 0X00, 0X00, 0X00, 0XD9, 0XC5 };                                  //USB继电器关闭指令

        byte[] bt_sensor_reset = new byte[5] { 0X02, 0X55, 0X51, 0X00, 0X00 };                          //传感器复位指令
        byte[] bt_sensor_feedback = new byte[8] { 0X02, 0X55, 0X47, 0X03, 0X01, 0X01, 0XF4, 0X00 };     //传感器反馈方式设置指令
        byte[] bt_sensor_readDate = new byte[5] { 0X02, 0X55, 0X33, 0X00, 0X00 };                       //传感器读取数据指令
        byte[] bt_sensor_reset1 = new byte[5] {0X02,0XAA,0X51,0X00,0XFD};                            //传感器反馈指令1
        byte[] bt_sensor_reset2 = new byte[20] {0X02,0XAA,0XA1,0X05,0X56,0X00,0X00,0X00,0X00,0XA8,0X02,0XAA,0XA1,0X05,0X48,0X00,0X00,0X00,0X00,0X9A };
                                                                                                        //传感器反馈指令2
                                                         
        //Excel文件保存
        string str_fileName;                                                                            //定义变量Excel文件名
        Microsoft.Office.Interop.Excel.Application ExcelApp;                                            //声明Excel应用程序
        Workbook ExcelDoc;                                                                              //声明工作簿
        Worksheet ExcelSheet;                                                                           //声明工作表

        /*********************************************软件启动操作****************************************************************************/

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;                                              //解决线程操作无效
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //指令增加校验字节
            bt_sensor_reset[bt_sensor_reset.Length - 1] = checkout(bt_sensor_reset);              //传感器复位指令增加校验字节
            bt_sensor_feedback[bt_sensor_feedback.Length - 1] = checkout(bt_sensor_feedback);     //传感器反馈方式设置指令增加校验字节 
            bt_sensor_readDate[bt_sensor_readDate.Length - 1] = checkout(bt_sensor_readDate);     //传感器读取数据指令增加校验字节          
        }

        /*********************************************“手动测试”按钮******************************************************************/

        private void btn_ManualTest_Click(object sender, EventArgs e)
        {

            if (bl_ManualClick == false)
            {
                try
                {
                    StartTest();

                    if (sp_USB.IsOpen == true)
                    {
                        sp_USB.Write(bt_USB_open, 0, 8);                                               //发送USB继电器开启指令                                       
                    }

                    timer2.Enabled = true;

                    label3.Text = "已保存数据：";
                    lab_countDown.Text = i_add.ToString() + "/41";
                    d_current = 6.7;
                    txt_IOut.Text = d_current.ToString();

                    btn_AutoTest.Enabled = false;
                    btn_SaveDate.Enabled = true;
                   
                    btn_Reset.Enabled = false;
                    btn_aid.Enabled = false;

                    bl_ManualClick = true;
                    btn_ManualTest.Text = "停止测试";
                }
                catch
                {
                    label4.Text = "端口选择错误！";
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    timer2.Enabled = false;

                    btn_aid.Enabled = true;
                    btn_Reset.Enabled = true;
                    btn_ManualTest.Enabled = true;
                  
                    btn_SaveDate.Enabled = false;

                    if (sp_sensor.IsOpen == true)
                    {
                        sp_sensor.Close();
                    }
                    if (sp_USB.IsOpen == true)
                    {
                        sp_USB.Write(bt_USB_close, 0, 8);
                        sp_USB.Close();
                    }

                    bl_ManualClick = false;
                    btn_ManualTest.Text = "手动测试";
                }
            }

            else
            {

                if (MessageBox.Show("是否停止手动测试？", "信息确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)

                {

                    try
                    {
                        timer2.Enabled = false;
                        bl_ManualClick = false;
                        btn_ManualTest.Text = "手动测试";

                        if (sp_sensor.IsOpen == true)
                        {
                            sp_sensor.Close();
                        }
                        if (sp_USB.IsOpen == true)
                        {
                            sp_USB.Write(bt_USB_close, 0, 8);                             //发送关闭继电器指令
                            sp_USB.Close();
                        }

                        btn_aid.Enabled = true;
                        btn_Reset.Enabled = true;
                        btn_AutoTest.Enabled = true;
                      
                        btn_SaveDate.Enabled = false;
                    }
                    catch
                    {
                        timer2.Enabled = true;
                        bl_ManualClick = true;
                        btn_ManualTest.Text = "停止测试";

                        if (sp_sensor.IsOpen == false)
                        {
                            sp_sensor.Open();
                        }
                        if (sp_USB.IsOpen == false)
                        {                           
                            sp_USB.Open();
                        }
                        if(sp_USB.IsOpen==true)
                        {
                            sp_USB.Write(bt_USB_open, 0, 8);
                        }                                               
                                                  
                        btn_aid.Enabled = false;
                        btn_Reset.Enabled = false;
                        btn_AutoTest.Enabled = false;
                       
                        btn_SaveDate.Enabled = true;
                    }
                }
            }        
        }

        /*********************************************“自动测试”按钮******************************************************************/
       
        private void btn_test_Click(object sender, EventArgs e)
        {
            try
            {           
            if (timer1.Enabled == false)
            {
                    StartTest();
               
                    i_sumtime = 42 * i_timeinterview + 21 + 3;

                    btn_aid.Enabled = false;
                    btn_Reset.Enabled = false;
                    btn_ManualTest.Enabled = false;            
                    btn_EndTest.Enabled = true;
                    btn_AutoTest.Enabled = false;
                
                    timer1.Enabled = true;                                                            //启动定时器，开始计时     

               
                    AidShutDown = true;
                    label3.Text = "";
                    lab_countDown.Text = "";
                

                }

            }
            catch
            {
                label4.Text = "端口选择错误！";
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;

                btn_EndTest.Enabled = false;
                btn_ManualTest.Enabled = true;
                btn_Reset.Enabled = true;
                btn_aid.Enabled = true;
                btn_AutoTest.Enabled = true;

                if (sp_sensor.IsOpen == true)
                {
                    sp_sensor.Close();
                }
                if (sp_USB.IsOpen == true)
                {
                    sp_USB.Write(bt_USB_close, 0, 8);
                    sp_USB.Close();
                }

            }
        }

        /*********************************************************定时器程序*********************************************************************/

        private void timer1_Tick(object sender, EventArgs e)
        {
            d_time += 0.5;                                                    //每次计时加 0.5s

            if (AidShutDown)
            {
                if (d_time == 0.5)
                {
                    if(sp_USB.IsOpen==true)
                    {
                        sp_USB.Write(bt_USB_close, 0, 8);                                
                    }
                                            
                }

                if (d_time == 3)
                {
                    if(sp_USB.IsOpen==true)
                    {
                        sp_USB.Write(bt_USB_open, 0, 8);
                    }
                                                                                                                           
                    d_time = 0;
                    AidShutDown = false;
                    label3.Text = "测试次数：  1/2";
                    lab_countDown.Text = "倒计时：  00:" + (i_sumtime / 60).ToString().PadLeft(2, '0') + ":" + (i_sumtime % 60).ToString().PadLeft(2, '0');
                }
            }

            if(AidShutDown==false)
            {           
            db_time[i_add] = d_time;                                          //计时时间写入时间数组                   

            //显示倒计时
            if ((d_time / 0.5) % 2 == 0)
            {
                i_time = Convert.ToInt32(i_sumtime - d_time);
                lab_countDown.Text = "倒计时：  00:" + (i_time / 60).ToString().PadLeft(2, '0') + ":" + (i_time % 60).ToString().PadLeft(2, '0');
            }
           
            //显示当前电流值
            if (d_time == 1)
            {
                d_current = 6.8;
                txt_IOut.Text = d_current.ToString("0.0");                    //当计时为1s，电流赋值为2.6,文本框显示当前电流值                        
            }
            if ((d_time - 21) >= 0 && (d_time - (i_sumtime - i_timeinterview)) <= 0 && (d_time - 21) % i_timeinterview == 0)
            {
                d_current -= 0.1;                                             //从21s开始，每隔4/5/6/7/8s，电流值自加0.1
                txt_IOut.Text = d_current.ToString("0.0");                    //文本框显示当前电流值                   
            }
            db_current[i_add++] = d_current;                                  //电流值写入电流数组

            //发送读取传感器数据指令          
            if ((d_time == 18.5) || (d_time == 20) || (d_time - i_timeinterview - 19 >= 0 && d_time - i_sumtime + 2 <= 0 && (d_time - i_timeinterview - 19) % i_timeinterview == 0) || (d_time - i_timeinterview - 20 >= 0 && d_time - i_sumtime + 1 <= 0 && (d_time - i_timeinterview - 20) % i_timeinterview == 0))
            {
                    if(sp_sensor.IsOpen==true)
                    {
                        sp_sensor.Write(bt_sensor_readDate, 0, 5);
                    }
                         
                                                

                //将读取数据发送指令显示在文本框中
                txt_order.Text += "TX:";

                for (int j = 0; j < bt_sensor_readDate.Length; j++)
                {
                    if (j < bt_sensor_readDate.Length - 1)
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_readDate[j], 16).PadLeft(2, '0').ToUpper() + "-";
                    }
                    else
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_readDate[j], 16).PadLeft(2, '0').ToUpper() + "\r\n";
                    }
                }
            }

            if(d_time==i_sumtime-3)
            {
                    if(sp_USB.IsOpen==true)
                    {

                    
                    sp_USB.Write(bt_USB_close, 0, 8);                             //发送关闭继电器指令
                    sp_USB.Close();                                               //关闭USB串口                                    
            }
                }
                //计时结束，关闭定时器
                if (d_time == i_sumtime)
            {
                i_count++;
                timer1.Enabled = false;                                       //关闭定时器                            
                    if(sp_sensor.IsOpen==true)
                    {
                        sp_sensor.Close();                                            //关闭传感器串口  
                    }
                                                  
               
                if (i_count == 1)
                {
                    StartTest();
                    label3.Text = "测试次数： 2/2";
                    lab_countDown.Text = "倒计时：  00:" + (i_sumtime / 60).ToString().PadLeft(2, '0') + ":" + (i_sumtime % 60).ToString().PadLeft(2, '0');
                    timer1.Enabled = true;
                        if(sp_USB.IsOpen==true)
                        {
                            sp_USB.Write(bt_USB_open, 0, 8);
                        }
                                                       
                }

                if (i_count == 2)
                {
                    AutoTestCreatExcel();                                             //创建Excel文件  
                

                        btn_aid.Enabled = true;
                        btn_Reset.Enabled = true;
                        btn_ManualTest.Enabled = true;
                        btn_EndTest.Enabled = false;
                        btn_AutoTest.Enabled = true;

                        i_count = 0;                                         
                }
            }
          }
        }

        /*********************************************传感器串口接收指令函数*************************************************/
        public byte[] datareceived = new byte[33];
        int firstpartdatanum = 0;

        private void sp_sensor_DateReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp_sensor.IsOpen)                                                                           //判断传感器串口是否打开
            {
                byte[] bt_receive_readDate = new byte[sp_sensor.BytesToRead];           //创建接收字节数组
                sp_sensor.Read(bt_receive_readDate, 0, bt_receive_readDate.Length);     //读取数据

                //确保指令完整
                try
                {
                    if (bl_reset == true)
                    {
                        for (int i = 0; i < bt_receive_readDate.Length; i++)
                        {
                            datareceived[i] = bt_receive_readDate[i];
                        }
                        bl_reset = false;
                    }
                    else
                    {
                        //数据缺失
                        if (bt_receive_readDate.Length < 33)
                        {
                            //收到后半段
                            if (firstpartdatanum != 0)
                            {
                                for (int i = firstpartdatanum; i - firstpartdatanum < bt_receive_readDate.Length; i++)
                                { datareceived[i] = bt_receive_readDate[i - firstpartdatanum]; }
                                //为下次接收不正常数据做准备
                                firstpartdatanum = 0;
                            }
                            //收到的是前半段
                            else
                            {
                                for (int i = 0; i < datareceived.Length; i++)
                                { datareceived[i] = 0; }

                                for (int i = 0; i < bt_receive_readDate.Length; i++)
                                {
                                    datareceived[i] = bt_receive_readDate[i];
                                    firstpartdatanum++;
                                }
                            }
                        }
                        //接收到完整数据
                        else
                        {
                            for (int i = 0; i < datareceived.Length; i++)
                            { datareceived[i] = bt_receive_readDate[i]; }
                        }
                    }
                }
                catch
                {
                    return;
                }

                if (datareceived.Length != 0)                                                        //判断0：是否有接收到指令
                {
                    if (datareceived[0] == 0X02)                                                     //判断1：指令帧头是否正确
                    {
                        byte bt_checkout = 0X00;                                                     //初始化接收检验字节
                        bt_checkout = checkout(datareceived);                                        //根据检验计算函数计算检验字节

                        if (bt_checkout == datareceived[datareceived.Length - 1])                    //判断2：指令检验字节是否正确
                        {
                            if (datareceived[2] == 0XA3)                                             //判断3：是否为读取传感器数据接收指令
                            {
                                if (datareceived.Length == 33)                                       //判断4：指令字节长度是否正确
                                {
                                    //将读取到的指令显示在文本框
                                    txt_order.Text += "RX:";

                                    for (int j = 0; j < datareceived.Length; j++)
                                    {
                                        if (j < datareceived.Length - 1)
                                        {
                                            txt_order.Text += Convert.ToString(datareceived[j], 16).PadLeft(2, '0').ToUpper() + "-";
                                        }
                                        else
                                        {
                                            txt_order.Text += Convert.ToString(datareceived[j], 16).PadLeft(2, '0').ToUpper() + "\r\n";
                                        }
                                    }

                                    //获取EV值
                                    double d_ev = 0;                                                   //初始化EV值变量
                                    for (int k = 11; k < 15; k++)
                                    {
                                        if (datareceived[15] == 0x30)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 0.1; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 0.01; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 0.001; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 0.0001; }
                                            }
                                        }

                                        if (datareceived[15] == 0x31)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 1; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 0.1; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 0.01; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 0.001; }
                                            }
                                        }

                                        if (datareceived[15] == 0x32)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 10; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 1; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 0.1; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 0.01; }
                                            }
                                        }

                                        if (datareceived[15] == 0x33)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 100; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 10; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 1; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 0.1; }
                                            }
                                        }

                                        if (datareceived[15] == 0x34)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 1000; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 100; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 10; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 1; }
                                            }
                                        }

                                        if (datareceived[15] == 0x35)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 10000; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 1000; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 100; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 10; }
                                            }
                                        }

                                        if (datareceived[15] == 0x36)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 100000; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 10000; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 1000; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 100; }
                                            }
                                        }

                                        if (datareceived[15] == 0x37)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 1000000; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 100000; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 10000; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 1000; }
                                            }
                                        }

                                        if (datareceived[15] == 0x38)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 10000000; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 1000000; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 100000; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 10000; }
                                            }
                                        }

                                        if (datareceived[15] == 0x39)
                                        {
                                            if ((datareceived[k] - 0X30) >= 0)
                                            {
                                                if (k == 11) { d_ev += (datareceived[k] - 0X30) * 100000000; }
                                                if (k == 12) { d_ev += (datareceived[k] - 0X30) * 10000000; }
                                                if (k == 13) { d_ev += (datareceived[k] - 0X30) * 1000000; }
                                                if (k == 14) { d_ev += (datareceived[k] - 0X30) * 100000; }
                                            }
                                        }
                                    }

                                   

                                    txt_EV.Text = d_ev.ToString();                                     //EV值显示在文本框中                          

                                    if (btn_EndTest.Enabled)
                                    {
                                        if (i_count == 0)
                                        {
                                            db_EV1[i_light++] = d_ev;                               //EV值写入EV值数组
                                        }

                                        if (i_count == 1)
                                        {
                                            db_EV2[i_light++] = d_ev;                               //EV值写入EV值数组
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /***********************************************设置Combobox参数*********************************************************/

        //设置转台串口参数
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            
            sp_sensor.PortName = comboBox1.SelectedItem.ToString();      //设置传感器串口的端口号为复选框选中的端口号
            sp_sensor.BaudRate = 38400;                                  //设置传感器串口波特率为38400 baud
            sp_sensor.Parity = Parity.None;                              //设置传感器串口检验位为无
            sp_sensor.DataBits = 8;                                      //设置传感器串口数据为为8位
            sp_sensor.StopBits = StopBits.One;                           //设置传感器串口停止位为1位
        
            sp_sensor.DataReceived += new SerialDataReceivedEventHandler(sp_sensor_DateReceived);   //使用传感器串口指令接收函数

            //Combobox1、2已选，则设置全部按钮可用
            if (comboBox2.SelectedItem != null)
            {
                btn_Reset.Enabled = true;
                btn_ManualTest.Enabled = true;              
                btn_AutoTest.Enabled = true;                               
                btn_aid.Enabled = true;          
            }
                label4.Text = "";
            comboBox1.Enabled = false;
            }
       

        //设置USB继电器串口参数
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                sp_USB.PortName = comboBox2.SelectedItem.ToString();         //设置USB继电器串口的端口号为复选框选中的端口号
                sp_USB.BaudRate = 9600;                                      //设置USB继电器串口波特率为9600 baud
                sp_USB.Parity = Parity.None;                                 //设置USB继电器串口检验位为无
                sp_USB.DataBits = 8;                                         //设置USB继电器串口数据为为8位
                sp_USB.StopBits = StopBits.One;                              //设置USB继电器串口停止位为1位

                //Combobox1、2已选，则设置全部按钮可用
                if (comboBox1.SelectedItem != null)
                {
                    btn_Reset.Enabled = true;
                    btn_ManualTest.Enabled = true;                
                    btn_AutoTest.Enabled = true;     
                    btn_aid.Enabled = true;
                }

                label4.Text = "";
            comboBox2.Enabled = false;
        }

        /***********************************************生成检验字节********************************************************/

        public byte checkout(byte[] bt_addcheckout)
        {
            byte bt_checkout = 0X00;                                     //定义校验字节变量

            for (int i = 0; i < (bt_addcheckout.Length - 1); i++)
            {
                bt_checkout += bt_addcheckout[i];                        //检验字节=字节数组所有字节求和，取低8位
            }

            return bt_checkout;                                          //返回校验字节
        }

        /***********************************************创建excel文件********************************************************/
       
        //自动测试-Excel文件保存
        public void AutoTestCreatExcel()
        {            
            i_count = 0;
                
            for (int i = 0; i < 86; i++)
           { db_lightintensity[i] = (db_EV1[i] + db_EV2[i]) / 2; }

            //创建excel模板
            str_fileName = "d:\\自动测试调光曲线" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";    //文件保存路径及名称
            ExcelApp = new Microsoft.Office.Interop.Excel.Application();                          //创建Excel应用程序 ExcelApp
            ExcelDoc = ExcelApp.Workbooks.Add(Type.Missing);                                      //在应用程序ExcelApp下，创建工作簿ExcelDoc
            ExcelSheet = ExcelDoc.Worksheets.Add(Type.Missing);                                   //在工作簿ExcelDoc下，创建工作表ExcelSheet

            //设置Excel列名
           // ExcelSheet.Cells[1, 1] = "时间";                                                      //第一列标题为“时间”
            ExcelSheet.Cells[1, 1] = "电流";                                                      //第二列标题为“电流”       
            ExcelSheet.Cells[1, 2] = "EV值";                                                      //第三列标题为“EV值”           
           
            for (int i = 0; i < 41; i++)
            {
               // ExcelSheet.Cells[i + 2, 1] = db_time[39 + 2 * i_timeinterview + i * 2 * i_timeinterview].ToString();                           //将计时值保存到Excel工作表第一列
                ExcelSheet.Cells[i + 2, 1] = db_current[39 + 2 * i_timeinterview + i * 2 * i_timeinterview].ToString();              //将电流值保存到Excel工作表第二列                              
                ExcelSheet.Cells[i + 2, 2] = (db_lightintensity[3 + 2 * i]).ToString();
             }
         
            ExcelSheet.SaveAs(str_fileName);                                                      //保存Excel工作表
            ExcelDoc.Close(Type.Missing, str_fileName, Type.Missing);                             //关闭Excel工作簿
            ExcelApp.Quit();                                                                      //退出Excel应用程序
        }

        //手动测试-Excel文件保存
        public void ManualTestCreatExcel()
        {
            //创建excel模板
            str_fileName = "d:\\手动测试调光曲线" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";    //文件保存路径及名称
            ExcelApp = new Microsoft.Office.Interop.Excel.Application();                          //创建Excel应用程序 ExcelApp
            ExcelDoc = ExcelApp.Workbooks.Add(Type.Missing);                                      //在应用程序ExcelApp下，创建工作簿ExcelDoc
            ExcelSheet = ExcelDoc.Worksheets.Add(Type.Missing);                                   //在工作簿ExcelDoc下，创建工作表ExcelSheet

            //设置Excel列名
            ExcelSheet.Cells[1, 1] = "电流";                                                      //第一列标题为“时间”
            ExcelSheet.Cells[1, 2] = "EV值";                                                      //第二列标题为“电流”          
         
            for (int i = 0; i < 41; i++)
            {
                ExcelSheet.Cells[i + 2, 1] = db_current[i].ToString();                           //将计时值保存到Excel工作表第一列
                ExcelSheet.Cells[i + 2, 2] = db_lightintensity[i].ToString();                    //将电流值保存到Excel工作表第二列          
            }
       
            ExcelSheet.SaveAs(str_fileName);                                                      //保存Excel工作表
            ExcelDoc.Close(Type.Missing, str_fileName, Type.Missing);                             //关闭Excel工作簿
            ExcelApp.Quit();                                                                      //退出Excel应用程序
        }

        /***********************************************“转台复位”按钮********************************************************/

            //“转台复位”按钮，发送转台复位指令
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {

                if(sp_sensor.IsOpen==false)
                {
                    sp_sensor.Open();
                }
                if(sp_sensor.IsOpen==true)
                {
                    sp_sensor.Write(bt_sensor_reset, 0, 5);                                          //发送转台复位指令
                }
                      
                        
                bl_reset = true;                                                                 //标识符，接收指令为复位反馈指令，无需做处理   

                txt_order.Text += "TX:";

                for (int j = 0; j < bt_sensor_reset.Length; j++)
                {
                    if (j < bt_sensor_reset.Length - 1)
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_reset[j], 16).PadLeft(2, '0').ToUpper() + "-";
                    }
                    else
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_reset[j], 16).PadLeft(2, '0').ToUpper() + "\r\n";
                    }
                }

                /***************************/
                txt_order.Text += "RX:";

                for (int j = 0; j < bt_sensor_reset1.Length; j++)
                {
                    if (j < bt_sensor_reset1.Length - 1)
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_reset1[j], 16).PadLeft(2, '0').ToUpper() + "-";
                    }
                    else
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_reset1[j], 16).PadLeft(2, '0').ToUpper() + "\r\n";
                    }
                }

                txt_order.Text += "RX:";

                for (int j = 0; j < bt_sensor_reset2.Length; j++)
                {
                    if (j < bt_sensor_reset2.Length - 1)
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_reset2[j], 16).PadLeft(2, '0').ToUpper() + "-";
                    }
                    else
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_reset2[j], 16).PadLeft(2, '0').ToUpper() + "\r\n";
                    }
                }

                if(sp_sensor.IsOpen==true)
                {
                    sp_sensor.Close();
                }
                
            }
            catch
            {
                label4.Text = "端口选择错误！";
                comboBox1.Enabled = true;
                                            
            }
        }

        /************************************************文本框滚轮至底*******************************************************/
        
        private void txt_receive_TextChanged(object sender, EventArgs e)
        {
            if (txt_order.Text.Length > 0)
            {
                txt_order.SelectionStart = txt_order.Text.Length - 1;
                txt_order.ScrollToCaret();
            }
        }

        /***********************************************“停止测试”按钮********************************************************/
        private void btn_end_Click(object sender, EventArgs e)
        {
            try
            {
        
            if (MessageBox.Show("是否停止自动测试？", "信息确认",  MessageBoxButtons.OKCancel,MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                timer1.Enabled = false;                                       //关闭定时器timer1                                      

                btn_aid.Enabled = true;
                btn_Reset.Enabled = true;
                btn_ManualTest.Enabled = true;
                btn_EndTest.Enabled = false;
                btn_AutoTest.Enabled = true;

                if (sp_USB.IsOpen == true)
                {
                    sp_USB.Write(bt_USB_close, 0, 8);                         //发送关闭继电器指令
                    sp_USB.Close();                                           //关闭USB串口
                }

                if (sp_sensor.IsOpen == true)
                {
                    sp_sensor.Close();                                        //关闭传感器串口
                }

            }
            }
            catch
            {
                return;
            }
        }

        /***********************************************“开始测试”功能函数********************************************************/
        public void StartTest()
        {           
                //数据清零
                d_current = 0;                                                                     //电流值
                d_time = 0;                                                                        //定时时间   
                i_light = 0;                                                                       //光强数值计数
                i_add = 0;                                                                         //时间、电流数值计数               

                txt_IOut.Text ="";                                                                 //电流文本框
                txt_EV.Text = "";                                                                  //EV值文本框                   
                txt_order.Text = "";                                                               //指令文本框               

                if (sp_sensor.IsOpen == false)
                {            
                    sp_sensor.Open();                                                               //打开转台传感器串口                    
                 }

                if (sp_USB.IsOpen == false)
                {                
                    sp_USB.Open();                                                              //打开USB继电器串口            
                }                                       
        }             

        /************************************************手动测试“保存数据”按钮*******************************************************/
        //保存合格数据，并最终存放到Excel中
        private void button2_Click(object sender, EventArgs e)
        {           
            try
            {           
            if (sp_sensor.IsOpen == true)
            {                   
                    if (i_add < 40)
                {
                    db_current[i_add] = d_current;                                  //电流值写入电流数组               
                        db_lightintensity[i_add] = Convert.ToDouble(txt_EV.Text);

                    lab_countDown.Text = (++i_add).ToString() + "/41";
                    d_current -= 0.1;
                    txt_IOut.Text = d_current.ToString("0.0");
                        btn_SaveDate.Enabled = false;
                        timer3.Enabled = true;
                    }
                else
                {
                    db_current[i_add] = d_current;                                  //电流值写入电流数组               
                    db_lightintensity[i_add] = Convert.ToDouble(txt_EV.Text);

                    lab_countDown.Text = (++i_add).ToString() + "/41";

                    ManualTestCreatExcel();


                        timer2.Enabled = false;
                        btn_aid.Enabled = true;
                    btn_Reset.Enabled = true;
                    btn_AutoTest.Enabled = true;
                 
                    btn_SaveDate.Enabled = false;

                        if(sp_USB.IsOpen==true)
                        {
                            sp_USB.Write(bt_USB_close, 0, 8);
                            sp_USB.Close();
                        }
                         if(sp_sensor.IsOpen==true)
                        {
                            sp_sensor.Close();
                        }

                        bl_ManualClick = false;
                    btn_ManualTest.Text = "手动测试";
                }
                
            }
            }
            catch
            {
                return;
            }
        }
        /************************************************“辅助功能”按钮*******************************************************/
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if(sp_USB.IsOpen==false)
                {
                    sp_USB.Open();
                }
                if(sp_USB.IsOpen==true)
                {
                    sp_USB.Write(bt_USB_open, 0, 8);
                }      
                                                                                                               
            }
            catch
            {
                label4.Text = "端口选择错误！";
                comboBox2.Enabled = true;

                if(sp_USB.IsOpen==true)
                {
                    sp_USB.Write(bt_USB_close, 0, 8);
                    sp_USB.Close();
                }
                        
            }
        }

        /************************************************关闭软件保护*******************************************************/

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {          
            if (MessageBox.Show("是否关闭软件？", "信息确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                if (sp_sensor.IsOpen == true)
                {
                    sp_sensor.Close();
                }
                if (sp_USB.IsOpen == true)
                {
                    sp_USB.Write(bt_USB_close, 0, 8);                             //发送关闭继电器指令

                    sp_USB.Close();
                }
            }
            else
            {
                e.Cancel = true;
                return;
            }

            }
            catch
            {
                return;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if(sp_sensor.IsOpen==false)
                {
                    sp_sensor.Open();
                }

                if(sp_sensor.IsOpen==true)
                {
                    sp_sensor.Write(bt_sensor_readDate, 0, 5);
                }
                
                //将读取数据发送指令显示在文本框中
                txt_order.Text += "TX:";

                for (int j = 0; j < bt_sensor_readDate.Length; j++)
                {
                    if (j < bt_sensor_readDate.Length - 1)
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_readDate[j], 16).PadLeft(2, '0').ToUpper() + "-";
                    }
                    else
                    {
                        txt_order.Text += Convert.ToString(bt_sensor_readDate[j], 16).PadLeft(2, '0').ToUpper() + "\r\n";
                    }
                }
            }

            catch
            {
                if(sp_sensor.IsOpen==true)
                {
                    sp_sensor.Close();
                }

            }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            btn_SaveDate.Enabled = true;
            timer3.Enabled = false;
        }
    }
}

