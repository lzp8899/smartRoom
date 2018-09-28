using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public IOTDevice(Socket so, MessageInfo messageInfo)
        {
            Socket = so;
            ID = messageInfo.Id;
            TypeID = CommonHelper.ToSubIds(ID)[1];
            DeviceID = CommonHelper.ToSubIds(ID)[2];
            LastMessageInfo = messageInfo;
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
            try
            {
                string json = JsonConvert.SerializeObject(msg);

                NLog.LogManager.GetLogger("default").Info("发送数据:{0} {1}", Socket.RemoteEndPoint.ToString(), json);

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
                    sendCount = Socket.Send(data, SocketFlags.None);
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

    }

}
