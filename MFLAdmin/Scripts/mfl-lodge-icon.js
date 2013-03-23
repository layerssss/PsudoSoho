(function ($) {
    $.fn.lodgeIcon = function (o) {
        var options = {
            'text': '点击更改',
            'action': '获取图标',
            'fieldIcon': 'icon',
            'fieldText': 'text',
            'fieldValue': 'icon',
            'showText': false,
            'spirit': false,
            'collapsable': true,
            'cssClassImg': 'mfl-src-optionIcon',
            'cssClassIcons': 'mfl-icons',
            'cssClassSpan': '',
            'cssClassA': '',
            'diaplaySelected': true,
            'autoSelected': false,
            'gettingIcon': function (root) {
                return $(root).val();
            },
            'gettingText': function () {
                return "点击更改";
            },
            'selected': function (selectedJ) {
            }
        };

        options = $.extend(options, o);
        this.each(function () {
            if ($(root).data("MFLLodgeIcon")) {
                return;
            }
            $(root).data("MFLLodgeIcon", true);
            var root = this;
            var aFocusFun = function () {//added
                if ($(this).next().hasClass("mfl-lodge-icons")) {
                    return;
                }
                var icons = $('<div class="mfl-lodge-icons ' + options.cssClassIcons + '"><span class="mfl-list-itemTemplate"><a href="#" class="mfl-lodge-icon mfl-alt-' + options.fieldText + ' ' +
                options.cssClassA + '" onclick="return false;"><img src="/Style/blank.gif" class="mfl-' + (options.spirit ? 'style' : 'src') + '-' + options.fieldIcon + ' mfl-alt-' + options.fieldText + ' mfl-data-' + options.fieldValue + ' ' + options.cssClassImg + '" /></a></span></div>')
                .insertAfter(this);
                if (options.showText) {
                    $('<span class="mfl-' + options.fieldText + ' ' + options.cssClassSpan + '"></span>').appendTo(icons.find("a"));
                }
                MFLList({
                    url: options.url,
                    containerJ: icons,
                    action: options.action,
                    query: options.query,
                    callback: function (json) {
                        icons.find(".mfl-list-item a")
                        .mousedown(function () {
                            $(this).parent().siblings().children("a").removeClass("mfl-lodge-icon-selected");
                            $(this).addClass("mfl-lodge-icon-selected");
                            var newValue = options.spirit ? $(this).children("img").attr("style") : $(this).children("img").attr("src");
                            var newTitle = $(this).children("img").attr("title");
                            $(root).val($(this).children("img").data("MFLData-" + options.fieldValue));
                            if (options.spirit) {
                                $(root).next().children().attr("style", newValue);
                            } else {
                                $(root).next().children().attr("src", newValue);
                            }
                            options.selected(root);
                            if (text) {
                                text.text(newTitle);
                            }
                            if (options.collapsable) {
                                $(".mfl-lodge-icons").hide(500, function () {//added
                                    $(this).remove();
                                });
                            }
                        })
                        .filter(function () {
                            return $(this).children("img").data("MFLData-" + options.fieldValue) == $(root).val();
                        })
                        .addClass("mfl-lodge-icon-selected");
                    }
                });
            }
            var a = $("<a></a>", {
                "href": "#",
                "title": options.gettingText(this),
                "alt": options.gettingText(this)
            }).append($("<img />", {
                "alt": options.text,
                "src": options.spirit ? "/Style/blank.gif" : options.gettingIcon(root),
                "class": options.cssClassImg,
                "style": (options.spirit ? options.gettingIcon(root) : "")
            }))
            .click(aFocusFun)//added
            .click(function () {
                return false;
            })
            .insertAfter(this)
            .focus(aFocusFun); //edited
            if (!options.diaplaySelected) {
                a.hide();
            }
            var text;
            if (options.showText) {
                text = $('<span class="mfl-lodge-icon-textWrapper"><span class="mfl-' + options.fieldText + ' ' + options.cssClassSpan + '"></span></span>').insertBefore(root).children('span').text(options.gettingText(root));
            }
            if (options.collapsable) {
                a.blur(function () {
                    $(root).next().next().hide(500, function () {
                        $(root).next().next().remove(".mfl-lodge-icons"); //edited
                    });
                });
            } else {
                a.trigger("focus");
            }
            if (options.autoSelected) {
                options.selected(root);
            }
        });
    };
})(jQuery);