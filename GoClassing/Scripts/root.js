var storage = function (t, u, p) {
    $('.storage-text').text("储存空间已使用" + u + "/" + t + "(" + p + "%)");
    $('.storage').height(30).progressbar({
        value: p
    });
};
var redirect = null;
var openLogin = function (callback) {
    $("#notifications").attr("id", "notificationsOld");
    $("#username").attr("id", "usernameOld");
    $("#password").attr("id", "passwordOld");
    MFLDialog("登陆", $('<div class="loginDlg"></div>')
        .append($('<div id="notifications"></div>'))
        .append($('<p><label for="username">用户名/电子邮箱</label>：<input type="text" name="username" id="username" /></p>'))
        .append($('<p><label for="password">密码</label>：<input type="password" name="password" id="password" /></p>'))
        .append($('<p><label for="remember">在这台电脑上记住我</label>：<input type="checkbox" name="remember" id="remember" /></p>'))
        .append($('<p style="text-align:right;color:#7E9C23;"><a href="/?PasswordRetrieval=true">忘记密码？</a></p>'))
        .append($('<p style="text-align:right;color:#7E9C23;"><a href="/">还未注册？</a></p>')), function () {
            $("#notificationsOld").attr("id", "notifications");
            $("#usernameOld").attr("id", "username");
            $("#passwordOld").attr("id", "password");
        }, function (btnset) {
            if (!redirect) {
                redirect = (location.href == "/" || location.href == "/Default.aspx") ? "/Home/" : null;
            }
            MFLAjax({
                validationFormJ: $(".loginDlg"),
                loadingJ: $(btnset),
                query: {
                    username: $("#username").val(),
                    password: $("#password").val(),
                    remember: $("#remember").attr("checked"),
                    redirect: redirect
                },
                action: "Login",
                success: function () {
                    if (!$.isFunction(callback)) {
                        if (!redirect) {
                            location.reload();
                            return;
                        }
                        location.href = redirect;
                        return;
                    }
                    $("#dlgForm").dialog("close").dialog("destroy").remove();
                    callback();
                }
            });
            return false;
        });
    };
var gcSearch = null;
$(function () {
    $(window).unload(function () {

        var overlay = $('<div></div>').addClass('ui-widget-overlay').appendTo('body').css({ height: $('body').height() > $(window).height() ? $('body').height() : $(window).height(), zIndex: 999 }).fadeIn();
        $('<img src="/styles/loading.gif" style="position:absolute;top:240px;left:410px;z-index:1001;" />').prependTo('#main');
    });
    var keypressed;
    var last = null;
    setInterval(function () {
        if (gcSearch == null) {
            return;
        }
        if (last == null) {
            $(".searchText").val(decodeURIComponent(location.hash.substr(1)));
            last = $(".searchText").val();
        }
        if ($(".searchText").val() == last) {
            return;
        }
        last = $(".searchText").val();
        if (keypressed) {
            clearTimeout(keypressed);
        }
        keypressed = setTimeout(function () {
            var arr = $(".searchText").val().split(' ');
            arr = $.map(arr, function (e, i) {
                var e = $.trim(e);
                if (e != "") {
                    return decodeURIComponent(e);
                }
            });

            location.hash = arr.join(',');
            gcSearch(location.hash.substr(1));
        }, 500);
    }, 50);
    $('input[type="text"]:not([readonly="readonly"]),input[type="password"],textarea').live("focus", function () {
        if (!$(this).attr('readonly')) {
            $(this).addClass("active");
        }
    }).live("blur", function () {
        $(this).removeClass("active");
    });

    $('#LoginView1_LoginName').button({
        "icons": {
            "secondary": "ui-icon-triangle-1-s"
        }
    });
    var msgNum = Number($('.msgBtn').text());
    if (msgNum) {
        $('.msgBtn').button();
    }
    $('.msgBtn').html('&nbsp;').append($('<img src="/Styles/msg' + (msgNum ? '-new' : '') + '.gif" />')).click(function () {
        if (msgTimer) {
            clearInterval(msgTimer);
            msgTimer = null;
        }
        msgTimer = setInterval(function () {
            $('.msg').position({
                of: $(".msgBtn"),
                my: 'right top',
                at: 'right bottom',
                collision: 'none none',
                offset: '0 0'
            }).show()
        }, 30);
        MFLAjax({
            action: "GetMessages",
            loadingJ: $('.msg .msg-list'),
            success: function (json, o) {
                MFLList({
                    containerJ: $('.msg .msg-list'),
                    json: json
                });
                $('.msg-list .mfl-list-item a.ok').button({
                    icons: {
                        primary: 'ui-icon-check'
                    }
                }).click(function () {
                    MFLAjax({
                        action: "ReadMessage",
                        query: {
                            id: Number($(this).siblings('.mfl-data-id').text())
                        },
                        loadingJ: $(this).siblings('.mfl-content'),
                        success: function () {
                            $(this).parent().remove();
                        }
                    });
                });
                $('.msg-list .mfl-list-item a.check').button({
                    icons: {
                        primary: 'ui-icon-arrowthick-1-w'
                    }
                }).click(function () {
                    location.href = $(this).siblings('.mfl-content').attr('href');
                });
            }
        });
    });
    //    if (msgNum) {
    //        $('.msgBtn').trigger('click');
    //    }
    $(".msgClose").button({
        icons: {
            primary: "ui-icon-close"
        }
    }).click(function () {
        clearInterval(msgTimer);
        msgTimer = null;
        $('.msg').hide();
        return false;
    });
    $(".loginButton").button({
        "icons": {
            "secondary": "ui-icon-locked"
        }
    }).click(function () {
        openLogin();
        return false;
    });
    $(".lbp0").button({
        "icons": {
            "primary": "ui-icon-home"
        }
    });
    $(".lbp1").button({
        "icons": {
            "primary": "ui-icon-person"
        }
    });
    $(".lbp2").button({
        "icons": {
            "primary": "ui-icon-wrench"
        }
    });
    $(".lbp3").button({
        "icons": {
            "primary": "ui-icon-key"
        }
    });
    $(".lbp4").button({
        "icons": {
            "primary": "ui-icon-unlocked"
        }
    }).click(function () {
        $(".lbp0,.lbp1,.lbp3,.lbp3").remove();
        MFLAjax({
            "action": "Logout",
            "loadingJ": $("#LoginView1_LoginName"),
            "success": function () {
                location.reload();
            }
        });
        return false;
    });
    $(".loginButtonPanel a:not(#LoginView1_LoginName)").hide();
    $(".loginButtonPanel").hover(function () {
        $(".loginButtonPanel a").show();
    }, function () {
        $(".loginButtonPanel a:not(#LoginView1_LoginName)").hide();
    });
    $('input[type="submit"],input[type="button"]').button().css({ "marginLeft": 3, "marginTop": 10 });
    var h = location.href;
    h = h.substr(10);
    h = h.substr(h.indexOf('/'));
    var i = h.toLowerCase().indexOf('default.aspx');
    if (i > 0) {
        h = h.substr(0, i);
    }
    if (h.toLowerCase().substr(0, 3) == "/c/") {
    } else {
        $('#left .links-header a[href="' + h + '"]').addClass("active");
    }
});
var msgTimer;