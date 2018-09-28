using System;
using System.Net.Sockets;

namespace Web
{
	/// <summary>
	/// ΪConnected��ص��¼�������
	/// </summary>
	[Serializable]
	public class ConnectedEventArgs : EventArgs
	{
		private readonly Socket socket;
		private bool used;

		/// <summary>
		/// �����¼�����ʵ����
		/// </summary>
		/// <param name="socket">����¼���ص�Connected��</param>
		public ConnectedEventArgs(Socket socket)
		{
			this.socket = socket;
			this.used = false;
		}
		
		/// <summary>
		/// ��ȡ����¼���ص�Connected��
		/// </summary>
		public Socket Socket
		{
			get{return socket;}
		}

		/// <summary>
		/// ���û��ȡһ��ֵ��True��ʾ��Socket��ʹ���ˣ�False��ʾδ��ʹ�á�
		/// δ��ʹ�õ�SocketӦ�������Ͽ���
		/// </summary>
		public bool Used
		{
			get { return used; }
			set { used = value; }
		}
	}
}
