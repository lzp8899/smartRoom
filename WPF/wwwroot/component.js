Vue.component('alarm-wc', {
    props: {
        level: Number
    },
    template: '<div style="margin-top: 15px; margin-left:30px;" :style="bgcolor" class="yuan">',
    data: function () {
        return {
            bgcolor: 'background-color: #B3EE3A',
            int: "",
            i: 0
        }
    },
    computed: {
        levelChange: function () {
            return this.level
        }
    },
    watch: {
        levelChange: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        }
    },
    mounted: function () {
        clearTimeout(this.int)
        this.init()
    },
    methods: {
        init: function () {
            var self = this

            if (isNaN(+self.level))
                self.level = 5

            var color5 = 'background-color: #B3EE3A'
            var color4 = 'background-color: yellow'
            var color3 = 'background-color: #FF5809'
            var color2 = 'background-color: red'
            var color1 = 'background-color: #CD2626'

            var color = eval('color' + self.level)
            self.bgcolor = color
        }
    }
})
// 绿  黄 橙 红 深红
Vue.component('alarm-wc-icon', {
    props: {
        level: Number
    },
    template: '<div style="height: 10px;width: 80px; padding-top: 5px;"><div class="icon" :style="color5" /><div class="icon" :style="color4" /><div class="icon" :style="color3" /><div class="icon" :style="color2" /><div class="icon" :style="color1" /></div>',
    data: function () {
        return {
            color5: 'background-color: #B3EE3A',
            color4: 'background-color: yellow',
            color3: 'background-color: #FF5809',
            color2: 'background-color: red',
            color1: 'background-color: #CD2626',
            int: "",
            i: 0
        }
    },
    computed: {
        levelChange: function () {
            return this.level
        }
    },
    watch: {
        levelChange: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        }
    },
    mounted: function () {
        clearTimeout(this.int)
        this.init()
    },
    methods: {
        init: function () {
            var self = this

            if (isNaN(+self.level))
                self.level = 5

            var color5 = 'background-color: #B3EE3A'
            var color4 = 'background-color: yellow'
            var color3 = 'background-color: #FF5809'
            var color2 = 'background-color: red'
            var color1 = 'background-color: #CD2626'

            var color = eval('color' + self.level)

            var colorList = [color, "background-color: rgba(48, 48, 48, 1)"]
            self.int = setTimeout(function () {
                (self.i == 1 ? self.i = 0 : self.i++)
                switch (+self.level) {
                    case 1:
                        self.color1 = colorList[self.i]
                        // self.color1 = color1
                        self.color2 = color2
                        self.color3 = color3
                        self.color4 = color4
                        self.color5 = color5
                        break
                    case 2:
                        self.color2 = colorList[self.i]
                        self.color1 = color1
                        // self.color2 = color2
                        self.color3 = color3
                        self.color4 = color4
                        self.color5 = color5
                        break
                    case 3:
                        self.color3 = colorList[self.i]
                        self.color1 = color1
                        self.color2 = color2
                        // self.color3 = color3
                        self.color4 = color4
                        self.color5 = color5
                        break
                    case 4:
                        self.color4 = colorList[self.i]
                        self.color1 = color1
                        self.color2 = color2
                        self.color3 = color3
                        // self.color4 = color4
                        self.color5 = color5
                        break
                    case 5:
                        self.color5 = colorList[self.i]
                        self.color1 = color1
                        self.color2 = color2
                        self.color3 = color3
                        self.color4 = color4
                        // self.color5 = color5
                        break
                }
                self.init()
            }, 1000)
        }
    }
})

Vue.component('incense-device', {
    props: {
        info: Object
    },
    template: '<div><img :src="src"><div class="incense_device_box"><div class="incense_device_box_div"><div class="incense_device_box_color_div" :style="color_div"></div></div></div></div>',
    data: function () {
        return {
            srcList: ["images/u84.png", "images/u87.png", "images/u90.png"],
            src: '',
            int: null,
            color_div: 'width: 0px;'
        }
    },
    computed: {
        alarmChecked: function () {
            return this.info.alarm
        },
        workingChecked: function () {
            return this.info.working
        },
        ramainderChecked: function () {
            return this.info.ramainder
        }
    },
    watch: {
        alarmChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        workingChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        ramainderChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.ramainder();
            }
        }
    },
    mounted: function () {
        this.init();
    },
    methods: {
        init: function () {
            var self = this
            if (self.info.alarm) {
                self.alarm()
            } else {
                if (self.info.working) {
                    self.working()
                } else {
                    self.stop()
                }
            }

            self.ramainder()
        },
        alarm: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 2]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        working: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 1]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        stop: function () {
            clearInterval(this.int)
            this.src = this.srcList[0]
        },
        ramainder: function () {
            var a = ((+this.info.ramainder) / (+this.info.total)) * 60
            this.color_div = 'width: ' + a + 'px;'
        }
    }
})

Vue.component('general-device', {
    props: {
        info: Object
    },
    template: '<div class="general_service_device" style="left: 37px;"><img :src="src"><div class="general_service_device_box"><div class="general_service_device_box_div"><div class="general_service_device_box_color_div" :style="color_div"></div></div></div><div v-if="info.alarm" class="general_service_device_msg">{{info.info}}</div></div>',
    data: function () {
        return {
            srcList: ["images/u309.png", "images/u312.png", "images/u315.png"],
            src: '',
            int: null,
            color_div: 'width: 0px;'
        }
    },
    computed: {
        alarmChecked: function () {
            return this.info.alarm
        },
        workingChecked: function () {
            return this.info.working
        },
        ramainderChecked: function () {
            return this.info.ramainder
        }
    },
    watch: {
        alarmChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        workingChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        ramainderChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.ramainder();
            }
        }
    },
    mounted: function () {
        this.init();
    },
    methods: {
        init: function () {
            var self = this
            if (self.info.alarm) {
                self.alarm()
            } else {
                if (self.info.working) {
                    self.working()
                } else {
                    self.stop()
                }
            }

            self.ramainder()
        },
        alarm: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 2]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        working: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 1]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        stop: function () {
            clearInterval(this.int)
            this.src = this.srcList[0]
        },
        ramainder: function () {
            var a = ((+this.info.ramainder) / (+this.info.total)) * 130
            this.color_div = 'width: ' + a + 'px;'
        }
    }
})

Vue.component('xsysb-device', {
    props: {
        info: Object
    },
    template: '<div class="general_service_device" style="left: 37px;"><img :src="src"><div class="general_service_device_box"><div class="general_service_device_box_div"><div class="general_service_device_box_color_div" :style="color_div"></div></div></div><div v-if="info.alarm" class="general_service_device_msg">{{info.info}}</div></div>',
    data: function () {
        return {
            srcList: ["images/u333.png", "images/u336.png", "images/u339.png"],
            src: '',
            int: null,
            color_div: 'width: 0px;'
        }
    },
    computed: {
        alarmChecked: function () {
            return this.info.alarm
        },
        workingChecked: function () {
            return this.info.working
        },
        ramainderChecked: function () {
            return this.info.ramainder
        }
    },
    watch: {
        alarmChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        workingChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        ramainderChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.ramainder();
            }
        }
    },
    mounted: function () {
        this.init();
    },
    methods: {
        init: function () {
            var self = this
            if (self.info.alarm) {
                self.alarm()
            } else {
                if (self.info.working) {
                    self.working()
                } else {
                    self.stop()
                }
            }

            self.ramainder()
        },
        alarm: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 2]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        working: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 1]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        stop: function () {
            clearInterval(this.int)
            this.src = this.srcList[0]
        },
        ramainder: function () {
            var a = ((+this.info.ramainder) / (+this.info.total)) * 130
            this.color_div = 'width: ' + a + 'px;'
        }
    }
})

Vue.component('cszsb-device', {
    props: {
        info: Object
    },
    template: '<div class="general_service_device" style="left: 37px;"><img :src="src"><div class="general_service_device_box"><div class="general_service_device_box_div"><div class="general_service_device_box_color_div" :style="color_div"></div></div></div><div v-if="info.alarm" class="general_service_device_msg">{{info.info}}</div></div>',
    data: function () {
        return {
            srcList: ["images/u345.png", "images/u348.png", "images/u351.png"],
            src: '',
            int: null,
            color_div: 'width: 0px;'
        }
    },
    computed: {
        alarmChecked: function () {
            return this.info.alarm
        },
        workingChecked: function () {
            return this.info.working
        },
        ramainderChecked: function () {
            return this.info.ramainder
        }
    },
    watch: {
        alarmChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        workingChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.init();
            }
        },
        ramainderChecked: function (val, oldVal) {
            if (val != oldVal) {
                this.ramainder();
            }
        }
    },
    mounted: function () {
        this.init();
    },
    methods: {
        init: function () {
            var self = this
            if (self.info.alarm) {
                self.alarm()
            } else {
                if (self.info.working) {
                    self.working()
                } else {
                    self.stop()
                }
            }

            self.ramainder()
        },
        alarm: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 2]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        working: function () {
            var self = this
            clearInterval(self.int)
            var i = 0
            var x = [0, 1]
            this.int = setInterval(function () {
                (i < 1 ? i = i + 1 : i = 0)
                self.src = self.srcList[x[i]]
            }, 200)
        },
        stop: function () {
            clearInterval(this.int)
            this.src = this.srcList[0]
        },
        ramainder: function () {
            var a = ((+this.info.ramainder) / (+this.info.total)) * 130
            this.color_div = 'width: ' + a + 'px;'
        }
    }
})

Vue.component('fans-icon', {
    props: {
        working: false
    },
    template: '<img src="images/u239.png" :style="transform">',
    data: function () {
        return {
            transform: "transform: rotate(0deg);",
            int: null
        }
    },
    watch: {
        working: function (val, oldVal) {
            if (val != oldVal) {
                this.working = val
                if (this.working)
                    this.start()
                else
                    this.stop()
            }
        }
    },
    mounted: function () {
        var self = this
        if (self.working) {
            var i = 0
            self.int = setInterval(function () {
                (i < 10000000000000000 ? i = i + 10 : i = 0)
                self.transform = "transform: rotate(" + i + "deg);"
            }, 20)
        } else {
            clearInterval(self.int)
            self.transform = "transform: rotate(0deg);"
        }
    },
    methods: {
        start: function () {
            var self = this
            var i = 0
            this.int = setInterval(function () {
                (i < 10000000000000000 ? i = i + 10 : i = 0)
                self.transform = "transform: rotate(" + i + "deg);"
            }, 20)
        },
        stop: function () {
            clearInterval(this.int)
            this.transform = "transform: rotate(0deg);"
        }
    }
})