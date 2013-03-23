var MFLList = function (o) {
    var options = {
        "url": "",
        "query": "",
        'json': null,
        "success": function () {
        }
    };
    options = $.extend(options, o);
    options.containerJ.children(":not(.mfl-list-itemTemplate)").remove();
    options.containerJ.children(".mfl-list-itemTemplate").hide();
    var successFunc = function (json) {
        if (options.filter) {
            json.listItems = $.map(json.listItems, function (item) {
                if (!options.filter(item)) {
                    return null;
                }
                return item;
            });
        }
        $.each(json.listItems, function (number, item) {
            var line = options.containerJ.children(".mfl-list-itemTemplate").clone(true)
                .show()
            .removeClass("mfl-list-itemTemplate")
            .addClass("mfl-list-item");
            item.i = number;
            $.each(item, function (key, value) {
                line.find('*').andSelf().filter(".mfl-" + key).text(String(value));
                line.find('*').andSelf().filter(".mfl-html-" + key).html(String(value));
                line.find('*').andSelf().filter('.mfl-list-' + key).each(function (i, list) {
                    MFLList({
                        json: value,
                        containerJ: $(list)
                    });
                });
                line.find('*').andSelf().filter(".mfl-class-" + key).each(function () {
                    $(this).attr("class", $(this).attr("class").replaceAll("\\$" + key + "\\$", String(value)));
                });
                line.find('*').andSelf().filter(".mfl-src-" + key).attr("src", String(value));
                line.find('*').andSelf().filter(".mfl-name-" + key).attr("name", String(value));
                line.find('*').andSelf().filter(".mfl-style-" + key).attr("style", String(value));
                line.find('*').andSelf().filter(".mfl-alt-" + key).attr("title", String(value)).attr("alt", String(value));
                line.find('*').andSelf().filter(".mfl-val-" + key).val(String(value));
                line.find('*').andSelf().filter(".mfl-data-" + key).data("MFLData-" + key, String(value));
                line.find('*').andSelf().filter(".mfl-href-" + key).each(function () {
                    $(this).attr("href", $(this).attr("href").replaceAll("\\$" + key + "\\$", String(value)));
                });
            });
            line.appendTo(options.containerJ);
        });
        if ($.isFunction(options.callback)) {
            options.callback(json);
        }
    };
    if (options.json) {
        successFunc(options.json);
    } else {
        MFLAjax(
            {
                "url": options.url,
                "action": options.action,
                "query": options.query,
                "success": successFunc
            });
    }
};
var MFLDialog = function (title, content, next, ok) {
    if (typeof (content) == "string") {
        content = $("<div></div>").html(content);
    }
    if (!next) {
        next = function () {
        };
    }
    content.appendTo("body").dialog({
        "title": title,
        "autoOpen": false,
        "modal": true,
        "width": 500,
        "close": function () {
            next();
            $(this).dialog("destroy").remove();
        }
    });
    if ($.isFunction(ok)) {
        content.dialog("option", "buttons", [{
            "text": "确定",
            "click": function () {
                ok();
                $(this).dialog("close");
            }
        }, {
            "text": "取消",
            "click": function () {
                $(this).dialog("close");
            }
        }]);
    } else {

        content.dialog("option", "buttons", [{
            "text": "确定",
            "click": function () {
                $(this).dialog("close");
            }
        }]);
    }
    content.dialog("open");
};
var MFLAjax = function (o) {
    var options = {
        "type": 'post',
        "filePosting": false,
        "basePath": '',
        'filename': 'Data.aspx'
    };
    MFLAjax.globalJsonFilter = MFLAjax.globalJsonFilter ? MFLAjax.globalJsonFilter : function (json) {
        return json;
    };
    options = $.extend(options, o);
    if (typeof (options.action) == "string") {
        options.url = options.basePath + options.filename + "?lodge=" + (MFLLodgeIdent ? encodeURI(MFLLodgeIdent) : "") + "&action=" + encodeURI(options.action);
    }
    $("<div></div>", {
        "class": "ui-widget-overlay mfl-overlay-ajax"
    }).css(
    {
        opacity: 0.01
    }
    ).hide().appendTo("body").fadeIn(100);
    var ajaxOpt = {
        'url': options.url,
        'data': options.query,
        'type': options.type,
        'cache': false,
        'dataType': 'json',
        'success': function (jsonData) {
            jsonData = MFLAjax.globalJsonFilter(jsonData);
            $(".mfl-overlay-ajax").fadeOut(100, function () {
                $(".mfl-overlay-ajax").remove();
            });
            if (jsonData.success) {
                if (jsonData.message) {
                    MFLDialog("提示", jsonData.message, function () {
                        options.success(jsonData);
                    });
                } else {
                    options.success(jsonData);
                }
            } else {
                MFLDialog("抱歉，该操作未能执行", jsonData.message, function () {
                    if (options.failed) {
                        options.failed(jsonData);
                    }
                });
            }
        },
        'error': function (a, b, c) {
            if (options.failed) {
                options.failed();
            } else {
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
                        ajaxOpt.success(eval("obj=" + responseText + ";"));
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
            ajaxOpt.error("请求超时", "", "");
        };
        var timeOut = 60000;
        setTimeout(detect, 100);
    } else {
        $.ajax(ajaxOpt);
    }
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
        $(a).removeAttr("target");
    }
    if (isJs) {
        setTimeout(function () {
            $(a).attr("target", oldTarget);
        }, 100);
    }
};
String.prototype.replaceAll = function (reallyDo, replaceWith, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(reallyDo)) {
        return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi" : "g")), replaceWith);
    } else {
        return this.replace(reallyDo, replaceWith);
    }
}
