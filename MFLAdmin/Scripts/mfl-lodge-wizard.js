var MFLWizardSteps = [
    {
        'text': '您好，我是旅馆设置向导，您的旅馆在注册之后已经包含了一些默认的配置，我将领您一步一步修改并完善这些配置。点击“开始”吧。',
        'title': '设置向导-开始'
    }, {
        'text': '请选择旅馆前台界面使用的模版。点击“保存修改”进入下一步。',
        'title': '设置向导-步骤(1/8)',
        'init': '#root,template',
        'autoNext': function () {
            MFLSubmit($(".mfl-lodge-template.mfl-currentPage .mfl-list-item form"));
        }
    }, {
        'text': '请填写该页中的旅馆设置信息，然后点击“保存修改”以进入下一步。',
        'title': '设置向导-步骤(2/8)',
        'init': '#root,properties',
        'autoNext': function () {
            MFLSubmit($(".mfl-lodge-properties.mfl-currentPage .mfl-list-item form"));
        }
    }, {
        'text': '请填写旅馆附近的交通方式，指引客人来到旅馆，并且用鼠标拖动地图标明旅馆所在位置。点击“保存修改”进入下一步。',
        'title': '设置向导-步骤(3/8)',
        'init': '#root,traffic',
        'autoNext': function () {
            MFLSubmit($(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.trafficForm"));
        }
    }, {
        'text': '请修改并添加完整旅馆房间的特性列表，特性是旅馆中不同房间之间用来对比的属性，您还可以为每一个特性设定多个不同的选项，别忘了给特性和选项选择一个好看的图标哦。添加完毕后请点击“下一步”。',
        'title': '设置向导-步骤(4/8)',
        'init': '#root,attributes'
    }, {
        'text': '请添加每一个房间的信息，并选择这个房间的特性，然后添加每个房间单独的照片。添加完毕后请点击“下一步”。',
        'title': '设置向导-步骤(5/8)',
        'init': '#root,rooms'
    }, {
        'text': '请添加一些旅馆总体各方面的照片，旅馆的一些特点（如早餐、周边景点）也可以在此附图说明。添加完毕后请点击“下一步”。',
        'title': '设置向导-步骤(6/8)',
        'init': '#root,lodgeAlbum'
    }, {
        'text': '请设置旅馆的管理员密码，管理员平常负责更新旅馆各房间的入住状态以供访问者查询。点击“设定密码”进入下一步。',
        'title': '设置向导-步骤(7/8)',
        'init': '#root,admin',
        'autoNext': function () {
            MFLSubmit($(".mfl-lodge-admin.mfl-currentPage form"));
        }
    }, {
        'text': '请点击“应用更改”按钮，可以将所有刚才做出的更改更新到前台，便会生成一个漂亮的旅馆网站啦。',
        'title': '设置向导-步骤(8/8)',
        'init': '#root',
        'autoNext': function () {
            $(".mfl-refresh-btn").trigger('click');
            MFLWizardCur++;
            _MFLWizardInitPanel();
        }
    }, {
        'text': '您已经完成了这个旅馆的设置，您以后仍然可以再次打开此向导。点击“完成”可以关闭该向导。',
        'title': '设置向导,完成'
    }
];
var MFLWizardCur;
var _MFLCurInit = "";
var MFLWizardLoad = function () {
    if (!_MFLChecking) {
        _MFLChecking = true;
        _MFLChecker();
    }
    $("<div></div>", {
        "class": "wizard"
    })
    .append($('<h3><span></span><img src="../Style/lodge/cross_circle.png" class="closeBtn" onclick="MFLWizardUnload();" /></h3>').attr({
        "class": "ui-widget-header wizard-title"
    }))
    .append($("<div></div>", {
        "class": "ui-widget-content wizard-text"
    }))
    .append($("<a></a>", {
        "class": "wizard-prev ui-state-default",
        "href": "#"
    }))
    .append($("<a></a>", {
        "class": "wizard-next ui-state-default",
        "href": "#"
    }))
    .append($("<div></div>", {
        "class": "wizard-progress"
    }).progressbar())
    .hide()
    .insertAfter("#nav")
    .fadeIn(300)
    .draggable({
        handle: "h3"
    })
    .children(".ui-state-default").button();
    MFLWizardCur = 0;
    _MFLWizardInitPanel();
  
    $(".wizardSwitcher");//.addClass("ui-state-highlight");
    $('<div class="wizardNavDisabler"></div>').appendTo('.topbar');
    MFLWizardLoaded = MFLWizardLoad;
    MFLWizardLoad = function () {
    };
};
var _MFLWizardInitPanel = function () {
    $("#nav a").unbind("click.wizardDiasabled").removeClass("wizard-disabledNav");
    var step = MFLWizardSteps[MFLWizardCur];
    if (step.init) {
        location.href = step.init;	
    }
    _MFLCurInit = step.init;
    $("#nav a").each(function () {
        if ($(this).attr("href") == step.init) {
            return false;
        }
        $(this).bind("click.wizardDiasabled", function () {
            return false;
        }).addClass("wizard-disabledNav");
    });
    if (MFLWizardCur + 1 == MFLWizardSteps.length) {
        $(".wizard .wizard-next").unbind("click").text("完成").click(function () {
            MFLWizardUnload();
            return false;
        }).removeClass("ui-state-disabled");
    } else {
        var btn = $(".wizard .wizard-next").unbind("click").text("下一步");
        if (step.autoNext) {
            btn.click(function () {
                return false;
            }).addClass("ui-state-disabled");
            if ($.isFunction(step.autoNext)) {
                btn.click(step.autoNext).removeClass("ui-state-disabled");
            }
        } else {
            btn.click(function () {
                MFLWizardCur++;
                _MFLWizardInitPanel();
                return false;
            }).removeClass("ui-state-disabled");
        }
    }
    if (MFLWizardCur == 0) {
        $(".wizard .wizard-prev").unbind("click").text("取消").click(function () {
            MFLWizardUnload();
            return false;
        });
        $(".wizard .wizard-next").text("开始")
    } else {
        $(".wizard .wizard-prev").unbind("click").text("上一步").click(function () {
            MFLWizardCur--;
            _MFLWizardInitPanel();
            return false;
        });
    }
	
    $(".wizard .wizard-progress").progressbar("value", 100 * (MFLWizardCur + 1) / MFLWizardSteps.length);
    $(".wizard .wizard-title span").text(step.title);
    $(".wizard .wizard-text").fadeOut(500, function () {
        $(this).text(step.text).fadeIn(500);
    });
};
var _MFLChecker = function () {
    if (MFLWizardLoaded) {
        if (_MFLCurInit && location.hash.substr(0, _MFLCurInit.length) != _MFLCurInit) {
            MFLWizardCur++;
            setTimeout("_MFLWizardInitPanel();setTimeout(_MFLChecker, 400);", 500);
            return;
        }
    }
    setTimeout(_MFLChecker, 400);
};
var _MFLChecking = false;
var MFLWizardLoaded = null;
var MFLWizardUnload = function () {
    if (MFLWizardLoaded) {
        $('.wizardNavDisabler').remove();
        $(".wizardSwitcher").removeClass("ui-state-highlight");
        $("#leftNav .ui-button").button("enable");
        $("#nav a").unbind("click").removeClass("wizard-disabledNav");
        MFLWizardLoad = MFLWizardLoaded;
        MFLWizardLoaded = null;
        $(".wizard").fadeOut(
            300,
        function () {
            $(this).remove();
        });
    }
};
$(function () {
    $(".wizardSwitcher").click(function () {
        if (MFLWizardLoaded) {
            MFLWizardUnload();
        } else {
            MFLWizardLoad();
        }
    });
});