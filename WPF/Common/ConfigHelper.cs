using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web
{
    /// <summary>
    /// Class ConfigHelper.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Reads the setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key].ToString();
            return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading app settings"+ex.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Adds the update application settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing app settings" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the web port.
        /// </summary>
        /// <value>The web port.</value>
        public static int WebPort
        {
            get
            {
                string strValue = ReadSetting("WebPort");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }
        /// <summary>
        /// Gets the flow alarm count.
        /// </summary>
        /// <value>The flow alarm count.</value>
        public static int FlowAlarmCount
        {
            get
            {
                string strValue = ReadSetting("FlowAlarmCount");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }

        /// <summary>
        /// Gets the n h3 alarm count.
        /// </summary>
        /// <value>The n h3 alarm count.</value>
        public static int NH3AlarmCount
        {
            get
            {
                string strValue = ReadSetting("NH3AlarmCount");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }
        /// <summary>
        /// Gets the PXJ maximum count.
        /// </summary>
        /// <value>The PXJ maximum count.</value>
        public static int PXJMaxCount
        {
            get
            {
                string strValue = ReadSetting("PXJMaxCount");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }
        /// <summary>
        /// Gets the PXJ alarm remain count.
        /// </summary>
        /// <value>The PXJ alarm remain count.</value>
        public static int PXJAlarmRemainCount
        {
            get
            {
                string strValue = ReadSetting("PXJAlarmRemainCount");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }
        /// <summary>
        /// Gets the PXJ work interval second.
        /// </summary>
        /// <value>The PXJ work interval second.</value>
        public static int PXJWorkIntervalSecond
        {
            get
            {
                string strValue = ReadSetting("PXJWorkIntervalSecond");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }
        /// <summary>
        /// Gets the PXJ sub interval second.
        /// </summary>
        /// <value>The PXJ sub interval second.</value>
        public static int PXJSubIntervalSecond
        {
            get
            {
                string strValue = ReadSetting("PXJSubIntervalSecond");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }

        
        /// <summary>
        /// Gets the PXJ work count.
        /// </summary>
        /// <value>The PXJ work count.</value>
        public static int PXJWorkCount
        {
            get
            {
                string strValue = ReadSetting("PXJWorkCount");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }
        /// <summary>
        /// Gets the fan work Second.
        /// </summary>
        /// <value>The fan work minutes.</value>
        public static int FanWorkSecond
        {
            get
            {
                string strValue = ReadSetting("FanWorkSecond");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }
        /// <summary>
        /// Gets the fan work interval second.
        /// </summary>
        /// <value>The fan work interval second.</value>
        public static int FanWorkIntervalSecond
        {
            get
            {
                string strValue = ReadSetting("FanWorkIntervalSecond");
                int intValue = 5;
                int.TryParse(strValue, out intValue);
                return intValue;
            }
        }

        /// <summary>
        /// Gets the hik ip.
        /// </summary>
        /// <value>The hik ip.</value>
        public static string HikIP
        {
            get
            {
                string strValue = ReadSetting("HikIP");
                return strValue;
            }
        }
        /// <summary>
        /// Gets the name of the hik user.
        /// </summary>
        /// <value>The name of the hik user.</value>
        public static string HikUserName
        {
            get
            {
                string strValue = ReadSetting("HikUserName");
                return strValue;
            }
        }
        /// <summary>
        /// Gets the hik password.
        /// </summary>
        /// <value>The hik password.</value>
        public static string HikPwd
        {
            get
            {
                string strValue = ReadSetting("HikPwd");
                return strValue;
            }
        }
    }
}
