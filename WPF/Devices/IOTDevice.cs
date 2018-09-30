using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Web
{

    /// <summary>
    /// Class Device.
    /// </summary>
    public class IOTDevice
    {
        /// <summary>
        /// Gets or sets the last message information.
        /// </summary>
        /// <value>The last message information.</value>
        public MessageInfo LastMessageInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOTDevice"/> class.
        /// </summary>
        /// <param name="so">The so.</param>
        /// <param name="messageInfo">The message information.</param>
        public IOTDevice()
        {
            //Socket = so;
            //ID = messageInfo.Id;
            //TypeID = CommonHelper.ToSubIds(ID)[1];
            //DeviceID = CommonHelper.ToSubIds(ID)[2];
            //LastMessageInfo = messageInfo;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the socket.
        /// </summary>
        /// <value>The socket.</value>
        public Socket Socket { get; set; }

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        /// <value>The type identifier.</value>
        public string TypeID { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is working.
        /// </summary>
        /// <value><c>true</c> if this instance is working; otherwise, <c>false</c>.</value>
        public bool IsWorking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is alarm.
        /// </summary>
        /// <value><c>true</c> if this instance is alarm; otherwise, <c>false</c>.</value>
        public bool IsAlarm { get; set; }

        /// <summary>
        /// Gets or sets 报警时间.
        /// </summary>
        /// <value>The alarm time.</value>
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// Gets or sets 已工作持续时间.
        /// </summary>
        /// <value>The worked time span.</value>
        public TimeSpan WorkedTimeSpan { get; set; }

        /// <summary>
        /// Gets or sets the work time watch.
        /// </summary>
        /// <value>The work time watch.</value>
        public Stopwatch WorkTimeWatch { get; set; }

        /// <summary>
        /// Sends the response.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="par">The par.</param>
        /// <returns>System.Int32.</returns>
        public bool SendResponse(Command command, MessageType messageType = MessageType.response, string par = "")
        {
            if (!IsWorking)
            {
                return false;
            }

            if (String.IsNullOrEmpty(par))
            {
                par = "1";
            }

            MessageInfo messageInfo = new MessageInfo();
            messageInfo.messagetype = messageType.ToString();
            messageInfo.command = command.ToString();
            messageInfo.Id = ID;
            messageInfo.Seq = (long.Parse(LastMessageInfo.Seq) + 1).ToString();

            LastMessageInfo.Seq = messageInfo.Seq;
            messageInfo.parameter = par;

            return SendMsg(messageInfo);
        }

        /// <summary>
        /// Sends the MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>System.Int32.</returns>
        public bool SendMsg(MessageInfo msg)
        {
            if (!IsWorking)
            {
                return false;
            }

            try
            {
                string json = JsonConvert.SerializeObject(msg);

                //NLog.LogManager.GetLogger("default").Info("发送数据:{0} {1}", Socket.RemoteEndPoint.ToString(), json);

                byte[] command = Encoding.GetEncoding("gb2312").GetBytes(json);
                return SendCommand(command) > 0;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error("发送消息发生错误，ID:{0}.参考信息:{1}。", ID, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 发送控制命令
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private int SendCommand(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return 0;
            }

            int sendCount = 0;

            try
            {
                if (Socket.Connected)
                {
                    SocketError socketError = SocketError.IsConnected;
                    IAsyncResult asyncResult = Socket.BeginSend(data, 0, data.Length, SocketFlags.None, out socketError, null, null);
                }
                else
                {
                    NLog.LogManager.GetLogger("default").Warn("发送控制命令时TCP已断开，ID:{0}。", ID);
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Error("发送控制命令发生错误，ID:{0}.参考信息:{1}。", ID, ex.Message);
            }
            return sendCount;
        }

        #region Socket

        /// <summary>
        /// 连接成功。
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// 与设备断开连接后发生。
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        /// <summary>
        /// 开始接收数据
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ReceiveAsync(Socket socket)
        {
            //在连接过程中执行断开命令，socket 可能为空。
            if (socket == null || !socket.Connected)
            {
                NLog.LogManager.GetLogger("default").Info("连接断开:{0}", socket?.RemoteEndPoint?.ToString());
                return false;
            }

            EndPoint iep;

            try
            {
                iep = socket.RemoteEndPoint;

                //获取异步套接字操作对象
                socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(SocketEventArg_Completed);
                socketEventArg.RemoteEndPoint = iep;
                socketEventArg.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                socketEventArg.UserToken = socket;
                Socket = socket;

                if (socketEventArg != null)
                {
                    if (!socket.ReceiveAsync(socketEventArg))
                    {
                        ProcessReceive(socketEventArg);
                    }
                }
                //NLog.LogManager.GetLogger("default").Info("成功处理设备上线:{0}", socket?.RemoteEndPoint?.ToString());

                return socket.Connected;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Info("ReceiveAsync异常:{0}", socket?.RemoteEndPoint?.ToString());
                Disconnect(socket);
            }
            return false;
        }

        /// <summary>
        /// 断开连接。
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="reason">The reason.</param>
        private void Disconnect(Socket socket, string reason = "")
        {
            if (socket != null)
            {
                try
                {
                    if (socketEventArg != null)
                    {
                        socketEventArg.Completed -= new EventHandler<SocketAsyncEventArgs>(SocketEventArg_Completed);
                        socketEventArg.Dispose();
                        socketEventArg = null;
                    }

                    socket.Shutdown(SocketShutdown.Receive);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    NLog.LogManager.GetLogger("default").Info("Disconnect 异常：{0}", ex.Message);
                }
                socket = null;
            }
            if (Disconnected != null)
            {
                Disconnected(this, new DisconnectedEventArgs(reason));
            }
        }

        /// <summary>
        /// A single callback is used for all socket operations. This method forwards execution on to the correct handler
        /// based on the type of completed operation
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception"></exception>
        private void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;

                //case SocketAsyncOperation.Connect:
                //    break;

                //case SocketAsyncOperation.Send:
                //    ProcessSend(e);
                //    break;
                default:
                    throw new Exception(String.Format("收到非法套接字操作类型:{0},错误码:{1}", e.LastOperation, e.SocketError));
            }
        }

        /// <summary>
        /// Processes the receive.
        /// </summary>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            Socket socket = (Socket)e.UserToken;

            try
            {
                if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
                {
                    Disconnect(socket);
                    return;
                }

                ProcessData(socket, e.BytesTransferred);
                if (socket != null)
                {
                    if (!socket.ReceiveAsync(e))
                    {
                        ProcessReceive(e);
                    }
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Info("接收出错:{0} {1}", socket?.RemoteEndPoint.ToString(), ex.Message);
                Disconnect(socket);
            }
        }

        /// <summary>
        /// The receive buffer
        /// </summary>
        private byte[] receiveBuffer = new byte[4096];
        /// <summary>
        /// The socket event argument
        /// </summary>
        private SocketAsyncEventArgs socketEventArg;

        /// <summary>
        /// Processes the data.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="size">The size.</param>
        private void ProcessData(Socket socket, int size)
        {
            if (receiveBuffer != null && size <= receiveBuffer.Length)
            {
                string allText = String.Empty;
                try
                {
                    allText = Encoding.GetEncoding("gb2312").GetString(receiveBuffer, 0, size).Trim();

                    var msgs = allText.Split(new string[] { "}" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (var item in msgs)
                    {
                        var oneMsg = item + "}";
                        try
                        {
                            //NLog.LogManager.GetLogger("default").Info("收到设备数据包:{0} {1}", socket?.RemoteEndPoint?.ToString(), msg);
                            //反序列化json
                            MessageInfo msgInfo = JsonConvert.DeserializeObject<MessageInfo>(oneMsg);

                            if (msgInfo != null)
                            {
                                if (msgInfo.Id.Length != 12)
                                {
                                    //无效Id 丢弃该包
                                    NLog.LogManager.GetLogger("default").Info("收到设备无效Id数据包:{0} {1}", socket?.RemoteEndPoint?.ToString(), allText);
                                    return;
                                }
                                //ping 包创建设备 暂时只处理ping包
                                if (msgInfo.command == Command.ping.ToString())
                                {
                                    if (LastMessageInfo == null)
                                    {
                                        NLog.LogManager.GetLogger("default").Info("收到设备首次上线数据包:{0} {1}", socket?.RemoteEndPoint?.ToString(), allText);

                                        LastMessageInfo = msgInfo;
                                        ID = LastMessageInfo.Id;
                                        TypeID = CommonHelper.ToSubIds(ID)[1];
                                        DeviceID = CommonHelper.ToSubIds(ID)[2];
                                        DeviceStateChanged?.Invoke(this, new DeviceStateChangedEventArgs(msgInfo, this));
                                    }
                                    //else
                                    //    NLog.LogManager.GetLogger("default").Info("收到设备数据包:{0} {1}", socket?.RemoteEndPoint?.ToString(), msg);

                                    SendResponse(Command.ping);
                                    IsWorking = true;

                                    if (LastMessageInfo.parameter != msgInfo.parameter)
                                    {
                                        LastMessageInfo = msgInfo;
                                        DeviceStateChanged?.Invoke(this, new DeviceStateChangedEventArgs(msgInfo, this));
                                    }
                                }
                                if (msgInfo.command == Command.control.ToString())
                                {
                                    NLog.LogManager.GetLogger("default").Info("收到设备回应数据包:{0} {1}", socket?.RemoteEndPoint?.ToString(), allText);
                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            NLog.LogManager.GetLogger("default").Error("解析单条json发生错误:{0} {1}", socket?.RemoteEndPoint?.ToString(), oneMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    NLog.LogManager.GetLogger("default").Info("解析json数据发生错误，设备:{0} {1} \r\n 错误:{2}", socket?.RemoteEndPoint?.ToString(), allText, ex.Message);
                }
            }
        }

        /// <summary>
        /// 当设备状态信息改变时发生。
        /// </summary>
        public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;

        #endregion

    }

}
