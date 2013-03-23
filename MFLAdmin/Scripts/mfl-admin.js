var lodgeName;
$(function () {
    $('.roomAttributesIcon').easyTooltip();
    //查看具体特性
    $('.optionName').hide();
    $('.more').click(function () {
        var switchShow = function (e) {
            $(e).children().css({ 'display': 'block' });
            $(e).find('.optionName').show();
        };
        var switchHide = function (e) {
            $(e).children().css({ 'display': 'inline' });
            $(e).find('.optionName').hide();
        }
        var list = $(this).parent().siblings('.list-roomAttributes');
        $(list).closest('#admin-main').find('.list-roomAttributes.open').each(function (i, e) {
            var oh = $(e).height();
            switchHide(e);
            var h = $(e).height();
            switchShow(e);
            $(e).height(oh).animate({ 'height': h }, function () {
                $(e).css('height', 'auto').removeClass('open')
                switchHide(e);
            });
        });
        if ($(list).hasClass('open')) {
            return;
        }
        var oh = $(list).height();
        switchShow(list);
        var h = $(list).height();
        switchHide(list);
        $(list).height(oh).animate({ 'height': h }, function () {
            $(list).css('height', 'auto').addClass('open');
            switchShow(list);
        });
    });
    $('.dialog').hide();
    $('.dialog form').submit(function () {
        var dlg = $(this).closest('.dialog');
        data = $(this).serializeArray();
        MFLAjax({
            'basePath': '/',
            'query': data,
            'filename': 'State.aspx',
            'action': '更改入住状态',
            'success': function (json) {
                $(dlg).dialog('close');
                var full = false;
                var odlg = $('#admin-main .dialog:has([name="roomId"][value="' + $(dlg).find('[name="roomId"]').val() + '"])');
                for (var i in data) {
                    odlg.find('input[type!="radio"][name="' + data[i].name + '"]').attr('value', data[i].value);
                    odlg.find('textarea[name="' + data[i].name + '"]').html(data[i].value);
                    odlg.find('input[type="radio"][name="' + data[i].name + '"][value!="' + data[i].value + '"]').removeAttr('checked');
                    odlg.find('input[type="radio"][name="' + data[i].name + '"][value="' + data[i].value + '"]').attr('checked', 'checked');
                    if (data[i].name == 'state') {
                        full = data[i].value == 'full';
                    }
                    if (data[i].name == 'people') {
                        odlg.prev().find('.statusNum').text('入住人数：' + Number(data[i].value));
                    }
                }
                if (full) {
                    odlg.prev().removeClass('available');
                } else {
                    odlg.prev().addClass('available');
                }
                $('#datepicker').datepicker('option', 'onChangeMonthYear')();
                var countFull = 0;
                var countPeople = 0;
                var countEmpty = $('.room-list-item').each(function (i, e) {
                    countFull += $(e).hasClass('available') ? 0 : 1;
                    countPeople += Number($(e).find('.statusNum').text().substring(5));
                }).length - countFull;
                $('.countFull').text(String(countFull));
                $('.countPeople').text(String(countPeople));
                $('.countEmpty').text(String(countEmpty));
            },
            failed: function () {
                dlg.
                siblings('.ui-dialog-buttonpane')
                .find('.ui-button')
                .button('enable').siblings('.refreshBtn').remove();
            }
        });
        dlg.siblings('.ui-dialog-buttonpane')
        .children()
        .prepend('<span style="float:left;position:static;" class="refreshBtn refreshing"></span>')
        .find('.ui-button')
        .button('disable');
        return false;
    });
    $('.edit>.edit').each(function (i, e) {
        var odlg = $(this).closest('.room-list-item').next();
        $(e).click(function () {
            var dlg = $(odlg).clone(true).appendTo('body').dialog({
                zIndex: 20000,
                width: 400,
                buttons: {
                    "保存": function () {
                        $(this).find('form').trigger('submit');
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                },
                'resizable': false,
                'focus': function () {
                    $('.dialog').find('[id]').removeAttr('id');
                    $(this).find('[name]').each(function (i, e) {
                        $(e).attr('id', $(e).attr('name'));
                    });
                }
            });

            var people = $(dlg).find('[name="people"]');
            var func = function () {
                if ($(dlg).find('input[name="state"][value="full"]').attr('checked')) {
                    $(dlg).find('.state').prop("readonly", null).each(function (i, e) {
                        var d = $(e).data('MFLAdminOldValue');
                        if (d) {
                            $(e).val(d);
                        }
                    });
                    var d = Number(people.data('MFLAdminOldValue'));
                    if (people.val() == '') {
                        people.val(d ? d : 1);
                    }
                    $(dlg).find(".slider").slider('enable');
                } else {
                    $(dlg).find('.state').attr("readonly", "readonly").each(function (i, e) {
                        $(e).data('MFLAdminOldValue', $(e).val()).val('');
                    });
                    people.data('MFLAdminOldValue', people.val()).val('');
                    $(dlg).find(".slider").slider('disable');
                }
            }
            $(dlg).find('input[name="state"]').click(func);
            func();
            $(dlg).find(".slider").slider({
                min: 1,
                max: 10,
                disabled: !Number(people.val()),
                slide: function (event, ui) {
                    $(dlg).find("[name='people']").val(ui.value);
                },
                value: $(dlg).find("[name='people']").val()
            });
            return false;
        });
    });
    //人数slider

    $('.dialog').find('input,.ui-slider-handle').keypress(function (e) {
        if (e.keyCode == 13) {
            $(this).closest('.ui-dialog').find('.ui-dialog-buttonset>:first').trigger('click');
        }
    });

    $.datepicker.setDefaults($.datepicker.regional['zh-Cn']);
    $('#datepicker').datepicker({
        inline: true,
        'defaultDate': $('.today').text(),
        'onSelect': function (date, ui) {
            var str = String(location.pathname).toLowerCase();
            location.search = '?day=' + date + ((str.substring(str.length - 10) == 'login.aspx') ? '&lodge=' + lodgeName : '');
        },
        'onChangeMonthYear': function (year, month, ui) {
            if (year == undefined) {
                year = Number($('#datepicker .ui-datepicker-year').text());
                var mName = $('#datepicker').datepicker('option', 'monthNames');
                month = $('#datepicker .ui-datepicker-month').text();
                month = $.map(mName, function (e, i) {
                    return e == month ? i : null;
                })[0] + 1;
            }
            $('#datepicker>.numberFull').remove();
            $('.refreshBtn').addClass('refreshing_black');
            setTimeout(function () {
                var callback = function (text) {
                    $('#datepicker>.numberFull').remove();
                    text = text.split('\r\n');
                    var count = 0;
                    $('#datepicker').find('td').each(function (i, e) {
                        var offset = $(e).offset();
                        var base = $('#datepicker').offset();
                        var day = Number($(e).children().text());
                        if (day) {
                            $('<span></span>', {
                                'class': 'numberFull',
                                'style': 'z-index:100;position:absolute;left:'
                        + (offset.left - base.left) + 'px;top:'
                        + (offset.top - base.top) + 'px;'
                            }).text((text[day] == undefined ? '?' : text[day - 1]) + '').appendTo('#datepicker');
                        }
                    });
                    $('.refreshBtn').removeClass('refreshing_black');
                };
                $.ajax({
                    dataType: 'html',
                    cache: false,
                    url: '/State.aspx?lodge=' + lodgeName + '&year=' + year + '&month=' + month,
                    success: callback,
                    error: function () {
                        callback("");
                    }
                });
            }, 30);
        }
    }).css('position', 'relative').datepicker('option', 'onChangeMonthYear')();
    $('<a href="#" onclick="return false;" class="refreshBtn black" title="刷新"></a>').appendTo('#datepicker').click(function () {
        $('#datepicker').datepicker('option', 'onChangeMonthYear')();
    });
});