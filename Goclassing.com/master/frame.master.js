/*<!--*/
/*--><!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- Le styles -->
    <link href="/css/688137.mybootstrap.css" rel="stylesheet" />
    <style type="text/css">
        body>.container
        {
            padding-top: 60px;
        }
    </style>
    <title><!--*/
if (typeof (arguments[0].title) == 'string') {
    $(arguments[0].title + ' - ');
} /*-->Goclassing.com</title>
    <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
<script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
    <!-- Le fav and touch icons -->
    <link rel="shortcut icon" href="/favicon.ico" />
    <link href="/css/StyleSheets.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="/js/bootstrap.min.js"></script>
    <script src="/js/scripts.js" type="text/javascript"></script>
    <!--*/
arguments[0].head(); /*-->
</head>
<body>
    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container">
                <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse"><span
                    class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </a>
                <a class="btn btn-navbar profile hidden-desktop" href="/">
                            <img class="avatar" src="/img/signin.png" alt="Sign in" /></a>
                <a class="brand brand-gc" href="/">Goclassing</a>
                <div class="nav-collapse">
                    <ul class="nav">
                        <!--*/
var act1 = ($cur == 'Default.htm.isp.js') ? 'active' : '';
var act2 = (($cur == 'users.isp.js') ||
($cur == 'courses.isp.js') ||
($cur == 'tags.isp.js') ||
($cur == 'user.isp.js') ||
($cur == 'course.isp.js') ||
($cur == 'tag.isp.js')) ? 'active' : $cur;
var act3 = ($cur == 'Mobile.htm.isp.js') ? 'active' : '';
/*-->
                        <li class="{$act1$}"><a href="/"><i class="icon-home icon-white"></i>Home</a></li>
                        <li class="dropdown {$act2$}"><a href="#" class="dropdown-toggle  visible-desktop" data-toggle="dropdown">
                            <i class="icon-search icon-white"></i>Browse <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="/All.users"><i class="icon-user"></i>All Users</a></li>
                                <li><a href="/All.courses"><i class="icon-book"></i>All Courses</a></li>
                                <li><a href="/tags"><i class="icon-tags"></i>All Tags</a></li>
                            </ul>
                        </li>
                        <li>
                            <form class="navbar-search pull-left" action="/Course/Search">
                            <input type="text" class="search-query" name="input" placeholder="Search" />
                            <input type="hidden" name="redirect" value="" />
                            </form>
                        </li>
                        <li class="divider-vertical"></li>
                        <li><a href="/getting_started.course"><i class="icon-info-sign icon-white"></i>Help</a></li>
                        <li class="{$act3$} visible-desktop"><a href="/Mobile.htm" title="Mobile"><i class="icon-mobile icon-white"></i></a></li>
                    </ul>
                    <ul class="pull-right nav" id="slf">
                        <li class="visible-desktop"><a href="/" class="profile">
                            <img class="avatar" src="/img/signin.png" style="display: none;" alt="" /></a></li>
                        <li class=""><a href="/" class="profile name"><i class="icon-time icon-white"></i>loading...</a></li>
                        <li class="dropdown"><a href="#" class="dropdown-toggle  visible-desktop" data-toggle="dropdown"><b
                            class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="/Auth/Logout?redirect=%20"><i class="icon-off"></i>Logout</a></li>
                            </ul>
                        </li>
                        <li class="offline"><a href="#" id="fb_btn"><span>Sign in with Facebook</span></a></li>
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
<p>&copy; Goclassing.com 2012 <a href="/82.file">privacy</a>  
<a href="/Mobile.htm">mobile</a>  
<a href="https://github.com/layerssss/goclassing.com/blob/master/{$$cur$}" target="_blank">source</a></p>
</footer>
    </div>
    <!-- /container -->
    <div class="modal fade" id="myModal">
        <div class="modal-header">
            <a class="close" data-dismiss="modal">×</a>
            <h3>
                Message</h3>
        </div>
        <div class="modal-body">
        </div>
        <div class="modal-footer">
            <a href="#" data-dismiss="modal" class="btn btn-primary">Close</a>
        </div>
    </div>
    <div class="loading"></div>
    <!--*/
$load('inline/ga.isp.js')(); /*-->
</body>
</html>
<!--*/

//-->
