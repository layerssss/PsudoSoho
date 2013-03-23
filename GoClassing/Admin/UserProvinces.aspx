<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UserProvinces.aspx.cs" Inherits="GoClassing.Admin.UserProvinces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="province" DataSourceID="EntityDataSource1" 
        onselectedindexchanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowSelectButton="True" />
            <asp:BoundField DataField="province" HeaderText="province" ReadOnly="True" 
                SortExpression="province" />
        </Columns>
    </asp:GridView>
    <br />
    province:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Create" />
    <br />
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="city" DataSourceID="EntityDataSource2">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="province" HeaderText="province" 
                SortExpression="province" />
            <asp:BoundField DataField="city" HeaderText="city" 
                SortExpression="city" ReadOnly="True" />
        </Columns>
    </asp:GridView>
    province:<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
   city: <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Create" />
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=gc_localtestEntities" 
        DefaultContainerName="gc_localtestEntities" EnableDelete="True" 
        EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="gccon_province">
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSource2" runat="server" 
        AutoGenerateWhereClause="True" ConnectionString="name=gc_localtestEntities" 
        DefaultContainerName="gc_localtestEntities" EnableDelete="True" 
        EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="gccon_city" Where="">
        <WhereParameters>
            <asp:ControlParameter ControlID="GridView1" Name="province" 
                PropertyName="SelectedValue" Type="String" />
        </WhereParameters>
    </asp:EntityDataSource>
</asp:Content>
