<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProfileName.aspx.cs" Inherits="GoClassing.Admin.ProfileName" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="name" DataSourceID="EntityDataSource1" AllowSorting="True">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="name" HeaderText="name" ReadOnly="True" 
                SortExpression="name" />
            <asp:BoundField DataField="defaultSort" HeaderText="defaultSort" 
                SortExpression="defaultSort" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=gc_localtestEntities" 
        DefaultContainerName="gc_localtestEntities" EnableDelete="True" 
        EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="gccon_profilename">
    </asp:EntityDataSource>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Create" />
</asp:Content>
