<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Admin/Frame.master" AutoEventWireup="true" CodeBehind="TOfDay.aspx.cs" Inherits="MyFamilyLodge.Account.Admin.TOfDay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <script src="<%=MFL.SharedConfig.AdminBaseUrl %>Scripts/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $.datepicker.setDefaults($.datepicker.regional['zh-Cn']);
        $('.datepicker').datepicker({
            onSelect: function () {
                $(this).next().trigger("click");
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
日期：<asp:TextBox ID="TextBox1" runat="server" CssClass="datepicker"></asp:TextBox>
    <asp:Button ID="Button1"
    runat="server" Text="刷新" onclick="Button1_Click" />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="ID" DataSourceID="ObjectDataSource1">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                SortExpression="ID" />
            <asp:BoundField DataField="操作类型" HeaderText="操作类型" SortExpression="操作类型" />
            <asp:BoundField DataField="操作发起用户" HeaderText="操作发起用户" 
                SortExpression="操作发起用户" />
            <asp:BoundField DataField="操作状态" HeaderText="操作状态" SortExpression="操作状态" />
            <asp:BoundField DataField="操作状态信息" HeaderText="操作状态信息" ItemStyle-CssClass="popup"
                SortExpression="操作状态信息" HtmlEncode="False" />
            <asp:BoundField DataField="提交时间" HeaderText="提交时间" SortExpression="提交时间" />
            <asp:BoundField DataField="执行时间" HeaderText="执行时间" SortExpression="执行时间" />
            <asp:BoundField DataField="操作描述" HeaderText="操作描述" SortExpression="操作描述" ItemStyle-CssClass="popup" />
            
            <asp:BoundField DataField="错误原因" HeaderText="错误原因" SortExpression="错误原因">
            <ItemStyle CssClass="popup" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="GetTransactionsOfDay" TypeName="MFL.MFLTransactionCenter">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBox1" DefaultValue="" Name="day" 
                PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
