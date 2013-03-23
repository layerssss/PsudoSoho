
var MGMasonryMode = false;
var MGMasonryModeSwitching = false;

var MGSwitchMode = function () {
    MGMasonryModeSwitching = true;
    MGMasonryMode = !MGMasonryMode;
    if (MGMasonryMode) {
        var len = $('.album').length;
        $('.album').each(function (i, album) {
            var oldSize = $(album).data('MGOldSize');
            if (!oldSize) {
                oldSize = { width: $(album).width(), height: $(album).height() };
            }
            $(album).animate(oldSize, MGAnimateDuration, function () {
                if (len == i + 1) {
                    //album is currently still not available
                    if (MGMasonryLogon) {
                        $('.subpics').sortable({
                            'items': '>div:has(img)',
                            'stop': function () {
                                var str = "";
                                $(this).find('img.subpic').each(function (i, subpic) {
                                    str += MGGetOriginUrl($(subpic).attr('src')) + '|';
                                });
                                MFLAjax({
                                    'type': 'post',
                                    'action': 'OrderSubpics',
                                    'query': {
                                        'url': MGGetOriginUrl($(this).parent().children('.mainpic').attr('src')),
                                        'str': str
                                    },
                                    loadingJ: $('.mouse-loading'),
                                    success: function () { }
                                });
                            }
                        });
                    }
                    $('.gallery').sortable('destroy').masonry({
                        itemSelector: '.album,.btn-newalbum',
                        columnWidth: data.Width + data.Margin * 2 + data.BorderWidth * 2,
                        isAnimated: true,
                        isFitWidth: false,
                        animationOptions: { queue: false, duration: MGAnimateDuration2 }
                    }).children('.album').each(function (i, e) {
                        if (MGMasonryLogon) {
                            var aratio = $(e).width() / $(e).height();
                            var oldZindex;
                            $(e).resizable({
                                aspectRatio: aratio,
                                grid: [data.Width + (data.Margin + data.BorderWidth) * 2, (data.Width + (data.Margin + data.BorderWidth) * 2) / aratio],
                                start: function () {
                                    oldZindex = $(this).css("z-index");
                                    $(this).css("z-index", 3000);
                                },
                                stop: function () {
                                    $(this).css("z-index", oldZindex);
                                    if ($(this).hasClass('album-open')) {
                                        $(this).children('.data-show-width').text($(this).width());
                                        MFLAjax({
                                            'type': 'post',
                                            'action': 'SetShowWidth',
                                            'query': {
                                                'url': MGGetOriginUrl($(this).children('.mainpic').attr('src')),
                                                'width': $(this).width()
                                            },
                                            loadingJ: $('.mouse-loading'),
                                            success: function () { }
                                        });
                                    } else {
                                        MFLAjax({
                                            'type': 'post',
                                            'action': 'SetThumbWidth',
                                            'query': {
                                                'url': MGGetOriginUrl($(this).children('.mainpic').attr('src')),
                                                'width': $(this).width()
                                            },
                                            loadingJ: $('.mouse-loading'),
                                            success: function () { }
                                        });
                                    }
                                    $('.gallery').masonry('reload');
                                },
                                resize: function () {
                                    $(this).find('.mainpic').width($(this).width())
                                    .height($(this).height());
                                    if ($(this).hasClass('album-open')) {
                                        _MGRefreshSubpics(this);
                                    }
                                }
                            });
                            $('.ui-resizable-handle').css('z-index', 201);
                        }
                    }).bind('click.MGOpenAlbum', function () {
                        _MGDebug('"click.MGOpenAlbum" fired.');
                        var album = this;
                        if ($(album).hasClass('album-open')) {
                            return;
                        }
                        MGCloseAllGallery(function () {
                            var oldSize = { width: $(album).width(), height: $(album).height() };
                            _MGDebug('Opening album: oldSize:[' + oldSize.width + ',' + oldSize.height + ']');
                            var aratio = oldSize.width / oldSize.height;
                            var showWidth = Number($.trim($(album).children('.data-show-width').text()));
                            var size = { width: showWidth, height: showWidth / aratio };
                            $(album).data('MGOldSize', oldSize).addClass('album-open').animate(size, MGAnimateDuration);
                            $(album).children('.mainpic').attr('src', MGGetOriginUrl($(album).children('.mainpic').attr('src')));
                            $(album).children('.mainpic').animate(size, MGAnimateDuration, function () {
                                $(album).children('.comments-handle').fadeIn(MGAnimateDuration);
                                $(album).children('.btn-editdescription').show();
                                $(album).children('.description').fadeTo(MGAnimateDuration, data.Opacity);
                                _MGRefreshSubpics(album);
                                if (MGMasonryLogon) {
                                    $('<a class="btn btn-edit" href="#"></a>')
                                    .appendTo($(album).children('.title')).click(function () {
                                        var btn = this;
                                        $(btn).hide();
                                        $(btn).prev().mflEdit({
                                            cancel: function () {
                                                $(btn).show();
                                            },
                                            callback: function () {
                                                var newTitle = $(btn).prev().text();
                                                MFLAjax({
                                                    'type': 'post',
                                                    'action': 'SetTitle',
                                                    'query': {
                                                        'url': MGGetOriginUrl($(album).children('.mainpic').attr('src')),
                                                        'title': newTitle,
                                                        'langCode': data.LangCode
                                                    },
                                                    loadingJ: $('.mouse-loading'),
                                                    success: function () { }
                                                });
                                                $(btn).show();
                                            },
                                            height: $(this).parent().height()
                                        });
                                        return false;
                                    });
                                }
                                $('.gallery').masonry(); //:not(.album-open)//
                            });
                        });
                    });
                    MGMasonryModeSwitching = false;
                }
            });
        });
    } else {
        _MGDebug('Starting switch to LIST');
        MGCloseAllGallery(function () {
            $('.subpics').sortable('destroy');
            $('.gallery')
            .masonry('destroy')
            .sortable({
                stop: function () {
                    var str = "";
                    $(this).find('img.mainpic').each(function (i, subpic) {
                        str += MGGetOriginUrl($(subpic).attr('src')) + '|';
                    });
                    MFLAjax({
                        'type': 'post',
                        'action': 'OrderAlbums',
                        'query': {
                            'str': str
                        },
                        loadingJ: $('.mouse-loading'),
                        success: function () { }
                    });
                },
                items: '>:has(img.mainpic)'
            })
            .children('.album')
            .unbind('click.MGOpenAlbum')
            .resizable('destroy')
            .each(function (i, album) {
                var oldSize = { width: $(album).width(), height: $(album).height() };
                _MGDebug('Switching to LIST: oldSize:[' + oldSize.width + ',' + oldSize.height + ']');
                var size = { width: 400, height: 40 };
                setTimeout(function () {
                    $(album).data('MGOldSize', oldSize).animate(size, MGAnimateDuration, function () {
                        MGMasonryModeSwitching = false;
                    });
                }, 10);
            });
        });

    }
};
var MGCloseAllGallery = function (callback) {
    var g = $('.album-open');
    if (g.length) {
        if (g.length != 1) {
            _MGDebug(g.length + " opened albums, callback cancled! ");
            callback = function () {
            };
        }
        MGCloseSubpic(function () {
            g.each(function (i, e) {
                var oldSize = $(e).data('MGOldSize');
                _MGDebug('Closing .album-open: oldSize:[' + oldSize.width + ',' + oldSize.height + ']');
                $(e).children('.subpics').hide();
                $(e).children('.subpics-handle').hide();
                $(e).children('.comments-handle').fadeOut(MGAnimateDuration);
                $(e).children('.btn-editdescription,.description').hide();
                $(e).find('.title>.btn-edit').remove();
                $(e).next('.comments').fadeOut(MGAnimateDuration, function () {
                    _MGDebug(this);
                    $(e).removeClass('album-open').animate(oldSize, MGAnimateDuration);
                    $(e).children('.mainpic').attr('src', MGGetOriginUrl($(e).children('.mainpic').attr('src')));
                    $(e).children('.mainpic').animate(oldSize, MGAnimateDuration, function () {
                        _MGDebug('"MGCloseAllGallery+callback" fired')
                        callback(); //!!!!!!!!!Masonry animate duration!!!!
                    });
                });
            }).children('.title').children('.mfl-btn').remove();
        });
    } else {
        callback();
    }
};
var MGCloseSubpic = function (callback) {
    var g = $('.subpic-open');
    if (g.length) {
        if (g.length != 1) {
            _MGDebug(g.length + " opened subpics, callback cancled! ");
            callback = function () {
            };
        }
        $('.subpic-active').removeClass('subpic-active');
        $('.overlay').remove();
        g.each(function (i, subpic) {
            var album = $(subpic).parent();
            album.resizable('option', 'delay', 0);
            var size = { width: album.children('.mainpic').width(), height: album.children('.mainpic').height() };
            _MGDebug('Album mainpic size:');
            _MGDebug(size);
            $(album).css(size);
            album.children('.mainpic').show();
            $(subpic).fadeOut(MGAnimateDuration, function () {
                $(subpic).remove();
                callback();
            });
        });
    } else {
        callback();
    }
};
var _MGRefreshSubpics = function (album) {
    {//retrieve subpics slide's actual height //obseleted
        var subpics = $(album).children('.subpics-handle').show();
        subpics.css('height', 'auto');
    }
};