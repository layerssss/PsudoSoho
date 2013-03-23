/*<!--*/
var c = arguments[0].course;
/*--><!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- Le styles -->
    <link href="/css/{$c.color$}.mybootstrap.css" rel="stylesheet" />
    <style type="text/css">
        body>.container
        {
            padding-top: 30px;
            
        }
    </style>
    <title><!--*/
if (typeof (arguments[0].title) == 'string') {
    $(arguments[0].title + ' - ');
} /*-->Goclassing.com</title>
    <link href="/css/responsive.css" rel="stylesheet" />
    <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
<script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
    <!-- Le fav and touch icons -->
    <link rel="shortcut icon" href="/favicon.ico" />
    <link href="/css/StyleSheets.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery-1.7.1.min.js"></script>
    <link href="/js/fancyBox/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="/js/fancyBox/jquery.fancybox.pack.js" type="text/javascript"></script>
    <script src="/js/fancyBox/helpers/jquery.fancybox-thumbs.js" type="text/javascript"></script>
    <link href="/js/fancyBox/helpers/jquery.fancybox-buttons.css" rel="stylesheet" type="text/css" />
    <script src="/js/fancyBox/helpers/jquery.fancybox-buttons.js" type="text/javascript"></script>
    <link href="/js/fancyBox/helpers/jquery.fancybox-thumbs.css" rel="stylesheet" type="text/css" />
    <script src="/js/fancyBox/jquery.mousewheel-3.0.6.pack.js" type="text/javascript"></script>
    <script src="/js/jquery.backstretch.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/bootstrap.min.js"></script>
    <script src="/js/scripts.js" type="text/javascript"></script>
    <script type="text/javascript">
        teacherUsername = "{$c.user.username$}";
        var bg = "{$c.bg$}";
    </script>
    <script type="text/javascript">
        $(function () {
            $('a.fancybox').fancybox({
                helpers: {
                    thumbs: {
                        width: 75,
                        height: 50
                    }
                }
            }); 
            $.backstretch(bg, { speed: 150 });
        });
    </script>
    <!--*/
arguments[0].head();


var act1 = ($cur == 'course.isp.js') ? 'active' : '';
var act2 = (($cur == 'classroom.isp.js') ||
($cur == 'file.isp.js')) ? 'active' : $cur;

var act3 = ($cur == 'Mobile.htm.isp.js') ? 'active' : '';
/*-->
</head>
<body data-spy="scroll" data-target=".subnav">
    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container" style="position: relative">
                <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse"><span
                    class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </a>
                <a class="btn btn-navbar profile hidden-desktop" href="/">
                            <img class="avatar" src="/img/signin.png"  alt="Sign in" /></a>
                <a class="brand" href="/{$c.id$}.course">
                    <img src="{$c.img$}" style="width: 35px; height: 35px; position: absolute; left: 2px;
                        top: 2px;" alt="" />
                    <span style="margin-left: 40px;">{$c.title$}</span></a>
                <div class="nav-collapse">
                    <ul class="nav">
                        <li class="divider-vertical"></li>
                        <li class="{$act1$}" ><a href="{$c.id$}.course"><i class="icon-home icon-white"></i>Introduction</a></li>
                        <li class="{$act2$}"><a href="{$c.id$}.classroom"><i class="icon-book icon-white"></i><!--*/
if (c.pub) { /*-->Plaza<!--*/ } else { /*-->Classroom<!--*/ } /*--></a></li>
                        <li class="divider-vertical"></li>
                        <li><a href="/"><span class="brand-gc-mini">Goclassing.com</span></a></li>
                        <li class="{$act3$} visible-desktop"><a title="Mobile" href="/Mobile.htm"><i class="icon-mobile icon-white"></i></a></li>
                    </ul>
                    <ul class="pull-right nav" id="slf">
                        <li class="visible-desktop"><a href="/" class="profile">
                            <img class="avatar" src="/img/signin.png" style="display: none;" alt=""  /></a></li>
                        <li class=""><a href="/" class="profile name"><i class="icon-time icon-white"></i>loading...</a></li>
                        <li class="dropdown"><a href="#" class="dropdown-toggle visible-desktop" data-toggle="dropdown"><b
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
    <div class="modal fade" id="imgUploadingAdvice">
        <div class="modal-header">
            <a class="close" data-dismiss="modal">×</a>
            <h3>
                Image Uploading Advice</h3>
        </div>
        <div class="modal-body">
    <!--*/$load('inline/imgUploadingAdvice.isp.js')();/*-->
        </div>
        <div class="modal-footer">
            <a href="#" data-dismiss="modal" class="btn btn-primary">Close</a>
        </div>
    </div>
    <div class="loading"></div>
    <!-- Le javascript
================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <!--*/
$load('inline/ga.isp.js')(); /*-->
</body>
</html>
<!--*/

//-->
