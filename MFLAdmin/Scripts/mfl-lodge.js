
var MFLLocationCur = "#root";
var MFLLocationLast = "";
var MFLLodgeIdent = "";
var MFLTooltipPlugin = 'easyTooltip';
$(function () {
    $("#wizard a").addClass('ui-widget-header').hover(function () {
        $(this).addClass('ui-state-hover');
    }, function () {
        $(this).removeClass('ui-state-hover');
    });
    setInterval(function () {
        _MFLHashCheck();
        _MFLMouseCheck();
    }, 100);
    $("#main").children().hide();
    $('.topbar a')[MFLTooltipPlugin]({
        yOffset: -30,
        xOffset:-100
    });
    var mouse = $('<div class="mfl-mouse-loading"><img src="../style/loading.gif" alt="操作中，请稍候..." /></div>').appendTo('body');
    $('body').mousemove(function (e) {
        var pageCoords = "( " + e.pageX + ", " + e.pageY + " )";
        var clientCoords = "( " + e.clientX + ", " + e.clientY + " )";
        mouse.css({ top: e.pageY + 1, left: e.pageX + 1 });
    });
});
var _MFLMouseCheck = function () {
    var overlay = $('.mfl-overlay-ajax');
    $('.mfl-mouse-loading').css('display', overlay.length ? 'block' : 'none');
};
var _MFLHashCheck = function () {
    var hash = location.hash.substring(1, location.hash.length);
    if (hash != MFLLocationCur) {
        MFLLocationLast = MFLLocationCur;
        MFLLocationCur = hash;
        if (MFLLocationCur == "") {
            location.hash = "#root";
        } else {
            var arr = MFLLocationCur.split(",");
            //加载导航条
            _MFLInitNav(arr);
        }
    }
};
var MFLTemplateNoPreview = function () {
    MFLDialog("抱歉", "该模板暂时无法预览");
};
var MFLHash = function (path, rel) {
    if (!rel) {
        rel = 0;
    }
    var oldHash = location.hash
    for (var i = rel; i < 0; i++) {
        oldHash = oldHash.substr(0, oldHash.lastIndexOf(","));
    }
    if (path) {
        oldHash += "," + path;
    }
    MFLLocationCur = "!";
    location.hash = oldHash;
};
var _MFLInitNav = function (arr) {

    $('#easyTooltip').remove();
    var ident = arr[arr.length - 1];
    var l = $("#nav a").length;
    var forward = arr.length > l;
    var equal = arr.length == l;
    var readyFunc = function (lastPage, last, animated) {
        $(lastPage).data("MFLNav").text($(lastPage).children(".mfl-lodge-title").text()).removeClass('mfl-nav-loading');
        if (last) {
            document.title = 'MFL旅馆配置面板 -'+$(lastPage).children(".mfl-lodge-title").text();
            $("#main .mfl-currentPage").show("drop", {
                "direction": forward || equal ? "right" : "left",
                "distance": 150
            }, 200, function () {
                if (animated) {
                    animated();
                }
            });
        }
    };
    if (arr.length) {
        $('.nav>li').removeClass('active').filter(':has(a[href="#root,' + arr[1] + '"])').addClass('active');
        if (arr[1] == undefined) {
            $('.nav>li:has(a[href="#root"])').addClass('active');
        }
    }
    $("#main .mfl-currentPage").hide("drop", {
        "direction": forward ? "left" : "right",
        "distance": 150
    }, 200, function () {
        var j = $(".mfl-lodge-" + ident.split('|')[0]);
        if (j.length == 0) {
            $(this).addClass("mfl-currentPage").show();
            MFLDialog("抱歉", "该面板(" + ident.split('|')[0] + ")尚未完成", function () {
                location.hash = "#" + MFLLocationLast;
            });
            return;
        }
        $("#main .mfl-currentPage").show().remove();
        //获取对齐的长度
        var rl = 0;
        $("#nav a").each(function (i) {
            if ($(this).attr("href") == "#" + arr.slice(0, i + 1).join(",")) {
                rl = i + 1;
            }
        });
        for (var i = l - 1; i >= rl; i--) {//删除
            var nav = $("#nav a:eq(" + i + ")")[0];
            $(nav).parent().remove();
            l--;
        }
        var added = false;
        for (var i = l; i < arr.length; i++) {//创建
            added = true;
            var j = $(".mfl-lodge-" + arr[i].split('|')[0]);
            //延迟
            var newArr = arr.slice(0, i + 1);
            var nav = $('<a></a>')
            .attr("href", "#" + arr.slice(0, i + 1).join(","))
            .text(j.children(".mfl-lodge-title").text())

            .click(function () {
                if ($(this).is("#nav a:last-child")) {
                    MFLLocationCur = "";
                }
            });
            j.data("MFLNav", nav);
            $('<li></li>').append(nav)//创建导航条
			.appendTo("#nav .container ul.breadcrumb");
            $("<span class=" + "'divider'" + ">/</span>").insertAfter(nav);
            nav.hide().fadeIn(200);
            if (i + 1 == arr.length) {//克隆页面
                j.clone().addClass("mfl-currentPage").data("MFLOriginalPage", j).appendTo("#main").children(".mfl-lodge-title").remove();
            }
            var newi = i; //因为下面的代码要延迟加载，所以i会变化，用新变量暂存
            if ($.isFunction(j.data("MFLPageLoader"))) {//加载回调
                $(j).data("MFLNav").text('加载中...').addClass('mfl-nav-loading');
                j.data("MFLPageLoader")(arr[i].substring(arr[i].split('|')[0].length + 1), function (newj, animated) {
                    readyFunc(
                        newj, //直接调用j可以，延迟加载则必须重新查询，同理
                        newi + 1 == arr.length, animated
                        );
                });
            } else {
                readyFunc(j, newi + 1 == arr.length);
            }
        }
        if (!added) {//唤醒最后一页
            $(j[0]).clone().addClass("mfl-currentPage").data("MFLOriginalPage", j).appendTo("#main").children(".mfl-lodge-title").remove(); //JUI Effect不能完全将j的另一个元素清除，虽然在dom树里已经把它踢掉了
            if ($.isFunction(j.data("MFLPageLoader"))) {

                j.data("MFLPageLoader")(arr[arr.length - 1].substring(arr[arr.length - 1].split('|')[0].length + 1), function (newj, animated) {
                    readyFunc(j, true, animated);
                });
            } else {
                readyFunc(j, true);
            }
        }
    });

};