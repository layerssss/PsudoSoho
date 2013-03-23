<%@ Page Title="所有课程" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoClassing.C.All.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <script type="text/javascript">
        var ctypesLoading = [];
        $(function () {
            if (!ctypesLoading[0]) {
            }
            var feedsLast = -1;
            MFLAjax({
                "action": "GetAllCtypes1",
                loadingJ: $(".types1"),
                success: function (json, o) {
                    MFLList({
                        json: json,
                        containerJ: $(".types1")
                    });
                    $(".types1").prev().children(".infotag").remove();
                    $(".types1").prev().prepend('<span class="ftype ftype-info infotag">请选择课程类别</span>');
                    $(".types1 .mfl-list-item").click(function () {
                        $(".types1").prev().children(".infotag").remove();
                        $(".types1 .mfl-list-item").removeClass("ui-selected");
                        $(this).addClass("ui-selected");
                        MFLAjax({
                            "action": "GetAllCtypes2",
                            loadingJ: $(".types2"),
                            query: {
                                ctypes1: $.trim($(this).text())
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
                                    items.addClass("ui-selected");
                                } else {
                                }
                                $(".types2").parent().selectable("option", "stop")();
                                //                                items.addClass("ui-selected");
                                //                                $(".types2").parent().selectable("option", "stop")();
                            }
                        });
                    });
                    var items = $(".types1 .mfl-list-item");
                    if (ctypesLoading[0]) {
                        items = items.filter(':has(:contains("' + ctypesLoading[0] + '"))');
                        ctypesLoading[0] = false;
                        items.trigger("click");
                    } else {
                        resetFeeds();
                    }
                }
            });

        });
        var searchStr = "";
        gcSearch = function (s) {
            searchStr = s; 
            resetFeeds();
        }
        var firstReset = true;
        var resetFeeds = function () {
            if (!firstReset) {
                $(".ctypes-list").hide();
            } else {
                firstReset = false;
                if (!location.hash.substr(1).length) {
                    return;
                } else {
                    $(".ctypes-list").hide();
                }
            }
            if (location.hash.substr(1).length) {
                $('.courses-list>h3>span').text("名称中包含“" + location.hash.substr(1) + "”的课程列表");
            } else {
                $('.courses-list>h3>span').text("所有课程列表");
            }
            feedsLast = -1;
            MFLAjax({
                "action": "GetAllCourses",
                loadingJ: $("#courses"),
                "query": {
                    filter: location.hash.substr(1),
                    page: 0,
                    ctype1: $(".types1").find(".ui-selected").children().text(),
                    ctypes2: $.makeArray($(".types2").find(".ui-selected").map(function () { return $(this).children().text(); })).join(',')
                },
                "success": function (json, o) {
                    MFLList({
                        json: json.courses,
                        containerJ: $("#courses"),
                        pagerAjaxOptions: o,
                        pagerHolderJ: $("#courses").parent().find(".mfl-list-pages")
                    });
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="right" runat="server">
    <div>
        <h3 class="links-header">
            操作</h3>
        <div class="links-content">
            <a href="/Home/Courses/?createCourse=true" class="ftype createCourseBtn" style="background-position: 0 -1199px;">创建一个课程</a>
        </div>
    </div>
    <div>
        <h3 class="links-header">
            课程类别</h3>
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
    <div class="mfl-list-pages">
    </div>
    <div class="info">
    提示：拖动鼠标可以在标注“多选”的分类中一次选择多个类别。
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%this.m.RenderToStream(Response.OutputStream); %>
</asp:Content>
