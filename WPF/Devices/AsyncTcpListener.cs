using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Web
{
    /// <summary>
    /// ��ʾTCP���������ߡ�
    /// ���첽��ʽʵ�ֵ�������
    /// </summary>
    public class AsyncTcpListener
    {
        private int port;
        private bool isListening;
        private Socket listenSocket;

        /// <summary>
        /// ���������˿�ʵ����
        /// </summary>
        /// <param name="port">�����˿ںš�</param>
        public AsyncTcpListener(int port)
        {
            this.port = port;

            isListening = false;
            listenSocket = null;
        }

        /// <summary>
        /// ���пͻ������ӳɹ�ʱ������
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// ���û��ȡ�����������˿ڣ���������ʱ�޸Ķ˿�Ҫ���´�Start()�����Ч��
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// ��ȡһ��ֵ����ʾ�������Ƿ����������С�
        /// </summary>
        public bool IsListening
        {
            get { return isListening; }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns>�ɹ�����True�����򷵻�False</returns>
        public bool Start()
        {
            if (isListening)
            {
                Stop();
            }

            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);

            try
            {
                listenSocket.Bind(iep);
                listenSocket.Listen(500);
                listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), listenSocket);

                NLog.LogManager.GetLogger("default").Info("�ɹ������˿�����:{0}", port);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                OnListenErrored("�����˿�ʧ�ܣ��ο���Ϣ��" + ex.Message);
                return false;
            }

            isListening = true;

            return true;
        }

        /// <summary>
        /// ֹͣ�������ر�Socket��
        /// </summary>
        public void Stop()
        {
            NLog.LogManager.GetLogger("default").Info("��ʼֹͣ����");

            isListening = false;
            if (listenSocket != null)
            {
                listenSocket.Close();
                listenSocket = null;
            }
        }

        private void AcceptCallback(IAsyncResult iar)
        {
            Socket so = (Socket)iar.AsyncState;
            Socket socket = null;

            try
            {
                socket = so.EndAccept(iar);
                NLog.LogManager.GetLogger("default").Info("�յ���������:{0}", socket.RemoteEndPoint.ToString());
            }
            catch (SocketException sex)
            {
                if (socket != null)
                {
                    socket.Close();
                }

                string message = String.Format("�������ӷ���Socket����.�ο���Ϣ:{0}", sex.Message);
                OnListenErrored(message);
            }
            catch (Exception ex)
            {
                if (isListening)    //ֹͣ����ʱ�������쳣�¼���
                {
                    string message = String.Format("�������ӷ�������.�ο���Ϣ:{0}", ex.Message);

                    OnListenErrored(message);
                    Debug.WriteLine(ex.Message, "AcceptCallback");
                    if (socket != null)
                    {
                        socket.Close();
                    }
                }
            }

            try
            {
                if (isListening)    //ֹͣ����ʱ���ټ���������
                {
                    so.BeginAccept(new AsyncCallback(AcceptCallback), so);
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("��ʼ����ʧ��.�ο���Ϣ:{0}", ex.Message);
                OnListenErrored(message);
                Debug.WriteLine(ex.Message, "AcceptCallback.BeginAccept");
            }

            //if (readPurposeSuccess)
            {
                bool used = OnConnected(socket);
                if (!used && socket != null)
                {
                    NLog.LogManager.GetLogger("default").Info("socket is not use before close");
                    socket.Close();
                }
            }
        }

        private bool OnConnected(Socket socket)
        {
            ConnectedEventArgs args = new ConnectedEventArgs(socket);
            if (Connected != null)
            {
                Connected(this, args);
            }

            return args.Used;
        }

        private void OnListenErrored(string message)
        {
            NLog.LogManager.GetLogger("default").Error(message);
        }
    }
}
