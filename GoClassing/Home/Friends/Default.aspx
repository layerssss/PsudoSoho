<%@ Page Title="我的好友" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoClassing.Home.Friends.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<style type="text/css">
    .user-list .mfl-list-item
    {
        display:block;
        width:570px;
        float:left;
        margin:2px;
        padding:5px
    }
    
    .user-list .mfl-list-item .floatright
    {
        text-align:right;
    }
    #users>.mfl-list-pages
    {
        display:block;
        width:400px;
    }
    #users>h3>.mfl-list-pages
    {
        float:right;
    }
    #users>h3
    {
        height:30px;
    }
    .user-list .mfl-list-item .mfl-replace-src-id
    {
        margin-right:10px;
    }
    .user-list .mfl-truename
    {
        font-size:1.2em;
        font-weight:bold;
    }
    .salutting-list .mfl-list-item
    {
        margin:5px;
        display:block;
    }
    </style>
    
    <script type="text/javascript">
        var data = null;
        var resetFeeds = function () {
            if (location.hash.substr(1) == '') {
                $('#users>h3>span').text('我的好友');
            } else {
                $('#users>h3>span').text('在我的好友中查找“' + location.hash.substr(1) + '”的结果');
            }
            MFLAjax({
                action: "GetMyFriends",
                loadingJ: $('#users .user-list'),
                query: {
                    page: 0,
                    search: location.hash.substr(1)
                },
                success: function (json, o) {
                    MFLList({
                        json: json.friends,
                        pagerAjaxOptions: o,
                        containerJ: $('.user-list'),
                        pagerHolderJ: $('#users .mfl-list-pages')
                    });
                }
            });
        };
        gcSearch = function (sstr) {
            resetFeeds();
        };
        $(function () {
            resetFeeds();
            MFLAjax({
                action: "GetSaluttingUsers",
                loadingJ: $('#salutting .salutting-list'),
                success: function (json) {
                    MFLList({
                        json: json,
                        containerJ: $('#salutting .salutting-list')
                    });
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="right" runat="server"><div id="salutting" class="block">
        <h3>
            我收到的好友申请：</h3>
        <div class="salutting-list">
            <span class="mfl-list-itemTemplate">
                        <div class="teacher">
                                <a href="/T/$username$"><img src="/Avatars/$id$.small.jpg" /></a>
                                <span class="mfl-tags-coop"><a href="/$coop$/" title="$coop$"><img alt="$coop$" title="$coop$" src="/Styles/Tags/$coop$.gif" /></a></span>
                                <span class="mfl-teacherTruename">$truename$</span> </div></span>
        </div>
        <div class="clearfix">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="users" style="float: left">
            <h3>
                <div class="mfl-list-pages">
                </div>
                <span>我的好友</span>
            </h3>
            <div class="user-list">
                
                <div class="block mfl-list-itemTemplate">
                    
                    <a href="/T/$username$/" class="mfl-href-username">
                        <img src="/Avatars/$id$.jpg" class="mfl-replace-src-id" style="float: left" /></a>
                       <div class="floatright"> 
                    <a href="/T/$username$/" class="mfl-truename mfl-href-username">$truename$</a><span class=" mfl-tags-coop"><a href="/$coop$/" title="$coop$"><img alt="$coop$" title="$coop$" src="/Styles/Tags/$coop$.gif" /></a></span><br />
                        性别：<img src="/Styles/sex$sexprefix$.gif" title="$sexchar$" alt="$sexchar$" class="mfl-replace-src-sexprefix" /><br />
                        来自：<a class=" mfl-href-uprovince" onclick="$('.searchText').val($(this).attr('href').substring(1));return false;" href="#$uprovince$">$uprovince$</a>
                        <a class=" mfl-href-ucity" onclick="$('.searchText').val($(this).attr('href').substring(1));return false;" href="#$ucity$">$ucity$</a><br />
                        <a href="/T/$username$/#Courses">正在教授的课程数:$createdCourses$</a><br />
                        <a href="/T/$username$/#PCourses">正在学习的课程数:$paticipatedCourses$</a><br />
                        <span class="mfl-alt-sex" style="display:none">$sex$</span></div>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <div class="mfl-list-pages">
            </div>
        </div>
</asp:Content>
