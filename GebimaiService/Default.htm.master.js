/// <reference path="/ISPReferences/Default.htm.master.js" />
/*<!--*/
/*--><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <title>隔壁买管理中心</title>
    <!-- Le styles -->
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <style type="text/css">
        body
        {
            padding-top: 60px;
            padding-bottom: 40px;
        }
    </style>
    <link href="/css/responsive.css" rel="stylesheet" />
    <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->
    <!-- Le fav and touch icons -->
    <link rel="shortcut icon" href="/favicon.ico" />
    <link href="/css/StyleSheets.css" rel="stylesheet" type="text/css" />
    <link href="/css/bootstrap.docs.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">
    </script>
    <script type="text/javascript">
        $(function () {
            $('[title]').tooltip({ placement: 'bottom' });
            $.ajax({
                url: '/Auth/GetStatus',
                dataType: 'jsonp',
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
                                provider: 'weibo',
                                remember: false
                            },
                            dataType: 'jsonp',
                            success: function (j) {
                                $('#fb_btn').click(function () {
                                    window.open(j.url, null, 'toolbars=no,left=' + (window.screenX + 100) + ',top=' + (window.screenY + 120) + ',width=' + ($(window).width() - 200) + ',height=500');
                                    return false;
                                });
                            }
                        });
                    } else {
                        $('a.profile').attr('href', '/' + j.me.username + '.user');
                        if (j.senderInfo) {
                            $('a.sender').attr('href', '/' + j.senderInfo.alias + '.sender');
                        }
                        if (j.adminInfo) {
                            $('a.admin').attr('href', '/' + j.adminInfo.alias + '.admin');
                        }
                        $('#slf a.name').text(j.me.name);
                        $('#slf img.avatar').attr('src', j.me.avatarUrl);
                        var f = function () {
                            $.ajax({
                                url: '/GetMessage.aspx',
                                dataType: 'text',
                                error: function () {
                                    f();
                                },
                                success: function (msg) {
                                    $('#myModal .modal-body').text(String(msg));
                                    $('#myModal').modal('show');
                                    $('#myModal').on('hidden', function () {
                                        location.href = "User/DismissMessage?redirect=";
                                    })
                                    $('title').text("【"+msg+"】");
                                }
                            });
                        };
                        f();
                    }
                }
            });

        });
        var oAuthFinished = function () {
            location.reload();
        };
    </script>
    <!--*/
if (typeof (arguments[0].head) == 'function') {
    arguments[0].head();
}
    /*-->
</head>
<body data-spy="scroll" data-target=".subnav" data-offset="60">
    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container">
                <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse"><span
                    class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </a><a class="brand" href="/">隔壁买<span class="label label-info">管理中心</span></a>
                <div class="nav-collapse">
                    <ul class="nav">
                        <li class="active"><a href="/">首页</a></li>
                        <li><a target="_blank" href="http://gebimai.com/">隔壁买首页</a> </li>
                        <li class="divider-vertical"></li>
                        <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown">帮助 <b
                            class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="/Help/FAQ">常见问题</a></li>
                                <li><a href="/Help/Contact">联系我们</a></li>
                            </ul>
                        </li>
                    </ul>
                    <ul class="pull-right nav" id="slf">
                        <li class=""><a href="/" class="profile">
                            <img class="avatar" /></a></li>
                        <li class=""><a href="/" class="profile name">加载中...</a></li>
                        <li class="">
                            <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown"><b
                                class="caret"></b></a>
                                <ul class="dropdown-menu">
                                    <li><a href="/" class="profile">我的资料</a></li>
                                    <li><a href="/Auth/Logout?redirect=%2f">注销</a></li>
                                </ul>
                            </li>
                        </li>
                        <li class="offline"><a href="#" id="fb_btn"><span>用微博登陆</span></a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <!--*/
        arguments[0].body();
        /*-->
        <hr>
        <footer>
        <p>&copy; gebimai.com 2012</p>
        </footer>
    </div>
    <!-- /container -->
    <div class="modal fade" id="myModal">
        <div class="modal-header">
            <a class="close" data-dismiss="modal">×</a>
            <h3>
                亲，这样不行啊~~</h3>
        </div>
        <div class="modal-body">
        </div>
        <div class="modal-footer">
            <a href="#" data-dismiss="modal" class="btn btn-primary">确定(Esc)</a>
        </div>
    </div>
    <!-- Le javascript
================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script type src="/js/bootstrap.min.js"></script>
</body>
</html>
<!--*/
//-->
