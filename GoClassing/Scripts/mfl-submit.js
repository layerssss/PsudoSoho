(function ($) {
    $.fn.mflSubmit = function (o) {
        var options = {
            'submit': function () {
            },
            formJ: $("#form1")
        };
        options = $.extend(options, o);
        this.each(function () {
            var root = this;
            $(root).find("input").focus(function () {
                options.formJ.unbind("submit.mflsubmit");
                options.formJ.bind("submit.mflsubmit", function () {
                    options.submit();
                    return false;
                });
            });
            //            options.formJ.find("input").filter(function () {
            //                return !$.contains(root, this);
            //            }).focus(function () {
            //                options.formJ.unbind("submit.mflsubmit" + options.namespace, submitFunc);
            //            });
        });
    };
})(jQuery);