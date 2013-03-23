<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MFLAdmin.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var RecaptchaOptions = {
        theme: 'clean'
    };
    $(function () {
        $('input[name="password"]').focus();
    });
	function showErrors(eText){
            $('.errorMesg').fadeIn(1000).text(eText).prepend('<span></span>');
    }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<h1>寻旅.筑家-旅馆管理中心：请登录后继续</h1>
	<form method="post" id="adminLoginForm">
		密码：
		<input type="password" name="password" id="loginPassword"/><br />
		<p style="margin-top:10px;">验证码：</p>
		<div class="recaptchaDiv">
			<script type="text/javascript"
				 src="http://www.google.com/recaptcha/api/challenge?k=<%=MFL.SharedConfig.RecaptchaPublicKey %>">
			</script>
			<noscript>
				 <iframe src="http://www.google.com/recaptcha/api/noscript?k=<%=MFL.SharedConfig.RecaptchaPublicKey %>"
					 height="300" width="500" frameborder="0"></iframe><br>
				 <textarea name="recaptcha_challenge_field" rows="3" cols="40">
				 </textarea>
				 <input type="hidden" name="recaptcha_response_field"
					 value="manual_challenge"/>
			</noscript>
		</div>
		<p class="errorMesg"><span></span></p>
		<input type="submit" value="登录" name="submit" class="btn large success" id="loginSubmit"/>
	</form>

</asp:Content>
