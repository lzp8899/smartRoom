using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Web
{
    /// <summary>
    /// 表示TCP网络侦听者。
    /// 以异步方式实现的侦听。
    /// </summary>
    public class AsyncTcpListener
    {
        private int port;
        private bool isListening;
        private Socket listenSocket;

        /// <summary>
        /// 构造侦听端口实例。
        /// </summary>
        /// <param name="port">侦听端口号。</param>
        public AsyncTcpListener(int port)
        {
            this.port = port;

            isListening = false;
            listenSocket = null;
        }

        /// <summary>
        /// 当有客户端连接成功时发生。
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// 设置或获取服务器监听端口，正在侦听时修改端口要在下次Start()后才生效。
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// 获取一个值，表示侦听者是否正在侦听中。
        /// </summary>
        public bool IsListening
        {
            get { return isListening; }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <returns>成功返回True，否则返回False</returns>
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

                NLog.LogManager.GetLogger("default").Info("成功启动端口侦听:{0}", port);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                OnListenErrored("侦听端口失败，参考信息：" + ex.Message);
                return false;
            }

            isListening = true;

            return true;
        }

        /// <summary>
        /// 停止监听并关闭Socket。
        /// </summary>
        public void Stop()
        {
            NLog.LogManager.GetLogger("default").Info("开始停止监听");

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
                NLog.LogManager.GetLogger("default").Info("收到连接请求:{0}", socket.RemoteEndPoint.ToString());
            }
            catch (SocketException sex)
            {
                if (socket != null)
                {
                    socket.Close();
                }

                string message = String.Format("接受连接发生Socket错误.参考消息:{0}", sex.Message);
                OnListenErrored(message);
            }
            catch (Exception ex)
            {
                if (isListening)    //停止侦听时不引发异常事件。
                {
                    string message = String.Format("接受连接发生错误.参考消息:{0}", ex.Message);

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
                if (isListening)    //停止侦听时不再继续侦听。
                {
                    so.BeginAccept(new AsyncCallback(AcceptCallback), so);
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("开始侦听失败.参考消息:{0}", ex.Message);
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
