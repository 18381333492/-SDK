

//全局变量

window.client = {
    ajax: new ajax(),
    form: new form(),
    string: new string(),
    regex: new regex(),
    datetime: new datetime(),
}

window.dialog = function () {

    //加载等待提示框   
    function loading(msg) {
        if ($(".alert_box").length) {
            $(".alert_box .content p:first").text(msg).parents(".alert_box").css({
                "opacity": "1", "width": "100%", "height": "100%"
            });
        }
        else {
            var win = "<div class='alert_box' style='position:fixed;z-index:1500;width:100%;opacity:1;height:100%;left:0;top:0px;background-color:rgba(68,68,68,0.4);transition:all 0.3s linear'><div class='content' style='width:250px;text-align:center;position:absolute;font-size:14px;left:50%;top:50%; transform:translateY(-50%) translateX(-50%);-webkit-transform:translateY(-50%) translateX(-50%);border-radius:10px'><P style='padding:20px 15px; line-height:25px; color:#fff;font-weight:600;font-size:14px'><span style='background-image:url(/OrderPage/img/loading.gif);display:inline-block;width:17px;height:17px;background-repeat:no-repeat;background-size:100% 100%;vertical-align:middle;margin-right:10px'></span>" + msg + "<P><div><div>";
            $("body").append(win);
            $("body").css({ 'position': 'fixed', 'width': '100%', 'height': '100%', 'top': '0', 'left': '0' });
        }
    };

    //关闭加载效果
    function closeLoading() {
        if ($(".alert_box").length) {
            $('body').attr("style", "");
            $(".alert_box").remove();
        }
    }

    //提示框 msg-提示的消息
    function alert(msg, hide, time) {
        if ($(".msgtipbar").length == 0) {
            var maskPanelHtml = "<div class='msgtipbar animated flipInX' style='bottom:80px;font-weight:500;left: 0;position:fixed;text-align:center;width:100%;z-index:100000;opacity:0.9;transition:all .2s linear'><tip style='background: rgba(0,0,0,0.6) none repeat scroll 0 0;border-radius: 2px;color:#fff;display: inline-block;line-height:35px;padding: 0 10px;'>" + msg + "</tip></div>";
            $("body").append(maskPanelHtml);
            $(".msgtipbar").show();
            $('.msgtipbar').css({ 'bottom': '150px' });
            if (!hide) {
                setTimeout(function () {
                    $(".msgtipbar").remove();
                }, (time || 1500));
            }
        }
    };

    /**
    * 带确认取消按钮的确认框
    * @param {String} msg 提示的信息
    * @param {Object} okOptopn 确认按钮配置，格式如下：{"text":"确定按钮",fn:function(){ //这里是按了确定后触发的事件 }}
    * @param {Object} canclOption 取消按钮配置，格式如下：{"text":"取消按钮",fn:function(){ //这里是按了取消后触发的事件 }}
    * @param {Function} callback 可选：按了确定后，调了确定方法后的回调事件*/
    function confrim(msg, okOptopn, canclOption, callback) {

        //创建背景DIV(创建遮罩层)
        var divBackground = document.createElement("div");
        divBackground.style.cssText = "position:fixed;top: 0;left:0;width: 100%;height: 100%;background: rgba(0,0,0,0.5);z-index:9999999999;";
        //创建内容DIV
        var divContent = document.createElement("div");

        //设置DIV样式
        divContent.style.cssText = "width: 72%;background: #ffffff;border-radius: 8px;margin:70% auto;position: relative;text-align: center;overflow:hidden;font-size:14px;";

        //创建消息SPAN
        var outSpan = document.createElement("span");
        var iconSpan = document.createElement("span");
        var textSpan = document.createElement("span");
        //设置消息提示内容
        textSpan.innerHTML = msg;
        //设置消息span样式
        outSpan.style.cssText = "margin:20px;display:block;";
        iconSpan.style.cssText = "background-image:url('img/logo.png');display:inline-block;width:30px;height:30px;background-repeat:no-repeat;background-size:100% 100%;vertical-align:middle";
        textSpan.style.cssText = "display:inline-block;vertical-align:middle;max-width:calc(100% - 40px);margin-left: 10px";

        //创建下方两个按钮的ul容器
        var ul = document.createElement("ul");
        //设置ulstyle
        ul.style.cssText = "margin-top:10px;border-top: 1px solid #dcdcdc;";

        // 取消按钮（默认）
        var li1 = document.createElement("li");
        //设置按钮样式（默认）
        li1.style.cssText = "text-align: center;line-height: 40px;float:left;width:50%;border-right: 1px solid gainsboro;margin-left:-1px;font-size:12px;color:red;";
        li1.innerHTML = "取消";
        //检查是否自定义取消按钮文本
        if (canclOption && canclOption.text) {
            li1.innerHTML = canclOption.text;
        }
        //绑定取消按钮事件
        li1.onclick = function () {
            if (canclOption) {
                if (canclOption.fn) canclOption.fn();
            }
            document.body.removeChild(divBackground);
        };

        var li2 = document.createElement("li");
        //设置按钮样式（默认）
        li2.style.cssText = "text-align: center;line-height: 40px;float:left;width:50%;font-size:12px;color:red;";
        li2.innerHTML = "确定";
        //检查是否自定义确认按钮文本
        if (okOptopn && okOptopn.text) {
            li2.innerHTML = okOptopn.text;
        }
        //绑定确认按钮事件
        li2.onclick = function () {
            if (okOptopn.fn) okOptopn.fn();
            if (callback) callback();
            document.body.removeChild(divBackground);
        };

        ul.appendChild(li1);
        ul.appendChild(li2);
        outSpan.appendChild(iconSpan);
        outSpan.appendChild(textSpan);
        divContent.appendChild(outSpan);
        divContent.appendChild(ul);
        divBackground.appendChild(divContent);
        document.body.appendChild(divBackground);
    }

    return {
        alert: alert,
        loading: loading,
        confrim: confrim,
        closeLoading: closeLoading
    }
}();

/****************************
 * 前端常用的对象的封装
 *
****************************/

function regex() {

    //验证是否为空
    function isEmpty(val) {
        val = val.replace(/(^\s*)|(\s*$)/g, "");//去掉前后空格
        if (val.length == 0)
            return true;
        return false;
    }

    //是否手机号码格式
    function isPhone(val) {
        var regex = /^((13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9])\d{8})*$/;
        if (regex.exec(val))
            return true;
        return false;
    }

    //验证是否是邮箱
    function isEmail(val) {
        var regex = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
        if (regex.test(val))
            return true;
        return false;
    }

    //验证是否是数字
    function isNumber() {
        var regex = /^[0-9]*$/;
        if (regex.test(val))
            return true;
        return false;
    }

    //验证是否是数字和字母的组合(忽略大小写)
    function isLetterNum(val) {
        var regex = /^[A-Za-z0-9]+$/;
        if (regex.test(val))
            return true;
        return false;
    }
    /**返回的接口**/
    return {
        isEmpty: isEmpty,
        isPhone: isPhone,
        isEmail: isEmail,
        isNumber: isNumber,
        isLetterNum: isLetterNum
    }

}

function string() {

    //验证是否为空
    function isEmpty(val) {
        val = val.replace(/(^\s*)|(\s*$)/g, "");//去掉前后空格
        if (val.length == 0)
            return true;
        return false;
    }

    /*去掉字符串前后字符*/
    function trim(val, char) {
        char = char == undefined ? "" : char;
        return val.replace(/(^\s*)|(\s*$)/g, char);
    }

    /*去掉字符串开始的字符*/
    function trimSta(val, char) {
        char = char == undefined ? "" : char;
        return val.replace(/(^\s*)/g, char);
    }

    /*去掉字符串结束的字符*/
    function trimEnd(val, char) {
        char = char == undefined ? "" : char;
        return val.replace(/(\s*$)/g, char);
    }

    /*
    *字符串的插入
    *{params:val(要插入的字符串),
             staIndex(开始擦入的索引)
             str(插入的字符串)}
    */
    function insert(val, staIndex, str) {
        var strArray = val.split("");
        if (staIndex < strArray.length) {
            strArray[staIndex] = str + strArray[staIndex];
        }
        return strArray.join("");
    }

    /**返回的接口**/
    return {
        isEmpty: isEmpty,
        trim: trim,
        insert: insert,
        trimSta: trimSta,
        trimEnd: trimEnd
    }
}

/*前端ajax的封装*/
function ajax() {
    function ajaxRequest(url, params, callback, er_callback,async) {
        $.ajax({
            url: url,
            data: params,
            type: 'POST',
            dataType: 'json',
            async: (async == null) ? true : async,
            success: function (r) {
                if (callback) {
                    callback(r);
                }
            },
            // jqXHR 是经过jQuery封装的XMLHttpRequest对象
            // textStatus 可能为null、 'timeout（超时）'、 'error（错误）'、 'abort(中止)'和'parsererror（解析错误)'等
            // errorMsg 是错误信息字符串(响应状态的文本描述部分，例如'Not Found'或'Internal Server Error')
            error: function (jqXHR, textStatus, errorMsg) {
                dialog.closeLoading();//关闭loading效果
                switch (jqXHR.status) {
                    case 404: dialog.tip('链接地址错误!'); break;
                    case 500: dialog.tip('服务器内部错误!'); break;
                    default: dialog.tip(jqXHR.status + ":" + jqXHR.statusText);
                }
            }
        });
    }
    //返回的接口
    return {
        ajaxRequest: ajaxRequest
    }

}

/****tip：js对日期的处理****/
function datetime() {

    /*
    * js时间的格式化.
    * @version 1.0.0
    * @param   date(要格式化的时间字符串或日期), format(要格式的格式类型)
    * format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
    * format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
    * @return {string}
    */
    function format(date, format) {
        if (typeof (date) === "string")
            date = new Date(date.replace(/-/g, "/"));//处理苹果时间格式兼容性的问题
        var o = {
            "M+": date.getMonth() + 1, //月份 
            "d+": date.getDate(), //日 
            "h+": date.getHours(), //小时 
            "m+": date.getMinutes(), //分 
            "s+": date.getSeconds(), //秒 
            "q+": Math.floor((date.getMonth() + 3) / 3), //季度 
            "S": date.getMilliseconds() //毫秒 
        };
        for (var time in o) {
            if (isNaN(o[time])) {
                return "";
            }
        }
        if (/(y+)/.test(format))
            format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return format;
    }

    //获取当月的第一天
    function getCurrentMonthFirstDay(value) {
        var date = value == undefined ? new Date() : value;
        date.setDate(1);
        return date;
    }

    //获取某月的有多少天
    function GetDaysInMonth(value) {
        var date = value == undefined ? new Date() : value;
        date = typeof (value) == "string" ? new Date(date) : date;
        return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
    }

    //返回的接口
    return {
        format: format,
        GetDaysInMonth: GetDaysInMonth,
        getCurrentMonthFirstDay: getCurrentMonthFirstDay
    }
}

/****表单的处理****/
function form() {

    /*
    * 将表单元素序列化成Json对象.
    * @version 1.0.0
    * @param   form(要序列化Jquery表单对象)
    * @return {json}
    */
    function parseJson(form) {
        var array = form.serializeArray();
        var obj = {};
        for (var i = 0; i < array.length; i++) {
            var key = array[i]["name"];
            var value = array[i]["value"];
            obj[key] = value;
        }
        return obj;
    }

    return {
        parseJson: parseJson
    }
}
