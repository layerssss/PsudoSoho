<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Wizards/Frame.master" AutoEventWireup="true" CodeBehind="Deactive.aspx.cs" Inherits="MyFamilyLodge.Account.Wizards.Deactive" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<div class="container">
		<div class="accountWrapper">
		<h1>反激活旅馆</h1>
		<p>如果您想更换旅馆标识，可以在反激活之后重新激活旅馆，并设定新的标识。</p>
		<p>注意：反激活旅馆之后，您的旅馆所有相关数据，包括：旅馆设定、房间数据、入住信息、图片信息和已经更新的前台页面将会被删除。</p>
		<p>如果您同意删除以上数据，则可以</p>
		<asp:Button ID="Button1" CssClass="btn danger" runat="server" Text="反激活我的旅馆" onclick="Button1_Click" />
		</div>
	</div>
	
</asp:Content>
