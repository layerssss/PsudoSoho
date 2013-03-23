<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Admin/Frame.master" AutoEventWireup="true" CodeBehind="TPushing.aspx.cs" Inherits="MyFamilyLodge.Account.Admin.TPushing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="ID" DataSourceID="ObjectDataSource1">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                SortExpression="ID" />
            <asp:BoundField DataField="操作类型" HeaderText="操作类型" SortExpression="操作类型" />
            <asp:BoundField DataField="操作发起用户" HeaderText="操作发起用户" 
                SortExpression="操作发起用户" />
            <asp:BoundField DataField="操作状态" HeaderText="操作状态" SortExpression="操作状态" />
            <asp:BoundField DataField="操作状态信息"  HeaderText="操作状态信息"  ItemStyle-CssClass="popup"
                SortExpression="操作状态信息" HtmlEncode="False" >
<ItemStyle CssClass="popup"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="提交时间" HeaderText="提交时间" SortExpression="提交时间" />
            <asp:BoundField DataField="执行时间" HeaderText="执行时间" SortExpression="执行时间" />
            <asp:BoundField DataField="操作描述" HeaderText="操作描述" SortExpression="操作描述" 
                ItemStyle-CssClass="popup" >
            
<ItemStyle CssClass="popup"></ItemStyle>
            </asp:BoundField>
            
            <asp:BoundField DataField="错误原因" HeaderText="错误原因" SortExpression="错误原因">
            <ItemStyle CssClass="popup" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="GetPushingTransactions" TypeName="MFL.MFLTransactionCenter">
    </asp:ObjectDataSource>
</asp:Content>
