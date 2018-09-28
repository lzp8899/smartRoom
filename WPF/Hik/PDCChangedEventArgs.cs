using System;
using System.Net.Sockets;

namespace Web
{
    /// <summary>
    /// 客流信息改变相关的事件参数。
    /// </summary>
    public class PDCChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 构造事件参数实例。
        /// </summary>
        /// <param name="enterNumber">The enter number.</param>
        /// <param name="leaveNumber">The leave number.</param>
        public PDCChangedEventArgs(uint enterNumber, uint leaveNumber)
        {
            EnterNum = enterNumber;
            LeaveNum = leaveNumber;
        }
        /// <summary>
        /// Gets or sets the leave number.
        /// </summary>
        /// <value>The leave number.</value>
        public uint LeaveNum { get; set; }       // 离开人数

        /// <summary>
        /// Gets or sets the enter number.
        /// </summary>
        /// <value>The enter number.</value>
        public uint EnterNum { get; set; }        // 进入人数
	}
}
