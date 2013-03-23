$(function () {
    $('[data-confirm]').live('click', function () {
        if ($(this).parent().hasClass('modal-footer')) {
            return;
        }
        $('.modal-body').text($(this).attr('data-confirm'));
        $('.modal-footer>:not(:contains("Close"))').remove();
        $(this)
        .clone(true)
        .prependTo('.modal-footer')
        .removeClass('btn-mini')
        .append($('<span></span>').text($(this).attr('data-original-title')));
        $('#myModal').modal();
        return false;
    });
    if ($('.brand').css('color').substring(4, 7) != '255') {
        $('.nav a:not(.btn) i').removeClass('icon-white');
    }
    $('[data-file="mp3"]').click(function () {
    });
});
var teacherUsername;
$(function () {
    $('.icon-mobile').closest('a').attr('href', '/Mobile.htm#' + location.href);
    $('.admin').toggleClass('not');
    $('[title]').tooltip({ placement: 'bottom' });
    $('textarea').keydown(function (e) {
        if (e.ctrlKey && e.keyCode == 13) {
            $(this).closest("form").trigger("submit");
        }
    });
    window.onunload = function () {
        $('.loading').show();
    };
    $.ajax({
        url: '/Auth/GetStatus',
        cache: false,
        dataType: 'json',
        success: function (j) {
            if (j.message) {
                $('#myModal .modal-body').text(String(j.message));
                $('#myModal').modal('show');
            }
            if (j.me == null) {
                $('#slf>li').toggle();
                $('#fb_btn').click(function () {
                    return false;
                });
                $.ajax({
                    url: '/Auth/GetLoginUrl',
                    data: {
                        provider: 'facebook',
                        remember: true
                    },
                    dataType: 'json',
                    success: function (j) {
                        $('#fb_btn,.profile.btn-navbar').unbind('click').attr('href', j.url);
                    }
                });
            } else {
                if (j.me.username == teacherUsername) {
                    $('.admin').toggleClass('not');
                    if (typeof (admin) == 'function') {
                        admin();
                    }
                }
                $('a.profile').attr('href', '/' + j.me.username + '.user');
                $('#slf a.name').text(j.me.name);
                $('img.avatar').attr('src', j.me.avatarUrl).show();
                if (j.newFiles.length) {
                    if (typeof (showNewFiles) == 'function') {
                        showNewFiles(j.newFiles);
                    } else {
                        $('<a title="' + j.newFiles.length + ' new topic' + (j.newFiles.length == 1 ? '' : 's') + '" class="msg badge badge-info" href="/"><i class="icon-info-sign icon-white"></i>' + j.newFiles.length + '</a>').appendTo('.brand').tooltip({ placement: 'bottom' });
                    }
                }
                $('[data-anticsrf]').each(function (i, e) {
                    var ac = $(e).attr('data-anticsrf');
                    $(e).attr(ac, $(e).attr(ac).replace('ANTICSRF=', 'ANTICSRF=' + j.antiCSRF));
                });
            }
        }
    });

});
var oAuthFinished = function () {
    location.reload();
};
var modal = function (header, callback) {
    $('<div></div>').appendTo('body').load('/piece/bootstrap.modal.htm', function () {
        $(this).find('.modal-header h3').text(header);
        $(this).find('.modal').modal('show').on('hidden', function () {
            $(this).parent().remove();
        });
        var ui = { footer: $(this).find('.modal-footer')[0] };
        $.proxy(callback, $(this).find('.modal-body')[0])(ui);
    });
};