using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Web
{
	/// <summary>
	/// ʵ�ù��ߡ�
	/// </summary>
	internal class Utilities
	{
		/// <summary>
		/// �Ƚ�����Byte�����ֵ�Ƿ���ȡ�
		/// </summary>
		/// <param name="array1">Ҫ�Ƚϵ�����1</param>
		/// <param name="array2">Ҫ�Ƚϵ�����1</param>
		/// <returns>���������е�ֵ��ȫ��ȷ���True�����򷵻�False</returns>
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
		/// ������ʽ��ȡ Socket �е�һ��Int32ֵ��
		/// </summary>
		/// <param name="socket">Ҫ��ȡ�� Socket ��</param>
		/// <param name="value">��ȡ����Int32ֵ��</param>
		/// <returns>��ȡ�ɹ�����True�����򷵻�Flase��</returns>
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
		/// ������ʽ��ȡ Socket �е����ݡ�
		/// </summary>
		/// <param name="socket">Ҫ��ȡ�� Socket ��</param>
		/// <param name="size">Ҫ��ȡ�����ݳ��ȡ�</param>
		/// <param name="buffer">��ȡ�����ֽ����顣</param>
		/// <returns>��ȡ�ɹ�����True�����򷵻�Flase��</returns>
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
		/// ������ʽ�� Socket �������ݡ�
		/// </summary>
		/// <param name="socket">���ڷ������ݵ� Socket ��</param>
		/// <param name="value">Ҫ���͵����ݡ�</param>
		/// <returns>���ͳɹ�����True�����򷵻�Flase��</returns>
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
		/// �жϸ�����һ���ַ����Ƿ�ΪIP v4��ַ��
		/// </summary>
		/// <param name="address">Ҫ�����ַ�����</param>
		/// <returns>�����IPV4��ַ��ΪTrue������ΪFalse��</returns>
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
