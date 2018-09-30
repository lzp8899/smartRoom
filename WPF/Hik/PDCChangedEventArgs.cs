using System;
using System.Net.Sockets;

namespace Web
{
    /// <summary>
    /// ������Ϣ�ı���ص��¼�������
    /// </summary>
    public class PDCChangedEventArgs : EventArgs
    {
        /// <summary>
        /// �����¼�����ʵ����
        /// </summary>
        /// <param name="enterNumber">The enter number.</param>
        /// <param name="leaveNumber">The leave number.</param>
        public PDCChangedEventArgs(uint enterNumber, uint leaveNumber, byte mode)
        {
            EnterNum = enterNumber;
            LeaveNum = leaveNumber;
            Mode = mode;
        }

        public byte Mode { get; set; }             // 0 ��֡ͳ�ƽ�� 1��Сʱ���ͳ�ƽ��  

        /// <summary>
        /// Gets or sets the leave number.
        /// </summary>
        /// <value>The leave number.</value>
        public uint LeaveNum { get; set; }       // �뿪����

        /// <summary>
        /// Gets or sets the enter number.
        /// </summary>
        /// <value>The enter number.</value>
        public uint EnterNum { get; set; }        // ��������
    }
}
