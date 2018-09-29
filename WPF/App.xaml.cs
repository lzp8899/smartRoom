using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            string aa = " {\"seq\":\"1000007418\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}{\"seq\":\"1000007419\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}{\"seq\":\"1000007420\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}{\"seq\":\"1000007421\",\"id\":\"100000100111\",\"city\":\"0028\",\"messagetype\":\"request\",\"command\":\"ping\",\"parameter\":\"0\"}";

         var  bb=   aa.Split(new string[] { "}" },StringSplitOptions.RemoveEmptyEntries); ;
      var cc=      bb[0] + "}";
        }

    }
}
