<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoClassing.T.All.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <style type="text/css">
    .user-list .mfl-list-item
    {
        display:block;
        width:570px;
        float:left;
        margin:2px;
        padding:5px;
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
        var stopAjax = false;
        var data=null;
        var resetFeeds = function () {
            if (location.hash.substr(1) == '') {
                $('#users>h3>span').text('所有用户');
            } else {
                $('#users>h3>span').text('在所有用户中查找“' + location.hash.substr(1) + '”的结果');
            }
            MFLAjax({
                action: "GetAllUsers",
                loadingJ: $('#users .user-list'),
                query: {
                    page: 0,
                    search: location.hash.substr(1)
                },
                success: function (json, o) {
                    MFLList({
                        json: json.users,
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
            if (location.hash.substr(1) != "") {
                resetFeeds();
            } else {
                var appendBtn = function () {
                    $('.user-list .mfl-list-item .floatright').each(function (i, v) {
                        var sex = $(v).find('.mfl-alt-sex').text();
                        var href = $($(v).find('a')[0]).attr('href');
                        var username = href.substr(0, href.length - 1);
                        username = username.substr(username.lastIndexOf('/') + 1);
                        $('<a  href="' + href + '">访问' + sex + '的主页</a>').appendTo(v).button();
                        $('<br/>').appendTo(v);
                        $('<a  href="#">添加' + sex + '为好友</a>').appendTo(v).button().click(function () {
                            MFLAjax({
                                action: 'Salut',
                                loadingJ: $(this),
                                query: {
                                    username: username
                                },
                                success: function (json) {
                                    if (json.salutting) {
                                        MFLDialog("好友请求已发送", "好友请求已发送，等待对方确认即可添加为好友", function () {
                                        });
                                    } else {
                                        MFLDialog("好友已添加", "成功将" + sex + "添加为好友", function () {
                                        });
                                    }
                                },
                                failed: function (msg) {
                                    var i;
                                    if (i = msg.lastIndexOf('|')) {
                                        msg = msg.substr(0, i);
                                    }
                                    MFLDialog("好友添加失败", msg, function () {
                                    });
                                }
                            });
                            return false;
                        });
                    });
                };
                MFLList({
                    json: data,
                    pagerAjaxOptions: stopAjax ? null : {
                        action: "GetAllUsers",
                        loadingJ: $('#users .user-list'),
                        query: {
                            page: 0,
                            search: location.hash.substr(1)
                        },
                        success: function (json, o) {
                            MFLList({
                                json: json.users,
                                pagerAjaxOptions: o,
                                containerJ: $('.user-list'),
                                pagerHolderJ: $('#users .mfl-list-pages')
                            });
                            appendBtn();
                        }
                    },
                    containerJ: $('.user-list'),
                    pagerHolderJ: $('#users .mfl-list-pages')
                });
                appendBtn();
            }
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
<asp:Content ID="Content2" ContentPlaceHolderID="right" runat="server">
    <div id="salutting" class="block">
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
<%
    this.m.RenderToStream(Response.OutputStream); %>
</asp:Content>
