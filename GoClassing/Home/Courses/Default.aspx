<%@ Page Title="我的课程" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="GoClassing.Home.Courses.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <script src="/Scripts/mfl-lodge-icon.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ctypesLoading = [null, null];
        var feedsLast = -1;
        $(function () {
            MFLAjax({
                "action": "GetCtypes1",
                loadingJ: $(".types1"),
                success: function (json) {
                    MFLList({
                        json: json,
                        containerJ: $(".types1")
                    });
                    $(".types1").parent().selectable({
                        filter: ".mfl-list-item",
                        stop: function (event, ui) {
                            var arr = $.makeArray($(".types1").find(".ui-selected").map(function () { return $(this).children().text(); }));
                            $(".types1").prev().children(".infotag").remove();
                            if (!arr.length) {
                                $(".types1").prev().prepend('<span class="ftype ftype-info infotag">请选择课程类别</span>');
                            }
                            MFLAjax({
                                "action": "GetCtypes2",
                                loadingJ: $(".types2"),
                                query: {
                                    ctypes1: arr.join(',')
                                },
                                success: function (json, o) {
                                    MFLList({
                                        json: json,
                                        containerJ: $(".types2")
                                    });
                                    $(".types2").parent().selectable({
                                        filter: ".mfl-list-item",
                                        stop: function (event, ui) {
                                            var arr = $.makeArray($(".types2").find(".ui-selected").map(function () { return $(this).children().text(); }));
                                            $(".types2").prev().children(".infotag").remove();
                                            if (!arr.length) {
                                                $(".types2").prev().prepend('<span class="ftype ftype-info infotag">请选择一级分类</span>');
                                            }
                                            resetFeeds();
                                        }
                                    });
                                    var items = $(".types2 .mfl-list-item");
                                    if (ctypesLoading[1]) {
                                        items = items.filter(':has(:contains("' + ctypesLoading[1] + '"))');
                                        ctypesLoading[1] = false;
                                    }
                                    items.addClass("ui-selected");
                                    $(".types2").parent().selectable("option", "stop")();
                                }
                            });
                        }
                    });
                    var items = $(".types1 .mfl-list-item");
                    if (ctypesLoading[0]) {
                        items = items.filter(':has(:contains("' + ctypesLoading[0] + '"))');
                        ctypesLoading[0] = false;
                    }
                    items.addClass("ui-selected");
                    $(".types1").parent().selectable("option", "stop")();
                }
            });
            $(".createCourse").hide();
            $(".createCourseBtn").click(function () {
                $(".createCourse").slideToggle();
                return false;
            });
            $(".courseCancel").click(function () {
                $(".createCourse").slideToggle();
            });
            $(".createCourse").mflSubmit({
                namespace: "createCourse",
                submit: function () {
                    $(".courseCreate").trigger("click");
                }
            });
            $(".courseCreate").click(function () {
                MFLAjax({
                    validationFormJ: $(".createCourse"),
                    action: "创建课程",
                    query: {
                        cname: $("#cname").val(),
                        ctype1: $("#ctype1").val(),
                        ctype2: $("#ctype2").val()
                    },
                    success: function (json) {
                        $(".createCourse").slideToggle();
                        location.href = "/C/" + json.showId;
                    },
                    loadingJ: $(".courseCreate")
                });
                return false;
            });
            $("#ctype1").lodgeIcon({
                action: "GetAllCtypes1",
                fieldText: "type",
                fieldValue: "type",
                fieldIcon: "icon",
                showText: true,
                cssClassA: "ftype-ctype1 ftype",
                'diaplaySelected': false,
                listFilter: function (item) {
                    item.icon = "/Styles/blank.gif";
                    return true;
                },
                gettingIcon: function (h) {
                    return "/Styles/blank.gif";
                },
                selected: function (root) {
                    $("#ctype2").lodgeIcon({
                        action: "GetAllCtypes2",
                        query: { ctypes1: $(root).val() },
                        fieldText: "type",
                        fieldValue: "type",
                        fieldIcon: "icon",
                        showText: true,
                        cssClassA: "ftype-ctype2 ftype",
                        'diaplaySelected': false,
                        listFilter: function (item) {
                            item.icon = "/Styles/blank.gif";
                            return true;
                        },
                        gettingIcon: function () {
                            return "/Styles/blank.gif";
                        },
                        selected: function (root) {
                        }
                    });
                }
            });

        });
        var resetFeeds = function () {
            feedsLast = -1;
            MFLAjax({
                "action": "GetMyCourses",
                loadingJ: $("#myCourses"),
                "query": {
                    page:0,
                    ctypes1: $.makeArray($(".types1").find(".ui-selected").map(function () { return $(this).children().text(); })).join(','),
                    ctypes2: $.makeArray($(".types2").find(".ui-selected").map(function () { return $(this).children().text(); })).join(',')
                },
                "success": function (json, o) {
                    MFLList({
                        json: json.courses,
                        containerJ: $("#myCourses"),
                        pagerAjaxOptions: o,
                        pagerHolderJ: $("#myCourses").parent().find(".mfl-list-pages")
                    });
                }
            });
            MFLAjax({
                "action": "GetMyPaticipatedCourses",
                loadingJ: $("#myPaticipatedCourses"),
                "query": {
                    page: 0,
                    ctypes1: $.makeArray($(".types1").find(".ui-selected").map(function () { return $(this).children().text(); })).join(','),
                    ctypes2: $.makeArray($(".types2").find(".ui-selected").map(function () { return $(this).children().text(); })).join(',')
                },
                "success": function (json, o) {
                    MFLList({
                        json: json.paticipatedCourses,
                        containerJ: $("#myPaticipatedCourses"),
                        pagerAjaxOptions: o,
                        pagerHolderJ: $("#myPaticipatedCourses").parent().find(".mfl-list-pages")
                    });
                }
            });
        }
    </script>
    <link href="/Styles/root-courses.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="right" runat="server">
    <div> 
        <h3 class="links-header">
            操作</h3>
        <div class="links-content">
            <a href="#" class="ftype createCourseBtn" style="background-position: 0 -1199px;">创建一个课程</a>
            <a href="/C/All/" class="ftype" style="background-position: 0 -1154px;">
                加入一个课程</a>
        </div>
    </div>
    <div>
        <h3 class="links-header">
            课程类别(多选)</h3>
        <div class="links-content types1 types">
            <div class="mfl-list-itemTemplate">
                <div class="mfl-type ftype" style="background-position: 0 -830px;">
                    类型</div>
            </div>
            <div class="mfl-list-empty">
                没有可以显示的条目</div>
        </div>
    </div>
    <div>
        <h3 class="links-header">
            课程主题(多选)</h3>
        <div class="links-content types2 types">
            <div class="mfl-list-itemTemplate">
                <div class="mfl-type ftype" style="background-position: 0 -1130px;">
                    学科</div>
            </div>
            <div class="mfl-list-empty">
                没有可以显示的条目</div>
        </div>
    </div>
    <div class="info">
    提示：拖动鼠标可以在标注“多选”的分类中一次选择多个类别。
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="createCourse">
    <h3>创建一个课程</h3>
        <label for="cname">
            课程名称</label>：<br />
        <input type="text" id="cname" /><br />
        <label for="ctype1">
            课程所属类别</label>：<br />
        <input type="text" readonly="readonly" id="ctype1" /><br />
        <label for="ctype2">
            课程所属主题</label>：<br />
        <input type="text" readonly="readonly" id="ctype2" /><br />
        <input type="button" class="courseCreate" value="创建" />
        <input type="button" class="courseCancel" value="取消" />
    </div>
    <div class="courses-list">
        <h3>
            <div class="mfl-list-pages floatright">
            </div>
            我正在教的课程</h3>
        <div id="myCourses">
            <div class="mfl-list-itemTemplate">
                        <div>
                            <div class="teacher">
                                <a href="/T/$teacherUsername$"><img src="/Avatars/$teacherId$.small.jpg" /></a>
                                <span class="mfl-tags-teacherCoop"><a href="/$teacherCoop$/" title="$teacherCoop$"><img alt="$teacherCoop$" title="$teacherCoop$" src="/Styles/Tags/$teacherCoop$.gif" /></a></span>
                                <span class="mfl-teacherTruename">$teacherTruename$</span> </div>
                                <a href="/c/$showId$"
                                    class="ftype-course ftype mfl-name">$name$</a></div>
                        <a class="mfl-ctype2 ftype-ctype2 ftype ctype" href="/C/$ctype1$/$ctype2$/">$ctype2$</a> 
                        <a class="mfl-ctype1 ftype-ctype1 ftype ctype" href="/C/$ctype1$/">$ctype1$</a>
                        <span class="ftype-friend ftype mfl-numMembers">$numMembers$</span>
            </div>
            <div class="mfl-list-empty">
                没有可以显示的条目</div>
        </div>
        <div class="mfl-list-pages"></div>
    </div>
    <div class="courses-list">
        <h3>
            <div class="mfl-list-pages floatright">
            </div>
            我正在学习的课程</h3>
        <div id="myPaticipatedCourses">
            <div class="mfl-list-itemTemplate">
                
                        <div>
                            <div class="teacher">
                                <a href="/T/$teacherUsername$"><img src="/Avatars/$teacherId$.small.jpg" /></a>
                                <span class="mfl-tags-teacherCoop"><a href="/$teacherCoop$/" title="$teacherCoop$"><img alt="$teacherCoop$" title="$teacherCoop$" src="/Styles/Tags/$teacherCoop$.gif" /></a></span>
                                <span class="mfl-teacherTruename">$teacherTruename$</span> </div>
                                <a href="/c/$showId$"
                                    class="ftype-course ftype mfl-name">$name$</a></div>
                        <a class="mfl-ctype2 ftype-ctype2 ftype ctype" href="/C/$ctype1$/$ctype2$/">$ctype2$</a> 
                        <a class="mfl-ctype1 ftype-ctype1 ftype ctype" href="/C/$ctype1$/">$ctype1$</a>
                        <span class="ftype-friend ftype mfl-numMembers">$numMembers$</span>
            </div>
            <div class="mfl-list-empty">
                没有可以显示的条目</div>
        </div>
        <div class="mfl-list-pages"></div>
    </div>
</asp:Content>
