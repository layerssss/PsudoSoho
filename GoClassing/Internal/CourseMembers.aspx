<%@ Page Title="" Language="C#" MasterPageFile="~/Internal/Course.master" AutoEventWireup="true" CodeBehind="CourseMembers.aspx.cs" Inherits="GoClassing.Internal.CourseMembers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(function () {
        setTimeout(function () {
            MFLAjax({
                action: 'GetCourseMembers',
                query: {
                    courseId: cid,
                    page: 0
                },
                loadingJ: $('.loading'),
                success: function (json, o) {
                    MFLList({
                        containerJ: $('#members .members-list'),
                        json: json.members,
                        pagerHolderJ: $('#members .mfl-list-pages'),
                        pagerAjaxOptions: o
                    });
                    MFLList({
                        containerJ: $('#newMembers .members-list'),
                        json: json.newMembers
                    });

                    $('.teacher:not(.floatright .teacher)').draggable({
                        revert: true
                    });
                }
            });
        }, 100);
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="middle2">
            <div id="newMembers">
                <h3 class="links-header">
                    收到的加入申请</h3>
                <div class="members-list">
                    <span class="mfl-list-itemTemplate">
                        <div class="teacher">
                            <a href="/T/$username$">
                                <img src="/Avatars/$id$.small.jpg" /></a> <span class="mfl-tags-coop"><a href="/$coop$/"
                                    title="$coop$">
                                    <img alt="$coop$" title="$coop$" src="/Styles/Tags/$coop$.gif" /></a></span>
                            <span class="mfl-truename"><span class="truename">$truename$</span></span><span style="display: none"
                                class="mfl-alt-id" title="$id$"></span>
                        </div>
                    </span>
                </div>
                <div class="clearfix" style="height: 5px;">
                </div><div class="info">（将申请人头像拖至成员列表可同意起加入该课程）</div>
            </div>
            <div id="members" style="">
                <a href="." >
                    返回课程首页...</a>
                <h3 class="links-header">
                    已加入的成员</h3>
                <div class="members-list">
                    <span class="mfl-list-itemTemplate">
                        <div class="teacher" style="margin:5px;">
                            <a href="/T/$username$">
                                <img src="/Avatars/$id$.small.jpg" /></a> <span class="mfl-tags-coop"><a href="/$coop$/"
                                    title="$coop$">
                                    <img alt="$coop$" title="$coop$" src="/Styles/Tags/$coop$.gif" /></a></span>
                            <span class="mfl-truename"><span class="truename">$truename$</span><br />
                                过期：<br />
                                <span class="mfl-due">$due$</span></span><span style="display: none" class="mfl-alt-id"
                                    title="$id$"></span>
                        </div>
                    </span>
                </div>
                <div class="mfl-list-pages" style="height: 50px; width: 560px; float: left;">
                    <div class="clearfix">
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
</asp:Content>
