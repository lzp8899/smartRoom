using System;

namespace Web
{

    /// <summary>
    /// ��ʾ�����������ӶϿ��¼����д���ķ���
    /// </summary>
    public delegate void DisconnectedEventHandler(object sender, DisconnectedEventArgs e);

    /// <summary>
    /// Ϊ�������ӶϿ��¼�������
    /// </summary>
    [Serializable]
    public class DisconnectedEventArgs : EventArgs
    {
        private readonly string reason;

        /// <summary>
        /// �����¼�������ʵ����
        /// </summary>
        /// <param name="reason">�Ͽ�ԭ��</param>
        public DisconnectedEventArgs(string reason)
        {
            this.reason = reason;
        }

        /// <summary>
        /// ��ȡ�Ͽ����ӵ�ԭ��
        /// </summary>
        public string Reason
        {
            get { return reason; }
        }
    }

}
