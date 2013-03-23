<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Wizards/Frame.master" AutoEventWireup="true" CodeBehind="Renewal.aspx.cs" Inherits="MyFamilyLodge.Account.Wizards.Renewal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<div class="container">
		<div class="accountWrapper">
			<h1>添加旅馆</h1>
			
			<p>旅馆类型：
			    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                <asp:HiddenField ID="HiddenField1" runat="server" />
			</p>
			
			
			<p>开通时长：
			<asp:DropDownList CssClass="" ID="DropDownList3" runat="server" AutoPostBack="True" 
				onselectedindexchanged="DropDownList3_SelectedIndexChanged">
			</asp:DropDownList>
			</p>
			
			<p>对应价格：￥<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
			.00</p>
			<p>账户余额：￥<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
			.00</p>
			
			<asp:Button ID="Button1" CssClass="btn primary" runat="server" Text="确定" onclick="Button1_Click" />
		</div>
	</div>
</asp:Content>
