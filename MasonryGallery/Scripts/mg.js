var login;
$(function () {
    {//dom init
        if ($.browser.msie && Number($.browser.version) < 8) {
            jQuery.fx.off = true;
        }
        $('.pre-loading').remove();
        if (data.Logon) {
            $('.btn-logout,.btn-login').toggle();
        }
        $('.btn-login').click(function () {
            var f = $('<form action="javascript:login();" style="float:left;"><span class="btn btn-password"></span><input style="float:left;padding:5px;" type="password" id="pwd" /><input class="btn btn-ok" type="submit" value="Login" /></form>')
            .hide()
            .insertAfter(this)
            .show();
            $('#pwd').focus();
            $(this).hide();
            login = function () {
                MFLAjax({
                    loadingJ: $('.mouse-loading'),
                    action: "Login",
                    query: {
                        'username': data.Username,
                        'password': $('#pwd').val()
                    },
                    success: function () {
                        MFLNotify('Welcome! :D');
                        location.reload();
                    }
                });
                $('.btn-login').show();
                f.remove();
            }
            return false;
        });
        var msg = $('<div class="mouse"><div class="mouse-msg"></div><div class="mouse-loading"></div></div>').appendTo('body');
        $('body').mousemove(function (e) {
            var pageCoords = "( " + e.pageX + ", " + e.pageY + " )";
            var clientCoords = "( " + e.clientX + ", " + e.clientY + " )";
            msg.css({ top: e.pageY + 1, left: e.pageX + 1 });
        });
    }
    //selection prevention
    {
        $('body').select(function () {
            return false;
        });
    }
    {//MFLReplacing
        var timerMsg;
        MFLNotify = function (msg) {
            $('.mouse-msg').html('<span class="btn btn-msg"></span>' + msg).fadeIn();
            if (timerMsg) {
                clearTimeout(timerMsg);
            }
            timerMsg = setTimeout(function () {
                $('.mouse-msg').fadeOut(MGAnimateDuration);
            }, 3000);
        };
        MFLDialog = function (title, cont, next, ok, o) {
            if (typeof (cont) == 'string') {
                cont = $('<div></div>').html(cont).appendTo('body');
            }
            $.colorbox({
                href: cont,
                inline: true,
                onClosed: function () {
                    if ($.isFunction(next)) {
                        next();
                    }
                    cont.remove();
                },
                onComplete: function () {
                    _MGDebug('onComplete');
                    if ($.isFunction(ok)) {
                        cont.submit(function () {
                            cont.parent().siblings('#cboxPrevious').trigger('click');
                            return false;
                        });
                        if (cont.find('input').length) {
                            cont.find('input').eq(0).focus();
                        }
                        cont.parent().siblings('#cboxPrevious').show().addClass('btn').addClass('btn-ok').unbind('click').click(function () {
                            _MGDebug('ok');
                            $.proxy(ok, cont)();
                            $.colorbox.close();
                            return false;
                        });
                    }
                    return false;
                }
            });

        };
    }
    {//init
        {//auto textarea
            $('textarea').autoTextarea();
        }
        {//leave comment
            $('.btn-newcomment').click(function () {
                var album = $(this).parent().parent().prev();
                var form = $(this).parent();
                var url = MGGetOriginUrl(album.children('.mainpic').attr('src'));
                MFLAjax({
                    action: "LeaveComment",
                    query: {
                        url: url,
                        email: $(this).siblings('.email').val(),
                        name: $(this).siblings('.name').val(),
                        content: $(this).siblings('textarea').val()
                    },
                    loadingJ: $('.mouse-loading'),
                    success: function (json) {
                        form[0].reset();
                        form.siblings('.comment').fadeIn(MGAnimateDuration).children('.magic-handler').remove();
                        $(json.Html).insertBefore(form).hide().fadeIn(MGAnimateDuration2);
                    }
                });
                return false;
            });
        }
        {//comments handle
            $('<a href="#" class="comments-handle"></a>').appendTo('.album').click(function () {
                var album = $(this).parent();
                album.next('.comments').toggle();
                if (album.next('.comments').css('display') == 'block') {//magic scoller
                    var init = 1;
                    var next = 5;
                    var func = function () {
                        var c = album.next('.comments').children('.comment');
                        c.children('.magic-handler').remove();
                        if (!c.length) {
                            return;
                        }
                        var i = 0;
                        for (; i < init && i < c.length; i++) {
                            $(c[i]).show();
                        }
                        var last = c[i - 1];
                        var count = 0;
                        for (; i < c.length; i++) {
                            count++;
                            $(c[i]).hide();
                        }
                        if (!count) {
                            count = 'no';
                        }
                        $('<a href="#" class="magic-handler"></a>').text(count + ' more..').click(function () {
                            init += next;
                            func();
                            return false;
                        }).appendTo(last);
                    };
                    func();
                }
                return false;
            }).each(function (i, h) {
                $(h).html('<span class="btn btn-comments"></span>' + $(h).parent().next('.comments').find('.comment').length + ' comments..');
            });
        }
        {//subpics slide
            $('.subpics-handle')
            .click(function () {
                $(this).siblings('.subpics').toggle();
                $(this).siblings('.subpics').find('img.subpic').each(function (i, img) {
                    $(img).attr('src', $(img).attr('alt'));
                });
            });
            $('.subpics');
            setTimeout(function () {
                $('.subpics').each(function (i, subpics) {
                    if ($(subpics).find('.subpic').length) {
                        return;
                    }
                    $(subpics).siblings('.subpics-handle').remove();
                });
            }, 300);
        }
        $('.album').css({ 'marginRight': data.Margin, 'marginBottom': data.Margin }); //margin
        $('a.title-text').each(function () {//replace link to span (for editting features)
            $(this).replaceWith($('<span></span>').text($(this).text()).attr('class', $(this).attr('class')));
        });
        //bind overlay event supports
        $(window).resize(function () {
            var w = $(window).height();
            var b = $('body').height();
            $('.overlay').css({
                'zIndex': 10,
                'height': w > b ? w : b,
                'opacity': 0.5
            });
            if (data.BodyWidth == 'auto') {
                var ww = $(window).width();
                var cw = data.Width + (data.Margin + data.BorderWidth) * 2;
                $('.center').width(ww - ww % cw);
            }
            if ($.browser.msie && Number($.browser.version) < 8) {
                $('.overlay').hide();
            }
        });

        $('.center').width(data.BodyWidth);
        $(window).trigger('resize');
        //title slide
        $('.title').show;
        $('.album').hover(function () {
            $(this).children('.title').show();
            $(this).children('.subpics-handle').show();
        }, function () {
            if (!$(this).hasClass('album-open')) {
                $(this).children('.title,.subpics-handle').hide();
            }
        }).children('.title').hide();
        $('.subpics').css('opacity', data.Opacity).hide();
        //subpic open event
        $('.subpic').click(function () {
            var album = $(this).parent().parent().parent();
            var subpic = $(this);
            var size = {
                width: album.width()
            };
            size.height = size.width * $(this).height() / $(this).width();
            _MGDebug('Subpic size:');
            _MGDebug(size);
            if (subpic.parent().hasClass('subpic-active')) {
                MGCloseSubpic(function () { });
                return;
            }
            MGCloseSubpic(function () {
                //album.children('.subpics').hide();
                {//create overlay
                    $('<div class="overlay"></div>').appendTo('body');
                    $(window).trigger('resize');
                }
                album.children('.mainpic').hide();
                album.resizable('option', 'delay', 60000);
                subpic
                .parent()
                .addClass('subpic-active');
                _MGDebug(subpic.parent()[0]);
                subpic
                .clone()
                .addClass('subpic-open')
                .css(size)
                .hide()
                .appendTo(album)
                .fadeIn(MGAnimateDuration)
                .click(function () {
                    _MGDebug('“subpic-open” clicked');
                    $('.subpic-active img').trigger('click');
                    return false;
                }).attr('src', MGGetOriginUrl(subpic.attr('src')));
                size.height = 'auto';
                album.css(size);
            });
        });
        {//comments positioning timer
            setInterval(function () {
                var c = $('.comments:visible');
                if (c.length) {
                    var offset = c.prev().offset();
                    var center = c.prev().offsetParent().offset();
                    c.css({
                        top: offset.top - center.top + c.prev().height(),
                        left: offset.left - center.left
                    }).width(c.prev().width() - 20);
                }
            }, 50);
        }
        {//clash
            var clash;
            $('body').click(function () {
                if (!MGMasonryMode) {
                    return;
                }
                _MGDebug('Body clicked!');
                clash = setTimeout(function () {
                    MGCloseAllGallery(function () { $('.gallery').masonry('reload'); });
                }, 100);
            });
            $('.tools,.tools-theme,.album,.subpic-open,.comments,.colorpicker,#colorbox').live('click', function () {
                setTimeout(function () {
                    if (clash) {
                        _MGDebug('Clash cleared!');
                        clearTimeout(clash);
                    }
                }, 1);
            });
        }
        if (data.Logon) {
            MGMasonryLogin();
        }
        MGSwitchMode();
        {//debug
            _MGLog.push('Debug start.');
        }
    }

});
var MGAnimateDuration = 150;
var MGAnimateDuration2 = 300;
var MGGetOriginUrl = function (url,gettingThumb) {
    for (var i = 0; i < data.ImageMap.length; i++) {
        if (data.ImageMap[i].Url == url||data.ImageMap[i].UrlOrigin == url) {
            return gettingThumb ? data.ImageMap[i].Url : data.ImageMap[i].UrlOrigin;
        }
    }
};
var _MGDebug = function (msg) {
    if (_MGLog.length) {
        if (typeof (msg) == 'Object') {
            var p = ['nodeName', 'nodeType', 'id', 'className', 'width', 'height'];
            var str = "Object:{";
            $.each(p, function (i, v) {
                if (typeof (msg[v]) == 'string') {
                    str += v + ':' + '\'' + msg[v] + '\',';
                }
            });
            if (str.substr(str.length - 1) == ',') {
                str = str.substr(0, str.length - 1);
            }
            str += '}';
            _MGDebug(str);
            return;
        }
        _MGLog.push(msg);
        var c = {
            log: function (msg) {
                $('<p>' + msg + '</p>').appendTo('.debug-log');
            }
        };
        c.log(msg);
    }
};
var _MGLog = [];