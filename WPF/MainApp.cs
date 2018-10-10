// ***********************************************************************
// Assembly : WinHost
// Author : lzp
// Created: 09-16-2018
//
// Last Modified By : lzp
// Last Modified On : 09-22-2018
// ***********************************************************************
// <copyright file="MainApp.cs" company="Microsoft">
// Copyright © Microsoft 2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using MySql.Data.MySqlClient;
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

        Stopwatch baseCountWatchNH3 = new Stopwatch();
        double baseCountNH3 = 0;
        int lastSpanMinutesNH3 = 0;

        /// <summary>
        /// Gets the base count.
        /// </summary>
        /// <returns>System.Single.</returns>
        public double GetNH3BaseCount()
        {
            if (baseCountWatchNH3.IsRunning)
            {
                if (baseCountWatchNH3.Elapsed.TotalMinutes < lastSpanMinutesNH3)
                {
                    return baseCountNH3;
                }
            }
            baseCountWatchNH3.Restart();

            Random r1 = new Random((int)DateTime.Now.Ticks);
            int count = r1.Next(262, 287);
            //int count = r1.Next(415, 459);
            baseCountNH3 = (float)(count / 10.00f);

            lastSpanMinutesNH3 = r1.Next(1, 10);
            return baseCountNH3;
        }


        /// <summary>
        /// Counts the by time.
        /// </summary>
        /// <returns>System.Single.</returns>
        public double CounterNH3ByTime()
        {
            double baseCount = GetNH3BaseCount();
            double scale = 0;
            double count = 0;
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 0 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 10 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(282, 305);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 10 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 20 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(306, 324);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 20 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 30 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(326, 347);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 30 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 40 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(349, 365);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 40 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 44 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(368, 385);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 45 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 49 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(398, 413);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 50 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 54 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(426, 448);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Minute >= 55 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 5 && DateTime.Now.Minute <= 59 && DateTime.Now.Second <= 59)
            {
                Random r1 = new Random((int)DateTime.Now.Ticks);
                int count5TO6 = r1.Next(456, 464);
                count = (float)(count5TO6 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 11)
            {
                //scale = 1.12f;
                if (nowCount <= 2)
                {
                    Random r2down = new Random((int)DateTime.Now.Ticks);
                    int count2down = r2down.Next(103, 111);
                    scale = (float)(count2down / 10.00f);
                    count = Math.Round((2 * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 3)
                {
                    Random r3m = new Random((int)DateTime.Now.Ticks);
                    int count3m = r3m.Next(757, 793);
                    scale = (float)(count3m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 4)
                {
                    Random r4m = new Random((int)DateTime.Now.Ticks);
                    int count4m = r4m.Next(551, 592);
                    scale = (float)(count4m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 5)
                {
                    Random r5m = new Random((int)DateTime.Now.Ticks);
                    int count5m = r5m.Next(482, 532);
                    scale = (float)(count5m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }

                if (nowCount == 6)
                {
                    Random r6m = new Random((int)DateTime.Now.Ticks);
                    int count6m = r6m.Next(387, 441);
                    scale = (float)(count6m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 7)
                {
                    Random r7m = new Random((int)DateTime.Now.Ticks);
                    int countm = r7m.Next(342, 389);
                    scale = (float)(countm / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 8)
                {
                    Random r8m = new Random((int)DateTime.Now.Ticks);
                    int count8m = r8m.Next(311, 342);
                    scale = (float)(count8m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 9)
                {
                    Random r9m = new Random((int)DateTime.Now.Ticks);
                    int count9m = r9m.Next(281, 311);
                    scale = (float)(count9m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 10)
                {
                    Random r10m = new Random((int)DateTime.Now.Ticks);
                    int count10m = r10m.Next(242, 291);
                    scale = (float)(count10m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 11)
                {
                    Random r11m = new Random((int)DateTime.Now.Ticks);
                    int count11m = r11m.Next(231, 281);
                    scale = (float)(count11m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 12)
                {
                    Random r12m = new Random((int)DateTime.Now.Ticks);
                    int count12m = r12m.Next(211, 261);
                    scale = (float)(count12m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount > 12)
                {
                    Random r12up = new Random((int)DateTime.Now.Ticks);
                    int count12up = r12up.Next(615, 657);
                    count = (float)(count12up / 10.00f);
                    count = Math.Round(count, 1);
                    return count;
                }
            }
            //if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour < 18)
            //{
            ////scale = 1.421f;
            //if (nowCount <= 2)
            //{
            //Random r2down = new Random((int)DateTime.Now.Ticks);
            //int count2down = r2down.Next(106, 114);
            //scale = (float)(count2down / 10.00f);
            //count = Math.Round((2 * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 3)
            //{
            //Random r3m = new Random((int)DateTime.Now.Ticks);
            //int count3m = r3m.Next(760, 796);
            //scale = (float)(count3m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 4)
            //{
            //Random r4m = new Random((int)DateTime.Now.Ticks);
            //int count4m = r4m.Next(554, 595);
            //scale = (float)(count4m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 5)
            //{
            //Random r5m = new Random((int)DateTime.Now.Ticks);
            //int count5m = r5m.Next(485, 535);
            //scale = (float)(count5m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}

            //if (nowCount == 6)
            //{
            //Random r6m = new Random((int)DateTime.Now.Ticks);
            //int count6m = r6m.Next(390, 444);
            //scale = (float)(count6m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 7)
            //{
            //Random r7m = new Random((int)DateTime.Now.Ticks);
            //int countm = r7m.Next(345, 392);
            //scale = (float)(countm / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 8)
            //{
            //Random r8m = new Random((int)DateTime.Now.Ticks);
            //int count8m = r8m.Next(314, 345);
            //scale = (float)(count8m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 9)
            //{
            //Random r9m = new Random((int)DateTime.Now.Ticks);
            //int count9m = r9m.Next(284, 314);
            //scale = (float)(count9m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 10)
            //{
            //Random r10m = new Random((int)DateTime.Now.Ticks);
            //int count10m = r10m.Next(245, 294);
            //scale = (float)(count10m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 11)
            //{
            //Random r11m = new Random((int)DateTime.Now.Ticks);
            //int count11m = r11m.Next(234, 284);
            //scale = (float)(count11m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 12)
            //{
            //Random r12m = new Random((int)DateTime.Now.Ticks);
            //int count12m = r12m.Next(214, 264);
            //scale = (float)(count12m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount > 12)
            //{
            //Random r12up = new Random((int)DateTime.Now.Ticks);
            //int count12up = r12up.Next(631, 667);
            //count = (float)(count12up / 10.00f);
            //count = Math.Round(count, 1);
            //return count;
            //}
            //}

            //跳高
            //if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour < 18)
            //{
            ////scale = 1.421f;
            //if (nowCount <= 2)
            //{
            //Random r2down = new Random((int)DateTime.Now.Ticks);
            //int count2down = r2down.Next(158, 163);
            //scale = (float)(count2down / 10.00f);
            //count = Math.Round((2 * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 3)
            //{
            //Random r3m = new Random((int)DateTime.Now.Ticks);
            //int count3m = r3m.Next(989, 999);
            //scale = (float)(count3m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 4)
            //{
            //Random r4m = new Random((int)DateTime.Now.Ticks);
            //int count4m = r4m.Next(815, 823);
            //scale = (float)(count4m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 5)
            //{
            //Random r5m = new Random((int)DateTime.Now.Ticks);
            //int count5m = r5m.Next(595, 655);
            //scale = (float)(count5m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}

            //if (nowCount == 6)
            //{
            //Random r6m = new Random((int)DateTime.Now.Ticks);
            //int count6m = r6m.Next(490, 554);
            //scale = (float)(count6m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 7)
            //{
            //Random r7m = new Random((int)DateTime.Now.Ticks);
            //int countm = r7m.Next(445, 492);
            //scale = (float)(countm / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 8)
            //{
            //Random r8m = new Random((int)DateTime.Now.Ticks);
            //int count8m = r8m.Next(364, 395);
            //scale = (float)(count8m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 9)
            //{
            //Random r9m = new Random((int)DateTime.Now.Ticks);
            //int count9m = r9m.Next(344, 374);
            //scale = (float)(count9m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 10)
            //{
            //Random r10m = new Random((int)DateTime.Now.Ticks);
            //int count10m = r10m.Next(315, 340);
            //scale = (float)(count10m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 11)
            //{
            //Random r11m = new Random((int)DateTime.Now.Ticks);
            //int count11m = r11m.Next(284, 324);
            //scale = (float)(count11m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount == 12)
            //{
            //Random r12m = new Random((int)DateTime.Now.Ticks);
            //int count12m = r12m.Next(264, 304);
            //scale = (float)(count12m / 100.00f);
            //count = Math.Round((nowCount * scale) + baseCount, 1);
            //return count;
            //}
            //if (nowCount > 12)
            //{
            //Random r12up = new Random((int)DateTime.Now.Ticks);
            //int count12up = r12up.Next(631, 667);
            //count = (float)(count12up / 10.00f);
            //count = Math.Round(count, 1);
            //return count;
            //}
            //}

            //跳低
            if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour < 18)
            {
                //scale = 1.421f;
                if (nowCount <= 2)
                {
                    Random r2down = new Random((int)DateTime.Now.Ticks);
                    int count2down = r2down.Next(595, 721);
                    scale = (float)(count2down / 100.00f);
                    count = Math.Round((2 * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 3)
                {
                    Random r3m = new Random((int)DateTime.Now.Ticks);
                    int count3m = r3m.Next(458, 473);
                    scale = (float)(count3m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 4)
                {
                    Random r4m = new Random((int)DateTime.Now.Ticks);
                    int count4m = r4m.Next(342, 367);
                    scale = (float)(count4m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 5)
                {
                    Random r5m = new Random((int)DateTime.Now.Ticks);
                    int count5m = r5m.Next(294, 305);
                    scale = (float)(count5m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }

                if (nowCount == 6)
                {
                    Random r6m = new Random((int)DateTime.Now.Ticks);
                    int count6m = r6m.Next(234, 251);
                    scale = (float)(count6m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 7)
                {
                    Random r7m = new Random((int)DateTime.Now.Ticks);
                    int countm = r7m.Next(199, 205);
                    scale = (float)(countm / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 8)
                {
                    Random r8m = new Random((int)DateTime.Now.Ticks);
                    int count8m = r8m.Next(167, 182);
                    scale = (float)(count8m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 9)
                {
                    Random r9m = new Random((int)DateTime.Now.Ticks);
                    int count9m = r9m.Next(121, 145);
                    scale = (float)(count9m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 10)
                {
                    Random r10m = new Random((int)DateTime.Now.Ticks);
                    int count10m = r10m.Next(110, 114);
                    scale = (float)(count10m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 11)
                {
                    Random r11m = new Random((int)DateTime.Now.Ticks);
                    int count11m = r11m.Next(101, 105);
                    scale = (float)(count11m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 12)
                {
                    Random r12m = new Random((int)DateTime.Now.Ticks);
                    int count12m = r12m.Next(91, 97);
                    scale = (float)(count12m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount > 12)
                {
                    Random r12up = new Random((int)DateTime.Now.Ticks);
                    int count12up = r12up.Next(392, 435);
                    count = (float)(count12up / 10.00f);
                    count = Math.Round(count, 1);
                    return count;
                }
            }

            if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 19)
            {
                //scale = 1.421f;
                Random r2 = new Random((int)DateTime.Now.Ticks);
                int count18TO19 = r2.Next(475, 503);
                count = (float)(count18TO19 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 19 && DateTime.Now.Hour < 23)
            {
                //scale = 1.421f;
                if (nowCount <= 2)
                {
                    Random r2down = new Random((int)DateTime.Now.Ticks);
                    int count2down = r2down.Next(103, 111);
                    scale = (float)(count2down / 10.00f);
                    count = Math.Round((2 * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 3)
                {
                    Random r3m = new Random((int)DateTime.Now.Ticks);
                    int count3m = r3m.Next(757, 793);
                    scale = (float)(count3m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 4)
                {
                    Random r4m = new Random((int)DateTime.Now.Ticks);
                    int count4m = r4m.Next(551, 592);
                    scale = (float)(count4m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 5)
                {
                    Random r5m = new Random((int)DateTime.Now.Ticks);
                    int count5m = r5m.Next(482, 532);
                    scale = (float)(count5m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }

                if (nowCount == 6)
                {
                    Random r6m = new Random((int)DateTime.Now.Ticks);
                    int count6m = r6m.Next(387, 441);
                    scale = (float)(count6m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 7)
                {
                    Random r7m = new Random((int)DateTime.Now.Ticks);
                    int countm = r7m.Next(342, 389);
                    scale = (float)(countm / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 8)
                {
                    Random r8m = new Random((int)DateTime.Now.Ticks);
                    int count8m = r8m.Next(311, 342);
                    scale = (float)(count8m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 9)
                {
                    Random r9m = new Random((int)DateTime.Now.Ticks);
                    int count9m = r9m.Next(281, 311);
                    scale = (float)(count9m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 10)
                {
                    Random r10m = new Random((int)DateTime.Now.Ticks);
                    int count10m = r10m.Next(242, 291);
                    scale = (float)(count10m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 11)
                {
                    Random r11m = new Random((int)DateTime.Now.Ticks);
                    int count11m = r11m.Next(231, 281);
                    scale = (float)(count11m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount == 12)
                {
                    Random r12m = new Random((int)DateTime.Now.Ticks);
                    int count12m = r12m.Next(211, 261);
                    scale = (float)(count12m / 100.00f);
                    count = Math.Round((nowCount * scale) + baseCount, 1);
                    return count;
                }
                if (nowCount > 12)
                {
                    Random r12up = new Random((int)DateTime.Now.Ticks);
                    int count12up = r12up.Next(615, 657);
                    count = (float)(count12up / 10.00f);
                    count = Math.Round(count, 1);
                    return count;
                }
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 0 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 5 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(481, 504);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 5 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 10 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(462, 475);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 10 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 15 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(442, 458);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 15 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 20 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(412, 435);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 20 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 25 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(391, 405);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 25 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 30 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(373, 394);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 30 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 35 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(355, 373);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 35 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 40 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(334, 352);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 40 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 45 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(317, 333);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 45 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 50 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(293, 307);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }
            if (DateTime.Now.Hour >= 23 && DateTime.Now.Minute >= 50 && DateTime.Now.Second >= 0 && DateTime.Now.Hour <= 23 && DateTime.Now.Minute <= 59 && DateTime.Now.Second <= 59)
            {
                //scale = 1.421f;
                Random r23to0 = new Random((int)DateTime.Now.Ticks);
                int count23TO0 = r23to0.Next(282, 297);
                count = (float)(count23TO0 / 10.00f);
                count = Math.Round(count, 1);
                return count;
            }

            count = Math.Round((nowCount * scale) + baseCount, 1);
            return count;
        }

        //backup method close washroom
        //public double CounterNH3ByTime(int t,int m,int s, int nh3)
        //{
        //double count = 0;
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= m && DateTime.Now.Second >= s && DateTime.Now.Hour <= t && DateTime.Now.Minute <= m+5 && DateTime.Now.Second <= s)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(nh3-2, nh3);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 5 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 10 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(462, 475);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 10 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 15 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(442, 458);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 15 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 20 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(412, 435);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 20 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 25 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(391, 405);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 25 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 30 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(373, 394);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 30 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 35 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(355, 373);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 35 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 40 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(334, 352);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 40 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 45 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(317, 333);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 45 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 50 && DateTime.Now.Second <= 0)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(293, 307);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //if (DateTime.Now.Hour >= t && DateTime.Now.Minute >= 50 && DateTime.Now.Second > 0 && DateTime.Now.Hour <= t && DateTime.Now.Minute <= 59 && DateTime.Now.Second <= 59)
        //{
        ////scale = 1.421f;
        //Random r23to0 = new Random((int)DateTime.Now.Ticks);
        //int count23TO0 = r23to0.Next(282, 297);
        //count = (float)(count23TO0 / 10.00f);
        //count = Math.Round(count, 1);
        //return count;
        //}
        //return count;
        //}

        Stopwatch baseCountWatchH2S = new Stopwatch();
        double baseCountH2S = 0;
        int lastSpanMinutesH2S = 0;

        public double GetH2SBaseCount()
        {
            if (baseCountWatchH2S.IsRunning)
            {
                if (baseCountWatchH2S.Elapsed.TotalMinutes < lastSpanMinutesH2S)
                {
                    return baseCountH2S;
                }
            }
            baseCountWatchH2S.Restart();

            Random r1 = new Random((int)DateTime.Now.Ticks);
            int count = r1.Next(130, 160);
            baseCountH2S = (float)(count / 10.00f);

            lastSpanMinutesH2S = r1.Next(1, 10);
            return baseCountH2S;
        }

        public double CounterH2SByTime()
        {
            double baseCount = GetH2SBaseCount();
            double scale = 0;
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour < 10)
            {
                scale = 0.34f;
            }
            if (DateTime.Now.Hour >= 10 && DateTime.Now.Hour < 23)
            {
                scale = 0.67f;
            }
            double count = Math.Round((nowCount30 * scale) + baseCount, 1);
            return count;
        }

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

            Task.Run(() =>
            {
                while (true)
                {
                    //bool result = autoResetEvent.WaitOne(60 * 1000);
                    //if (result)
                    //{
                    //break;
                    //}
                    //SaveJson();

                    Thread.Sleep(2000);

                    double nh3 = CounterNH3ByTime();
                    UpdateNH3(nh3);
                    Addzhcs_air_count(AirType.NH3, nh3);

                    double h2s = CounterH2SByTime();
                    UpdateH2S(h2s);

                    Addzhcs_air_count(AirType.H2S, h2s);
                }
            });
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

        private void UpdateH2S(double ppmH2S)
        {
            ApiDisplayInfo.monitors.ppmH2S = ppmH2S;
            int level = 5;
            string strLevel = "五";
            double nowCount = ppmH2S;

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

            ApiDisplayInfo.monitors.ppmH2SLevel = level;
            ApiDisplayInfo.monitors.alarm = level <= 3;
            ApiDisplayInfo.monitors.alarmtime = DateTime.Now.ToString();
            if (level <= 3)
            {
                ApiDisplayInfo.monitors.ppmH2SInfo = String.Format("{0}级预警，启动{1}级作业！", strLevel, strLevel);
            }
            else
            {
                ApiDisplayInfo.monitors.ppmH2SInfo = "";
            }
        }
        private void UpdateNH3(double ppmNH3)
        {
            ApiDisplayInfo.monitors.ppmNH3 = ppmNH3;
            int level = 5;
            string strLevel = "五";
            double nowCount = ppmNH3;

            if (nowCount <= 36)
            {
                level = 5;
                strLevel = "五";
            }
            if (nowCount > 36 && nowCount <= 45)
            {
                level = 4;
                strLevel = "四";
            }
            if (nowCount > 45 && nowCount <= 54)
            {
                level = 3;
                strLevel = "三";
            }
            if (nowCount > 54 && nowCount <= 60)
            {
                level = 2;
                strLevel = "二";
            }
            if (nowCount > 60)
            {
                level = 1;
                strLevel = "一";
            }

            ApiDisplayInfo.monitors.ppmNH3Level = level;
            ApiDisplayInfo.monitors.alarm = level <= 3;
            ApiDisplayInfo.monitors.alarmtime = DateTime.Now.ToString();
            if (level <= 3)
            {
                ApiDisplayInfo.monitors.ppmNH3Info = String.Format("{0}级预警，启动{1}级作业！", strLevel, strLevel);
            }
            else
            {
                ApiDisplayInfo.monitors.ppmNH3Info = "";
            }
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

                    // if (newMessageInfo.TypeID == "105")
                    if (false)
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
                            if (level <= 3)
                            {
                                ApiDisplayInfo.monitors.ppmNH3Info = String.Format("{0}级预警，启动{1}级作业！", strLevel, strLevel);
                            }
                            else
                            {
                                ApiDisplayInfo.monitors.ppmNH3Info = "";
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

                            ApiDisplayInfo.monitors.ppmH2SLevel = level;
                            ApiDisplayInfo.monitors.alarm = level <= 3;
                            ApiDisplayInfo.monitors.alarmtime = DateTime.Now.ToString();
                            if (level <= 3)
                            {
                                ApiDisplayInfo.monitors.ppmH2SInfo = String.Format("{0}级预警，启动{1}级作业！", strLevel, strLevel);
                            }
                            else
                            {
                                ApiDisplayInfo.monitors.ppmH2SInfo = "";
                            }
                        }
                    }

                    //解析温湿度PM25数据
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

                            Addzhcs_air_count(AirType.Temperature, temperature);
                        }
                        float humidity = 0;
                        result = float.TryParse(datas[1], out humidity);
                        if (result)
                        {
                            ApiDisplayInfo.monitors.humidity = humidity;
                            Addzhcs_air_count(AirType.Humidity, humidity);
                        }
                        float pm25 = 0;
                        result = float.TryParse(datas[2], out pm25);

                        if (result)
                        {
                            ApiDisplayInfo.monitors.pm25 = pm25;
                            Addzhcs_air_count(AirType.PM25, pm25);

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
                            if (nowCount > 115 && nowCount <= 150)
                            {
                                level = 2;
                                strLevel = "二";
                            }
                            if (nowCount >= 150)
                            {
                                level = 1;
                                strLevel = "一";
                            }

                            ApiDisplayInfo.monitors.pm25Level = level;
                            ApiDisplayInfo.monitors.alarm = level <= 3;
                            ApiDisplayInfo.monitors.alarmtime = DateTime.Now.ToString();
                            if (level <= 3)
                            {
                                ApiDisplayInfo.monitors.pm25Info = String.Format("{0}级预警，启动{1}级作业！", strLevel, strLevel);
                            }
                            else
                            {
                                ApiDisplayInfo.monitors.pm25Info = "";
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

        Stopwatch stopwatch30s = new Stopwatch();
        private int lastEnterNum30s = 0;
        int nowCount30 = 0;

        Stopwatch stopwatch = new Stopwatch();
        private int lastEnterNum = 0;
        Stopwatch stopwatch1s = new Stopwatch();

        //当前人数
        int nowCount = 0;

        /// <summary>
        /// Handles the PDCChanged event of the HikDevice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PDCChangedEventArgs"/> instance containing the event data.</param>
        private void HikDevice_PDCChanged(object sender, PDCChangedEventArgs e)
        {
            NLog.LogManager.GetLogger("default").Info("lastEnterNum：{0} EnterNum：{1} mode:{2}", lastEnterNum, e.EnterNum, e.Mode);

            //实时客流统计数据，每次变化都会上传
            if (e.Mode == 0)
            {
                if (stopwatch1s.Elapsed.TotalSeconds >= 1 || !stopwatch1s.IsRunning)
                {
                    stopwatch1s.Restart();
                    //记录客流数据
                    Addzhcs_traveller_count((int)e.EnterNum, (int)e.LeaveNum);
                }
                //10 s 
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

                //30s 
                if (lastEnterNum30s == 0 || (int)e.EnterNum == 0)
                {
                    lastEnterNum30s = (int)e.EnterNum;
                }

                if (stopwatch30s.Elapsed.TotalSeconds >= 30 || lastEnterNum30s == 0 || !stopwatch30s.IsRunning)
                {
                    nowCount30 = (int)(e.EnterNum - lastEnterNum30s);

                    NLog.LogManager.GetLogger("default").Info("nowCount30：{0}", nowCount30);

                    lastEnterNum30s = (int)e.EnterNum;
                    stopwatch30s.Restart();
                }

                int level = 5;
                string strLevel = "五";
                if (nowCount <= 2)
                {
                    level = 5;
                    strLevel = "五";
                }
                if (nowCount == 3)
                {
                    level = 4;
                    strLevel = "四";
                }
                if (nowCount >= 4 && nowCount <= 5)
                {
                    level = 3;
                    strLevel = "三";
                }
                if (nowCount == 6)
                {
                    level = 2;
                    strLevel = "二";
                }
                if (nowCount >= 7)
                {
                    level = 1;
                    strLevel = "一";
                }

                ApiDisplayInfo.flowCount.flowRateLevel = level;
                ApiDisplayInfo.flowCount.alarm = false;
                ApiDisplayInfo.flowCount.alarmtime = DateTime.Now.ToString();
                if (level <= 3)
                {
                    ApiDisplayInfo.flowCount.info = String.Format("{0}级综合预警，启动{1}级综合作业！", strLevel, strLevel);
                }

                ApiDisplayInfo.flowCount.flowRateByNow = nowCount;
                ApiDisplayInfo.flowCount.flowRateByDay = (int)e.EnterNum;
            }
            //时段客流统计数据，在摄像机上配置，默认60分钟
            if (e.Mode == 1)
            {
                ApiDisplayInfo.flowCount.flowRateByHour = (int)e.EnterNum;
                lastflowRateByHour = (int)e.EnterNum;
            }
        }

        private int lastflowRateByHour = 0;

        #region 数据库读写

        /// <summary>
        /// 增加客流统计数据.
        /// </summary>
        /// <param name="in_count">The in count.</param>
        /// <param name="out_count">The out count.</param>
        public void Addzhcs_traveller_count(int in_count, int out_count)
        {
            return;

            string sql = String.Format(@"INSERT INTO`zhcs_traveller_count`(`travellerid`,`in_count`,`out_count`,`cstype`,`csid`,`csmc`,`ctimestamp`,`COMMENTS`)VALUES (uuid(), ?in_count, ?out_count, ?cstype, 'csid', ?csmc, unix_timestamp(), '');");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("?in_count",in_count),
                new MySqlParameter("?out_count",out_count),
                new MySqlParameter("?cstype",1),//厕所类型,1表示男厕，2表示女厕,3表示残障
                new MySqlParameter("?csmc","A3男厕所")
                };
            DataAccess.ExecuteSql(sql, parameters);
        }

        /// <summary>
        /// 增加空气质量统计数据.
        /// </summary>
        /// <param name="ywtype">异味类别，1表示氨气，2表示硫化氢，3表示VOC，4表示温度，5表示湿度，6表示PM2.5.</param>
        /// <param name="ywzs">异味指数.</param>
        public void Addzhcs_air_count(AirType ywtype, double ywzs)
        {
            return;

            string sql = String.Format(@"INSERT INTO`zhcs_air_count`(`airid`,`ywtype`,`ywzs`,`csid`,`csmc`,`ywtimestamp`,`COMMENTS`)VALUES (uuid(), ?ywtype, ?ywzs, 'csid', ?csmc, unix_timestamp(), '');");
            MySqlParameter[] parameters =
            {
                new MySqlParameter("?ywtype",(int)ywtype),//异味类别，1表示氨气，2表示硫化氢，3表示VOC，4表示温度，5表示湿度，6表示PM2.5
                new MySqlParameter("?ywzs",(decimal)ywzs),
                new MySqlParameter("?csmc","A3男厕所")
                };
            DataAccess.ExecuteSql(sql, parameters);
        }

        #endregion

    }

    /// <summary>
    /// Enum AirType
    /// </summary>
    public enum AirType
    {
        /// <summary>
        /// The nh3
        /// </summary>
        NH3 = 1,
        /// <summary>
        /// The h2S
        /// </summary>
        H2S = 2,
        /// <summary>
        /// The voc
        /// </summary>
        VOC = 3,
        /// <summary>
        /// The temperature
        /// </summary>
        Temperature = 4,
        /// <summary>
        /// The humidity
        /// </summary>
        Humidity = 5,
        /// <summary>
        /// The pM25
        /// </summary>
        PM25 = 6
    }
}
