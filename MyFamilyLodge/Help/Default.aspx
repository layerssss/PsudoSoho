<%@ Page Title="" Language="C#" MasterPageFile="Frame.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyFamilyLodge.Help.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="accordion">
	<p>快速配置旅馆：</p>
    <div class="ac-head"><span>展开</span><a href="#">快速配置旅馆</a></div>
    <div class="ac-content">快速配置旅馆的具体内容</div>
	
	<p>前台管理员操作流程：</p>
    <div class="ac-head"><span>展开</span><a href="#">查询当前入住状态</a></div>
    <div class="ac-content">如何查询的具体内容</div>
	<div class="ac-head"><span>展开</span><a href="#">修改当前入住状态</a></div>
    <div class="ac-content">如何修改的具体内容</div>
	<div class="ac-head"><span>展开</span><a href="#">离线备份入住状态</a></div>
    <div class="ac-content">如何备份的具体内容</div>
	
	<p>高级旅馆配置</p>
	<div class="ac-head"><span>展开</span><a href="#">怎样绑定自己的域名</a></div>
    <div class="ac-content">具体内容</div>
	<div class="ac-head"><span>展开</span><a href="#">怎样绑定自己的淘宝店铺</a></div>
    <div class="ac-content">具体内容</div>
	
	<p>常见问题</p>
	<div class="ac-head"><span>展开</span><a href="#">常见问题实例说明</a></div>
    <div class="ac-content">具体内容</div>
	
</div>
</asp:Content>
