using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Web
{
    class HikDevice
    {
        public HikDevice(string ip, short port, string username, string pwd)
        {
            try
            {
                m_bInitSDK = CHCNetSDK.NET_DVR_Init();
                if (m_bInitSDK == false)
                {
                    MessageBox.Show("NET_DVR_Init error!");
                    return;
                }

                DVRIPAddress = ip;
                DVRPortNumber = port;
                DVRUserName = username;
                DVRPassword = pwd;

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error("初始化海康设备发生错误:{0}", ex.Message);
            }
        }

        private bool m_bInitSDK = false;
        private Int32 m_lUserID = -1;
        string DVRIPAddress = "192.168.0.100"; //设备IP地址或者域名 Device IP
        Int16 DVRPortNumber = 8000;//设备服务端口号 Device Port
        string DVRUserName = "admin";//设备登录用户名 User name to login
        string DVRPassword = "12345";//设备登录密码 Password to login
        public CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;

        private uint iLastErr = 0;
        private Int32 m_lRealHandle = -1;

        public void LogInfo(string str)
        {
            if (str.Length > 0)
            {
                NLog.LogManager.GetLogger("default").Info("str");
                Console.WriteLine(str);
            }
        }
        private CHCNetSDK.MSGCallBack m_falarmData = null;


        /// <summary>
        /// 当客流信息改变时发生。
        /// </summary>
        public event EventHandler<PDCChangedEventArgs> PDCChanged;


        public void Login()
        {
            try
            {
                NLog.LogManager.GetLogger("default").Info("开始登陆海康设备");

                string str = String.Empty;
                //登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号 Failed to login and output the error code
                    LogInfo(str);
                    return;
                }
                else
                {
                    //登录成功
                    LogInfo("NET_DVR_Login_V30 succ!");

                    ////设置连接时间与重连时间
                    CHCNetSDK.NET_DVR_SetConnectTime(2000, 1);
                    CHCNetSDK.NET_DVR_SetReconnect(10000, 1);

                    ////---------------------------------------
                    //设置报警回调函数
                    if (m_falarmData == null)
                    {
                        m_falarmData = new CHCNetSDK.MSGCallBack(MsgCallback);
                    }
                    CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V30(m_falarmData, IntPtr.Zero);

                    //启用布防
                    CHCNetSDK.NET_DVR_SETUPALARM_PARAM struAlarmParam = new CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
                    struAlarmParam.dwSize = (uint)Marshal.SizeOf(struAlarmParam);
                    struAlarmParam.byLevel = 1; //0- 一级布防,1- 二级布防
                    struAlarmParam.byAlarmInfoType = 1;//智能交通设备有效，新报警信息类型

                    lHandle = CHCNetSDK.NET_DVR_SetupAlarmChan_V41(m_lUserID, ref struAlarmParam);

                    if (lHandle < 0)
                    {
                        Logout();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error("登陆海康设备发生错误:{0}", ex.Message);
            }
        }
        int lHandle;

        public void MsgCallback(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            AlarmMessageHandle(lCommand, ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
        }

        public void AlarmMessageHandle(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            switch (lCommand)
            {
                case CHCNetSDK.COMM_ALARM_PDC://客流量统计报警信息
                    ProcessCommAlarm_PDC(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                default:
                    {
                        //报警信息类型
                        string stringAlarm = "报警上传，未处理报警类型：" + lCommand + "设备:" + pAlarmer.sDeviceIP;
                        NLog.LogManager.GetLogger("default").Debug("{0}", stringAlarm);
                    }
                    break;
            }
        }

        public void Logout()
        {
            NLog.LogManager.GetLogger("default").Info("开始注销海康设备");

            try
            {
                string str = String.Empty;

                if (!CHCNetSDK.NET_DVR_CloseAlarmChan_V30(lHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    string strErr = "撤防失败，错误号：" + iLastErr; //撤防失败，输出错误号
                    LogInfo(strErr);
                }
                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    LogInfo(str);
                    return;
                }
                LogInfo("NET_DVR_Logout succ!");
                m_lUserID = -1;
                NLog.LogManager.GetLogger("default").Info("完成注销海康设备");
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error("注销海康设备发生错误:{0}", ex.Message);
            }
        }

        private CHCNetSDK.NET_VCA_INTRUSION m_struIntrusion = new CHCNetSDK.NET_VCA_INTRUSION();
        private CHCNetSDK.UNION_STATFRAME m_struStatFrame = new CHCNetSDK.UNION_STATFRAME();
        private CHCNetSDK.UNION_STATTIME m_struStatTime = new CHCNetSDK.UNION_STATTIME();

        private void ProcessCommAlarm_PDC(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            CHCNetSDK.NET_DVR_PDC_ALRAM_INFO struPDCInfo = new CHCNetSDK.NET_DVR_PDC_ALRAM_INFO();
            uint dwSize = (uint)Marshal.SizeOf(struPDCInfo);
            struPDCInfo = (CHCNetSDK.NET_DVR_PDC_ALRAM_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_PDC_ALRAM_INFO));

            string stringAlarm = "客流量统计，进入人数：" + struPDCInfo.dwEnterNum + "，离开人数：" + struPDCInfo.dwLeaveNum;

            uint dwUnionSize = (uint)Marshal.SizeOf(struPDCInfo.uStatModeParam);
            IntPtr ptrPDCUnion = Marshal.AllocHGlobal((Int32)dwUnionSize);
            Marshal.StructureToPtr(struPDCInfo.uStatModeParam, ptrPDCUnion, false);

            if (PDCChanged != null)
            {
                PDCChanged(this, new PDCChangedEventArgs(struPDCInfo.dwEnterNum, struPDCInfo.dwLeaveNum));
            }
            if (struPDCInfo.byMode == 0) //单帧统计结果，此处为UTC时间
            {
                m_struStatFrame = (CHCNetSDK.UNION_STATFRAME)Marshal.PtrToStructure(ptrPDCUnion, typeof(CHCNetSDK.UNION_STATFRAME));
                stringAlarm = stringAlarm + "，单帧统计，相对时标：" + m_struStatFrame.dwRelativeTime + "，绝对时标：" + m_struStatFrame.dwAbsTime;
            }
            if (struPDCInfo.byMode == 1) //最小时间段统计结果
            {
                m_struStatTime = (CHCNetSDK.UNION_STATTIME)Marshal.PtrToStructure(ptrPDCUnion, typeof(CHCNetSDK.UNION_STATTIME));

                //开始时间
                string strStartTime = string.Format("{0:D4}", m_struStatTime.tmStart.dwYear) +
                string.Format("{0:D2}", m_struStatTime.tmStart.dwMonth) +
                string.Format("{0:D2}", m_struStatTime.tmStart.dwDay) + " "
                + string.Format("{0:D2}", m_struStatTime.tmStart.dwHour) + ":"
                + string.Format("{0:D2}", m_struStatTime.tmStart.dwMinute) + ":"
                + string.Format("{0:D2}", m_struStatTime.tmStart.dwSecond);

                //结束时间
                string strEndTime = string.Format("{0:D4}", m_struStatTime.tmEnd.dwYear) +
                string.Format("{0:D2}", m_struStatTime.tmEnd.dwMonth) +
                string.Format("{0:D2}", m_struStatTime.tmEnd.dwDay) + " "
                + string.Format("{0:D2}", m_struStatTime.tmEnd.dwHour) + ":"
                + string.Format("{0:D2}", m_struStatTime.tmEnd.dwMinute) + ":"
                + string.Format("{0:D2}", m_struStatTime.tmEnd.dwSecond);

                //stringAlarm = stringAlarm + "，最小时间段统计，开始时间：" + strStartTime + "，结束时间：" + strEndTime;
            }
            //NLog.LogManager.GetLogger("default").Info("stringAlarm");
            Marshal.FreeHGlobal(ptrPDCUnion);
        }

    }
}
