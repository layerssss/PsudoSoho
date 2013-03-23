<%@ Page Title="" Language="C#" MasterPageFile="Frame.master" AutoEventWireup="true" CodeBehind="PasswordRetrieval.aspx.cs" Inherits="MyFamilyLodge.Membership.PasswordRetrieval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="passwordRetrieval">
		<div class="container">
			<div class="form-header">
				<h2 class="form-header-title">忘记密码</h2>
			</div>
			<div class="form-content">
				<div class="form-wrapper">
					<form action="">
						<label for="username">电子邮件：</label><input type="text"/><br/>
						<button name="submit" type="submit" value="找回密码" class="btn primary large pull-right passwordRetrieval">找回密码</button>
					</form>
				</div>
			</div>
			<div class="form-footer">
			</div>
		</div>
	</div>
</asp:Content>
