<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Frame.Master" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="MyFamilyLodge.Account.Transactions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<style type="text/css">
.pagerTableRow table
{
    width:auto;
}
tr.pagerTableRow td:hover
{
    background:inherit !important;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
<div class="container">
	<div class="accountWrapper">
		<h1>我的操作</h1>
    <asp:GridView ID="GridView1" CssClass="zebra-striped" runat="server" 
            AllowPaging="True" AllowSorting="True" 
                AutoGenerateColumns="False" DataKeyNames="ID" 
                DataSourceID="ObjectDataSource1" 
            onselectedindexchanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:CommandField  SelectText="查看详情" ShowSelectButton="True" />
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                SortExpression="ID" />
            <asp:BoundField DataField="操作类型" HeaderText="操作类型" SortExpression="操作类型" />
            <asp:BoundField DataField="操作状态" HeaderText="操作状态" SortExpression="操作状态" />
            <asp:BoundField DataField="提交时间" HeaderText="提交时间" SortExpression="提交时间" />
            <asp:BoundField DataField="执行时间" HeaderText="执行时间" SortExpression="执行时间" />
        </Columns>
        <SelectedRowStyle CssClass="blue"  />
        <PagerStyle CssClass="pagerTableRow" />
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                SelectMethod="GetMyTransactions" TypeName="MFL.MFLTransactionCenter">
    </asp:ObjectDataSource>
        <asp:DetailsView CssClass="dialog" ID="DetailsView1" runat="server" Height="50px" Width="125px" 
            AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="ObjectDataSource2">
            <Fields>
                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                    SortExpression="ID" />
                <asp:BoundField DataField="错误原因" HeaderText="错误原因" SortExpression="错误原因" />
                <asp:BoundField DataField="操作类型" HeaderText="操作类型" SortExpression="操作类型" />
                <asp:BoundField DataField="操作状态" HeaderText="操作状态" SortExpression="操作状态" />
                <asp:BoundField DataField="提交时间" HeaderText="提交时间" SortExpression="提交时间" />
                <asp:BoundField DataField="执行时间" HeaderText="执行时间" SortExpression="执行时间" />
                <asp:BoundField DataField="操作描述" HeaderText="操作描述" SortExpression="操作描述" />
            </Fields>
        </asp:DetailsView>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
            SelectMethod="GetTransaction" TypeName="MFL.MFLTransactionCenter">
            <SelectParameters>
                <asp:ControlParameter ControlID="GridView1" DefaultValue="0" Name="id" 
                    PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </div>
    </form>
</asp:Content>
