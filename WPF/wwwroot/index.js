var getjsonUrl = '/api/Service/allInfos'
//getjsonUrl = "./cesuo.json"

var _data = {}

var default_data = {"flowCount":{"flowRateByNow":50,"flowRateByHour":3000,"flowRateByDay":600,"alarm":false,"alarmtime":"2018-09-16 22:22:02","info":"人eqwrewrqr流数据超标，马上启动dfafsaf喷香作业和除异味作业！"},"monitors":{"ppmNH3":12,"ppmH2S":0,"ppmVOC":5,"ppmC8H7N":8,"temperature":130,"humidity":280,"pm25":225,"alarm":true,"alarmtime":"2018-09-16 20:23:03","info":"异味数据超标，马上启动喷香作业和dddd除异味作业！"},"fans":[{"name":"1号","working":false,"workMinutes":220,"workHourCount":5000},{"name":"2号","working":true,"workMinutes":220,"workHourCount":2400},{"name":"3号","working":true,"workMinutes":220,"workHourCount":1000}],"pxj":[{"name":"1号","working":true,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:24:02","info":"1号喷香机余量不足，555请及时更换！"},{"name":"2号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"2号喷香机余量不足，请及时更换！"},{"name":"3号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"3号喷香机余量不足，请及时更换！"},{"name":"4号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"4号喷香机余量不足，请及时更换！"},{"name":"5号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 22:53:02","info":"5号喷香机余量不足，请及时更换！"},{"name":"6号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:16:02","info":"6号喷香机余量不足，请及时更换！"},{"name":"7号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"7号喷香机余量不足，请及时更换！"},{"name":"8号","working":false,"total":3000,"ramainder":500,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"8号喷香机余量不足，请及时更换！"},{"name":"9号","working":true,"total":3000,"ramainder":500,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"9号喷香机余量不足，请及时更换！"},{"name":"10号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"10号喷香机余量不足，请及时更换！"},{"name":"11号","working":false,"total":3000,"ramainder":500,"alarm":false,"alarmtime":"2018-09-16 22:52:02","info":"11号喷香机余量ewqrW不足，请及时更换！"},{"name":"12号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 22:51:02","info":"12号喷香机余量不足，请及时更换！"},{"name":"13号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"13号喷香机余量不足，请及时更换！"},{"name":"14号","working":true,"total":3000,"ramainder":500,"alarm":false,"alarmtime":"2018-09-16 20:25:02","info":"14号喷香机余量不足，请及时更换！"},{"name":"15号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"15号喷香机余量不足，请及时更换！"},{"name":"16号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"16号喷香机余量不足，请及时更换！"},{"name":"17号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"17号喷香机余量不足，请及时更换！"},{"name":"18号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"18号喷香机余量不足，请及时更换！"},{"name":"19号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"19号喷香机余量不足，请及时更换！"},{"name":"20号","working":true,"total":3000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"20号喷香机余量不足，请及时更换！"}],"smczj":[{"name":"1号","working":true,"total":1000,"ramainder":1000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"1号物联网综合服务器卷纸剩余量不足，请及时更换！"},{"name":"2号","working":false,"total":1000,"ramainder":0,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"2号物联网综合服tret务器卷纸剩余量不足，请及时更换！"}],"xsysb":[{"name":"1号","working":true,"total":1000,"ramainder":100,"alarm":true,"alarmtime":"2018-09-16 20:26:02","info":"1号洗手液设ddddd备余量不足，请及时更换！"}],"cszsb":[{"name":"1号","working":false,"total":1000,"ramainder":500,"alarm":true,"alarmtime":"2018-09-16 20:15:02","info":"1号擦手纸设备余量不足，请及时更换!1号擦手纸设备余量不足，请及时更换!"}]}

sortOutDate(default_data)

var date = new Date()

var weekday = new Array(7)
weekday[0] = "星期天"
weekday[1] = "星期一"
weekday[2] = "星期二"
weekday[3] = "星期三"
weekday[4] = "星期四"
weekday[5] = "星期五"
weekday[6] = "星期六"

var dateTime = "";
dateTime += date.getFullYear() + "年"
dateTime += (date.getMonth() + 1) + "月"
dateTime += date.getDate() + "日"
dateTime += " " + weekday[date.getDay()]

function sortOutDate(dataJSON) {

    window._data['flowCount'] = dataJSON.flowCount
    window._data['monitors'] = dataJSON.monitors

    for (var i = 1; i <= 3; i++) {
        var p = 'fans' + i
        if (!window._data[p])
            window._data[p] = {}

        for (var key in dataJSON.fans[(i - 1)]) {
            window._data[p][key] = dataJSON.fans[(i - 1)][key];
        }
    }

    for (var i = 1; i <= 20; i++) {
        var p = 'pxj' + i
        if (!window._data[p])
            window._data[p] = {}

        for (var key in dataJSON.pxj[(i - 1)]) {
            window._data[p][key] = dataJSON.pxj[(i - 1)][key];
        }
    }

    for (var i = 1; i <= 1; i++) {
        var p = 'smczj' + i
        if (!window._data[p])
            window._data[p] = {}

        for (var key in dataJSON.smczj[(i - 1)]) {
            window._data[p][key] = dataJSON.smczj[(i - 1)][key];
        }
    }

    if (!window._data['xsysb1'])
        window._data['xsysb1'] = {}

    for (var key in dataJSON.xsysb[0]) {
        window._data['xsysb1'][key] = dataJSON.xsysb[0][key];
    }

    if (!window._data['cszsb1'])
        window._data['cszsb1'] = {}

    for (var key in dataJSON.cszsb[0]) {
        window._data['cszsb1'][key] = dataJSON.cszsb[0][key];
    }

    malfunctionMsg(dataJSON)
}

function compare(x, y) {
    x = x.dateTime
    y = y.dateTime
    if (x < y) {
        return 1;
    } else if (x > y) {
        return -1;
    } else {
        return 0;
    }
}

function getTime(parse) {
    var date = new Date(parse);
    return date.getHours() + " : " + date.getMinutes()
}

function malfunctionMsg(msg_data) {
    var malfunction_msg = new Array()
    if (msg_data.flowCount.alarm && msg_data.flowCount.info != "") {
        var a = {}
        a['info'] = msg_data.flowCount.info
        a['dateTime'] = Date.parse(new Date(msg_data.flowCount.alarmtime.replace(/-/g, "/")))
        a['time'] = getTime(a['dateTime'])
        malfunction_msg.push(a)
    }

    if (msg_data.flowCount.alarm && msg_data.monitors.info != "") {
        var a = {}
        a['info'] = msg_data.monitors.info
        a['dateTime'] = Date.parse(new Date(msg_data.monitors.alarmtime.replace(/-/g, "/")))
        a['time'] = getTime(a['dateTime'])
        malfunction_msg.push(a)
    }

    for (var i = 0, l = msg_data.pxj.length; i < l; i++) {
        if (msg_data.pxj[i].info != "") {
            var a = {}
            a['info'] = msg_data.pxj[i].info
            a['dateTime'] = Date.parse(new Date(msg_data.pxj[i].alarmtime.replace(/-/g, "/")))
            a['time'] = getTime(a['dateTime'])
            malfunction_msg.push(a)
        }
    }

    malfunction_msg = malfunction_msg.sort(compare)

    window._data['malfunctionmsg'] = malfunction_msg
}

function getJSON() {
    $.ajax({
        url: getjsonUrl,
        dataType: "json",
        cache: false,
        success: function (data) {
            sortOutDate(data)
            if ($('#wait-div').css('display') != 'none') {
                setTimeout(function () {
                    $("#wait-div").hide()
                }, 1000 * 1)
            }

            setTimeout(function () {
                getJSON()
            }, 1000 * 1);
        },
		error: function() {
			setTimeout(function () {
                getJSON()
            }, 1000 * 5);
		}
    });
}

new Vue({
    el: '#title',
    data: {
        dateTime: dateTime
    }
})

new Vue({
    el: '#wcInfo',
    data: {
        wcinfo: _data
    }
})

new Vue({
    el: '#pxj1',
    data: {
        pxjinfo: _data.pxj1
    }
})

new Vue({
    el: '#pxj2',
    data: {
        pxjinfo: _data.pxj2
    }
})

new Vue({
    el: '#pxj3',
    data: {
        pxjinfo: _data.pxj3
    }
})

new Vue({
    el: '#pxj4',
    data: {
        pxjinfo: _data.pxj4
    }
})

new Vue({
    el: '#pxj5',
    data: {
        pxjinfo: _data.pxj5
    }
})

new Vue({
    el: '#pxj6',
    data: {
        pxjinfo: _data.pxj6
    }
})

new Vue({
    el: '#pxj7',
    data: {
        pxjinfo: _data.pxj7
    }
})

new Vue({
    el: '#pxj8',
    data: {
        pxjinfo: _data.pxj8
    }
})

new Vue({
    el: '#pxj9',
    data: {
        pxjinfo: _data.pxj9
    }
})

new Vue({
    el: '#pxj10',
    data: {
        pxjinfo: _data.pxj10
    }
})

new Vue({
    el: '#pxj11',
    data: {
        pxjinfo: _data.pxj11
    }
})

new Vue({
    el: '#pxj12',
    data: {
        pxjinfo: _data.pxj12
    }
})

new Vue({
    el: '#pxj13',
    data: {
        pxjinfo: _data.pxj13
    }
})

new Vue({
    el: '#pxj14',
    data: {
        pxjinfo: _data.pxj14
    }
})

new Vue({
    el: '#pxj15',
    data: {
        pxjinfo: _data.pxj15
    }
})

new Vue({
    el: '#pxj16',
    data: {
        pxjinfo: _data.pxj16
    }
})

new Vue({
    el: '#pxj17',
    data: {
        pxjinfo: _data.pxj17
    }
})

new Vue({
    el: '#pxj18',
    data: {
        pxjinfo: _data.pxj18
    }
})

new Vue({
    el: '#pxj19',
    data: {
        pxjinfo: _data.pxj19
    }
})

new Vue({
    el: '#pxj20',
    data: {
        pxjinfo: _data.pxj20
    }
})

new Vue({
    el: '#smczj1',
    data: {
        smczjinfo: _data.smczj1
    }
})

/* new Vue({
    el: '#smczj2',
    data: {
        smczjinfo: _data.smczj2
    }
}) */

new Vue({
    el: '#xsysb1',
    data: {
        xsysbinfo: _data.xsysb1
    }
})

new Vue({
    el: '#cszsb1',
    data: {
        cszsbinfo: _data.cszsb1
    }
})

new Vue({
    el: '#fans1',
    data: {
        info: _data
    },
    methods: {
        doFans: function () {
            this.info.fans1.working = !this.info.fans1.working
        }
    }
})

new Vue({
    el: '#fans2',
    data: {
        info: _data
    },
    methods: {
        doFans: function () {
            this.info.fans2.working = !this.info.fans2.working
        }
    }
})

new Vue({
    el: '#fans3',
    data: {
        info: _data
    },
    methods: {
        doFans: function () {
            this.info.fans3.working = !this.info.fans3.working
        }
    }
})

getJSON()