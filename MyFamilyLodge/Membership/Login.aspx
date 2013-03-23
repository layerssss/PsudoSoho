<%@ Page Title="" Language="C#" MasterPageFile="Frame.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyFamilyLodge.Membership.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<script type="text/javascript">
    var showErrors = function (msg) {
        alert(msg);
    };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="login">
		<div class="container">
			<div class="form-header">
				<h2 class="form-header-title">登录 Login</h2>
			</div>
			<div class="form-content">
				<div class="form-wrapper">
					<form method="post">
						<label for="username">电子邮件：</label><input name="username" id="username" type="text"/><br/>
						<label for="password">密码：</label><input name="password" id="password" type="password"/><br/>
						<p class="errorMesg"><span></span>你的email和密码不符，请再试一次</p>
						<button name="submit" value="登陆" type="submit" class="btn primary large pull-right login">登陆</button>
					</form>
				</div>
			</div>
			<div class="form-footer">
				<a href="/Membership/">还没账户？&nbsp &nbsp 请注册 &gt;&gt;</a>
				<a href="/Membership/PasswordRetrieval.aspx">忘记密码？&nbsp &nbsp 找回密码 &gt;&gt;</a>
			</div>
		</div>
	</div>
</asp:Content>
