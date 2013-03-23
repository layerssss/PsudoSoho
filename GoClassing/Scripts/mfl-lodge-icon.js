(function ($) {
    $.fn.lodgeIcon = function (o) {
        var options = {
            'text': '点击更改',
            'action': '获取图标',
            'fieldIcon': 'icon',
            'fieldText': 'text',
            'fieldValue': 'icon',
            'showText': false,
            'collapsable': true,
            'cssClassImg': 'mfl-src-optionIcon',
            'cssClassA': 'mfl-lodge-option',
            'diaplaySelected': true,
            'autoSelected': false,
            'jsonSelector': function (json) {
                return json;
            },
            'gettingIcon': function (root) {
                return $(root).val();
            },
            'gettingText': function () {
                return "点击更改";
            },
            'selected': function (selectedJ) {
            },
            listFilter: function (item) {
                return true;
            }
        };

        options = $.extend(options, o);
        this.each(function () {
            var root = this;
            $(root).next(".mfl-lodge-iconsBtn").remove();
            $(root).next(".mfl-lodge-icons").remove();

            var a = $("<a></a>", {
                "href": "#",
                "class": "mfl-lodge-iconsBtn",
                "title": options.gettingText(this),
                "alt": options.gettingText(this)
            }).append($("<img />", {
                "alt": options.text,
                "src": options.gettingIcon(this),
                "class": options.cssClassImg
            }))
            .click(function () {
                return false;
            })
            .insertAfter(this)
            .focus(function () {
                if ($(this).next().hasClass("mfl-lodge-icons")) {
                    return;
                }
                var icons = $('<div class="mfl-lodge-icons"><span class="mfl-list-itemTemplate"><a href="#" class="mfl-lodge-icon ' + options.cssClassA + ' mfl-alt-' + options.fieldText + '" onclick="return false;"><img class="mfl-src-' + options.fieldIcon + ' mfl-alt-' + options.fieldText + ' mfl-data-' + options.fieldValue + ' ' + options.cssClassImg + '" /></a></span></div>')
                .insertAfter(this);
                if (options.showText) {
                    $('<span class="mfl-' + options.fieldText + '"></span>').appendTo(icons.find("a"));
                }
                MFLAjax({
                    url: options.url,
                    action: options.action,
                    query: options.query,
                    loadingJ: icons,
                    success: function (json) {
                        MFLList({
                            containerJ: icons,
                            json: options.jsonSelector(json),
                            filter: options.listFilter
                        });
                        icons.find(".mfl-list-item a")
                        .mousedown(function () {
                            $(this).parent().siblings().children("a").removeClass("mfl-lodge-icon-selected");
                            $(this).addClass("mfl-lodge-icon-selected");
                            var newValue = $(this).children("img").attr("src");
                            $(root).val($(this).children("img").data("MFLData-" + options.fieldValue));
                            $(root).next().children().attr("src", newValue);
                            options.selected(root);
                        })
                        .filter(function () {
                            return $(this).children("img").data("MFLData-" + options.fieldValue) == $(root).val();
                        })
                        .addClass("mfl-lodge-icon-selected");
                        $('<div style="width:1px;"></div>').appendTo(icons);
                    }
                });
            });
            if (!options.diaplaySelected) {
                a.hide();
            }
            if (options.collapsable) {
                a.blur(function () {
                    $(root).next().next().remove();
                });
            } else {
                a.trigger("focus");
            }
            if (options.autoSelected) {
                options.selected(root);
            }
            $(root).unbind("blur.lodgeIcon").unbind("focus.lodgeIcon");
            $(root).bind("blur.lodgeIcon", function () {
                a.triggerHandler("blur");
            }).bind("focus.lodgeIcon", function () {
                a.triggerHandler("focus");
            });
        });
        return this;
    };
})(jQuery);