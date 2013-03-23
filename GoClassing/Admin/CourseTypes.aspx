<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CourseTypes.aspx.cs" Inherits="GoClassing.Admin.CourseTypes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="type" DataSourceID="EntityDataSource1" 
        onselectedindexchanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowSelectButton="True" />
            <asp:BoundField DataField="type" HeaderText="type" ReadOnly="True" 
                SortExpression="type" />
        </Columns>
    </asp:GridView>
    <br />
    type1:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Create" />
    <br />
    <br />
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="type" DataSourceID="EntityDataSource2" AllowSorting="True">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="ctype1_type" HeaderText="ctype1_type" 
                SortExpression="ctype1_type" />
            <asp:BoundField DataField="type" HeaderText="type" ReadOnly="True" 
                SortExpression="type" />
        </Columns>
    </asp:GridView>
    ctype1_type:<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    ctype2:<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Create" />
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=gc_localtestEntities" 
        DefaultContainerName="gc_localtestEntities" EnableDelete="True" 
        EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="gccon_ctype1">
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSource2" runat="server" 
        AutoGenerateWhereClause="True" ConnectionString="name=gc_localtestEntities" 
        DefaultContainerName="gc_localtestEntities" EnableDelete="True" 
        EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="gccon_ctype2" Where="">
        <WhereParameters>
            <asp:ControlParameter ControlID="GridView1" Name="ctype1_type" 
                PropertyName="SelectedValue" Type="String" />
        </WhereParameters>
    </asp:EntityDataSource>
</asp:Content>
