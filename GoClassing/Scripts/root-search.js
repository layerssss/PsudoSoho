
$(function () {
    $(".searchText").parent().mflSubmit({
        submit: function () {
            var s = $.map($(".searchText").val().split(' '), function (i, v) {
                return i == "" ? null : i;
            }).join(',');
            if (s.length > 0) {
                if (searchTimer) {
                    clearInterval(searchTimer);
                    searchTimer = null;
                }
                searchTimer = setInterval(function () {
                    $('.search').position({
                        of: $(".searchText"),
                        my: 'left top',
                        at: 'left bottom',
                        collision: 'none none',
                        offset: '0 0'
                    }).show()
                }, 30);
                MFLAjax({
                    action: "QuickSearch",
                    query: {
                        page: 0,
                        search: s
                    },
                    loadingJ: $('.searchCtypesList'),
                    success: function (json, o) {
                        MFLList({
                            containerJ: $('.searchCtypesList'),
                            pagerAjaxOptions: o,
                            pagerHolderJ: $('.search .mfl-list-pages'),
                            json: json
                        });
                        MFLList({
                            autoEmpty: false,
                            containerJ: $('.searchFriendsList'),
                            json: json.friends
                        });
                        MFLList({
                            autoEmpty: false,
                            containerJ: $('.searchCoursesList'),
                            json: json.courses
                        });
                    }
                });
                $('.search>p>a.ftype-course').text('查看所有名称包含“' + s + '”的课程...').attr('href', '/C/All/#' + s);
                $('.search>p>a.ftype-friend').text('查看所有名称包含“' + s + '”的用户...').attr('href', '/T/All/#' + s);
            }
            $(".searchText").focus();
        },
        namespace: "search"
    });
    $(".searchClose").button({
        icons: {
            primary: "ui-icon-close"
        }
    }).click(function () {
        clearInterval(searchTimer);
        searchTimer = null;
        $('.search').hide();
        return false;
    });
});
var searchTimer;