using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Web
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public static class CommonHelper
    {
        public static string[] ToSubIds(string id)
        {
            List<string> subIds = new List<string>();
            try
            {
                subIds.Add(id.Substring(0, 6));
                subIds.Add(id.Substring(6, 3));
                subIds.Add(id.Substring(9, 3));

                return subIds.ToArray();
            }
            catch
            {
                return new string[0];
            }
        }
       
        /// <summary>
        /// 把字符串表示的IP转换为UInt表示的IP(高-低)
        /// </summary>
        /// <param name="stringIp">要转换的Ip</param>
        /// <returns></returns>
        public static uint IPToUInt(string stringIp)
        {
            try
            {
                stringIp = stringIp.Trim();
                IPAddress ipAddress = IPAddress.Parse(stringIp);
                return ((uint)ipAddress.GetAddressBytes()[0] << 24) |
                       ((uint)ipAddress.GetAddressBytes()[1] << 16) |
                       ((uint)ipAddress.GetAddressBytes()[2] << 8) |
                       ((uint)ipAddress.GetAddressBytes()[3]);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 把字符串表示的IP转换为UInt表示的IP(低－高)
        /// </summary>
        public static uint IPToUIntL(string stringIp)
        {
            stringIp = stringIp.Trim();
            IPAddress ipAddress = IPAddress.Parse(stringIp);
            return ((uint)ipAddress.GetAddressBytes()[3] << 24) |
                   ((uint)ipAddress.GetAddressBytes()[2] << 16) |
                   ((uint)ipAddress.GetAddressBytes()[1] << 8) |
                   ((uint)ipAddress.GetAddressBytes()[0]);
        }


        /// <summary>
        /// UInt表示的IP转换为字符串表示的IP
        /// </summary>
        /// <param name="uintIp">The uint ip.</param>
        /// <returns></returns>
        public static string UIntToIP(uint uintIp)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(uintIp.ToString());
                return ipAddress.ToString();
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// UInt表示的IP转换为字符串表示的IP
        /// </summary>
        public static string ToIp(this uint uintIp)
        {
            return UIntToIP(uintIp);
        }

        /// <summary>
        /// 计算两个IP地址间的有效应IP，包括开始和结束IP本身
        /// </summary>
        /// <param name="beginIp">开始IP地址</param>
        /// <param name="endIp">结束IP</param>
        /// <returns>有效IP地址集合</returns>
        public static string[] Subtract(string beginIp, string endIp)
        {
            beginIp = beginIp.Trim();
            endIp = endIp.Trim();

            uint uintBeginIp = IPToUInt(beginIp);
            uint uintEndIp = IPToUInt(endIp);

            uint ipCount = uintEndIp - uintBeginIp + 1;
            string[] ips = new string[ipCount];

            for (uint i = 0; i < ipCount; i++)
            {
                ips[i] = UIntToIP(uintBeginIp + i);
            }

            return ips;
        }

        /// <summary>
        /// 计算两个IP地址间的有效应IP，包括开始和结束IP本身
        /// </summary>
        /// <param name="beginIp">开始IP地址</param>
        /// <param name="endIp">结束IP</param>
        /// <returns>有效IP地址集合</returns>
        public static uint[] Subtract(uint beginIp, uint endIp)
        {
            if (beginIp > endIp)
            {
                return new uint[0];
            }

            uint ipCount = endIp - beginIp + 1;
            uint[] ips = new uint[ipCount];

            for (uint i = 0; i < ipCount; i++)
            {
                ips[i] = beginIp + i;
            }

            return ips;
        }

        /// <summary>
        /// IPs to bytes according to video file standard.
        /// </summary>
        /// <param name="ipv4">The ipv4.</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] IPToBytesAccordingToVideoFileStandard(uint ipv4)
        {
            byte[] ipBytes = new byte[16];
            ipBytes[10] = 255;
            ipBytes[11] = 255;

            byte[] ipv4Bytes = BitConverter.GetBytes(ipv4);

            Array.Copy(ipv4Bytes, 0, ipBytes, 12, ipv4Bytes.Length);
            return ipBytes;
        }

        /// <summary>
        /// Byteses to IP according to video file standard.
        /// </summary>
        /// <param name="ipBytes">The ip bytes.</param>
        /// <returns>System.UInt32.</returns>
        public static uint BytesToIPAccordingToVideoFileStandard(byte[] ipBytes)
        {
            if (ipBytes.Length != 16)
            {
                return 0;
            }

            byte[] ipBytes2 = new byte[4];

            Array.Copy(ipBytes, 12, ipBytes2, 0, ipBytes2.Length);
            return BitConverter.ToUInt32(ipBytes2, 0);
        }

    }
}
