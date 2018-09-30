using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Web.Controllers;

namespace Web
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            NLog.LogManager.GetLogger("default").Info("");

            Task.Run(() =>
            {
                NLog.LogManager.GetLogger("default").Info("开始启动网站，端口：{0}", ConfigHelper.WebPort);

                Web.Startup.StartWeb(ConfigHelper.WebPort, typeof(ServiceController).Assembly);
            });

            MainApp.Instance.Start();
            //string one = "{\"seq\":\"1000007418\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}";
            //string two = " {\"seq\":\"1000007418\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}{\"seq\":\"1000007419\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}{\"seq\":\"1000007420\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}{\"seq\":\"1000007421\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}";
            ////one = "100000741";
            //var bb = one.Split(new string[] { "}" }, StringSplitOptions.RemoveEmptyEntries); ;
            //var cc = bb[0] + "}";


            //Task.Run(() =>
            //{
            //    Random r2 = new Random((int)DateTime.Now.Ticks);
            //    Random r3 = new Random((int)DateTime.Now.Ticks);

            //    while (true)
            //    {
            //        //bool result = autoResetEvent.WaitOne(60 * 1000);
            //        //if (result)
            //        //{
            //        //    break;
            //        //}
            //        //SaveJson();


            //        Get5To10(r2, r3);
            //    }
            //});
        }

        /// <summary>
        /// Get5s the to10.
        /// </summary>
        /// <param name="r2">The r2.</param>
        /// <param name="r3">The r3.</param>
        public void Get5To10(Random r2, Random r3)
        {
            int count = r2.Next(280, 319);
            int span = r3.Next(1000, 9999);
            Thread.Sleep(span);
            int pepleCount = r3.Next(10, 100);

            float baseCount = (float)(count / 10.00f);
            float baseCount2 = (float)(span / 10000.0);
            float baseCount3 = (float)(pepleCount / 100.0);
            float baseCount4 = baseCount + baseCount2 + baseCount3;

            //float result = (float)(count / 10.00f + span / 1000.0 + pepleCount / 100.0);
            NLog.LogManager.GetLogger("default").Info($"结果:{baseCount4}  基数{count / 10.00f} 间隔时间:{span / 10000.0} 人数:{pepleCount / 100.0}");
            //NLog.LogManager.GetLogger("default").Info("{0}  {1}  {2}  {3}", baseCount, baseCount2, baseCount3, baseCount4);
        }


    }

}
