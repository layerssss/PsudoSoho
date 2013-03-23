/// <reference path="/ISPReferences/Error.htm.isp.js" />
/*<!--*/ /*--><!DOCTYPE HTML>
<html lang="zh-CN">
<head>
	<meta charset="UTF-8">
	<title>吃饭社 · 全世界吃货联合起来</title>
	<link rel="stylesheet" type="text/css" href="./css/style.css" media="screen" />
	<!--[if lte IE 9]><link rel="stylesheet" href="./css/1140ie.css" type="text/css" media="screen" /><![endif]-->
	<script type="text/javascript" src="./js/jquery-1.6.2.min.js"></script>
	<link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
	<meta property="wb:webmaster" content="7dbe0bef4c8f21ac" />
</head>
<body>
        <script type="text/javascript">
            $(function () {
                var s = location.search;
                s = s.substring(s.lastIndexOf('message=') + 'message='.length);
                if (s.indexOf('&') != -1) {
                    s = s.substring(0, s.indexOf('&'));
                }

                alert(String(decodeURIComponent(s).replace(/\+/g, ' ')));
                history.go(-1);
                //
            });
        </script></body></html><!--*///-->