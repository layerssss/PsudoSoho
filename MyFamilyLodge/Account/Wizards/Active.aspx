<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Wizards/Frame.master" AutoEventWireup="true" CodeBehind="Active.aspx.cs" Inherits="MyFamilyLodge.Account.Wizards.ActiveLodge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container">
	<div class="accountWrapper">
		<h1>激活旅馆</h1>
		<p>旅馆标识：</p>
		<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
		<p style="margin:10px 0;">（提示：可以由英文字母、数字及下划线组成，如xinyuexiaozhu、xinyue_123）</p>
		<asp:Button ID="Button1" CssClass="btn primary" runat="server" Text="确定" onclick="Button1_Click" />
	</div>
</div>
</asp:Content>
