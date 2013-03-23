<%@ Page Title="" Language="C#" MasterPageFile="~/Internal/Course.master" AutoEventWireup="true"
    CodeBehind="CoursePost.aspx.cs" Inherits="GoClassing.Internal.CoursePost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/jplayer.blue.monday.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.jplayer.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pid;
        var courseId;
    </script>
    <script type="text/javascript">
        var data;
        var playVideo = function (dom, path) {
            MFLDialog("抱歉", "播放器尚未准备就绪，请稍候。");
        };
        var playSound = function (dom, path) {
            MFLDialog("抱歉", "播放器尚未准备就绪，请稍候。");
        };
        var playSwf = function (dom, path) {
            MFLDialog("抱歉", "播放器尚未准备就绪，请稍候。");
        };
        
        var play = function (player, btn, callback, destroying) {
            var prev = $(btn).parent()[0];
            var f1 = function () {
                var loading = $('<div style="position:absolute;left:0;top:0;z-index:10000;background-color:#fff;"><img src="/Styles/loading.gif" style="display:block;margin:30px auto;" /></div>');
                var p = $(player).hide().insertAfter($(btn).parent()).addClass("player").attr('style','float:right;').css({
                    width: 730,
                    clear: 'right',
                    position: 'relative'
                }).show();
                loading.height(p.height()).width(p.width()).prependTo(p);
                p.hide();
                $(btn).parent().ready(function () {
                    p.slideDown(function () {
                        p.show();
                        callback(function () {
                            $(prev).parent().parent().next().find('[onclick^="play"]').trigger('click');
                        });
                        loading.fadeOut();
                    });
                }).data("PlayerDestroying", destroying);
            };
            if ($('.player').length) {
                $('.player').prev().data("PlayerDestroying")()
                $('.player').slideUp(function () {
                    var cur = $('.player').prev()[0];
                    $('.player').remove();
                    if (cur != $(btn).parent()[0]) {
                        f1();
                    }
                });
            } else {
                f1();
            }
        };
        MFLAjax({
            action: "GetPlayerVariebles",
            success: function (json) {
                playVideo = function (btn, path) {
                    play(json.jplayerVideoTemplate, btn, function (nextcall) {
                        $("#jquery_jplayer_1").jPlayer({
                            ready: function () {
                                $(this).jPlayer("setMedia", {
                                    m4v: json.baseUrl + path.substring(1),
                                    poster: json.baseUrl + path.substring(1) + ".big.jpg"
                                }).jPlayer('play');
                            },
                            ended: function () {
                                nextcall();
                            },
                            solution: "flash",
                            swfPath: "/Flash",
                            supplied: "m4v",
                            size: {
                                width: "730px",
                                height: "548px",
                                cssClass: "jp-video-360p"
                            }
                        });
                    }, function () {
                        $("#jquery_jplayer_1").jPlayer('destroy');
                    });
                };
                playSound = function (btn, path) {
                    play(json.jplayerAudioTemplate, btn, function (nextcall) {
                        var ext = path.substr(path.length - 3, 3);
                        $("#jquery_jplayer_1").jPlayer({
                            ready: function () {
                                var m = new Object();
                                m[ext] = json.baseUrl + path.substring(1)
                                $(this).jPlayer("setMedia", m).jPlayer('play');
                            },
                            ended: function () {
                                if (!$(this).jPlayer('option', 'loop')) {
                                    nextcall();
                                }
                            },
                            solution: "flash",
                            swfPath: "/Flash",
                            supplied: ext
                        });
                    }, function () {
                        $("#jquery_jplayer_1").jPlayer('destroy');
                    });
                };
                playSwf = function (btn, path) {
                    play(json.docTemplate, btn, function () {
                        swfobject.embedSWF(path, "myAlternativeContent", "730", "548", "9.0.0", "expressInstall.swf", {}, {}, {});
                    }, function () {
                    });
                };
            }
        });
        $(function () {
            $('.preview').click(function () {
                $(this).siblings('a').trigger('click');
            });
            $('#middle3 .reply-list .mfl-list-item').each(function () {
                if (data.isCreatorOf) {
                    $('<a href="#">[删除]</a>').appendTo($(this).find('.time').append('<br />')).click(function () {
                        var rid = $(this).parent().parent().children('.mfl-alt-id').attr("title");
                        MFLAjax({
                            action: "DelCourseReply",
                            query: {
                                postId: pid,
                                courseId: courseId,
                                replyId: rid
                            },
                            loadingJ: $(".loading"),
                            success: function () {
                                location.reload();
                            }
                        });
                        return false;
                    });
                } else {
                    $('<br/>').appendTo($(this).find('.time'));
                    $('<span>&nbsp;</span>').appendTo($(this).find('.time'));
                }
            });
            if (!data.canReply) {
                $('.btn-reply-author').text('');
            }
            if (data.canReply) {
                setInterval(function () {
                    $(".reply .numChar").text($('.reply textarea').val().length + "/1000字");
                }, 50);
                $('.reply').show();
                $('.noupload').button().click(function () {
                    MFLAjax({
                        action: "CreateReply",
                        loadingJ: $('.loading'),
                        type: 'post',
                        query: {
                            postId: pid,
                            courseId: courseId,
                            content: $('textarea').val()
                        },
                        success: function (json) {
                            MFLDialog("提示", "提交成功。", function () { location.reload(); });

                        }
                    });
                });

                $('.swfu1').mflSwfUpload({
                    types: "swf",
                    text: "上传文档/演示文稿(" + data.limitationSwf+')',
                    ajaxOptions: { action: "CreateReply",
                        type: 'post',
                        query: {
                            postId: pid,
                            courseId: courseId,
                            content: ''
                        },
                        success: function (json) {
                            MFLDialog("提示", "提交成功，请等候系统处理您提交的内容，稍后即可显示。", function () { location.reload(); });
                        }
                    },
                    callback: function () {
                        $('.swfu2').mflSwfUpload({
                            types: "mp3",
                            text: "上传音频文件" + data.limitationMp3 + ')',
                            ajaxOptions: { action: "CreateReply",
                                type: 'post',
                                query: {
                                    postId: pid,
                                    courseId: courseId,
                                    content: ''
                                },
                                success: function (json) {
                                    MFLDialog("提示", "提交成功，请等候系统处理您提交的内容，稍后即可显示。", function () { location.reload(); });
                                }
                            },
                            callback: function () {
                                $('.swfu3').mflSwfUpload({
                                    types: "mp4",
                                    text: "上传视频文件" + data.limitationMp4 + ')',
                                    ajaxOptions: { action: "CreateReply",
                                        type: 'post',
                                        query: {
                                            postId: pid,
                                            courseId: courseId,
                                            content: ''
                                        },
                                        success: function (json) {
                                            MFLDialog("提示", "提交成功，请等候系统处理您提交的内容，稍后即可显示。", function () { location.reload(); });
                                        }
                                    },
                                    callback: function () {
                                        $('.swfuloading').remove();
                                    }
                                });
                            }
                        });
                    }
                });
            }
        });
    </script>
    <style type="text/css">
    img.preview
    {
        width:72px;
        height:54px;
        position:absolute;
        left:250px;
        top:5px;
        cursor:pointer;
    }
    span.time
    {
        float:left;
        color:#555;
        font-size:0.9em;
    }
    p.info
    {
        font-size:0.8em;
        color:#555;
    }
    #middle3 .reply-list .mfl-list-item
    {
        position:relative;
    }
    .reply-list .mfl-item-template
    {
        border-top:1px solid #ccc;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="middle3">
        <%
            this.m.RenderToStream(Response.OutputStream);
             %>
    </div>
</asp:Content>
