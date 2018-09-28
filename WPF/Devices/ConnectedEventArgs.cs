using System;
using System.Net.Sockets;

namespace Web
{
	/// <summary>
	/// 为Connected相关的事件参数。
	/// </summary>
	[Serializable]
	public class ConnectedEventArgs : EventArgs
	{
		private readonly Socket socket;
		private bool used;

		/// <summary>
		/// 构造事件参数实例。
		/// </summary>
		/// <param name="socket">与该事件相关的Connected。</param>
		public ConnectedEventArgs(Socket socket)
		{
			this.socket = socket;
			this.used = false;
		}
		
		/// <summary>
		/// 获取与该事件相关的Connected。
		/// </summary>
		public Socket Socket
		{
			get{return socket;}
		}

		/// <summary>
		/// 设置或获取一个值，True表示该Socket被使用了，False表示未被使用。
		/// 未被使用的Socket应该立即断开。
		/// </summary>
		public bool Used
		{
			get { return used; }
			set { used = value; }
		}
	}
}
