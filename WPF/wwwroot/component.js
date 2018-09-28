Vue.component('alarm-wc', {
    template: '<img :src="img_src" style="margin-top: 10px; margin-left:5px; ">',
    data: function () {
        return {
            img_src: "images/u34.png"
        }
    },
    mounted: function () {
        var self = this
        var i = 0
        var imgList = ["images/u34.png", "images/u35.png"]
        setInterval(function () {
            (i == 1 ? i = 0 : i++)
            self.img_src = imgList[i]
        }, 200)
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