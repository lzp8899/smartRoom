// ***********************************************************************
// Assembly         : WinHost
// Author           : lzp
// Created          : 09-22-2018
//
// Last Modified By : lzp
// Last Modified On : 09-22-2018
// ***********************************************************************
// <copyright file="ApiDisplayInfo.cs" company="Microsoft">
//     Copyright © Microsoft 2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web
{
    /// <summary>
    /// Class ApiDisplayInfo.
    /// </summary>
    public class ApiDisplayInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDisplayInfo"/> class.
        /// </summary>
        public ApiDisplayInfo()
        {
            flowCount = new FlowCount();
            monitors = new Monitors();
            fans = new Fan[0];
            pxj = new Pxj[0];
            smczj = new Smczj[0];
            xsysb = new Xsysb[0];
            cszsb = new Cszsb[0];
        }
        /// <summary>
        /// Gets or sets the flow count.
        /// </summary>
        /// <value>The flow count.</value>
        public FlowCount flowCount { get; set; }
        /// <summary>
        /// Gets or sets the monitors.
        /// </summary>
        /// <value>The monitor.</value>
        public Monitors monitors { get; set; }

        /// <summary>
        /// Gets or sets the fans.
        /// </summary>
        /// <value>The fans.</value>
        public IList<Fan> fans { get; set; }
        /// <summary>
        /// Gets or sets the PXJ.
        /// </summary>
        /// <value>The PXJ.</value>
        public IList<Pxj> pxj { get; set; }
        /// <summary>
        /// Gets or sets the SMCZJ.
        /// </summary>
        /// <value>The SMCZJ.</value>
        public IList<Smczj> smczj { get; set; }
        /// <summary>
        /// Gets or sets the xsysb.
        /// </summary>
        /// <value>The xsysb.</value>
        public IList<Xsysb> xsysb { get; set; }
        /// <summary>
        /// Gets or sets the CSZSB.
        /// </summary>
        /// <value>The CSZSB.</value>
        public IList<Cszsb> cszsb { get; set; }
    }

    /// <summary>
    /// Class FlowCount.
    /// </summary>
    public class FlowCount
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the flow rate by now.
        /// </summary>
        /// <value>The flow rate by now.</value>
        public int flowRateByNow { get; set; }
        /// <summary>
        /// Gets or sets the flow rate by hour.
        /// </summary>
        /// <value>The flow rate by hour.</value>
        public int flowRateByHour { get; set; }
        /// <summary>
        /// Gets or sets the flow rate by day.
        /// </summary>
        /// <value>The flow rate by day.</value>
        public int flowRateByDay { get; set; }

        /// <summary>
        /// Gets or sets the flow rate level.
        /// </summary>
        /// <value>The flow rate level.</value>
        public int flowRateLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FlowCount"/> is alarm.
        /// </summary>
        /// <value><c>true</c> if alarm; otherwise, <c>false</c>.</value>
        public bool alarm { get; set; }

        /// <summary>
        /// Gets or sets the alarmtime.
        /// </summary>
        /// <value>The alarmtime.</value>
        public string alarmtime { get; set; }
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string info { get; set; }
    }

    /// <summary>
    /// Class Monitors.
    /// </summary>
    public class Monitors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Monitors"/> class.
        /// </summary>
        public Monitors()
        {

        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// 氨气
        /// </summary>
        /// <value>The PPM n h3.</value>
        public float ppmNH3 { get; set; }

        /// <summary>
        /// 氨气 level.
        /// </summary>
        /// <value>The PPM n h3 level.</value>
        public int ppmNH3Level { get; set; }

        /// <summary>
        /// 硫化氢
        /// </summary>
        /// <value>The PPM h2 s.</value>
        public float ppmH2S { get; set; }

        /// <summary>
        /// 硫化氢 level.
        /// </summary>
        /// <value>The PPM h2 s level.</value>
        public int ppmH2SLevel { get; set; }

        /// <summary>
        /// Gets or sets the PPM voc.
        /// </summary>
        /// <value>The PPM voc.</value>
        public float ppmVOC { get; set; }
        /// <summary>
        /// Gets or sets the PPM c8 h7 n.
        /// </summary>
        /// <value>The PPM c8 h7 n.</value>
        public float ppmC8H7N { get; set; }
        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        /// <value>The temperature.</value>
        public float temperature { get; set; }
        /// <summary>
        /// Gets or sets the humidity.
        /// </summary>
        /// <value>The humidity.</value>
        public float humidity { get; set; }
        /// <summary>
        /// Gets or sets the PM25.
        /// </summary>
        /// <value>The PM25.</value>
        public float pm25 { get; set; }

        /// <summary>
        /// Gets or sets the PM25 level.
        /// </summary>
        /// <value>The PM25 level.</value>
        public int pm25Level { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Monitors"/> is alarm.
        /// </summary>
        /// <value><c>true</c> if alarm; otherwise, <c>false</c>.</value>
        public bool alarm { get; set; }
        /// <summary>
        /// Gets or sets the alarmtime.
        /// </summary>
        /// <value>The alarmtime.</value>
        public string alarmtime { get; set; }
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string info { get; set; }
    }

    /// <summary>
    /// Class Fan.
    /// </summary>
    public class Fan
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Fan"/> is working.
        /// </summary>
        /// <value><c>true</c> if working; otherwise, <c>false</c>.</value>
        public bool working { get; set; }
        /// <summary>
        /// Gets or sets the work minutes.
        /// </summary>
        /// <value>The work minutes.</value>
        public int workMinutes { get; set; }
        /// <summary>
        /// Gets or sets the work hour count.
        /// </summary>
        /// <value>The work hour count.</value>
        public int workHourCount { get; set; }

        /// <summary>
        /// Gets or sets the work minutes count.
        /// </summary>
        /// <value>The work minutes count.</value>
        public int workMinutesCount { get; set; }
    }

    /// <summary>
    /// Class Pxj.
    /// </summary>
    public class Pxj
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Pxj"/> is working.
        /// </summary>
        /// <value><c>true</c> if working; otherwise, <c>false</c>.</value>
        public bool working { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int total { get; set; }
        /// <summary>
        /// Gets or sets the ramainder.
        /// </summary>
        /// <value>The ramainder.</value>
        public int ramainder { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Pxj"/> is alarm.
        /// </summary>
        /// <value><c>true</c> if alarm; otherwise, <c>false</c>.</value>
        public bool alarm { get; set; }
        /// <summary>
        /// Gets or sets the alarmtime.
        /// </summary>
        /// <value>The alarmtime.</value>
        public string alarmtime { get; set; }
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string info { get; set; }
    }

    /// <summary>
    /// Class Smczj.
    /// </summary>
    public class Smczj
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Smczj"/> is working.
        /// </summary>
        /// <value><c>true</c> if working; otherwise, <c>false</c>.</value>
        public bool working { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int total { get; set; }
        /// <summary>
        /// Gets or sets the ramainder.
        /// </summary>
        /// <value>The ramainder.</value>
        public int ramainder { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Smczj"/> is alarm.
        /// </summary>
        /// <value><c>true</c> if alarm; otherwise, <c>false</c>.</value>
        public bool alarm { get; set; }
        /// <summary>
        /// Gets or sets the alarmtime.
        /// </summary>
        /// <value>The alarmtime.</value>
        public string alarmtime { get; set; }
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string info { get; set; }
    }

    /// <summary>
    /// Class Xsysb.
    /// </summary>
    public class Xsysb
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Xsysb"/> is working.
        /// </summary>
        /// <value><c>true</c> if working; otherwise, <c>false</c>.</value>
        public bool working { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int total { get; set; }
        /// <summary>
        /// Gets or sets the ramainder.
        /// </summary>
        /// <value>The ramainder.</value>
        public int ramainder { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Xsysb"/> is alarm.
        /// </summary>
        /// <value><c>true</c> if alarm; otherwise, <c>false</c>.</value>
        public bool alarm { get; set; }
        /// <summary>
        /// Gets or sets the alarmtime.
        /// </summary>
        /// <value>The alarmtime.</value>
        public string alarmtime { get; set; }
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string info { get; set; }
    }

    /// <summary>
    /// Class Cszsb.
    /// </summary>
    public class Cszsb
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Cszsb"/> is working.
        /// </summary>
        /// <value><c>true</c> if working; otherwise, <c>false</c>.</value>
        public bool working { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int total { get; set; }
        /// <summary>
        /// Gets or sets the ramainder.
        /// </summary>
        /// <value>The ramainder.</value>
        public int ramainder { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Cszsb"/> is alarm.
        /// </summary>
        /// <value><c>true</c> if alarm; otherwise, <c>false</c>.</value>
        public bool alarm { get; set; }
        /// <summary>
        /// Gets or sets the alarmtime.
        /// </summary>
        /// <value>The alarmtime.</value>
        public string alarmtime { get; set; }
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string info { get; set; }
    }
}
