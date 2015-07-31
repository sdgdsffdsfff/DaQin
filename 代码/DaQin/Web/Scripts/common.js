/*
 * 公共js
*/

//添加标签页面
function addTab(title, url) {
    var html = '<iframe id="frmWorkArea" width="100%" height="100%" frameborder="0" scrolling="yes"></iframe>';
    var isExist = parent.$("#mainTab").tabs('exists', title); //判断Tab标签中是否有相同的数据标签
    if (!isExist) {
        parent.$("#mainTab").tabs('add', {
            title: title,
            content: html,
            closable: true
        });
        parent.$('#mainTab').tabs('select', title);
        var tab = parent.$('#mainTab').tabs('getSelected');
        var frm = tab.find("iframe")[0];
        frm.contentWindow.location = url;
    }
    else {
        parent.$('#mainTab').tabs('select', title);
        var tab = parent.$('#mainTab').tabs('getSelected');
        var frm = tab.find("iframe")[0];
        frm.contentWindow.location = frm.contentWindow.location;
    }
}

//关闭标签页面
function refreshTab(title) {
    var currentTitle = parent.$('.tabs-selected').text();
    parent.$('#mainTab').tabs('select', title);
    var tab = parent.$('#mainTab').tabs('getSelected');
    parent.$("#mainTab").tabs('close', currentTitle);
    if (tab.find("iframe").length > 0) {
        var refresh = tab.find("iframe")[0].contentWindow.refresh;
        if (refresh) refresh();
    }
}

//提交表单
function submitForm(id, callback, noAlert) {
    var frm = $("#" + id);
    if ($("#ifrm").length == 0) {
        $("body").append('<iframe id="ifrm" name="ifrm" style="display: none;"></iframe>');
        $("#ifrm").load(function () {
            var result = $.trim($(this).contents().find("body").html());
            if (result == "OK") {
                if (!noAlert) alert("保存成功");
                if (callback) {
                    callback({
                        ok: true
                    });
                }
            }
            else {
                if (result.length > 200) result = result.substr(0, 200) + "……";
                if (!noAlert) alert("保存失败：" + result);
                if (callback) {
                    callback({
                        ok: false,
                        msg: result
                    });
                }
            }
        });
    }
    frm.attr("target", "ifrm");
    frm.attr("method", "post");
    frm.attr("enctype", "multipart/form-data");
    frm.submit();
}

//日期格式
function dateformatter(value) {
    if (!value) return new Date().format("yyyy-MM-dd");
    if (value instanceof Date) {
        return value.format("yyyy-MM-dd");
    }
    else if (value.indexOf("Date") != -1) {
        var date = new Date();
        date.setTime(value.replace(/\/Date\((-?\d+)\)\//, '$1'));
        if (date.getYear() < 0) {
            return new Date().format("yyyy-MM-dd");
        }
        return date.format("yyyy-MM-dd");
    }
    else if ($.trim(value) != "") {
        var date = new Date(value.replace(/-/g, '/'));
        return date.format("yyyy-MM-dd");
    }
}

//日期格式
function dateparser(value) {
    if (!value) return new Date();
    if (value.indexOf("Date") != -1) {
        var date = new Date();
        date.setTime(value.replace(/\/Date\((-?\d+)\)\//, '$1'));
        if (date.getYear() < 0) {
            return new Date();
        }
        return date;
    }
    else {
        var arr = value.split(' ');
        var arr1 = arr[0].split('-');
        var y = parseInt(arr1[0], 10);
        var M = parseInt(arr1[1], 10);
        var d = parseInt(arr1[2], 10);
        if (!isNaN(y) && !isNaN(M) && !isNaN(d)) {
            return new Date(y, M - 1, d);
        } else {
            return new Date();
        }
    }
}

// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//序列化表单
function formToJson(params) {
    var arr = new Array();
    for (var i = 0; i < params.length; i++) {
        arr.push('"' + params[i].name + '":"' + params[i].value + '"')
    }
    return '[{' + arr.join(',') + '}]';
}
