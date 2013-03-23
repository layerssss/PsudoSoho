<%@ Page Title="我的动态" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoClassing.Home.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<script type="text/javascript">
    var ftypes = "";
    var feedsLast = -1;
    $(function () {
        $(".bar .refresh").button({
            icons: {
                primary: "ui-icon-arrowrefresh-1-e"
            }
        }).click(function () {
            resetFeeds();
            return false;
        });
        var addTick = function (e) {
            $('<span class="tick"></span>').prependTo(e).button({
                disabled: true,
                icons: {
                    primary: "ui-icon-check"
                }
            }).removeClass("ui-state-default");
            ftypes += "," + $(e).attr("class").substring(12);
            resetFeeds();
        };
        $(".types a").toggle(function () {
            $(this).find(".tick").remove();
            ftypes = ftypes.replaceAll("," + $(this).attr("class").substring(12), "");
            resetFeeds();
        }, function () {
            addTick(this);
        }).each(function () {
            addTick(this);
        });
        resetFeeds = function () {
            feedsLast = -1;
            $("#feeds2").children().remove();
            getFeeds(true);
        };
        resetFeeds();
        $(".more a").click(function () {
            getFeeds();
            return false;
        });
    });
    var resetFeeds = function () {
    };
    var getFeeds = function (reset) {
        MFLAjax({
            "action": "GetFeeds",
            "query": "startid=" + feedsLast + "&ftypes=" + encodeURI(ftypes),
            "success": function (json) {
                if (reset) {
                    $("#feeds2").children().remove();
                }
                MFLList({
                    json: json,
                    containerJ: $("#feeds")
                });
                $("#feeds .mfl-list-item").appendTo("#feeds2");
                if (json.listItems.length) {
                    feedsLast = json.listItems[json.listItems.length - 1].id;
                }
            },
            "loadingJ": $(".more")
        });
    };
</script>
<style type="text/css">
.bar
{
    display:block;
    margin-bottom:5px;
    border-bottom:1px solid #ccc;
    text-align:right;
}
.bar a
{
    margin-right:20px;
    margin-bottom:5px;
}
.tick
{
    width:30px;
    height:15px;
    padding:0;
    margin:0;
    float:right;
}
.selected
{
}
.tick
{
}
#feeds .mfl-class-ftype,#feeds2 .mfl-class-ftype
{
    padding-left:20px;
    background-image:url('../Styles/icons_sprite.png');
    background-repeat:no-repeat;
    
}
.more
{
    display:block;
    margin:5px;
    padding:10px;
    text-align:center;
    border:1px solid #ccc;
}
.mfl-time
{
    float:right;
    color:#999;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="right" runat="server">
    <h3 class="links-header">
        查看类型</h3>
    <div class="links-header types">
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-doc">Word文档</a>
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-pdf">Pdf文档</a>
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-ppt">演示文稿</a>
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-video">视频</a>
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-sound">音频</a>
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-assign">作业</a>
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-discuz">讨论</a>
        <a href="/Home/Courses/Type3.aspx" class="ftype ftype-notify">通知</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="bar"><a href="#" class="refresh" title="刷新" >刷新</a></div>
<div id="feeds2"></div>
<div id="feeds">
<div class="mfl-list-itemTemplate" style="padding-left:30px;position:relative;margin-bottom:10px;"><span class="floatright mfl-time"></span><span class="mfl-class-ftype ftype-$ftype$" style="position:absolute;left:5px!important;left:-25px;">&nbsp;</span>

<div class=" mfl-text "></div>
</div>
</div><div class="more"><a href="#" title="查看更多...">查看更多...</a></div>
</asp:Content>
