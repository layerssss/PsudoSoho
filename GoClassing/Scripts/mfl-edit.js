(function ($) {
    $.fn.mflEdit = function (o) {
        o = $.extend({
            'callback': function () {
            },
            cancel: function () {
            },
            height: null,
            multiline: false,
            lengthLimitation: null
        }, o)
        this.each(function () {
            var root = this;
            var w = $(root).width();
            var h = $(root).height();
            var v = $(root).text();
            $(root).text("");
            var b = $(o.multiline ? '<textarea></textarea>' : '<input type="text" />').appendTo(root);
            $(root).data("MFLEditTemp", v);
            b.val(o.dataKey ? $(root).data(o.dataKey) : v).width(o.width ? o.width : w - 10).height(o.height ? o.height : h - (o.multiline ? 10 : 0)).focus();
            var btns = $('<div></div>').appendTo(root);
            if (!o.multiline) {
                btns.css({ "float": "right", "vertical-align": "middle" });
            }

            $('<a href="#" class="mfl-btn mfl-edit-ok"></a>').button({
                icons: {
                    primary: 'ui-icon-check'
                }
            }).appendTo(btns).click(function () {
                var v = $(b).val()
                if (o.dataKey) {
                    $(root).data(o.dataKey, v);
                } else {
                    $(root).text(v);
                }
                $(root).children().remove();
                $(root).text(v);
                $(root).html($(root).html().replaceAll('\n', '<br />'));
                $.proxy(o.callback, root)();
                return false;
            });
            $('<a href="#" class="mfl-btn mfl-edit-cancel"></a>').button({
                icons: {
                    primary: 'ui-icon-close'
                }
            }).appendTo(btns).click(function () {
                $(root).children().remove();
                $(root).text(o.dataKey ? $(root).data(o.dataKey) : $(root).data("MFLEditTemp"));
                $(root).html($(root).html().replaceAll('\n', '<br />'));
                $.proxy(o.cancel, root)();
                return false;
            });
            if ($(root).mflSubmit) {
                $(root).mflSubmit({
                    submit: function () {
                        btns.children(".mfl-edit-ok").trigger("click");
                    }
                });
            }
            if (o.lengthLimitation) {
                var l = $('<span style=""></span>').appendTo(btns);
                b.keypress(function () {
                    setTimeout(function () {
                        if ($(b).val().length >= o.lengthLimitation) {
                            $(b).val($(b).val().substr(0, o.lengthLimitation));
                        }
                        l.text($(b).val().length + "/" + o.lengthLimitation);
                    }, 100);
                });
                b.keypress();
            }
        });
    };
})(jQuery);