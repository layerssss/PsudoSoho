<%@ Page Title="账户摘要" Language="C#" MasterPageFile="Frame.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyFamilyLodge.Account.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">
	<div class="accountWrapper">
		<h1>账户摘要</h1>
		<p>账户余额：￥<%=this.MFLUsers_account.balance %>.00</p>
		<p>
		你尚有<%=this.MFLUsers_account.MFLUsers_product.Count(tp=>!tp.MFL_lodge.Any()) %>个旅馆未激活
		<%if (this.MFLUsers_account.MFLUsers_product.Any(tp => !tp.MFL_lodge.Any()))
		  { %>
		  ，激活这些旅馆以使用
		<%} %>
		</p>
		<a class="btn primary" href="Lodges.aspx">查看我的旅馆列表</a>
	</div>
</div>

</asp:Content>
