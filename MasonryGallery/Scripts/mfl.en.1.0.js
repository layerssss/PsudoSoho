var MFLList = function (o) {
    var options = {
        "json": null,
        "pending": false,
        "query": [],
        "pagerAjaxOptions": null,
        "pagerHolderJ": null,
        "autoEmpty": true
    };
    options = $.extend(options, o);
    if (!options.pending) {
        options.containerJ.children(".mfl-list-item,.mfl-list-empty").remove();
    }
    options.containerJ.children(".mfl-list-itemTemplate").hide();
    var json = options.json;
    var items = json.listItems ? json.listItems : json;
    if (options.filter) {
        items = $.map(items, function (item) {
            if (!options.filter(item)) {
                return null;
            }
            return item;
        });
    }
    var container = $("<div></div>");
    $.each(items, function (number, item) {
        var line = options.containerJ.children(".mfl-list-itemTemplate").clone(true)
                        .show()
                    .removeClass("mfl-list-itemTemplate")
                    .addClass("mfl-list-item");

        line.appendTo(options.containerJ);
        $.each(item, function (key, value) {
            line.find(".mfl-tags-" + key).each(function () {
                var tag = $(this);
                var newhtml = "";
                $.each(String(value).split(','), function (i, v) {
                    if (v != "") {
                        newhtml += tag.html().replaceAll("\\$" + key + "\\$", String(v));
                    }
                });
                tag.html(newhtml);
            });
            if (item.fromJsonMachine) {
                line.html(line.html().replaceAll("\\$" + key + "\\$", String(value)));
            } else {
                line.find(".mfl-" + key).html(String(value));
                line.find(".mfl-text-" + key).text(String(value));
                line.find(".mfl-text-" + key).each(function () {
                    $(this).html($(this).html().replaceAll('\n', '<br />'));
                });
                line.find(".mfl-src-" + key).attr("src", String(value));
                line.find(".mfl-alt-" + key).attr("title", String(value)).attr("alt", String(value));
                line.find(".mfl-val-" + key).val(String(value));
                line.find(".mfl-data-" + key).data("MFLData-" + key, String(value));
                line.find(".mfl-href-" + key).each(function () {
                    $(this).attr("href", $(this).attr("href").replaceAll("\\$" + key + "\\$", String(value)));
                });
                line.find(".mfl-class-" + key).each(function () {
                    $(this).attr("class", $(this).attr("class").replaceAll("\\$" + key + "\\$", String(value)));
                });
                line.find(".mfl-replace-src-" + key).each(function () {
                    $(this).attr("src", $(this).attr("src").replaceAll("\\$" + key + "\\$", String(value)));
                });
            }
        });
    });
    if (json.pages && options.pagerAjaxOptions) {
        options.pagerHolderJ.addClass("mfl-list-pages").children('.mfl-list-page').remove();
        $.each(json.pages, function (key, value) {
            var page = $('<a href="#" class="mfl-list-page">' + key + '</a>').click(function () {
                if ($(this).hasClass("mfl-list-pageDisabled")) {
                    return false;
                }
                options.pagerAjaxOptions.query["page"] = value;
                if (!$.isFunction(options.pagerAjaxOptions.success)) {
                    options.pagerAjaxOptions.success = function (json, jo) {
                        $.each(json, function (i, v) {
                            if (v.pages != undefined) {
                                o.json = v;
                            }
                        });
                        MFLList(o);
                    };
                }
                MFLAjax(options.pagerAjaxOptions);
                return false;
            });
            switch (value) {
                case -1:
                    page.addClass("mfl-list-pageDisabled");
                    break;
                case -2:
                    page.addClass("mfl-list-pageCurrent").addClass("mfl-list-pageDisabled");
                    break;
            }
            page.appendTo(options.pagerHolderJ);
        });
    }
    if (options.autoEmpty && !items.length) {
        $('<div class="mfl-list-empty">没有可以显示的条目</div>').appendTo(options.containerJ);
    }
};
var _MFLGlobalXhr = [];
var _MFLGlobalLoading = [];
var _MFLGlobalLoadingHidden = [];
var MFLNotify = function (text, root) {
    var not = $('<div class="mfl-notify ui-state-error"></div>').html('<span class="mfl-infoTag"></span>'+text ).hide().appendTo("#notifications");
    if (root) {
        not.appendTo(root);
    }
    if (root == false) {
        not.remove();
        return;
    }
    not.slideDown(500).click(function () {
        $(this).slideUp(500, function () {
            $(this).remove();
        });
    });
    setTimeout(function () {
        $(not).trigger("click");
    }, 10000);
};
var MFLDialog = function (title, cont, next, ok, o) {
    if (typeof (cont) == "string") {
        cont = $('<div></div>').html(cont).appendTo("body");
    }
    if (!next) {
        next = function () {
        };
    }
    cont = $('<form id="dlgForm" action="#"><input type="submit" name="S1" style="float:right;border:none;background:none;visibility:visible;width:5px;height:5px;left:-10px;top:0border-width:0;margin:0;padding:0;overflow:hidden;" /></form>').append(cont).appendTo("body").dialog({
        "title": title,
        "autoOpen": false,
        "modal": true,
        "width": 500,
        "zIndex":4000,
        "resizable":false,
        "close": function () {
            next($("#dlgForm").parent().find(".ui-dialog-buttonpane .ui-dialog-buttonset")[0]);
            $(this).dialog("destroy").remove();
        }
    });
    if ($.isFunction(ok)) {
        cont.dialog("option", "buttons", [{
            "text": "Ok",
            "click": function () {
                if (ok($("#dlgForm").parent().find(".ui-dialog-buttonpane .ui-dialog-buttonset")[0]) == false) {
                    return;
                }
                $(this).dialog("close");
            }
        }, {
            "text": "Cancel",
            "click": function () {
                $(this).dialog("close");
            }
        }]);
    } else {
        cont.dialog("option", "buttons", [{
            "text": "Ok",
            "click": function () {
                $(this).dialog("close");
            }
        }]);
    }
    if (o) {
        cont.dialog("option", o);
    }
    cont.dialog("open");
    cont.submit(function () {
        cont.parent().find("button:first-child").trigger("click");
        return false;
    });
    $(cont.find("input[type='text'],input[type='password'],textarea")[0]).focus();
};
var MFLAjax = function (o) {
    var options = {
        "type": 'GET',
        "query": new Object(),
        "filePosting": false,
        "loadingJ": null,
        "notifyRoot": null,
        complete: function () {
        },
        handler: null,
        handlerOptions: null
    };
    options = $.extend(options, o);
    if (_MFLGlobalXhr[options.action]) {
        $(_MFLGlobalLoadingHidden[options.action]).show().removeClass("mfl-loading-hidden");
        $(_MFLGlobalLoading[options.action]).remove();
        try {
            //            _MFLGlobalXhr[options.action].abort();//commented in MG
            _MFLGlobalXhr[options.action] = null;
        }
        catch (e) {
        }
    }
    if (options.validationFormJ) {
        options.validationFormJ.find(".mfl-infoTag:not(#notifications .mfl-infoTag)").remove();
        options.validationFormJ.find('input[type="text"]:not([readonly="readonly"]),input[type="password"],select').each(function () {
            if (!options.query) {
                options.query = new Object();
            }
            if ($(this).attr('id')) {
                options.query[$(this).attr('id')] = $(this).val();
            }
        });
        options.validationFormJ.find('.radio').each(function () {
            if ($(this).attr('id')) {
                options.query[$(this).attr('id')] = $(this).children('input[type="radio"]').serializeArray()[0].value;
            }
        });
    }
    if (typeof (options.action) == "string") {
        options.url = "/Data.aspx?action=" + encodeURI(options.action);
    }
    if (options.loadingJ) {
        _MFLGlobalLoadingHidden[options.action] = options.loadingJ.hide().addClass("mfl-loading-hidden")[0];
        _MFLGlobalLoading[options.action] = options.loading = $("<div></div>").addClass("mfl-loading").append($("<img />"
        , {
            alt: "加载中，请稍候...",
            title: "加载中，请稍候...",
            src: data.BaseUrl + "Styles/loading.gif"//modified in MG
        })).insertAfter(options.loadingJ);
    }

    var ajaxOpt = {
        'url': options.url,
        'data': options.query,
        'type': options.type,
        'completeJ': function () {//modified in MG
            //            _MFLGlobalXhr[options.action] = null;
            //            if (options.loadingJ) {
            //                $(_MFLGlobalLoadingHidden[options.action]).show().removeClass("mfl-loading-hidden");
            //                $(_MFLGlobalLoading[options.action]).remove();
            //            }
            $('.mfl-loading').remove();
            options.complete();
        },
        'cache': false,
        'dataType': 'json',
        'success': function (jsonData) {
            ajaxOpt.completeJ(jsonData);
            if (jsonData.success) {
                if (jsonData.message) {
                    MFLDialog("提示", jsonData.message, function () {
                        options.success(jsonData, options);
                    });
                } else {
                    options.success(jsonData, options);
                }
            } else {
                if (jsonData.messageType) {
                    switch (jsonData.messageType) {
                        case 1:
                            MFLReCaptcha({
                                callback: function (c, r) {
                                    options.query["recaptcha_response_field"] = r;
                                    options.query["recaptcha_challenge_field"] = c;
                                    MFLAjax(options);
                                }
                            });
                            break;
                        case 2:
                            MFLDialog("Confirmation", jsonData.message, function () { }, function () {
                                options.query["gc_confirmed"] = "gc_confirmed";
                                MFLAjax(options);
                            });
                            break;
                        case 4:
                        case 3:
                            var fname = options.validationFormJ.find("label[for='" + jsonData.message + "']");
                            if (fname.length) {
                                switch (jsonData.messageType) {
                                    case 3:
                                        MFLNotify("很抱歉，您必须填写“" + $.trim(fname.text()) + "”才能继续当前的操作。", options.notifyRoot);
                                        break;
                                    case 4:
                                        MFLNotify("很抱歉，您必须填写有效的" + $.trim(fname.text()) + "”才能继续当前的操作。", options.notifyRoot);
                                        break;
                                }
                                options.validationFormJ.find("#" + jsonData.message).trigger("focus");
                                $('<span class="mfl-infoTag" title="很抱歉，您必须填写“' + $.trim(fname.text()) + '”才能继续当前的操作。"></span>').appendTo(fname);
                            } else {
                                MFLNotify("很抱歉，您必须填写完整所需要的信息才能继续当前的操作。", options.notifyRoot)
                            }
                            break;
                        case 5:
                            var fname = options.validationFormJ.find("label[for='" + jsonData.message.split('|')[0] + "']");
                            if (fname.length) {
                                MFLNotify(jsonData.message.split('|')[1], options.notifyRoot);
                                options.validationFormJ.find("#" + jsonData.message).trigger("focus");
                                $('<span class="mfl-infoTag" title="' + jsonData.message.split('|')[1] + '"></span>').appendTo(fname);
                            } else {
                                MFLNotify(jsonData.message.split('|')[1], options.notifyRoot)
                            }
                            break;
                        case 6:
                            var msg = jsonData.message.split('|')[0];
                            var jsFunc = jsonData.message.split('|')[1];
                            eval(jsFunc + "(function(){MFLAjax(options);});");
                            MFLNotify(msg, options.notifyRoot);
                            break;
                    }
                    if (options.failed) {
                        options.failed(jsonData.message, options);
                    }
                    return;
                }
                if (options.failed) {
                    options.failed(jsonData.message, options);
                } else {
                    MFLNotify(jsonData.message, options.notifyRoot);
                }
            }
        },
        'error': function (a, b, c) {
            ajaxOpt.completeJ();
            if (c == "abort") {
                return false;
            }
            if (options.failed) {
                options.failed("A:" + a + ";B:" + b + ";C:" + c, options);
            } else {
                if (Number(a.status) == 500) {
                    MFLNotify("非常抱歉，您刚才的操作致使发生了一个错误，如果您经常看到该提示，请联系我们网站的管理员，谢谢。");
                }
            }
        }
    };
    if (options.filePosting) {
        $(options.query).attr({
            "action": options.url + "&cType=html",
            "method": "post",
            "enctype": "multipart/form-data",
            "target": "mfl-hidden-iframe",
            "onsubmit": "return true;"
        });
        _MFLGlobalXhr[options.action] = {
            abort: function () {
            }
        };
        var detect = function () {
            if (timeOut) {
                var io = $("iframe#mfl-hidden-iframe")[0];
                var responseText;
                if (io.contentWindow) {
                    responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                } else if (io.contentDocument) {
                    responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                }
                if (responseText) {
                    responseText = responseText.replaceAll("<pre>", "");
                    responseText = responseText.replaceAll("</pre>", "");
                    try {
                        ajaxOpt.complete();
                        ajaxOpt.success($.parseJSON(responseText), options);
                        $("iframe#mfl-hidden-iframe").attr("src", "about:blank");
                    } catch (e) {
                        alert(responseText + e);
                    }
                    return;
                }
                timeOut -= 100;
                setTimeout(detect, 100);
                return;
            }
            ajaxOpt.complete();
            ajaxOpt.error("请求超时", "", "");
        };
        var timeOut = 5000;
        setTimeout(detect, 100);
    } else {
        if (options.handler) {
            _MFLGlobalXhr[options.action] = options.handler(ajaxOpt, options.handlerOptions);
        } else {
            _MFLGlobalXhr[options.action] = $.ajax(ajaxOpt);
        }
    }
};
var MFLReCaptcha = function (oo) {
    if (!MFLReCaptchaReady) {
        var dlg;
        MFLDialog("请稍候", $("<div>正在加载验证码，请稍候……<br /></div>").addClass("mfl-loading").append($("<img />"
        , {
            alt: "加载中，请稍候...",
            title: "加载中，请稍候...",
            src: "/Styles/loading.gif"
        })), null, null, {
            buttons: new Object(),
            open: function (event, ui) {
                dlg = this;
                $.getScript("http://www.google.com/recaptcha/api/js/recaptcha_ajax.js", function () {
                    $(dlg).dialog("close");
                    MFLReCaptcha = function (o) {
                        var o = $.extend({
                            cancelable: true,
                            callback: function () {
                            }
                        }, o);
                        $('#reCaptcha').remove();
                        $('<div id="reCaptcha"></div>').appendTo('body');
                        Recaptcha.create("6LdopMcSAAAAAJs-iKzn7fn4Fq7O0MG0hjQT5gxt", "reCaptcha", {
                            custom_translations: {
                                instructions_visual: "请输入您所看到的字符和数字:",
                                instructions_audio: "请输入您所听到的字符和数字:",
                                play_again: "重听一遍",
                                cant_hear_this: "以MP3格式下载并播放",
                                visual_challenge: "显示验证码",
                                audio_challenge: "播放验证码",
                                refresh_btn: "点击获取另外一段字符串",
                                help_btn: "帮助",
                                incorrect_try_again: "验证错误，请再试一次。"
                            },
                            lang: 'zh-CN', // Unavailable while writing this code (just for audio challenge)
                            theme: 'white', // Make sure there is no trailing ',' at the end of the RecaptchaOptions dictionary
                            'callback': function () {
                                {
                                    try {
                                        Recaptcha.focus_response_field();
                                    }
                                    catch (e) {
                                    }
                                    $('<span style="float:right;width:130px;font-size:0.8em;">抱歉，出于安全原因，您必须要输入左边图像中的验证码才能继续。<br />您不一定要完全正确地输入图中的字符，只需要尽量地输入您所能认出的字符即可。</span>').prependTo("#reCaptcha");
                                    if (o.cancelable) {
                                        MFLDialog("请输入验证码", $("#reCaptcha"), function () { }, function () {
                                            var challenge = $('#reCaptcha input[name="recaptcha_challenge_field"]').val();
                                            var response = $('#reCaptcha input[name="recaptcha_response_field"]').val();
                                            o.callback(challenge, response);
                                        }, {
                                            "autoSize": false
                                        });
                                    } else {
                                        MFLDialog("请输入验证码", $("#reCaptcha"), function () {
                                            var challenge = $('#reCaptcha input[name="recaptcha_challenge_field"]').val();
                                            var response = $('#reCaptcha input[name="recaptcha_response_field"]').val();
                                            o.callback(challenge, response);
                                        }, null, {
                                            "autoSize": false
                                        });
                                    }
                                }
                            }
                        });

                    };
                    MFLReCaptchaReady = true;
                    MFLReCaptcha(oo);
                });
            }
        });

    }
};
var MFLReCaptchaReady = false;
var MFLReCaptchaTest = function () {
    MFLAjax({
        loadingJ: $(this),
        action: "TestReCaptcha",
        success: function () {
            MFLDialog("TEST", "ReCaptcha test passed!");
        }
    });
    return false;
};
var MFLSubmit = function (button) {
    $(button).closest("form").trigger("submit");
};
var MFLOpenIt = function (a) {
    var isJs = false;
    if ($(a).attr("href").substring(0, 11) == "javascript:") {
        isJs = true;
    }
    var oldTarget;
    if (isJs) {
        oldTarget = $(a).attr("target");
        $(a).attr("target", null);
    }
    var oldClick = $(a).attr("onclick");
    $(a).attr("onclick", null);
    $(a).trigger("click");

    $(a).attr("onclick", oldClick);
    if (isJs) {
        $(a).attr("target", oldTarget);
    }
};
String.prototype.replaceAll = function (reallyDo, replaceWith, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(reallyDo)) {
        return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi" : "g")), replaceWith);
    } else {
        return this.replace(reallyDo, replaceWith);
    }
};
