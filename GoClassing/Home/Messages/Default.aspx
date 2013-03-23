<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoClassing.Home.Messages.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<script type="text/javascript">
    $(function () {
        var func = function () {
            MFLAjax({
                action: "GetMessages",
                query: {
                    dateStart: $('.dateStart').val(),
                    dateStop: $('.dateStop').val(),
                    showOld: true
                },
                loadingJ: $('.msg-list.main'),
                success: function (json) {
                    MFLList({
                        json: json,
                        containerJ: $('.msg-list.main')
                    });
                }
            });
        };
        $('.dateStart,.dateStop').datepicker({
            showOn: "both",
            buttonImage: "/Styles/icon_calendar.gif",
            buttonImageOnly: true,
            onClose: function (dateText, inst) {
                func();
            }
        });
        func();
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="right" runat="server">
<div class="ui-widget-header" style="padding:10px;">
显示从<br /><asp:TextBox CssClass="dateStart"
    ID="TextBox1" runat="server"></asp:TextBox><br />到<br /><asp:TextBox CssClass="dateStop"
    ID="TextBox2" runat="server"></asp:TextBox><br />收到的所有系统消息。</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="msg-list main">
<div class="mfl-list-itemTemplate ui-widget-content" >
<span class="floatright" style="font-size:0.9em;color:#aaa;">$time$</span>
<div class="teacher">
    <a href="/T/$fromUsername$"><img src="/Avatars/$fromId$.small.jpg" /></a>
    <span class="mfl-tags-fromCoop mfl-tags-coop"><a href="/$fromCoop$/" title="$fromCoop$"><img alt="$fromCoop$" title="$fromCoop$" src="/Styles/Tags/$fromCoop$.gif" /></a></span>
    <span class="mfl-teacherTruename">$fromTruename$</span> </div>
<a href="/Message.aspx?id=$id$">$content$</a>
</div>

</div>
</asp:Content>
