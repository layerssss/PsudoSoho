<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Wizards/Frame.master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="MyFamilyLodge.Account.Wizards.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<div class="container">
		<div class="accountWrapper">
			<h1>操作失败</h1>
			<p><%=Server.HtmlEncode(Request["message"]) %></p>
			<a href="#" onclick="history.go(-1);return false;" class="btn primary">后退</a>
		</div>
	</div>
	
</asp:Content>
