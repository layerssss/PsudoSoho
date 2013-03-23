<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true" CodeBehind="MailVerifying.aspx.cs" Inherits="GoClassing.MailVerifying" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" >
    var verify = function (m, h, redirect) {
        MFLAjax({
            action: "VerifyMail",
            query: {
                mail: m,
                hash: h
            },
            success: function (json) {
                MFLDialog("邮箱验证成功", "您已成功验证了您的电子邮箱，点击确定马上进入登陆前页面。", function () {
                    location.href = redirect;
                });
            }
        });
    };

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="notifications" style="width:500px;margin:20px auto;border:1px solid #ccc;padding:30px 60px;">
</div>
</asp:Content>
