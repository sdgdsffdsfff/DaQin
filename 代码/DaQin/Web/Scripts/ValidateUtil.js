/**
* suxiang
* 2014年12月22日
* 验证插件
*/

SimpoValidate = {
    //验证规则
    rules: {
        int: /^[1-9]\d*$/,
        number: /^[+-]?\d*\.?\d+$/
    },

    //初始化
    init: function () {
        $(".valid").each(function () { //遍历span
            if ($(this)[0].tagName.toLowerCase() == "span") {
                var validSpan = $(this);
                var to = validSpan.attr("to");
                var target;
                if (to) {
                    target = $("input[name='" + to + "'],select[name='" + to + "'],textarea[name='" + to + "']");
                } else {
                    var target = validSpan.prev();
                }
                if (target) {
                    target.blur(function () {
                        SimpoValidate.validOne(target, validSpan);
                    });
                }
            }
        });
    },

    //验证全部，验证成功返回true
    valid: function () {
        SimpoValidate.ajaxCheckResult = true;
        var bl = true;

        $(".valid").each(function () { //遍历span
            if ($(this)[0].tagName.toLowerCase() == "span") {
                var validSpan = $(this);
                var to = validSpan.attr("to");
                var target;
                if (to) {
                    target = $("input[name='" + to + "'],select[name='" + to + "'],textarea[name='" + to + "']");
                } else {
                    target = validSpan.prev();
                }
                if (target) {
                    if (!SimpoValidate.validOne(target, validSpan)) {
                        bl = false;
                    }
                }
            }
        });

        return bl && SimpoValidate.ajaxCheckResult;
    },

    //单个验证，验证成功返回true
    validOne: function (target, validSpan) {
        SimpoValidate.removehilight(target, msg);

        var rule = SimpoValidate.getRule(validSpan);
        var msg = validSpan.attr("msg");
        var nullable = validSpan.attr("class").indexOf("nullable") == -1 ? false : true; //是否可为空
        var to = validSpan.attr("to");
        var ajaxAction = validSpan.attr("ajaxAction");

        if (target) {
            //checkbox或radio
            if (target[0].tagName.toLowerCase() == "input" && target.attr("type") && (target.attr("type").toLowerCase() == "checkbox" || target.attr("type").toLowerCase() == "radio")) {
                var checkedInput = $("input[name='" + to + "']:checked");
                if (!nullable) {
                    if (checkedInput.length == 0) {
                        SimpoValidate.hilight(target, msg);
                        return false;
                    }
                }
            }

            //input或select
            if (target[0].tagName.toLowerCase() == "input" || target[0].tagName.toLowerCase() == "select") {
                var val = target.val();
                if (!nullable) {
                    if ($.trim(val) == "") {
                        SimpoValidate.hilight(target, msg);
                        return false;
                    }
                }
                else {
                    if ($.trim(val) == "") {
                        SimpoValidate.removehilight(target, msg);
                        return true;
                    }
                }

                if (rule) {
                    var reg = new RegExp(rule);
                    if (!reg.test(val)) {
                        SimpoValidate.hilight(target, msg);
                        return false;
                    }
                }

                if (ajaxAction) {
                    SimpoValidate.ajaxCheck(target, val, ajaxAction);
                }
            }
            else if (target[0].tagName.toLowerCase() == "textarea") {
                var val = target.text();
                if (!nullable) {
                    if ($.trim(val) == "") {
                        SimpoValidate.hilight(target, msg);
                        return false;
                    }
                }
                else {
                    if ($.trim(val) == "") {
                        SimpoValidate.removehilight(target, msg);
                        return true;
                    }
                }

                if (rule) {
                    var reg = new RegExp(rule);
                    if (!reg.test(val)) {
                        SimpoValidate.hilight(target, msg);
                        return false;
                    }
                }

                if (ajaxAction) {
                    SimpoValidate.ajaxCheck(target, val, ajaxAction);
                }
            }
        }

        return true;
    },

    ajaxCheckResult: true,

    ajaxCheck: function (target, value, ajaxAction) {
        var targetName = target.attr("name");
        var data = new Object();
        data[targetName] = value;

        $.ajax({
            url: ajaxAction,
            type: "POST",
            data: data,
            async: false,
            success: function (data) {
                if (data.data == true) {
                    SimpoValidate.removehilight(target);
                }
                else {
                    SimpoValidate.ajaxCheckResult = false;
                    SimpoValidate.hilight(target, data.data);
                }
            }
        });
    },

    //获取验证规则
    getRule: function (validSpan) {
        var rule = validSpan.attr("rule");
        switch ($.trim(rule)) {
            case "int":
                return this.rules.int;
            case "number":
                return this.rules.number;
            default:
                return rule;
                break;
        }
    },

    //红边框及错误提示
    hilight: function (target, msg) {
        target.addClass("failvalid");
        target.bind("mouseover", function (e) {
            SimpoValidate.tips(target, msg, e);
        });
        target.bind("mouseout", function () {
            SimpoValidate.removetips();
        });
    },

    //取消红边框及错误提示
    removehilight: function (target) {
        target.unbind("mouseover");
        target.unbind("mouseout");
        target.removeClass("failvalid");
        SimpoValidate.removetips();
    },

    //显示提示
    tips: function (target, text, e) {
        var divtipsstyle = "position: absolute; z-index:99999; left: 0; top: 0; background-color: #dceaf2; padding: 3px; border: solid 1px #6dbde4; visibility: hidden; line-height:20px; font-size:12px;";
        $("body").append("<div class='div-tips' style='" + divtipsstyle + "'>" + text + "</div>");

        var divtips = $(".div-tips");
        divtips.css("visibility", "visible");

        var top = e.clientY + $(window).scrollTop() - divtips.height() - 18;
        var left = e.clientX;
        divtips.css("top", top);
        divtips.css("left", left);

        $(target).mousemove(function (e) {
            var top = e.clientY + $(window).scrollTop() - divtips.height() - 18;
            var left = e.clientX;
            divtips.css("top", top);
            divtips.css("left", left);
        });
    },

    //移除提示
    removetips: function () {
        $(".div-tips").remove();
    }
};

$(function () {
    SimpoValidate.init();
});