// ***********************************************************************
// Assembly         : WinHost
// Author           : lzp
// Created          : 09-16-2018
//
// Last Modified By : lzp
// Last Modified On : 09-22-2018
// ***********************************************************************
// <copyright file="MessageInfo.cs" company="Microsoft">
//     Copyright © Microsoft 2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web
{
    /// <summary>
    /// Enum MessageType
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The request
        /// </summary>
        request = 0,
        /// <summary>
        /// The response
        /// </summary>
        response = 1
    }

    /// <summary>
    /// Enum Command
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// The ping
        /// </summary>
        ping = 1,
        /// <summary>
        /// The control
        /// </summary>
        control = 2,
        /// <summary>
        /// The check
        /// </summary>
        check = 3
    }



    /// <summary>
    /// Class RequestInfo.
    /// </summary>
    [Serializable]
    public class MessageInfo
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInfo" /> class.
        /// </summary>
        public MessageInfo()
        {
            Id = "100000100100";
            city = "0028";
            command = "ping";
            parameter = "1";
            messagetype = "request";
        }

        /// <summary>
        /// Gets or sets the seq.
        /// </summary>
        /// <value>The seq.</value>
        public string Seq { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string city { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public string messagetype { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public string command { get; set; }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        public string parameter { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("Id:{0} command:{1} messageType：{2} parameter:{3}", Id, command, messagetype, parameter);
        }

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        /// <value>The type identifier.</value>
        [JsonIgnore]
        public string TypeID
        {
            get
            {
                return CommonHelper.ToSubIds(Id)[1];
            }
        }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonIgnore]
        public string DeviceID
        {
            get
            {
                return CommonHelper.ToSubIds(Id)[2];
            }
        }
    }
}
