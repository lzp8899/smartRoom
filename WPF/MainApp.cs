// ***********************************************************************
// Assembly         : WinHost
// Author           : lzp
// Created          : 09-16-2018
//
// Last Modified By : lzp
// Last Modified On : 09-22-2018
// ***********************************************************************
// <copyright file="MainApp.cs" company="Microsoft">
//     Copyright © Microsoft 2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web
{
    /// <summary>
    /// Class MainApp.
    /// </summary>
    public class MainApp
    {
        #region 静态成员

        /// <summary>
        /// The lock object
        /// </summary>
        private static object lockObject = new object();
        /// <summary>
        /// The instance
        /// </summary>
        private static MainApp instance;

        /// <summary>
        /// 获取对象的实例。
        /// </summary>
        /// <value>The instance.</value>
        public static MainApp Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new MainApp();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Saves the json.
        /// </summary>
        internal void SaveJson()
        {
            try
            {
                string jsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApiDisplayInfo.json");
                string jsonData = JsonConvert.SerializeObject(ApiDisplayInfo);
                File.WriteAllText(jsonFile, jsonData);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// Prevents a default instance of the <see cref="MainApp" /> class from being created.
        /// </summary>
        private MainApp()
        {
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();

            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                //空值处理
                setting.NullValueHandling = NullValueHandling.Ignore;
                // 设置为驼峰命名
                setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ////Bool类型转换 设置
                //setting.Converters.Add(new BoolConvert("是,否"));
                return setting;
            });

            LoadJson();
        }

        public void LoadJson()
        {
            try
            {
                string jsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApiDisplayInfo.json");
                string jsonData = File.ReadAllText(jsonFile);
                ApiDisplayInfo = JsonConvert.DeserializeObject<ApiDisplayInfo>(jsonData);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error(ex.Message);
            }
        }

        /// <summary>
        /// Gets or sets the API display information.
        /// </summary>
        /// <value>The API display information.</value>
        public ApiDisplayInfo ApiDisplayInfo { get; set; }

        /// <summary>
        /// The listener
        /// </summary>
        AsyncTcpListener listener = null;
        /// <summary>
        /// The device server
        /// </summary>
        public IOTDeviceManager deviceServer = null;
        /// <summary>
        /// The hik device
        /// </summary>
        HikDevice hikDevice = new HikDevice(ConfigHelper.HikIP, 8000, ConfigHelper.HikUserName, ConfigHelper.HikPwd);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            NLog.LogManager.GetLogger("default").Info("开始启动");

            listener = new AsyncTcpListener(6000);
            deviceServer = new IOTDeviceManager(listener);
            deviceServer.DeviceStateChanged += DeviceServer_DeviceStateChanged;
            deviceServer.Start();
            listener.Start();

            NLog.LogManager.GetLogger("default").Info("开始登陆海康设备");

            hikDevice.PDCChanged += HikDevice_PDCChanged;
            hikDevice.Login();

            MessageInfo request = new MessageInfo();
            var rt = JsonConvert.SerializeObject(request);
            NLog.LogManager.GetLogger("default").Info("启动完成");

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        bool result = autoResetEvent.WaitOne(60 * 1000);
            //        if (result)
            //        {
            //            break;
            //        }
            //        SaveJson();
            //    }
            //});
        }

        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Stop()
        {
            NLog.LogManager.GetLogger("default").Info("开始停止服务");
            autoResetEvent.Set();
            if (listener != null)
            {
                listener.Stop();
            }

            if (deviceServer != null)
            {
                deviceServer.Stop();
            }

            if (hikDevice != null)
            {
                hikDevice.Logout();
            }
            NLog.LogManager.GetLogger("default").Info("结束停止服务");

            return true;
        }

        private void DeviceServer_DeviceStateChanged(object sender, DeviceStateChangedEventArgs e)
        {
            ParserMsg(e.Info, e.Device);
        }

        private bool ParserMsg(MessageInfo newMessageInfo, IOTDevice device)
        {
            try
            {
                if (newMessageInfo.Id != device.ID)
                {
                    return false;
                }
                if (newMessageInfo.messagetype == MessageType.request.ToString())
                {
                    //if (newMessageInfo.TypeID == "101" || newMessageInfo.TypeID == "102" || newMessageInfo.TypeID == "103")
                    if (false)
                    {
                        int remainCount = 0;
                        bool alarm = false;
                        if (newMessageInfo.parameter == VolumeLevel.H.ToString())
                        {
                            remainCount = 100 / 1;
                        }

                        if (newMessageInfo.parameter == VolumeLevel.H.ToString())
                        {
                            remainCount = (100 / 3) * 2;
                        }

                        if (newMessageInfo.parameter == VolumeLevel.L.ToString())
                        {
                            remainCount = 100 / 3;
                            alarm = true;
                        }

                        ApiDisplayInfo.smczj.ToList().ForEach(item =>
                        {
                            if (item.id == newMessageInfo.Id)
                            {
                                item.ramainder = remainCount;
                                item.alarm = alarm;
                                item.alarmtime = DateTime.Now.ToString();
                            }
                        });
                    }

                    if (newMessageInfo.TypeID == "105")
                    {
                        string[] datas = newMessageInfo.parameter.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (datas.Length < 2)
                        {
                            NLog.LogManager.GetLogger("default").Error("异味检测数据异常，无法解析:{0}", newMessageInfo);
                            return false;
                        }
                        float ppmNH3 = 0;
                        bool result = float.TryParse(datas[0], out ppmNH3);
                        if (result)
                        {
                            ApiDisplayInfo.monitors.ppmNH3 = ppmNH3;
                            int level = 5;
                            string strLevel = "五";
                            float nowCount = ppmNH3;

                            if (nowCount <= 20)
                            {
                                level = 5;
                                strLevel = "五";
                            }
                            if (nowCount > 20 && nowCount <= 36)
                            {
                                level = 4;
                                strLevel = "四";
                            }
                            if (nowCount > 36 && nowCount <= 50)
                            {
                                level = 3;
                                strLevel = "三";
                            }
                            if (nowCount > 50 && nowCount <= 70)
                            {
                                level = 2;
                                strLevel = "二";
                            }
                            if (nowCount >= 70)
                            {
                                level = 1;
                                strLevel = "一";
                            }

                            ApiDisplayInfo.monitors.ppmNH3Level = level;
                            ApiDisplayInfo.monitors.alarm = level <= 3;
                            ApiDisplayInfo.monitors.alarmtime = DateTime.Now.ToString();
                            if (ApiDisplayInfo.monitors.alarm)
                            {
                                ApiDisplayInfo.monitors.info = String.Format("{0}级氨气报警，启动{1}级除异味作业！", strLevel, strLevel);
                            }
                        }

                        float ppmH2S = 0;
                        result = float.TryParse(datas[1], out ppmH2S);
                        if (result)
                        {
                            ApiDisplayInfo.monitors.ppmH2S = ppmH2S;
                            int level = 5;
                            string strLevel = "五";
                            float nowCount = ppmH2S;

                            if (nowCount <= 4.6)
                            {
                                level = 5;
                                strLevel = "五";
                            }
                            if (nowCount > 4.6 && nowCount <= 10)
                            {
                                level = 4;
                                strLevel = "四";
                            }
                            if (nowCount > 10 && nowCount <= 20)
                            {
                                level = 3;
                                strLevel = "三";
                            }
                            if (nowCount > 20 && nowCount <= 25)
                            {
                                level = 2;
                                strLevel = "二";
                            }
                            if (nowCount >= 25)
                            {
                                level = 1;
                                strLevel = "一";
                            }

                            ApiDisplayInfo.monitors.ppmH2S = level;
                            ApiDisplayInfo.monitors.alarm = level <= 3;
                            ApiDisplayInfo.monitors.alarmtime = DateTime.Now.ToString();
                            if (ApiDisplayInfo.monitors.alarm)
                            {
                                ApiDisplayInfo.monitors.info = String.Format("{0}级硫化氢报警，启动{1}级除异味作业！", strLevel, strLevel);
                            }
                        }
                    }

                    if (newMessageInfo.TypeID == "106")
                    {
                        string[] datas = newMessageInfo.parameter.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (datas.Length < 3)
                        {
                            NLog.LogManager.GetLogger("default").Error("温湿度数据异常，无法解析:{0}", newMessageInfo);
                            return false;
                        }

                        float temperature = 0;
                        bool result = float.TryParse(datas[0], out temperature);
                        if (result)
                        {
                            ApiDisplayInfo.monitors.temperature = temperature;
                        }
                        float humidity = 0;
                        result = float.TryParse(datas[1], out humidity);
                        if (result)
                        {
                            ApiDisplayInfo.monitors.humidity = humidity;
                        }
                        float pm25 = 0;
                        result = float.TryParse(datas[2], out pm25);

                        if (result)
                        {
                            ApiDisplayInfo.monitors.pm25 = pm25;
                            int level = 5;
                            string strLevel = "五";
                            float nowCount = pm25;

                            if (nowCount <= 35)
                            {
                                level = 5;
                                strLevel = "五";
                            }
                            if (nowCount > 35 && nowCount <= 75)
                            {
                                level = 4;
                                strLevel = "四";
                            }
                            if (nowCount > 75 && nowCount <= 115)
                            {
                                level = 3;
                                strLevel = "三";
                            }
                            if (nowCount > 150 && nowCount <= 250)
                            {
                                level = 2;
                                strLevel = "二";
                            }
                            if (nowCount >= 250)
                            {
                                level = 1;
                                strLevel = "一";
                            }

                            ApiDisplayInfo.monitors.pm25Level = level;
                            ApiDisplayInfo.monitors.alarm = level <= 3;
                            ApiDisplayInfo.monitors.alarmtime = DateTime.Now.ToString();
                            if (ApiDisplayInfo.monitors.alarm)
                            {
                                ApiDisplayInfo.monitors.info = String.Format("{0}级PM2.5报警，启动{1}级除异味作业！", strLevel, strLevel);
                            }

                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error("发送消息发生错误，ID:{0}.参考信息:{1}。", newMessageInfo.Id, ex.Message);
                return false;
            }
        }

        Stopwatch stopwatch = new Stopwatch();
        private int lastEnterNum = 0;
        //当前人数
        int nowCount = 0;

        /// <summary>
        /// Handles the PDCChanged event of the HikDevice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PDCChangedEventArgs"/> instance containing the event data.</param>
        private void HikDevice_PDCChanged(object sender, PDCChangedEventArgs e)
        {
            NLog.LogManager.GetLogger("default").Info("lastEnterNum：{0} EnterNum：{1}", lastEnterNum, e.EnterNum);

            if (lastEnterNum == 0 || (int)e.EnterNum == 0)
            {
                lastEnterNum = (int)e.EnterNum;
            }

            if (stopwatch.Elapsed.TotalSeconds >= 10 || lastEnterNum == 0 || !stopwatch.IsRunning)
            {
                nowCount = (int)(e.EnterNum - lastEnterNum);

                NLog.LogManager.GetLogger("default").Info("nowCount：{0}", nowCount);

                lastEnterNum = (int)e.EnterNum;
                stopwatch.Restart();
            }

            int level = 5;
            string strLevel = "五";
            if (nowCount <= 11)
            {
                level = 5;
                strLevel = "五";
            }
            if (nowCount > 11 && nowCount <= 17)
            {
                level = 4;
                strLevel = "四";
            }
            if (nowCount > 17 && nowCount <= 25)
            {
                level = 3;
                strLevel = "三";
            }
            if (nowCount > 25 && nowCount <= 32)
            {
                level = 2;
                strLevel = "二";
            }
            if (nowCount >= 32)
            {
                level = 1;
                strLevel = "一";
            }

            ApiDisplayInfo.flowCount.flowRateLevel = level;
            ApiDisplayInfo.flowCount.alarm = level <= 3;
            ApiDisplayInfo.flowCount.alarmtime = DateTime.Now.ToString();
            if (ApiDisplayInfo.flowCount.alarm)
            {
                ApiDisplayInfo.flowCount.info = String.Format("{0}级客流量报警，启动{1}级除异味作业！", strLevel, strLevel);
            }

            ApiDisplayInfo.flowCount.flowRateByNow = nowCount;
            ApiDisplayInfo.flowCount.flowRateByDay = (int)e.EnterNum;
            ApiDisplayInfo.flowCount.flowRateByHour = ApiDisplayInfo.flowCount.flowRateByDay / (DateTime.Now.Hour + 1);
        }
    }
}
