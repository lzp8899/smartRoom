using System;

namespace Web
{

    /// <summary>
    /// 表示将对网络连接断开事件进行处理的方法
    /// </summary>
    public delegate void DisconnectedEventHandler(object sender, DisconnectedEventArgs e);

    /// <summary>
    /// 为网络连接断开事件供参数
    /// </summary>
    [Serializable]
    public class DisconnectedEventArgs : EventArgs
    {
        private readonly string reason;

        /// <summary>
        /// 构造事件供参数实例。
        /// </summary>
        /// <param name="reason">断开原因。</param>
        public DisconnectedEventArgs(string reason)
        {
            this.reason = reason;
        }

        /// <summary>
        /// 获取断开连接的原因。
        /// </summary>
        public string Reason
        {
            get { return reason; }
        }
    }

}
