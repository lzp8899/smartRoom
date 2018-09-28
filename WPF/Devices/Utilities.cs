using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Web
{
	/// <summary>
	/// 实用工具。
	/// </summary>
	internal class Utilities
	{
		/// <summary>
		/// 比较两个Byte数组的值是否相等。
		/// </summary>
		/// <param name="array1">要比较的数组1</param>
		/// <param name="array2">要比较的数组1</param>
		/// <returns>两个数组中的值完全相等返回True，否则返回False</returns>
		public static bool CompareByteArray(byte[] array1, byte[] array2)
		{
			if(array1.Length != array2.Length)
				return false;

			for(int i=0; i<array1.Length; i++)
				if(array1[i] != array2[i])
					return false;
			return true;
		}

		
		/// <summary>
		/// 阻塞方式读取 Socket 中的一个Int32值。
		/// </summary>
		/// <param name="socket">要读取的 Socket 。</param>
		/// <param name="value">读取到的Int32值。</param>
		/// <returns>读取成功返回True，否则返回Flase。</returns>
		public static bool ReadData(Socket socket, out int value)
		{
			value = 0;

			byte[] buffer;
			bool success = ReadData(socket, 4, out buffer);
			if (!success)
				return false;

			value = BitConverter.ToInt32(buffer, 0);
			return true;
		}

		/// <summary>
		/// 阻塞方式读取 Socket 中的数据。
		/// </summary>
		/// <param name="socket">要读取的 Socket 。</param>
		/// <param name="size">要读取的数据长度。</param>
		/// <param name="buffer">读取到的字节数组。</param>
		/// <returns>读取成功返回True，否则返回Flase。</returns>
		public static bool ReadData(Socket socket, int size, out byte[] buffer)
		{
			buffer = null;

			if(socket == null || !socket.Connected)
				return false;

			buffer = new byte[size];
			int offset, readSize;
			offset = 0;

			while(true)
			{
				try
				{
					readSize = socket.Receive(buffer, offset, size, SocketFlags.None);

					if(readSize == 0)
						return false;

					if(readSize < size)
					{
						offset += readSize;
						size -= readSize;
					}
					else
					{
						break;
					}
				}
				catch(Exception ex)
				{
					Debug.WriteLine(ex.Message, "Connect");
					return false;
				}
			}
		
			return true;
		}

		/// <summary>
		/// 阻塞方式用 Socket 发送数据。
		/// </summary>
		/// <param name="socket">用于发送数据的 Socket 。</param>
		/// <param name="value">要发送的数据。</param>
		/// <returns>发送成功返回True，否则返回Flase。</returns>
		public static bool SendData(Socket socket, int value)
		{
			if (socket == null || !socket.Connected)
				return false;

			byte[] buffer = null;
			buffer = BitConverter.GetBytes(value);

			int offset, sendSize, dataSize;
			offset = 0;
			dataSize = buffer.Length;

			while (true)
			{
				try
				{
					sendSize = socket.Send(buffer, offset, dataSize, SocketFlags.None);

					if (sendSize == 0)
						return false;

					if (sendSize < dataSize)
					{
						offset += sendSize;
						dataSize -= sendSize;
					}
					else
					{
						break;
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message, "Connect");
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 判断给定的一个字符串是否为IP v4地址。
		/// </summary>
		/// <param name="address">要检查的字符串。</param>
		/// <returns>如果是IPV4地址则为True，否则为False。</returns>
		public static bool IsIPv4Address(string address)
		{
			if(address == null)
				return false;

			string pattern = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
			Regex regex = new Regex(pattern);
			return regex.IsMatch(address);			
		}
	}
}
