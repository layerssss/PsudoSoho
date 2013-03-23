
var MGMasonryLogon = false;
var MGMasonryLogin = function () {
    if (!MGMasonryLogon) {
        MGMasonryLogon = true;
        var colorChangeTimer;
        var colorChangeFunc = function () {
            if (colorChangeTimer) {
                clearTimeout(colorChangeTimer);
                colorChangeTimer = null;
            }
            colorChangeTimer = setTimeout(function () {
                $('.ColorStyleLink').remove();
                $('<link class="ColorStyleLink" href="' + data.BaseUrl + '/ColorStyles/'
                + $('#color1').text() + $('#color2').text() + $('#color3').text() + data.MarginP + data.BorderWidthP
                + '.css" rel="stylesheet" type="text/css" />').appendTo('head');
                MFLAjax({
                    action: "SetColors",
                    query: {
                        color1: $('#color1').text(),
                        color2: $('#color2').text(),
                        color3: $('#color3').text()
                    },
                    loadingJ: $('.mouse-loading'),
                    success: function () {
                    }
                });
            }, 1000);
        };
        {//tools positioning timer
            $('<div class="tools" style="border-bottom-left-radius:30px;"></div>')
            .appendTo('body')
            .append('<a class="handle-tools" href="#">&lt;&lt;</a>')
            .append('<a class="btn-reg btn" href="#">Get&nbsp;a&nbsp;NEW&nbsp;gallery!</a>')
            .append('<a class="btn-password btn" href="#">Change&nbsp;PASSWORD.</a>')
            .append('<a class="btn-options btn" href="#">Modify&nbsp;OPTIONS.</a>')
            .append('<a class="btn-size btn" href="#">Change&nbsp;SIZE&nbsp;settings.</a>')
            .append('<a class="btn-logout btn" href="#">Log&nbsp;Out</a>')
            .append('<a class="btn-themes" href="#">▼</a>')
            .append('<a id="color1" href="#" style="background-color:#' + data.Color1 + '">' + data.Color1 + '</a>')
            .append('<a id="color2" href="#" style="background-color:#' + data.Color2 + '">' + data.Color2 + '</a>')
            .append('<a id="color3" href="#" style="background-color:#' + data.Color3 + '">' + data.Color3 + '</a>');
            setInterval(function () {
                $('.tools').css({
                    left: $(window).scrollLeft() + $(window).width() - $('.tools').width(),
                    top: $(window).scrollTop(),
                    position: 'absolute'
                });
                $('.tools-theme').css({
                    left: $(window).scrollLeft() + $(window).width() - $('.tools-theme').width(),
                    top: $(window).scrollTop(),
                    position: 'absolute'
                });
                $('.theme:visible').each(function (i, theme) {
                    $(theme).children('.btn-selector-active').removeClass('btn-selector-active');
                    var color1 = $(theme).children('.color').eq(0).text();
                    var color2 = $(theme).children('.color').eq(1).text();
                    var color3 = $(theme).children('.color').eq(2).text();
                    if ($('#color1').text() == color1 && $('#color2').text() == color2 && $('#color3').text() == color3) {
                        $(theme).children('.btn-selector').addClass('btn-selector-active');
                    }
                });
            }, 100);
            $('.theme>.btn-selector').click(function () {
                var color1 = $(this).siblings('.color').eq(0).text();
                var color2 = $(this).siblings('.color').eq(1).text();
                var color3 = $(this).siblings('.color').eq(2).text();
                $('#color1').text(color1).css('background-color', '#' + color1);
                $('#color2').text(color2).css('background-color', '#' + color2);
                $('#color3').text(color3).css('background-color', '#' + color3);
                colorChangeFunc();
            });
            $('.handle-tools').toggle(function () {
                $('.tools').animate({ width: 600 }, MGAnimateDuration2, function () {
                    $('.tools').css('overflow', 'visible');
                });
                $(this).text('>>');
            }, function () {
                if ($('.tools-theme').css('display') == 'block') {
                    $('.btn-themes').trigger('click');
                }
                $('.tools').animate({ width: 30 }, MGAnimateDuration2, function () {
                    $('.tools').css('overflow', 'hidden');
                });
                $(this).text('<<');
            });
            $('.btn-themes').toggle(function () {
                $('.tools-theme').slideDown(MGAnimateDuration);
                $(this).text('▲');
            }, function () {
                $('.tools-theme').slideUp(MGAnimateDuration);
                $(this).text('▼');
            });
            var coloring;
            $('#color1,#color2,#color3').ColorPicker({
                onBeforeShow: function () {
                    var str = $(this).text();
                    $(this).ColorPickerSetColor(str);
                    coloring = this;
                },
                onChange: function (hsb, hex, rgb) {
                    $(coloring).css('backgroundColor', '#' + hex);
                    $(coloring).text(hex);
                    {//switching theme
                        colorChangeFunc();
                    }
                }, onSubmit: function (hsb, hex, rgb, el) {
                    $(el).val(hex);
                    $(el).ColorPickerHide();
                }
            });

            $('.btn-logout').click(function () {
                MFLAjax({
                    loadingJ: $('.mouse-loading'),
                    action: "Logout",
                    query: new Object(),
                    success: function () {
                        MFLNotify('Goodbye! :D');
                        location.reload();
                    }
                });
                return false;
            });
            var formPassword = MFLForm({
                items: [{
                    name: 'newPassword', type: 'password',
                    text: 'New password:<span class="btn btn-password"></span>'
                }, {
                    name: 'newPassword2', type: 'password',
                    text: 'Confirm new password:<span class="btn btn-password"></span>'
                }]
            }).appendTo('body').addClass('cboxform');
            var formPasswordData = formPassword.data("MFLFormData");
            $('.btn-password').click(function () {
                MFLDialog("", formPassword, null, function () {
                    var formdata = formPasswordData();
                    MFLAjax({
                        action: "SetPassword",
                        query: formdata,
                        loadingJ: $('.mouse-loading'),
                        success: function (json) {
                            location.reload();
                        }
                    });
                });
            });

            var formReg = MFLForm({
                items: [{
                    name: 'username',
                    type: 'text',
                    text: 'New username:<span class="btn btn-name"></span>'
                }, {
                    name: 'password',
                    type: 'password',
                    text: 'New password:<span class="btn btn-password"></span>'
                }, {
                    name: 'password2',
                    type: 'password',
                    text: 'Confirm new password:<span class="btn btn-password"></span>'
                }]
            }).appendTo('body').addClass('cboxform');
            var formRegData = formReg.data("MFLFormData");
            $('.btn-reg').click(function () {
                MFLDialog("", formReg, null, function () {
                    MFLAjax({
                        action: "Register",
                        query: formRegData(),
                        loadingJ: $('.mouse-loading'),
                        success: function (json) {
                            location.href = json.newUrl;
                        }
                    });
                });
            });

            var formOptions = MFLForm({
                items: [{
                    name: 'title',
                    type: 'text',
                    text: 'Title&nbsp;in&nbsp' + data.LangString + ':<span class="btn-tag btn"></span>',
                    value: data.Title
                }]
            }).appendTo('body').addClass('cboxform');
            var formOptionsData = formOptions.data("MFLFormData");
            $('.btn-options').click(function () {
                MFLDialog("", formOptions, null, function () {
                    var formdata = formOptionsData();
                    formdata.langCode = data.LangCode;
                    MFLAjax({
                        action: "SetOptions",
                        query: formdata,
                        loadingJ: $('.mouse-loading'),
                        success: function (json) {
                            location.reload();
                        }
                    });
                });
            });
            var slideEFunc = function () {
                var step = $('#margin').next().slider('option', 'value') + $('#borderWidth').next().slider('option', 'value');
                step *= 2;
                step += $('#width').next().slider('option', 'value');
                var curwidth = $('#bodyWidth').val();
                if (curwidth != 'auto') {
                    curwidth = Number(curwidth);
                    var newWidth = Math.floor(Math.round(curwidth / step)) * step;
                    $('#bodyWidth').val(String(newWidth)).next().slider('enable').slider('option', {
                        'min': step,
                        'step': step,
                        'value': newWidth
                    });
                } else {
                    $('#bodyWidth').next().slider('disable');
                }
            };

            var formSize = MFLForm({
                items: [{
                    name: 'margin',
                    type: 'slider',
                    text: 'Margin:',
                    value: data.Margin,
                    option: {
                        min: 0,
                        max: 30
                    },
                    events: {
                        slidechange: slideEFunc
                    }
                }, {
                    name: 'width',
                    type: 'slider',
                    text: 'Column:',
                    value: data.Width,
                    option: {
                        min: 80,
                        max: 300,
                        step: 10
                    },
                    events: {
                        slidechange: slideEFunc
                    }
                }, {
                    name: 'borderWidth',
                    type: 'slider',
                    text: 'Border:',
                    value: data.BorderWidth,
                    option: {
                        min: 0,
                        max: 30
                    },
                    events: {
                        slidechange: slideEFunc
                    }
                }, {
                    name: 'bodyWidth',
                    type: 'slider',
                    text: 'Width:',
                    value: data.BodyWidth,
                    option: {
                        min: 100,
                        max: 1400
                    }
                }, {
                    name: 'fullscreen',
                    type: 'checkbox',
                    text: 'Full screen:',
                    value: data.BodyWidth == 'auto',
                    events: {
                        click: function () {
                            setTimeout(function () {
                                if ($('#fullscreen:checked').length) {
                                    $('#bodyWidth').val('auto');
                                } else {
                                    $('#bodyWidth').val('1024');
                                }
                                slideEFunc();
                            }, 100);
                        }
                    }
                }]
            }).appendTo('body').addClass('cboxform');
            $('<img class="size" src="' + data.BaseUrl + 'Styles/size.png" style="position:absolute;left:10px;top:10px;" />').appendTo(formSize);

            slideEFunc();
            var formSizeData = formSize.data("MFLFormData");
            var s;
            $('.btn-size').click(function () {
                MFLDialog("", formSize, function () {
                    if (!s) {
                        location.reload();
                    }
                }, function () {
                    s = true;
                    var formdata = formSizeData();
                    MFLAjax({
                        action: "SetSize",
                        query: formdata,
                        loadingJ: $('.mouse-loading'),
                        success: function (json) {
                            location.reload();
                        }
                    });
                    return false;
                });
                return false;
            });
        }

        //new album
        $('<a class="btn-newalbum btn" href="' + data.BaseUrl + 'Uploader/">New&nbsp;ALBUM.</a>').css({
            width: 30,
            height: 30
        }).insertBefore('.btn-themes').click(function () {
        }).colorbox({
            'iframe': true,
            width: 534,
            height: "80%",
            onClosed: function () {
                location.reload();
            }
        });
        //mode swithcer
        $('<div class="switch-mode"><div class="tooltip"></div><div class="switch-mode-masonry">Masonry Mode</div><div class="switch-mode-list">Sortable Mode</div></div>')
        .insertBefore('.btn-themes')
        .click(function () {
            if (!MGMasonryModeSwitching) {
                MGSwitchMode();
                $(this).children('div').slideToggle(MGAnimateDuration);
            } else {
                _MGDebug('"MGMasonryModeSwitching" is true. Mode switching canceled! ');
            }
        });
        $('.switch-mode-list').hide();
        //tooltip
        $('.tools>a.btn').hover(function () {
            $('.tooltip').text($(this).text()).show();
        }, function () {
            $('.tooltip').hide();
        });
        //new subpic
        $('.album').each(function (i, album) {
            var url = MGGetOriginUrl($(album).children('.mainpic').attr('src'));
            var title = $(album).find('.title-text').text();
            $('<div><a class="btn-newsubpic subpic"><span class=" btn">Add</span></a></div>').insertBefore($(album).children('.subpics').children('.clearfix'))
            .children('.subpic').attr('href', data.BaseUrl + 'Uploader/?url=' + encodeURI(url) + '&title=' + encodeURI(title))
            .colorbox({
                'iframe': true,
                width: 534,
                height: "80%",
                onClosed: function () {
                    location.reload();
                }
            });
        });
        //del album
        $('<a class="btn-delalbum mfl-btn" href="#">Del</a>').appendTo('.album').click(function () {
            var album = $(this).parent();
            MFLAjax({
                action: "DelAlbum",
                query: {
                    url: MGGetOriginUrl(album.children('.mainpic').attr('src'))
                },
                loadingJ: $('.mouse-loading'),
                success: function () {
                    MGCloseAllGallery(function () {
                        album.remove();
                        if (MGMasonryMode) {
                            $('.gallery').masonry('reload');
                        }
                    });
                }
            });
            return false;
        });
        //del comment
        $('<a class="btn-delcomment mfl-btn" href="#">Del</a>').appendTo('.comment').click(function () {
            var comment = $(this).parent();
            MFLAjax({
                action: "DelComment",
                query: {
                    id: $.trim(comment.children('.data-commentid').text())
                },
                loadingJ: $('.mouse-loading'),
                success: function () {
                    comment.remove();
                }
            });
            return false;
        });
        //del subpic
        $('<a class="btn-delsubpic mfl-btn" href="#">Del</a>').appendTo($('img.subpic').parent()).click(function () {
            var subpic = $(this).parent().children('img.subpic');
            MFLAjax({
                action: "DelSubpic",
                query: {
                    url: MGGetOriginUrl($.trim(subpic.attr('src')))
                },
                loadingJ: $('.mouse-loading'),
                success: function () {
                    subpic.parent().fadeOut(MGAnimateDuration2, function () {
                        MGCloseSubpic(function () {
                            _MGRefreshSubpics(subpic.parent().parent().parent());
                            subpic.parent().remove();
                        });
                    });
                }
            });
            return false;
        });
        //edit description
        $('<a class="btn btn-edit btn-editdescription" href="#"></a>')
        .appendTo('.album').click(function () {
            var btn = this;
            $(btn).hide();
            $(btn).siblings('.description').children('.description-text').mflEdit({
                cancel: function () {
                    $(btn).show();
                },
                multiline: true,
                height: 50, //$(btn).siblings('.description').children('.description-text').height(),
                callback: function () {
                    var newTitle = $(btn).siblings('.description').children('.description-text').html().replaceAll('<br />', '\n').replaceAll('<br/>', '\n').replaceAll('<br>', '\n');
                    MFLAjax({
                        'type': 'post',
                        'action': 'SetDescription',
                        'query': {
                            'url': MGGetOriginUrl($(btn).siblings('.mainpic').attr('src')),
                            'description': newTitle,
                            'langCode': data.LangCode
                        },
                        loadingJ: $('.mouse-loading'),
                        success: function () { }
                    });
                    $(btn).show();
                },
                lengthLimitation: 500
            });
            $(btn).siblings('.description').find('textarea').autoTextarea();
            return false;
        });
        //lang activator

        $('.langlinks>*').css({
            'lineHeight': '30px',
            'display': 'block'
        });
        var langs = $('<div></div>').prependTo('.langlinks').hide().append(
            $('.langlinks>a.notavailable').show().click(function () {
                var code = $(this).attr('href');
                code = code.substr(code.lastIndexOf('/') + 1);
                MFLAjax({ action: "ActiveLang",
                    query: {
                        code: code
                    },
                    loadingJ: $('.mouse-loading'),
                    success: function (json) {
                        location.href = json.newUrl;
                    }
                });
                return false;
            })
        );
        $('.langlinks>a:not(.notavailable)').append($('<span class="btn btn-del">Delete</span>').click(function () {
            var code = $(this).parent().attr('href');
            code = code.substr(code.lastIndexOf('/') + 1);
            MFLAjax({
                action: "DeactiveLang",
                query: {
                    code: code
                },
                loadingJ: $('.mouse-loading'),
                success: function (json) {
                    location.reload();
                }
            });
            return false;
        }));
        $('<div>Active a new language</div>').insertBefore(langs).click(function () {
            langs.slideToggle(MGAnimateDuration2);
        });
    }
};