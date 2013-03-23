<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Wizards/Frame.master" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="MyFamilyLodge.Account.Wizards.Success" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">
	<div class="accountWrapper">
		<h1>操作成功提交</h1>
		<p>您成功提交了当前操作，该操作将<%if (Request["Audit"] == "True")
                      { %>在完成审核(24小时之内)之后<%}
                      else
                      { %>马上<%} %>被执行。您可以随时在“<a href="/Account/Transactions.aspx">我的操作</a>”页面查看已提交操作的状态。</p>
		<a href="/Account/" class="btn primary">返回我的账户</a>
	</div>
</div>
	
</asp:Content>
