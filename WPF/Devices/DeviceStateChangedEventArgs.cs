using System;
using System.Net.Sockets;

namespace Web
{
    /// <summary>
    /// 设备状态信息改变相关的事件参数。
    /// </summary>
    [Serializable]
    public class DeviceStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 构造事件参数实例。
        /// </summary>
        /// <param name="newMessageInfo">The new message information.</param>
        /// <param name="device">The device.</param>
        public DeviceStateChangedEventArgs(MessageInfo newMessageInfo,IOTDevice device)
        {
            Info = newMessageInfo;
            Device = device;
        }
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The leave number.</value>
        public MessageInfo Info { get; set; }

        public IOTDevice Device { get; set; }


    }
}
