/// <reference path="/ISPReferences/master/main.master.js" />
/*<!--*/

if (!arguments[0].title) {
    arguments[0].title = '吃饭社';
}
/*--><!DOCTYPE HTML>
<html lang="zh-CN">
<head>
<meta charset="UTF-8">
<title>{$arguments[0].title$}</title>
<link rel="stylesheet" type="text/css" href="./css/style.css" media="screen" />
<script type="text/javascript" src="./js/jquery-1.6.2.min.js"></script>
<script type="text/javascript" src="./js/select.js"></script>
<script type="text/javascript" src="./js/jquery.placeholder.min.js"></script>
<script type="text/javascript" src="./js/jquery.backstretch.min.js"></script>
<script type="text/javascript" src="./js/custom.js"></script>
<link rel="shortcut icon" type="image/x-icon" href="./images/favicon.ico" />
<meta property="wb:webmaster" content="7dbe0bef4c8f21ac" />
<!--*/
if (typeof (arguments[0].head) == 'function') {
    arguments[0].head();
} /*-->
</head>
<body>
<div id="wrap" class="clearfix">
<div id="logo">
<h1><a href="./">吃饭社</a></h1>
<span class="des">全世界吃货联合起来</span>
<div class="follow">
<iframe width="100%" height="24" frameborder="0" allowtransparency="true" marginwidth="0" marginheight="0" scrolling="no" border="0" src="http://widget.weibo.com/relationship/followbutton.php?language=zh_cn&width=100%&height=24&uid=2236633074&style=2&btn=light&dpc=1"></iframe>
</div>
<div id="nav">
	<ul class="menu clearfix">
		<li class="current">
			<a href="http://chifanshe.com">预订外卖</a>
			<!--*/
			var stores = arguments[0].stores;
			/*-->
			<span class="shop-num">{$stores.length$}</span>
		</li>
		<li>
			<a href="http://blog.chifanshe.com">社团活动</a>
		</li>
	</ul>
</div>
<div class="share-to-weibo">
	<script type="text/javascript" charset="utf-8">
	(function(){
	  var _w = 106 , _h = 58;
	  var param = {
	    url:location.href,
	    type:'5',
	    count:'1',
	    appkey:'1159390064',
	    title:'吃饭社，全世界吃货联合起来。在线预订暨大周边外卖，参与线下聚餐，结识同样口味的好友',
	    pic:'http://chifanshe.com/images/banner.png',
	    ralateUid:'2236633074',
		language:'zh_cn',
	    rnd:new Date().valueOf()
	  }
	  var temp = [];
	  for( var p in param ){
	    temp.push(p + '=' + encodeURIComponent( param[p] || '' ) )
	  }
	  document.write('<iframe allowTransparency="true" frameborder="0" scrolling="no" src="http://hits.sinajs.cn/A1/weiboshare.html?' + temp.join('&') + '" width="'+ _w+'" height="'+_h+'"></iframe>')
	})()
	</script>
</div>
</div>
<div id="cnt">
<div class="cnt-nav">
<ul>
<!--*/
for (var i = 0; i < stores.length; i++) {
    var s = stores[i];
    if (s.title == '') {
        continue;
    }
    s.active = (s.alias == store.alias) ? 'sub-cur' : '';
    /*-->
    <li class="{$s.active$}"><a href="{$s.alias$}.dian">{$s.title$}</a></li>
    <!--*/
} /*-->
</ul>
			</div>            
        <!--*/
arguments[0].main(); /*-->
		</div>
	</div>
	<div class="bgr"></div>
</body>
</html><!--*/
//-->