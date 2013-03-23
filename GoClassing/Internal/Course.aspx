<%@ Page Title="" Language="C#" MasterPageFile="~/Internal/Course.Master" AutoEventWireup="true"
    CodeBehind="Course.aspx.cs" Inherits="GoClassing.Internal.Course" %>

<%@ Import Namespace="MFLJson" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <div id="middle1">
            <div id="colLeft">
                <%
                    foreach (var tag in this.c.C.gc_tag.Where(t => t.leftcol))
                    {
                        this.mTag["tag"] = Json.String(tag.tag);
                        this.mTag["posts"] = this.c.GetPosts(tag);
                        this.mTag["canReplyText"] = Json.String(tag.canReply ? "是" : "否");
                        this.mTag["canGuestViewText"] = Json.String(tag.canGuestView ? "是" : "否");
                        this.mTag.RenderToStream(Response.OutputStream);
                    }
                %>
            </div>
            <div id="colRight">
                <%
                    foreach (var tag in this.c.C.gc_tag.Where(t => !t.leftcol))
                    {
                        this.mTag["tag"] = Json.String(tag.tag);
                        this.mTag["posts"] = this.c.GetPosts(tag);
                        this.mTag["canReplyText"] = Json.String(tag.canReply ? "是" : "否");
                        this.mTag["canGuestViewText"] = Json.String(tag.canGuestView ? "是" : "否");
                        this.mTag.RenderToStream(Response.OutputStream);
                    }
                %>
            </div>
        </div>
        
        
</asp:Content>
