using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web
{
    /// <summary>
    /// NLog日志记录对象。
    /// </summary>
    public static class NLogManager
    {
        private static NLog.Logger logger = NLog.LogManager.GetLogger("default");

        /// <summary>
        /// 获取NLogger实例
        /// </summary>
        public static NLog.Logger Logger
        {
            get { return logger;  }
        }
    }

}
