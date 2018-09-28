// ***********************************************************************
// Assembly         : WinHost
// Author           : lzp
// Created          : 09-16-2018
//
// Last Modified By : lzp
// Last Modified On : 09-22-2018
// ***********************************************************************
// <copyright file="DeviceServer.cs" company="Microsoft">
//     Copyright © Microsoft 2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web
{
    /// <summary>
    /// Class DeviceServer.
    /// </summary>
    public class IOTDeviceManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IOTDeviceManager"/> class.
        /// </summary>
        /// <param name="tcpListener">The TCP listener.</param>
        public IOTDeviceManager(AsyncTcpListener tcpListener)
        {
            tcpListener.Connected += TcpListener_Connected;
        }

        /// <summary>
        /// Handles the Connected event of the TcpListener control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ConnectedEventArgs"/> instance containing the event data.</param>
        private void TcpListener_Connected(object sender, ConnectedEventArgs e)
        {
            e.Used = ReceiveAsync(e.Socket);
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
        /// The check connect thread
        /// </summary>
        private Thread checkConnectThread;

        /// <summary>
        /// The wait login
        /// </summary>
        private AutoResetEvent waitLogin = new AutoResetEvent(false);
        /// <summary>
        /// The is thread aliving
        /// </summary>
        private bool isThreadAliving = false;

        /// <summary>
        /// The socket asynchronous events
        /// </summary>
        private List<SocketAsyncEventArgs> socketAsyncEvents = new List<SocketAsyncEventArgs>();
        /// <summary>
        /// The devices
        /// </summary>
        private List<IOTDevice> devices = new List<IOTDevice>();

        /// <summary>
        /// 当设备状态信息改变时发生。
        /// </summary>
        public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Start()
        {
            isThreadAliving = true;
            checkConnectThread = new Thread(new ThreadStart(Working));
            checkConnectThread.IsBackground = true;
            checkConnectThread.Start();

            return true;
        }

        /// <summary>
        /// Stops the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Stop()
        {
            isThreadAliving = false;
            waitLogin.Set();
            return true;
        }


        /// <summary>
        /// Opens the ZNKG.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool OpenZNKG(bool open = true)
        {
            if (!canStartFan && open)
            {
                return true;
            }

            if (open)
            {
                canStartFan = false;

                MainApp.Instance.ApiDisplayInfo.fans.ToList().ForEach(item =>
                {
                    item.working = open;
                    item.workMinutes = ConfigHelper.FanWorkSecond / 60;
                    item.workMinutesCount = ConfigHelper.FanWorkSecond / 60;
                    item.workHourCount = item.workMinutesCount / 60;
                });
            }
            znkgWorkWatch.Restart();

            ForceOpenZNKG(open);

            return true;
        }

        /// <summary>
        /// Forces the open ZNKG.
        /// </summary>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool ForceOpenZNKG(bool open = true)
        {
            lock (devices)
            {
                //TypeId 104
                var tempDevices = devices.FindAll(item => item.TypeID == "104");
                if (tempDevices != null)
                {
                    MessageInfo messageInfo = new MessageInfo();
                    messageInfo.messagetype = MessageType.request.ToString();
                    messageInfo.command = Command.control.ToString();
                    if (!open)
                    {
                        messageInfo.parameter = "0";
                    }

                    for (int i = 0; i < tempDevices.Count; i++)
                    {

                        messageInfo.Id = tempDevices[i].ID;
                        tempDevices[i].SendMsg(messageInfo);
                        tempDevices[i].IsWorking = open;
                    }
                }
            }
            return true;
        }

        Stopwatch znkgWorkWatch = new Stopwatch();
        Stopwatch pxjWorkWatch = new Stopwatch();

        private volatile bool canOpenPXJ = true;

        /// <summary>
        /// Opens the ZNKG.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool OpenPXJ()
        {
            if (!canOpenPXJ)
            {
                return false;
            }
            canOpenPXJ = false;

            MainApp.Instance.ApiDisplayInfo.pxj.ToList().ForEach(item =>
            {
                item.working = true;
                item.ramainder -= ConfigHelper.PXJWorkCount;
                item.alarm = false;
                if (ConfigHelper.PXJMaxCount - item.ramainder <= ConfigHelper.PXJAlarmRemainCount)
                {
                    item.alarm = true;
                }
                item.alarmtime = DateTime.Now.ToString();
                //item.info = "1号喷香机余量不足，请及时更换！";//默认报警提示

            });

            ForceOpenPXJ(false);
            return true;
        }

        /// <summary>
        /// Forces the open PXJ.
        /// </summary>
        /// <param name="manual">if set to <c>true</c> [manual].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool ForceOpenPXJ(bool manual = true)
        {
            Task.Run(() =>
            {
                List<IOTDevice> temps = new List<IOTDevice>();

                lock (devices)
                {
                    temps = devices.FindAll(item => item.TypeID == "100");
                }

                if (temps != null)
                {
                    MessageInfo messageInfo = new MessageInfo();
                    messageInfo.messagetype = MessageType.request.ToString();
                    messageInfo.command = Command.control.ToString();

                    for (int c = 0; c < ConfigHelper.PXJWorkCount; c++)
                    {
                        for (int i = 0; i < temps.Count; i++)
                        {
                            messageInfo.Id = temps[i].ID;
                            temps[i].SendMsg(messageInfo);
                        }
                        Thread.Sleep(ConfigHelper.PXJSubIntervalSecond * 1000);
                    }
                    if (!manual)
                    {
                        MainApp.Instance.ApiDisplayInfo.pxj.ToList().ForEach(item =>
                        {
                            item.working = false;
                        });
                        pxjWorkWatch.Restart();
                    }
                }
            });
            return true;
        }


        /// <summary>
        /// Workings this instance.
        /// </summary>
        private void Working()
        {
            waitLogin.Set();
            SocketAsyncEventArgs[] eventArgs = null;

            while (isThreadAliving)
            {
                //lock (socketAsyncEvents)
                //{
                //    eventArgs = socketAsyncEvents.ToArray();
                //}

                //foreach (var device in eventArgs)
                //{
                //    //todo 判断心跳
                //}

                //工作时间到达
                //if (znkgWorkWatch.Elapsed.TotalSeconds >= ConfigHelper.FanWorkSecond)
                //{
                //    OpenZNKG(false);
                //}
                //if (znkgWorkWatch.Elapsed.TotalSeconds >= ConfigHelper.FanWorkIntervalSecond)
                //{
                //    canStartFan = true;
                //}
                //if (pxjWorkWatch.Elapsed.TotalSeconds >= ConfigHelper.PXJWorkIntervalSecond)
                //{
                //    pxjWorkWatch.Reset();
                //    canOpenPXJ = true;
                //}
                waitLogin.WaitOne(1000);
            }
        }

        private bool canStopFan = false;
        private bool canStartFan = true;


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
        private bool ReceiveAsync(Socket socket)
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
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(SocketEventArg_Completed);
                socketEventArg.RemoteEndPoint = iep;
                socketEventArg.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                socketEventArg.UserToken = socket;

                if (socketEventArg != null)
                {
                    lock (socketAsyncEvents)
                    {
                        socketAsyncEvents.Add(socketEventArg);
                    }

                    if (!socket.ReceiveAsync(socketEventArg))
                    {
                        ProcessReceive(socketEventArg);
                    }
                }
                NLog.LogManager.GetLogger("default").Info("设备上线:{0}", socket.RemoteEndPoint.ToString());

                return socket.Connected;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetLogger("default").Info("ReceiveAsync异常:{0}", socket.RemoteEndPoint.ToString());
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
                    lock (socketAsyncEvents)
                    {
                        var socketEventArg = socketAsyncEvents.FirstOrDefault(item => item.UserToken == socket);
                        socketAsyncEvents.Remove(socketEventArg);
                    }

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
                    NLog.LogManager.GetLogger("default").Info("Disconnect 异常:{0}", socket.RemoteEndPoint.ToString());
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
                NLog.LogManager.GetLogger("default").Info("接收出错:{0} {1}", socket.RemoteEndPoint.ToString(), ex.Message);
                Disconnect(socket);
            }
        }

        /// <summary>
        /// Processes the data.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="size">The size.</param>
        private void ProcessData(Socket socket, int size)
        {
            if (receiveBuffer != null && size <= receiveBuffer.Length)
            {
                string msg = String.Empty;
                try
                {
                    msg = Encoding.GetEncoding("gb2312").GetString(receiveBuffer, 0, size).Trim();
                    NLog.LogManager.GetLogger("default").Info("收到设备数据包:{0} {1}", socket.RemoteEndPoint.ToString(), msg);
                    //反序列化json
                    MessageInfo msgInfo = JsonConvert.DeserializeObject<MessageInfo>(msg);

                    if (msgInfo != null)
                    {
                        IOTDevice device = null;

                        if (msgInfo.Id.Length != 12)
                        {
                            //无效Id 丢弃该包
                            NLog.LogManager.GetLogger("default").Info("收到设备无效Id数据包:{0} {1}", socket.RemoteEndPoint.ToString(), msg);
                            return;
                        }
                        //ping 包创建设备 暂时只处理ping包
                        if (msgInfo.command == Command.ping.ToString())
                        {
                            lock (devices)
                            {
                                device = devices.FirstOrDefault(item => item.ID == msgInfo.Id);
                            }

                            if (device == null)
                            {
                                device = new IOTDevice(socket, msgInfo);
                                NLog.LogManager.GetLogger("default").Info("设备首次上线:{0} Id:{1}", socket.RemoteEndPoint.ToString(), msgInfo.Id);
                                devices.Add(device);
                            }
                            else
                            {
                                if (device.Socket != socket)
                                {
                                    NLog.LogManager.GetLogger("default").Info("设备地址改变后上线:{0} Id:{1}", socket.RemoteEndPoint.ToString(), msgInfo.Id);
                                    Disconnect(device.Socket);
                                    device.Socket = socket;
                                }
                            }
                            if (device != null)
                            {
                                device.SendResponse(Command.ping);

                                if (device.LastMessageInfo.parameter != msgInfo.parameter)
                                {
                                    device.LastMessageInfo = msgInfo;
                                    if (DeviceStateChanged != null)
                                    {
                                        DeviceStateChanged(device, new DeviceStateChangedEventArgs(msgInfo, device));
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("解析json数据发生错误:" + ex.Message);
                }
            }
        }

    }

}
