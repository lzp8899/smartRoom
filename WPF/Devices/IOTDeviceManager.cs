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
            //e.Used = ReceiveAsync(e.Socket);

            IOTDevice device = new IOTDevice();
            bool result = device.ReceiveAsync(e.Socket);

            if (result)
            {
                lock (devices)
                {
                    devices.Add(device);
                }
            }
            e.Used = result;
            NLog.LogManager.GetLogger("default").Info("设备首次上线:{0} ", e.Socket.RemoteEndPoint.ToString());
        }

        private void CreateIOTDevice()
        {

        }

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

    }

}
