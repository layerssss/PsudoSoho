(function ($) {
    $.fn.roomQuery = function (option) {
        option = $.extend({
            numberOfMonths: 1,
            baseUrl: 'http://lodge.myfamilylodge.com/',
            rooms: [],
            juiCssPath: 'assets/shared/css/custom-theme/jquery-ui-1.8.16.custom.css',
            juiJsPath: 'assets/shared/js/jquery-ui-1.8.16.custom.min.js',
            tooltipTheme:'darkness',
            calenderTheme:'lightness',
            datepickerOption: {},
            generatingRoomName: function (room) {
                return $('<span>' + room.roomName + '</span>');
            }
        }, option);
        var roots = this;
        var loadJuiCallBack = function () {
            $.each(roots, function (i, root) {
                $(root).addClass('rq-widget');
                var dp = $(root).find('.rq-datepicker');
                var c = $(root).find('.rq-container');
                var rqc=c;
                var rqdate=$(root).find('.rq-date');
                var rqsum=$(root).find('.rq-sum');
                var editable=false;
                if(dp[0]==rqdate[0]){
                    dp=$('<input type="button" class="rq-datepicker-edit"/>').insertAfter(dp);
                    dp.val($.datepicker.formatDate('yymmdd', new Date()));
                    editable=true;
                }
                var monthJson = new Object();
                var dpwidget=dp.datepicker($.extend($.extend({
                    numberOfMonths: 1,
                    inline: true,
                }, option.datepickerOption), {
                    dateFormat: 'yymmdd',
                    onSelect: function (dateText, inst) {
                        var monthText = dateText.substr(0, 6);
                        $.ajax({
                            dateType: 'json',
                            url: monthText + ".js",
                            cache: false,
                            success: function (json) {
                                var day = json['day' + Number(dateText.substr(6))];
                                $.each(option.rooms, function (i, room) {
                                    var tr = c.find('.rq-room' + room.roomId);
                                    if (!day['room' + room.roomId]) {
                                        tr.addClass('available');
                                    } else {
                                        tr.removeClass('available');
                                    }
                                });
                                if(day.sum){
                                    rqsum.text(day.sum+'个房间可预订');
                                    rqsum.addClass('rq-availableSum');
                                }else{
                                    rqsum.text('无可预订房间');
                                }
                            },
                            complete: function () {
                                c.find('.rq-room').removeClass('loading');
                            },
                            mimeType: 'text/json',
                            error: function (jqXHR, textStatus, errorThrown) {
                                c.find('.rq-room').addClass('error');
                                var count=0;
                                for(var i in option.rooms){
                                count++;
                                }
                                    rqsum.text(count+'个房间可预订');
                                    rqsum.addClass('rq-availableSum');
                            }
                        });
                        rqsum.removeClass('rq-availableSum');
                        c.find('.rq-room').removeClass('available error').addClass('loading');
                        rqdate.text($.datepicker.formatDate('yy年 MM dd日 D', dp.datepicker('getDate')));
                    },
                    onChangeMonthYear: function () {
                        setTimeout(function () {
                            $('.ui-datepicker-calendar:has("td.loading")').each(function (i, e) {
                                var year = $(e).prev().find('.ui-datepicker-year').text();
                                var mName = dp.datepicker('option', 'monthNames');
                                var month = $(e).prev().find('.ui-datepicker-month').text();
                                month = $.map(mName, function (e, i) {
                                    return e == month ? i : null;
                                })[0] + 1;
                                month = String(month);
                                var monthText = year + (month.length == 1 ? '0' + month : month);
                                $.ajax({
                                    dateType: 'json',
                                    url: monthText + ".js",
                                    cache: false,
                                    success: function (json) {
                                        $(e).find('td>a').each(function (i, e) {
                                            var sum=json['day' + $(e).text()].sum;
                                            if (sum) {
                                                $(e).parent().addClass('available');
                                            }
                                            $(e).parent().attr('title',sum+'间可预订房间')
                                        });
                                        monthJson['month' + monthText] = json;
                                    },
                                    complete: function () {
                                        $(e).find('td:has(a)').removeClass('loading');
                                    },
                                    mimeType: 'text/json',
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        $(e).find('td:has(a)').addClass('error');
                                        monthJson['month' + monthText] = 0;
                                    }
                                });
                            });
                        }, 100);
                    },
                    beforeShowDay: function (date) {
                        var monthStr = $.datepicker.formatDate('yymm', date);
                        var day = Number($.datepicker.formatDate('dd', date));
                        return [true,
                        monthJson['month' + monthStr] != undefined ?
                            (monthJson['month' + monthStr] == 0 ? 'error' :
                            (monthJson['month' + monthStr]['day' + day].sum ? 'available' : '')) :
                            'loading',
                        '正在读取...'];
                    },
                    beforeShow:function(){
                        dp.datepicker('option', 'onChangeMonthYear')();
                    },
                    showOn: "button",
			        buttonImage: option.baseUrl+"assets/lodge/img/calendar_"+option.calenderTheme+".png",
			        buttonImageOnly: true
                })).datepicker('widget').addClass('rq-datepicker');
                $.each(option.rooms, function (i, room) {
                    if (!room.roomAttributesStringForRoomQuery) {
                        alert("模版错误：您只能在Reservation.htm上使用该控件。");
                        return false;
                    }
                    var tr = $('<tr class="rq-room rq-room' + room.roomId + '"><td class="rq-roomName"></td>'
                    + room.roomAttributesStringForRoomQuery + '</tr>');
                    tr.appendTo(c).find('.rq-roomName')
                    .append(option.generatingRoomName(room));
                    tr.find('.rq-roomAttribute>img').easyTooltip({
                        tooltipId:'easyTooltip-'+option.tooltipTheme
                    });
                });
                dp.datepicker('option', 'onSelect')($.datepicker.formatDate('yymmdd', new Date()));
                dp.datepicker('option', 'onChangeMonthYear')();
            });
        };
        if ($.isFunction($.fn.dialog)) {
            loadJuiCallBack();
        } else {
            $('<link rel="stylesheet" href="' + option.baseUrl + option.juiCssPath + '" type="text/css" media="screen">')
            .appendTo('head');
            $('<link rel="stylesheet" href="' + option.baseUrl + 'assets/lodge/css/roomQuery.css" type="text/css" media="screen">')
            .appendTo('head');
            $.getScript(option.baseUrl + 'assets/shared/js/easytooltip.js', function () {
                $.getScript('http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.min.js', function () {
                    loadJuiCallBack();
                });
            });
        }
    };
})(jQuery);