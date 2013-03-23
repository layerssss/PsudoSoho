<%@ Page Title="我的主页" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true"
    CodeBehind="Teacher.aspx.cs" Inherits="GoClassing.Internal.Teacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/mfl-edit.js" type="text/javascript"></script>
    <script src="/Scripts/mfl-lodge-icon.js" type="text/javascript"></script>
    <script type="text/javascript">
        var initPage = function (o) {
            o = $.extend({
                isSelf: false,
                isFriend: false,
                isSalutting: false
            }, o);
            $('#wizardNav a').button();
            $('#wizardNav a[href="#Profile"]').addClass('ui-state-highlight');
            if (o.isFriend || o.isSelf) {
                setInterval(function () {
                    $(".reply .numChar").text($('.reply textarea').val().length + "/300字");
                }, 50);
                $('.reply').show();
                $('.noupload').button().click(function () {
                    MFLAjax({
                        action: "CreateNote",
                        loadingJ: $('.reply textarea'),
                        type: 'post',
                        query: {
                            username: o.username,
                            "private": $('input#private').attr("checked") == "checked",
                            content: $('textarea').val()
                        },
                        success: function (json) {
                            MFLDialog("提示", "提交成功。", function () { location.reload(); });

                        }
                    });
                });


            }
            MFLAjax({
                loadingJ: $(".profiles"),
                notifyRoot: false,
                action: "GetUserProfiles",
                query: {
                    username: o.username
                },
                success: function (json) {
                    MFLList({
                        containerJ: $(".profiles"),
                        json: json.profiles,
                        autoEmpty: false
                    });
                    if (o.isSelf) {
                        MFLList({
                            containerJ: $(".moreprofiles"),
                            json: json.moreprofiles,
                            autoEmpty: false
                        });
                        $('<div class="block ui-state-highlight"><a href="#" onclick="return false;">显示/隐藏未填写项...</a></div>').insertAfter(".moreprofiles")
                        .children("a").click(function () {
                            $(".moreprofiles").slideToggle();
                        });
                        $(".moreprofiles").hide();
                        $(".profiles div h3").addClass("ui-widget-header")
                        .css("cursor", "move");
                        $(".moreprofiles div.mfl-list-item h3,.profiles div h3")
                        .prepend(
                        $('<a href="#" class="floatright mfl-btn ui-state-highlight"></a>').button({
                            icons: {
                                primary: 'ui-icon-pencil'
                            }
                        }).click(function () {
                            $(this).closest(".block").children("div").mflEdit({
                                height: 200,
                                multiline: true,
                                lengthLimitation: 300,
                                callback: function () {
                                    var has = $(this).data("MFLData-value") && $(this).data("MFLData-value").length;
                                    if (has && !$(this).parents(".profiles").length) {
                                        $(this).closest(".block").appendTo(".profiles").find("h3").addClass("ui-widget-header")
                                        .css("cursor", "move");
                                    }
                                    if (!has && $(this).parents(".profiles").length) {
                                        $(this).closest(".block").appendTo(".moreprofiles").find("h3").removeClass("ui-widget-header")
                                        .css("cursor", "default");
                                    }
                                    updateProfile();
                                },
                                dataKey: "MFLData-value"
                            });
                            return false;
                        }));
                    }
                }
            });
            if (o.isSelf) {
                $(".profiles").sortable({
                    stop: function () {
                        updateProfile();
                    }, handle: "h3"
                });
                $('<a href="#" class="mfl-btn floatright"></a>').prependTo(".user").button({
                    icons: {
                        primary: 'ui-icon-pencil'
                    }
                }).click(function () {
                    var t = $(this).hide();
                    $(".from").hide();
                    $("#sexicon").hide();
                    var sex = $('<input type="hidden" />').val(o.sexprefix).insertAfter("#sexicon").lodgeIcon({
                        action: "GetSex",
                        fieldText: "char",
                        fieldValue: "prefix",
                        fieldIcon: "icon",
                        showText: true,
                        cssClassA: "",
                        listFilter: function (item) {
                            item.icon = "/Styles/sex" + item.prefix + ".gif";
                            return true;
                        },
                        autoSelected: true,
                        gettingIcon: function () {
                            return "/Styles/sex" + o.sexprefix + ".gif";
                        }, gettingText: function () {
                            return "";
                        },
                        collapsable: false,
                        selected: function (sex) {
                        }
                    });
                    var ucity = $('<input readonly="readonly" class="ucity" />').val(o.ucity).appendTo($("<p>城市：</p>").insertAfter(".from"));
                    var first = true;
                    var uprovince = $('<input readonly="readonly" class="uprovince" />').val(o.uprovince).appendTo($("<p>省份：</p>").insertBefore(ucity.parent())).lodgeIcon({
                        action: "GetUprovince",
                        fieldText: "uprovince",
                        fieldValue: "uprovince",
                        fieldIcon: "icon",
                        showText: true,
                        cssClassA: "ucitya",
                        listFilter: function (item) {
                            item.icon = "/Styles/blank.gif";
                            return true;
                        },
                        autoSelected: true,
                        gettingIcon: function () {
                            return "/Styles/blank.gif";
                        },
                        collapsable: false,
                        selected: function (prov) {
                            if (!first) {
                                ucity.val("");
                            }
                            first = false;
                            ucity.lodgeIcon({
                                action: "GetUcity",
                                query: {
                                    uprovince: $(prov).val()
                                },
                                collapsable: false,
                                fieldText: "ucity",
                                fieldValue: "ucity",
                                fieldIcon: "icon",
                                autoSelected: true,
                                showText: true,
                                cssClassA: "ucitya",
                                'diaplaySelected': true,
                                listFilter: function (item) {
                                    item.icon = "/Styles/blank.gif";
                                    return true;
                                },
                                gettingIcon: function () {
                                    return "/Styles/blank.gif";
                                }
                            });
                        }
                    });
                    $(".truename").mflEdit({
                        width: 200,
                        callback: function () {
                            var root = this;
                            MFLAjax({
                                action: 'UpdateProfile',
                                query: {
                                    ucity: ucity.val(),
                                    uprovince: uprovince.val(),
                                    truename: $(root).text(),
                                    sex: sex.val()
                                },
                                loadingJ: $("#oploading"),
                                success: function () {
                                    MFLNotify("成功保存基础资料");
                                    o.uprovince = uprovince.val();
                                    o.truename = $(root).text();
                                    o.ucity = ucity.val();
                                    o.sexprefix = sex.val();
                                    o.sexchar = o.sexprefix == "" ? "保密" : (o.sexprefix == "m" ? "男" : "女");
                                    $("#sexicon").attr("src", "/styles/sex" + o.sexprefix + ".gif")
                                    .attr("title", "性别：" + o.sexchar).show();
                                    $(".from").text("来自：" + o.uprovince + " " + o.ucity).show();
                                    setTimeout(function () {
                                        $(".avatar").attr("src", "/avatars/" + o.id + ".jpg?t=" + (new Date()));
                                    }, 1000);
                                },
                                complete: function () {
                                    $("#op").slideUp();
                                }
                            });
                            $("#optext").text("正在保存基础资料更改...").parent().slideDown();
                            ucity.parent().remove();
                            uprovince.parent().remove();
                            sex.remove();
                            $(".mfl-lodge-iconsBtn,.mfl-lodge-icons").remove();
                            t.show();
                        },
                        "cancel": function () {
                            ucity.parent().remove();
                            uprovince.parent().remove();
                            sex.remove(); $(".mfl-lodge-iconsBtn,.mfl-lodge-icons").remove();
                            $("#sexicon").show()
                            t.show();
                        }
                    });
                    return false;
                });
                regSetting('AllowGuestViewProfile', '允许游客查看以下资料？', $('<div></div>').insertBefore('.profiles'));
                regSetting('AllowAddFriend', '允许陌生人向我发送好友请求？', $('<div></div>').insertAfter('.avatar'));
                regSetting('AllowGuestViewNotesList', '允许游客查看我收到的留言列表？', $('<div></div>').prependTo('.notes'));
                regSetting('AllowGuestViewFriendsList', '允许游客查看我的好友列表？', $('<div></div>').insertAfter('.friends'));
                regSetting('AllowGuestViewShareList', '允许游客查看我的分享列表？', $('<div></div>').insertBefore('.shares'));
                regSetting('AllowGuestViewCourseList', '允许游客查看我教授的课程列表？', $('<div></div>').insertBefore('.courses'));
                regSetting('AllowGuestViewPaticipatedList', '允许游客查看我学习的课程列表？', $('<div></div>').insertBefore('.pcourses'));
                $('<h3 class="block setting ui-widget-content">显示隐私设定？<input type="checkbox" id="setting" name="setting" /><label for="setting">否</label></h3>').appendTo("#right")
                .children("input").button()
                .click(function () {
                    if ($(this).attr("checked")) {
                        $(this).next().children().text("是");
                        $('div.setting').slideDown();
                    } else {
                        $(this).next().children().text("否");
                        $('div.setting').slideUp();
                    }
                });
                $('<div></div>').insertAfter(".avatar").mflSwfUpload({
                    types: "*.jpg;*.png;*.bmp;*.gif",
                    text: "修改头像",
                    typesDescription: "图像文件",
                    ajaxOptions: {
                        action: "UpdateAvatar",
                        success: function () {
                            MFLNotify("头像修改成功。");
                            $(".avatar").attr("src", "/Styles/loading.gif").css("margin", "94px 60px");
                            setTimeout(function () {
                                $(".avatar").attr("src", "/avatars/" + o.id + ".jpg?t=" + (new Date())).load(function () {
                                    $(this).css("margin", "0");
                                    return false;
                                });
                            }, 3000);
                        }
                    }
                });
            }
            else {
                var denyFunc = function () {
                    MFLAjax({
                        action: 'Deny',
                        loadingJ: $(this),
                        query: {
                            username: o.username
                        },
                        success: function (json) {
                            location.reload();
                        }
                    });
                    return false;
                };
                var b = $('<div class="block addfriend"></div>');
                if (o.isFriend) {
                    b.text(o.sex + "已经是我的好友");
                    b.append($('<a href="#" class="mfl-btn">解除好友关系</a>').button({
                        icons: {
                            primary: 'ui-icon-closethick'
                        }
                    }).click(denyFunc));
                } else {
                    if (_settingsVal.AllowAddFriend || o.isSalutting) {
                        var salFunc = function () {
                            MFLAjax({
                                action: 'Salut',
                                loadingJ: $(this),
                                query: {
                                    username: o.username
                                },
                                success: function (json) {
                                    if (json.salutting) {
                                        MFLDialog("好友请求已发送", "好友请求已发送，等待对方确认即可添加为好友", function () {
                                            location.reload();
                                        });
                                    } else {
                                        MFLDialog("好友已添加", "成功将" + o.sex + "添加为好友", function () {
                                            location.reload();
                                        });
                                    }
                                }
                            });
                            return false;
                        };

                        if (o.isSalutting) {
                            b.append($('<a href="#">同意将' + o.sex + '加为好友</a>').button({
                                icons: {
                                    primary: 'ui-icon-plusthick'
                                }
                            }).click(salFunc));
                            b.append($('<a href="#">拒绝该好友添加请求</a>').button({
                                icons: {
                                    primary: 'ui-icon-closethick'
                                }
                            }).click(denyFunc));
                        } else {
                            if (o.isSalutted) {
                                b.text("您已经向" + o.sex + "发送了好友请求");
                                b.append($('<a href="#" class="mfl-btn">取消</a>').button({
                                    icons: {
                                        primary: 'ui-icon-closethick'
                                    }
                                }).click(denyFunc));
                            } else {
                                b.append($('<a href="#">加为好友</a>').button({
                                    icons: {
                                        primary: 'ui-icon-plusthick'
                                    }
                                }).click(salFunc));
                            }
                        }
                    } else {
                        b.text(o.sex + "不接受陌生人的好友请求");
                    }
                }
                b.insertAfter(".avatar");
            }
            var listFunc = function (c, q, a, l) {
                $("." + c + " > a").toggle(function () {
                    MFLAjax({
                        action: a,
                        loadingJ: $("." + c + " > h3,." + c + " > span.list"),
                        query: q,
                        success: function (json, options) {
                            MFLList({
                                json: json[l],
                                containerJ: $("." + c + " > div.list"),
                                pagerAjaxOptions: options,
                                pagerHolderJ: $("." + c + " > p.list,." + c + "  span.list,." + c + " p.list")
                            });
                            $("." + c + " > div.origin").hide();
                            $("." + c + " .list").show();
                        }
                    });
                    $(this).text("收起");
                    return false;
                }, function () {
                    $(this).text("更多...");
                    $("." + c + " .list").hide();
                    $("." + c + " > div.origin").show();
                    return false;
                });
                $("." + c + "  .list").hide();
            };
            $(".courses .origin,.pcourses .origin").hide();

            listFunc("friends", {
                page: 0,
                "username": o.username
            }, "GetUserFriends", "friends");
            MFLAjax({
                action: "GetUserCourses",
                query: {
                    page: 0,
                    username: o.username
                },
                loadingJ: $(".courses>h3"),
                success: function (json, options) {
                    MFLList({
                        json: json.courses,
                        containerJ: $(".courses > div.list"),
                        pagerAjaxOptions: options,
                        pagerHolderJ: $(".courses>.mfl-list-pages")
                    });
                }
            });
            MFLAjax({
                action: "GetUserPaticipatedCourses",
                query: {
                    page: 0,
                    username: o.username
                },
                loadingJ: $(".pcourses>h3"),
                success: function (json, options) {
                    MFLList({
                        json: json.paticipatedCourses,
                        containerJ: $(".pcourses > div.list"),
                        pagerAjaxOptions: options,
                        pagerHolderJ: $(".pcourses>.mfl-list-pages")
                    });
                }
            });

            MFLAjax({
                action: "GetUserNotes",
                query: {
                    page: 0,
                    username: o.username
                },
                loadingJ: $(".notes>h3"),
                success: function (json, options) {
                    MFLList({
                        json: json.notes,
                        containerJ: $(".notes  .notes-list"),
                        pagerAjaxOptions: options,
                        pagerHolderJ: $(".notes .mfl-list-pages")
                    });
                    if (o.isSelf) {
                        $('.notes-list .mfl-list-item').each(function () {
                            $('<a href="#">[删除]</a>').appendTo($(this).find('.time').append('<br />')).click(function () {
                                var rid = $(this).parent().parent().children('.mfl-alt-id').attr("title");
                                MFLAjax({
                                    action: "DelNote",
                                    query: {
                                        noteId: rid,
                                        username: o.username
                                    },
                                    loadingJ: $(".notes-list"),
                                    success: function () {
                                        location.reload();
                                    }
                                });
                                return false;
                            });
                        });
                    }
                }
            });

        };
        var _settings = new Object();
        var _settingsVal = new Object();
        var regSetting = function (key, text, e) {
            var b = $(e).addClass("block").addClass("setting").addClass("ui-widget-content").text(text)
            .append($('<input type="checkbox"' + (_settingsVal[key] ? ' checked="checked"' : '') + ' id="' + key + '" name="' + key + '" /><label for="' + key + '">' + (_settingsVal[key] ? '是' : '否') + '</label>'));

            _settings[key] = b.children("input").button()
            .click(function () {
                if ($(this).attr("checked")) {
                    $(this).next().children().text("是");
                } else {
                    $(this).next().children().text("否");
                }
                updateSettings();
            });
            b.hide();
        };
        var _settingInited = false;
        var initSetting = function (key, value) {
            _settingsVal[key] = value;
        };

        var updateSettings = function () {
            if (_settingInited) {
                var q = new Object();
                $.each(_settings, function (key, value) {
                    q[key] = Boolean($(this).attr("checked"));
                });
                MFLAjax({
                    action: 'UpdateSettings',
                    query: q,
                    loadingJ: $("#oploading"),
                    success: function () {
                        MFLNotify("成功更改隐私设定");
                    },
                    complete: function () {
                        $("#op").slideUp();
                    }
                });
                $("#optext").text("正在保存隐私设定更改...").parent().slideDown();
            }

        };
        var updateProfile = function () {
            var q = new Object();
            q.num = $(".profiles").children(".mfl-list-item.block").each(function (i) {
                q['name' + i] = $.trim($(this).children("h3").text());
                q['value' + i] = $(this).children("div").data("MFLData-value");
            }).length;
            MFLAjax({
                type: 'post',
                action: 'UpdateProfiles',
                query: q,
                loadingJ: $("#oploading"),
                success: function () {
                    MFLNotify("成功更改个人资料");
                },
                complete: function () {
                    $("#op").slideUp();
                }
            });
            $("#optext").text("正在保存个人资料更改...").parent().slideDown();

        };
        var last = "";
        $(function () {
            setInterval(function () {
                if (location.hash.substr(1) != last) {
                    last = location.hash.substr(1);
                    showMiddle(last);
                }
            }, 50);
        });
        var showMiddle = function (id) {
            $("#middle,#middle2,#middle3,#middle4").slideUp(300, function () {
            });
            $('#wizardNav a').removeClass('ui-state-highlight');
            $('#wizardNav a[href="#' + id + '"]').addClass('ui-state-highlight');
            $(".middle" + id).slideDown(300, function () {
                var w = $(".middle" + id + " .floatfix").width();
                $(".middle" + id + " .floatfix").css("float", "left").width(w);
            });
        };

    </script>
    <style type="text/css">
        span.time
        {
            float: left;
            color: #555;
            font-size: 0.9em;
        }
        span.time a
        {
            color: #555;
            font-size: 0.9em;
        }
        p.info
        {
            font-size: 0.8em;
            color: #555;
        }
        #middle, #middle2, #middle3, #middle0, #middle4
        {
            margin-left: 200px;
            margin-right: 200px;
            padding: 5px 5px 0 5px;
            border: none;
        }
        #left
        {
            border-right: 1px solid #ccc;
        }
        .ucitya
        {
            padding: 5px;
        }
        #middle2, #middle3, #middle4
        {
            display: none;
        }
        #left
        {
            width: 200px;
        }
        #right
        {
            width: 200px;
        }
        .truename
        {
            width: 200px;
            line-height: 2;
        }
        .setting
        {
        }
        .addfriend
        {
            line-height: 2;
        }
        .friends .teacher
        {
            margin: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%this.m.RenderToStream(Response.OutputStream); %>
</asp:Content>
