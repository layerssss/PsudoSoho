var MFLForm = function (options) {
    var o = {
        wrappedUp: true,
        submit: function () {
            return false;
        },
        items: []
    };
    o = $.extend(o, options);

    var table = $('<table></table>');
    $.each(o.items, function (i, field) {
        var line = $(document.createElement("tr"));
        var input = $(document.createElement("input"));
        if (field.hidden) {
            line.hide();
        }
        switch (field.type) {
            case 'checkbox':
                input.attr("type", field.type);
                if (field.value == true) {
                    input.attr('checked', 'checked');
                }
                break;
            default:
                input.attr("type", field.type);
                input.attr('value', field.value);
                if (field.events) {
                    $.each(field.events, function (eventName, callback) {
                        input.bind(eventName, null, callback);
                    });
                }
                break;
        }
        input.attr('id', field.name);
        $(document.createElement("span")).html(field.text).appendTo($(document.createElement("td")).width(270).appendTo(line).css("padding", 5).css('text-align', 'right'));
        input.appendTo($(document.createElement("td")).appendTo(line).css("padding", 5));
        field.jLabel = $(document.createElement("span")).appendTo($(document.createElement("td")).appendTo(line).css({ "padding": 5, "display": "block" })).addClass("ui-state-error").css({ "padding": 5 }).hide();
        line.appendTo(table);
        field.jInput = input;

        if (field.type == 'slider') {
            field.jInput.attr('readonly', 'readonly');
            $('<div></div>').slider(field.option).insertAfter(field.jInput).bind('slide', function (e, u) {
                if (String(Number(field.jInput.val())) != 'NaN') {
                    field.jInput.val(u.value);
                }
            }).slider('option', 'value', Number(field.value));
        }
        if (field.events) {
            $.each(field.events, function (ename, efunc) {
                $(field.jInput).bind(ename, efunc);
                if (field.type == 'slider') {
                    field.jInput.next().bind(ename, efunc);
                }
            });
        }
    });
    table = $('<form action="#" method="post"><input type="submit" style="float: right; border: medium none; background: none repeat scroll 0% 0% transparent; visibility: visible; width: 5px; height: 5px; left: -10px; margin: 0pt; padding: 0pt; overflow: hidden;" name="S1"></form>').append(table);
    var okBtnFunc = function () {
        if (o.submit) {
            $.proxy(o.submit, table[0])(data);
        }
    };
    table.removeData("MFLFormData");
    table.data("MFLFormData", function () {
        var data = new Object();
        var allSet = true;
        $.each(o.items, function (i, field) {
            switch (field.type) {
                case "checkbox":
                    data[field.name] = field.jInput.attr("checked") == "checked" ? true : false;
                    break;
                default:
                    data[field.name] = field.jInput.val();
            }
        });
        if (!allSet) {
            return false;
        }
        return data;
    });
    table.submit(okBtnFunc);
    return table;
};
