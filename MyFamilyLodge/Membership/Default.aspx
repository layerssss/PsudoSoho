<%@ Page Title="注册用户" Language="C#" MasterPageFile="Frame.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyFamilyLodge.Membership.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="register">
			<div class="container">
				<div class="form-header">
					<h2 class="form-header-title">注册 Register</h2>
				</div>
				<div class="form-content">
					<div class="form-wrapper">
						<form action="">
							<label for="username">电子邮件：</label><input id="username" name="username" type="text"/><br/>
							<label for="password">密码：</label><input id="password" name="password" type="password"/><br/>
							<label for="confirmPassword">重复密码：</label><input id="confirmPassword" name="password2" type="password"/><br/>
                            
							<p class="errorMesg"><span></span>你的email和密码不符，请再试一次</p>
							<button name="submit" type="submit" value="注册" class="btn primary large pull-right register">注册</button>
						</form>
					</div>
				</div>
				<div class="form-footer">
					<a href="/Membership/Login.aspx">已经注册了一个账户？&nbsp &nbsp 请登录 &gt;&gt;</a>
					<a href="/Membership/PasswordRetrieval.aspx">忘记密码？&nbsp &nbsp 找回密码 &gt;&gt;</a>
				</div>
			</div>
		</div>
</asp:Content>
