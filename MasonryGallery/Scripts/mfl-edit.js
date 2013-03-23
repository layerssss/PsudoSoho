(function ($) {
    $.fn.mflEdit = function (o) {
        o = $.extend({
            'callback': function () {
            },
            cancel: function () {
            },
            height: null,
            multiline: false,
            lengthLimitation: null,
            width: null
        }, o)
        this.each(function () {
            var root = this;
            var w = $(root).width();
            var h = $(root).height();
            var v = $(root).html().replaceAll('<br/>', '\r\n').replaceAll('<br>', '\r\n').replaceAll('<br />', '\r\n');
            $(root).text("");
            var b = $(o.multiline ? '<textarea></textarea>' : '<input type="text" />').appendTo(root).css('display', 'block');
            $(root).data("MFLEditTemp", v);
            b.val(o.dataKey ? $(root).data(o.dataKey) : v).width(o.width ? o.width : w).height(o.height ? o.height : h)
            //            commented in MG
            //            if (o.multiline) {
            //                o.autoTextarea();
            //            }

            b.focus();

            var btns = $('<div></div>').appendTo(root);
            if (!o.multiline) {
                btns.css({ "position": "absolute", "top": 2, 'right': 2, "vertical-align": "middle" });
            }

            $('<a href="#" class="mfl-btn mfl-edit-ok"></a>')
            //commented in MG
            //            .button({
            //                icons: {
            //                    primary: 'ui-icon-check'
            //                }
            //            })
            .appendTo(btns).click(function () {
                var v = $(b).val()
                if (o.dataKey) {
                    $(root).data(o.dataKey, v);
                } else {
                    $(root).text(v);
                }
                $(root).children().remove();
                $(root).text(v);
                $(root).html($(root).html().replaceAll('\n', '<br />'));
                $(root).unbind('click.MFLEditAoid');
                $.proxy(o.callback, root)();
                return false;
            });
            $('<a href="#" class="mfl-btn mfl-edit-cancel"></a>')
            //commented in MG
            //            .button({
            //                icons: {
            //                    primary: 'ui-icon-close'
            //                }
            //            })
            .appendTo(btns).click(function () {
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

/*

* @author 愚人码头

* 【jQuery插件】autoTextarea-文本框根据输入内容自适应高度

* 参数 maxHeight:null,//文本框是否自动撑高，默认：null，不自动撑高；如果自动撑高必须输入数值，该值作为文本框自动撑高的最大高度

* 参数 minHeight:$(this).height()//默认最小高度，也就是文本框最初的高度，当内容高度小于这个高度的时候，文本以这个高度显示

* 更多http://www.css88.com/archives/3948

*/

(function ($) {

    $.fn.autoTextarea = function (options) {

        var defaults = {

            maxHeight: 200, //文本框是否自动撑高，默认：null，不自动撑高；如果自动撑高必须输入数值，该值作为文本框自动撑高的最大高度

            minHeight: $(this).height(), //默认最小高度，也就是文本框最初的高度，当内容高度小于这个高度的时候，文本以这个高度显示
            live:false

        };

        var opts = $.extend({}, defaults, options);

        return $(this).each(function () {
            var func=function () {
                var height;
                var style = this.style;
                this.style.height = opts.minHeight + 'px';
                if (this.scrollHeight > opts.minHeight) {
                    if (opts.maxHeight && this.scrollHeight > opts.maxHeight) {
                        height = opts.maxHeight;
                        style.overflowY = 'scroll';
                    } else {
                        height = this.scrollHeight+opts.minHeight;
                        style.overflowY = 'hidden';
                    }
                    style.height = height + 'px';
                }
            };
            if(opts.live){
                $(this).live("paste cut keydown keyup focus blur",func);
            }else{
                $(this).bind("paste cut keydown keyup focus blur",func);
            }
        });
    };
})(jQuery);