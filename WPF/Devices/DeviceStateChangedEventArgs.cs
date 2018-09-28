using System;
using System.Net.Sockets;

namespace Web
{
    /// <summary>
    /// �豸״̬��Ϣ�ı���ص��¼�������
    /// </summary>
    [Serializable]
    public class DeviceStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// �����¼�����ʵ����
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
