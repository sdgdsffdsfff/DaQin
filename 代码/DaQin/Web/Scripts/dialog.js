/**
* lhgdialog 弹出框封装
* @param 参数
*/
(function ($) {
    $.extend({
        iDialog: function (param) {
            var api, W;
            if (!frameElement) { //不在Iframe中
                api = null;
                W = window;
            } else if (frameElement.api) {
                api = frameElement.api;
                W = api.opener;
            } else {
                api = null;
                W = window;
            }
            var defaultParam = $.extend({
                max: false,
                min: false,
                drag: true,
                lock: true,
                top: "50%",
                width: '800px',
                resize: false,
                ok: true,
                okVal: "保存",
                cancelVal: "关闭",
                cancel: true, /*为true等价于function(){}*/
                parent: api
            }, param);
            W.$.dialog(defaultParam);
        }
    });
})(jQuery);

/**
* easyui的window插件再次封装
* 2014年11月10日
*/

SimpoWin = {
    showWin: function (title, url, width, height, okcallback, closecallback) {
        if (!top.SimpoWinId) top.SimpoWinId = 0;
        var divId = "simpoWin" + top.SimpoWinId;
        top.$("body").append('<div id="' + divId + '" style="overflow:hidden;"></div>');

        top.$('#' + divId).window({
            modal: true,
            title: title,
            width: width,
            height: height,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            resizable: false,
            content: function () {
                return '<iframe frameborder="0" src="' + url + '" style="width: ' + (width - 14).toString() + 'px; height: ' + (height - 36).toString() + 'px; margin: 0;">';
            },
            onClose: function () {
                top.$('#' + divId).window('destroy');
                top.SimpoWinId--;
                if (closecallback) closecallback();
            }
        }).window('open');

        top.SimpoWinId++;
        switch (top.SimpoWinId) {
            case 1:
                top.SimpoWinParent1 = window;
                top.SimpoWinOKCallback1 = okcallback;
                break;
            case 2:
                top.SimpoWinParent2 = window;
                top.SimpoWinOKCallback2 = okcallback;
                break;
            case 3:
                top.SimpoWinParent3 = window;
                top.SimpoWinOKCallback3 = okcallback;
                break;
            default:
                top.SimpoWinParent = window;
                top.SimpoWinOKCallback = okcallback;
                break;
        }
    },

    closeWin: function () {
        var divId = "simpoWin" + (top.SimpoWinId - 1).toString();
        top.$('#' + divId).window('close');
    },

    OK: function (data) {
        switch (top.SimpoWinId) {
            case 1:
                top.SimpoWinOKCallback1(data);
            case 2:
                top.SimpoWinOKCallback2(data);
            case 3:
                top.SimpoWinOKCallback3(data);
            default:
                top.SimpoWinOKCallback(data);
        }
    },

    GetWinParent: function () {
        switch (top.SimpoWinId) {
            case 1:
                return top.SimpoWinParent1;
            case 2:
                return top.SimpoWinParent2;
            case 3:
                return top.SimpoWinParent3;
            default:
                return top.SimpoWinParent;
        }
    },

    showWin2: function (title, containerId, width, height, closecallback) {
        top.$('#' + containerId).css("display", "");
        var l = (top.$(top.window).width() - width) / 2;
        var t = (top.$(top.window).height() - height) / 2 + top.$(top.document).scrollTop();
        top.$('#' + containerId).window({
            modal: true,
            title: title,
            width: width,
            height: height,
            left: l,
            top: t,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            onClose: function () {
                if (closecallback) closecallback();
            }
        }).window('open');
    },

    closeWin2: function (containerId) {
        top.$('#' + containerId).window('close');
    }
}
