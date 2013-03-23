<%@ Page Title="安全设置" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="GoClassing.Home.Security.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <script type="text/javascript">
        var last = "!!!!!";
        var refresh = function () {
            MFLAjax({
                action: 'GetMyPwdQuestion',
                loadingJ: $('.wiz'),
                success: function (json) {
                    if (json.pwdQuestion == '') {
                        $('.oldPwdQuestion').val("未设置");
                        var ipt = $('#oldPwdAnswer,#oldPwdAnswer2').val("不需填写");
                        if ($.browser.msie && Number($.browser.version) >= 8) {
                            ipt.attr("readonly", "readonly");
                        }
                    } else {
                        $('.oldPwdQuestion').val(json.pwdQuestion);
                        if ($.browser.msie && Number($.browser.version) >= 8) {
                            $('#oldPwdAnswer,#oldPwdAnswer2').attr("readOnly", false);
                        }
                    }
                    $('#msgType input[value="' + json.msg.type + '"]').trigger('click');
                    $('#msgClock').val(json.msg.clock);
                }
            });
            $('#form1')[0].reset();
        };
        $(function () {
            setInterval(function () {
                if (location.hash.substr(1) != last) {
                    last = location.hash.substr(1);
                    showWizard(last);
                }
            }, 50);
            refresh();
            $(".wizPassword").mflSubmit({
                submit: function () {
                    MFLAjax({
                        action: "UpdatePassword",
                        loadingJ: $('.wizPassword buttonset'),
                        validationFormJ: $('.wizPassword'),
                        success: function () {
                            MFLNotify("修改密码成功。");
                            refresh();
                            location.hash = '';
                        }
                    });
                }
            });
            $(".wizQuestion").mflSubmit({
                submit: function () {
                    MFLAjax({
                        action: "UpdatePwdQuestion",
                        loadingJ: $('.wizQuestion buttonset'),
                        validationFormJ: $('.wizQuestion'),
                        success: function () {
                            MFLNotify("修改安全问题成功。");
                            refresh();
                            location.hash = '';
                        }
                    });
                }
            });
            $(".wizEmail").mflSubmit({
                submit: function () {
                    MFLAjax({
                        action: "UpdateEmail",
                        loadingJ: $('.wizEmail buttonset'),
                        validationFormJ: $('.wizEmail'),
                        success: function () {
                            MFLNotify("修改电子邮件地址成功。");
                            setTimeout(function () {
                                location.href = "/?ReturnUrl=Home";
                            }, 1000);
                        }
                    });
                }
            });
            $(".wizMailing").mflSubmit({
                submit: function () {
                    MFLAjax({
                        action: "UpdateMailing",
                        loadingJ: $('.wizMailing buttonset'),
                        validationFormJ: $('.wizMailing'),
                        success: function () {
                            MFLNotify("修改邮件提醒设置成功。");
                            refresh();
                            location.hash = '';
                        }
                    });
                }
            });
            $('#msgType input').click(function () {
                if ($(this).val() == "2") {
                    $('#msgClock').parent().show();
                } else {
                    $('#msgClock').parent().hide();
                }
            });
        });
        var showWizard = function (c) {
            $(".wizard").slideUp(300, function () {
            });
            $(".wiz" + c).slideDown(300);
        };
    </script>
    <style type="text/css">
        .wizard
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="right" runat="server">
    <div>
        <h3 class="links-header">
            安全设置操作</h3>
        <div class="links-content">
            <a href="#Password" class="ftype createCourseBtn" style="background-position: 0 -1199px;">
                修改密码</a> <a href="#Question" class="ftype" style="background-position: 0 -1154px;">修改安全问题/答案</a>
            <a href="#Email" class="ftype ftype-mail">更换电子邮件地址</a>
            <a href="#Mailing" class="ftype ftype-notify">邮件提醒设置</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wizard wiz block">
        <h3>安全设置：</h3>
        
            请在右侧操作列表中选择一项：
    </div>
    <div class="wizard wizPassword block">
        <h3>
            修改密码：</h3>
        <label for="oldPassword">
            当前密码</label>：<br />
        <input type="password" id="oldPassword" /><br />
        <label for="password1">
            新密码</label>：<br />
        <input type="password" id="password1" /><br />
        <label for="password2">
            确认新密码</label>：<br />
        <input type="password" id="password2" /><br />
        <buttonset><input type="button" value="修改" onclick="MFLSubmit(this);" /><input type="button" value="取消" onclick="location.hash='';" /></buttonset>
    </div>
    <div class="wizard wizQuestion block">
        <h3>
            修改安全问题/答案：</h3>
        当前安全问题：<br />
        <input type="text" class="oldPwdQuestion" readonly="readonly" /><br />
        <label for="oldPwdAnswer">
            当前安全问题答案</label>：<br />
        <input type="text" id="oldPwdAnswer" /><br />
        <label for="pwdQuestion">
            新安全问题</label>：<br />
        <input type="text" id="pwdQuestion" /><br />
        <label for="pwdAnswer">
            新安全问题答案</label>：<br />
        <input type="text" id="pwdAnswer" /><br />
        <label for="pwdAnswer2">
            确认新安全问题答案</label>：<br />
        <input type="text" id="pwdAnswer2" /><br />
        <buttonset><input type="button" value="修改" onclick="MFLSubmit(this);" /><input type="button" value="取消" onclick="location.hash='';" /></buttonset>
    </div>
    <div class="wizard wizEmail block">
        <h3>
            更换电子邮件地址：(*更换电子邮件地址后账号将被暂时锁定，必须验证新的邮件地址才可重新启用账号。)</h3>
        当前安全问题：<br />
        <input type="text" class="oldPwdQuestion" readonly="readonly" /><br />
        <label for="oldPwdAnswer2">
            当前安全问题答案</label>：<br />
        <input type="text" id="oldPwdAnswer2" /><br />
        <label for="oldEmail">
            当前电子邮箱地址</label>：<br />
        <input type="text" id="oldEmail" /><br />
        <label for="email">
            新电子邮箱地址</label>：<br />
        <input type="text" id="email" /><br />
        <label for="email2">
            确认新电子邮箱地址</label>：<br />
        <input type="text" id="email2" /><br />
        <buttonset><input type="button" value="修改" onclick="MFLSubmit(this);" /><input type="button" value="取消" onclick="location.hash='';" /></buttonset>
    </div>
    
    <div class="wizard wizMailing block">
        <h3>
            邮件提醒设置：<br />
            </h3>
            请选择您希望我们给你发送邮件提醒的方式：
	<div id="msgType" class="radio">
		<input type="radio" value="1" id="msgType1" name="msgType" /><label for="msgType1">有新的消息时随时提醒我</label><br />
		<input type="radio" value="2" id="msgType2" name="msgType" /><label for="msgType2">每天固定时间给我发送消息汇总</label><br />
		<input type="radio" value="0" id="msgType3" name="msgType" /><label for="msgType3">不要给我发送提醒邮件</label><br />
	</div>
    <div>
    <label for="msgClock">发送消息汇总的时间：</label><br />每天
    <select id="msgClock">
    <option value="1">凌晨1点</option>
    <option value="2">凌晨2点</option>
    <option value="3">凌晨3点</option>
    <option value="4">早晨4点</option>
    <option value="5">早晨5点</option>
    <option value="6">早晨6点</option>
    <option value="7">早晨7点</option>
    <option value="8">早晨8点</option>
    <option value="9">上午9点</option>
    <option value="10">上午10点</option>
    <option value="11">上午11点</option>
    <option value="12">中午12点</option>
    <option value="13">下午1点</option>
    <option value="14">下午2点</option>
    <option value="15">下午3点</option>
    <option value="16">下午4点</option>
    <option value="17">下午5点</option>
    <option value="18">下午6点</option>
    <option value="19">晚上7点</option>
    <option value="20">晚上8点</option>
    <option value="21">晚上9点</option>
    <option value="22">晚上10点</option>
    <option value="23">晚上11点</option>
    <option value="24">午夜</option>
    </select><br /></div>
        <buttonset><input type="button" value="修改" onclick="MFLSubmit(this);" /><input type="button" value="取消" onclick="location.hash='';" /></buttonset>
    </div>
</asp:Content>
