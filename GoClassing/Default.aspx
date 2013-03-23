<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="GoClassing.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/mfl-submit.js" type="text/javascript"></script>
    <link href="/Styles/root-pages.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.jplayer.min.js" type="text/javascript"></script>
    <link href="/Styles/jplayer.blue.monday.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .wizard
        {
            width: 220px;
            float: right;
        }
        .tooltip
        {
            display:none;
            font-size:0.9em;
        }
        div.jp-video-700i
        {
           width:700px;
        }
        div.jp-video-700i div.jp-video-play 
        {
	        height:420px;
        }
        div#jp_container_1
        {
            position:relative;
            margin-bottom:10px;
            border:none;
            
        }
        div.jp-interface
        {
            position:absolute;
            bottom:0px;
        }
    </style>
    <script type="text/javascript">
        openLogin = function (jsNext) {
            wizard("login");
            if ($.isFunction(jsNext)) {
                redirect = jsNext;
            }
            $("#loginname").focus();
        };
        var wizard = function (tab) {
            $(".wizard").children(":not(#notifications)").slideUp();
            $(".wizard").children("."+tab).slideDown();
        };
        $(function () {
            $(".register").mflSubmit({
                submit: function () {
                    MFLAjax({
                        loadingJ: $(".register input[type=submit]"),
                        validationFormJ: $(".register"),
                        query: {
                            regusername: $("#regusername").val(),
                            regemail: $("#regemail").val(),
                            password1: $("#password1").val(),
                            password2: $("#password2").val(),
                            agreement: $("#agreement").attr("checked"),
                            redirect: redirect
                        },
                        action: "Register",
                        success: function () {
                            wizard("veryfication");
                        }
                    });
                }
            });
            $(".login").mflSubmit({
                submit: function () {
                    MFLAjax({
                        loadingJ: $(".login input[type=submit]"),
                        "validationFormJ": $(".login"),
                        "query": {
                            username: $("#username").val(),
                            password: $("#password").val(),
                            remember: $("#remember").attr("checked"),
                            redirect: redirect
                        },
                        "action": "Login",
                        "success": function (json) {
                            if ($.isFunction(redirect)) {
                                redirect();
                            } else {
                                location.href = redirect;
                            }
                        }
                    });
                }
            });
            $(".r1").mflSubmit({
                submit: function () {
                    MFLAjax({
                        loadingJ: $(".r1 input[type=submit]"),
                        validationFormJ: $(".r1"),
                        query: {
                            rloginname: $("#rloginname").val()
                        },
                        action: "GetPwdQuestion",
                        success: function (json) {
                            $("#r2loginname").text($("#rloginname").val());
                            $("#r2pwdquestion").text(json.pwdQuestion);
                            wizard("r2");
                        }
                    });
                }
            });
            $(".r2").mflSubmit({
                submit: function () {
                    MFLAjax({
                        loadingJ: $(".r2 input[type=submit]"),
                        validationFormJ: $(".r2"),
                        query: {
                            r2loginname: $("#rloginname").val(),
                            oldPwdAnser: $("#oldPwdAnser").val()
                        },
                        action: "RetrievePassword",
                        success: function (json) {
                            $("#r3email").text(json.mail);
                            wizard("r3");
                        }
                    });
                }
            });
            $(".r3").mflSubmit({
                submit: function () {
                    location.href = "/?ReturnUrl=" + encodeURI("/");
                }
            });

            $(".veryfication").mflSubmit({
                submit: function () {
                    wizard("login");
                }
            });
            $(".register input").focus(function () {
                $(this).nextAll("div.tooltip").eq(0).slideDown();
            }).blur(function () {
                $(this).nextAll("div.tooltip").eq(0).slideUp();
            });
            $("#jquery_jplayer_1").jPlayer({
                ready: function () {
                    $(this).jPlayer("setMedia", {
                        m4v: "/Styles/intro.mp4",
                        poster: "/Styles/intro.mp4.jpg"
                    });
                },
                solution:'flash',
                swfPath: "/Flash",
                supplied: "m4v",
                size: {
                    width: "700px",
                    height: "420px",
                    cssClass: "jp-video-700i"
                },
                pause: function () {
                    $('.jp-video-play').show();
                },
                click: function () {
                    $(this).jPlayer('pause');
                },
                play: function () {
                    $('#jp_flash_0').css({ width: 700, height: 420 });
                    $('img#jp_poster_0').hide();
                },
                ended: function () {
                    jPlayerEnded();
                }
            });
            $('.jp-video-play').click(function () {
                $('.jp-interface').slideDown();
                $('.jp-video-play').hide();
            });
        });
        var jPlayerEnded = function () {
            $('#jp_flash_0').css({ width: 0, height: 0 });
            $('.jp-interface').slideUp();
            $('img#jp_poster_0').show();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wizard">
        <div id="notifications">
        </div>
        <div class="register">
            <h3>
                现在就加入上课网：</h3>
            <label for="regusername">
                新用户名</label>
            ：<br />
            <input id="regusername"  type="text" name="username" /><br />
            <div class="tooltip ui-state-highlight">用户名由字母、数字、下划线和点构成，为您的个人主页地址的一部分，如：<br />www.Goclassing/T/<b>abc123</b></div>
            <label for="regemail">
                电子邮箱</label>
            ：<br />
            <input id="regemail" type="text" name="email" /><br />
            <div class="tooltip ui-state-highlight">您的电子邮箱不会被其他用户查看</div>
            <label for="password1">
                密码</label>
            ：<br />
            <input id="password1" type="password" name="password" /><br />
            <div class="tooltip ui-state-highlight">密码的最小长度为6位</div>
            <label for="password2">
                确认密码</label>
            ：<br />
            <input id="password2" type="password" name="passwordConfirm" /><br />
            <div class="tooltip ui-state-highlight">请再输入一次密码</div>
            <br />
            <input id="agreement" type="checkbox" name="remember" /><label class="agreement"
                for="agreement">
                我接受</label><a href="Eula/" style="font-size:0.8em;">《上课网免费服务使用条款》</a><br />
            <input value="注册" type="submit" /><br />
            <a href="#" onclick="openLogin();return false;">使用已有账户登陆</a></div>
        <div class="login" style="display: none">
            <h3>
                使用已有账户登陆上课网：</h3>
            <label for="username">
                用户名/电子邮箱</label>
            ：<br />
            <input id="username" type="text" name="username" /><br />
            <label for="password">
                密码</label>
            ：<br />
            <input id="password" type="password" name="password" /><br />
            <input id="remember" type="checkbox" name="remember" /><label class="remember"
                for="remember">
                在这台电脑上记住我</label>
            <input value="登陆" type="submit" /><br />
            <a href="#" onclick="wizard('register');return false;">还未注册？</a><br />
            <a href="#" onclick="wizard('r1');return false;">忘记密码？</a>
        </div>
        <div class="r1" style="display: none">
            <h3>
                重设您的上课网密码-第一步：</h3>
            <label for="rloginname">
                用户名/电子邮箱</label>
            ：<br />
            <input id="rloginname" type="text" name="loginname" /><br />
            <input value="下一步" type="submit" /><br />
            <a href="#" onclick="wizard('login');return false;">取消</a></div>
        <div class="r2" style="display: none">
            <h3>
                重设您的上课网密码-第二步：</h3>
            <label for="r2loginname">
                用户名/电子邮箱</label>
            ：<br />
            <span id="r2loginname"></span>
            <br />
            <label for="r2pwdquestion">
                密码安全问题</label>
            ：<br />
            <span id="r2pwdquestion"></span>
            <br />
            <label for="oldPwdAnser">
                密码安全问题答案</label>
            ：<br />
            <input id="oldPwdAnser" type="text" name="loginname" /><br />
            <input value="将密码发送到我的邮箱" type="submit" /><br />
            <a href="#" onclick="wizard('login');return false;">取消</a></div>
        <div class="r3" style="display: none">
            <h3>
                重设您的上课网密码-成功：</h3>
            您的账户已经成功重设密码，新的密码发送到了您的邮箱(<span id="r3email"></span>)，请注意查收。
            <input value="确定" type="submit" /><br />
        </div>
        <div class="veryfication" style="display: none">
            <h3>
                现在就加入上课网：</h3>
                一封邮件已经发送到了您的电子邮箱，请打开邮件中附带的链接以验证您的电子邮箱。
            <input value="确定" type="submit" /><br />
        </div>
    </div>
    <div id="jp_container_1" class="jp-video ">
    <div class="jp-type-single">
      <div id="jquery_jplayer_1" class="jp-jplayer"></div>
      <div class="jp-gui">
        <div class="jp-video-play">
          <a href="javascript:;" class="jp-video-play-icon" tabindex="1">play</a>
        </div>
        <div class="jp-interface" style="display:none;">
          <div class="jp-progress">
            <div class="jp-seek-bar">
              <div class="jp-play-bar"></div>
            </div>
          </div>
          <div class="jp-current-time"></div>
          <div class="jp-duration"></div>
          <div class="jp-controls-holder" style="clear:none;margin-top:11px">
            <ul class="jp-controls">
              <li><a href="javascript:;" class="jp-play" tabindex="1">播放</a></li>
              <li><a href="javascript:;" class="jp-pause" tabindex="1">暂停</a></li>
              <li><a href="javascript:;" class="jp-stop" onclick="jPlayerEnded()" tabindex="1">停止</a></li>
              <li><a href="javascript:;" class="jp-mute" tabindex="1" title="静音">静音</a></li>
              <li><a href="javascript:;" class="jp-unmute" tabindex="1" title="关闭静音">关闭静音</a></li>
              <li><a href="javascript:;" class="jp-volume-max" tabindex="1" title="音量最大">音量最大</a></li>
            </ul>
            <div class="jp-volume-bar">
              <div class="jp-volume-bar-value"></div>
            </div>
            <ul class="jp-toggles">
              <li><a href="javascript:;" class="jp-full-screen" tabindex="1" title="全屏显示">全屏显示</a></li>
              <li><a href="javascript:;" class="jp-restore-screen" tabindex="1" title="恢复正常显示">恢复正常显示</a></li>
            </ul>
          </div>
        </div>
      </div>
      <div class="jp-no-solution">
        <span>无法显示该视频</span>
        您需要升级您的<a href="http://get.adobe.com/flashplayer/" target="_blank">Flash插件</a>以播放这段视频内容.
      </div>
    </div>
  </div>
</asp:Content>
