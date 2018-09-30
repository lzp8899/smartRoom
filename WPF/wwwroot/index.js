var getjsonUrl = '/api/Service/allInfos'
//getjsonUrl = "./cesuo.json"

var _data = {}

var default_data = {"flowCount":{"id":"100000150100","flowRateByNow":0,"flowRateLevel":5,"flowRateByHour":0,"flowRateByDay":0,"alarm":false,"alarmtime":"2018-09-16 22:22:02","info":"一级综合预警，启动一级综合作业！"},"monitors":{"id":"100000105100","name":"气味探测器","ppmNH3":12.0,"ppmNH3Level":5,"ppmNH3Info":"一级预警，启动一级作业！","ppmH2S":21.0,"ppmH2SLevel":5,"ppmH2SInfo":"一级预警，启动一级作业！","ppmVOC":15.0,"ppmC8H7N":18.0,"temperature":28.0,"humidity":90.0,"pm25":40.0,"pm25Level":5,"pm25Info":"一级预警，启动一级作业！","alarm":false,"alarmtime":"2018-09-16 20:23:03","info":"人流数据超标，马上启动喷香作业和除异味作业！"},"fans":[{"id":"100000104100","name":"1号","working":false,"workMinutes":0,"workHourCount":0,"workMinutesCount":0},{"id":"100000104101","name":"2号","working":false,"workMinutes":0,"workHourCount":0,"workMinutesCount":0},{"id":"100000104102","name":"3号","working":false,"workMinutes":0,"workHourCount":0,"workMinutesCount":0}],"pxj":[{"id":"100000100100","name":"1号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:24:02","info":"1号喷香机余量不足，请及时更换！"},{"id":"100000100101","name":"2号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"2号喷香机余量不足，请及时更换！"},{"id":"100000100102","name":"3号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"3号喷香机余量不足，请及时更换！"},{"id":"100000100103","name":"4号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"4号喷香机余量不足，请及时更换！"},{"id":"100000100104","name":"5号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 22:53:02","info":"5号喷香机余量不足，请及时更换！"},{"id":"100000100105","name":"6号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:16:02","info":"6号喷香机余量不足，请及时更换！"},{"id":"100000100106","name":"7号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"7号喷香机余量不足，请及时更换！"},{"id":"100000100107","name":"8号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"8号喷香机余量不足，请及时更换！"},{"id":"100000100108","name":"9号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"9号喷香机余量不足，请及时更换！"},{"id":"100000100109","name":"10号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"10号喷香机余量不足，请及时更换！"},{"id":"100000100110","name":"11号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 22:52:02","info":"11号喷香机余量不足，请及时更换！"},{"id":"100000100111","name":"12号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 22:51:02","info":"12号喷香机余量不足，请及时更换！"},{"id":"100000100112","name":"13号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"13号喷香机余量不足，请及时更换！"},{"id":"100000100113","name":"14号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:25:02","info":"14号喷香机余量不足，请及时更换！"},{"id":"100000100114","name":"15号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"15号喷香机余量不足，请及时更换！"},{"id":"100000100115","name":"16号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"16号喷香机余量不足，请及时更换！"},{"id":"100000100116","name":"17号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"17号喷香机余量不足，请及时更换！"},{"id":"100000100117","name":"18号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"18号喷香机余量不足，请及时更换！"},{"id":"100000100118","name":"19号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"19号喷香机余量不足，请及时更换！"},{"id":"100000100119","name":"20号","working":false,"total":3000,"ramainder":3000,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"20号喷香机余量不足，请及时更换！"}],"smczj":[{"id":"100000103100","name":"1号","working":false,"total":30,"ramainder":30,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"1号物联网综合服务器卷纸剩余量不足，请及时更换！"}],"xsysb":[{"id":"100000101100","name":"1号","working":false,"total":1000,"ramainder":100,"alarm":false,"alarmtime":"2018-09-16 20:26:02","info":"1号洗手液设备余量不足，请及时更换！"}],"cszsb":[{"id":"100000102100","name":"1号","working":false,"total":30,"ramainder":30,"alarm":false,"alarmtime":"2018-09-16 20:15:02","info":"1号擦手纸设备余量不足，请及时更换!"}]}

sortOutDate(default_data)

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
    if (+msg_data.flowCount.flowRateLevel <= 3 && msg_data.flowCount.info != "") {
        var a = {}
        a['info'] = msg_data.flowCount.info
        a['dateTime'] = Date.parse(new Date(msg_data.flowCount.alarmtime.replace(/-/g, "/")))
        a['time'] = getTime(a['dateTime'])
        malfunction_msg.push(a)
    }

    if(top.location.href.indexOf("index1.html") > 0) {
        if (+msg_data.monitors.ppmNH3Level <= 3 && msg_data.monitors.ppmNH3Info != "") {
            var a = {}
            a['info'] = msg_data.monitors.ppmNH3Info
            a['dateTime'] = Date.parse(new Date(msg_data.monitors.alarmtime.replace(/-/g, "/")))
            a['time'] = getTime(a['dateTime'])
            malfunction_msg.push(a)
        }
    
        if (+msg_data.monitors.ppmH2SLevel <= 3 && msg_data.monitors.ppmH2SInfo != "") {
            var a = {}
            a['info'] = msg_data.monitors.ppmH2SInfo
            a['dateTime'] = Date.parse(new Date(msg_data.monitors.alarmtime.replace(/-/g, "/")))
            a['time'] = getTime(a['dateTime'])
            malfunction_msg.push(a)
        }
    
        if (+msg_data.monitors.pm25Level <= 3 && msg_data.monitors.pm25Info != "") {
            var a = {}
            a['info'] = msg_data.monitors.pm25Info
            a['dateTime'] = Date.parse(new Date(msg_data.monitors.alarmtime.replace(/-/g, "/")))
            a['time'] = getTime(a['dateTime'])
            malfunction_msg.push(a)
        }
    }

    for (var i = 0, l = msg_data.pxj.length; i < l; i++) {
        if (msg_data.pxj[i].alarm && msg_data.pxj[i].info != "") {
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

function getNowTime() {
    var _dateTime = ""
    var date = new Date()

    var weekday = new Array(7)
    weekday[0] = "星期天"
    weekday[1] = "星期一"
    weekday[2] = "星期二"
    weekday[3] = "星期三"
    weekday[4] = "星期四"
    weekday[5] = "星期五"
    weekday[6] = "星期六"

    _dateTime += date.getFullYear() + "年"
    _dateTime += (date.getMonth() + 1) + "月"
    _dateTime += date.getDate() + "日"

    _dateTime += " " + ((+date.getHours()) > 0 ? date.getHours() : "0"+date.getHours()) + ":" + ((+date.getMinutes()) > 0 ? date.getMinutes() : "0"+date.getMinutes()) + ":" + ((+date.getSeconds()) > 0 ? date.getSeconds() : "0"+date.getSeconds())

    _dateTime += " " + weekday[date.getDay()]
    return _dateTime
}

var _dateTime = {time: getNowTime()}
setInterval(function() {
    _dateTime['time'] = getNowTime()
}, 1000)

new Vue({
    el: '#title',
    data: {
        dateTime: _dateTime
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