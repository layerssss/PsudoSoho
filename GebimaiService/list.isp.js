/// <reference path="/ISPReferences/Error.isp.js">
/*<!--*/

/*--><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8">
<link rel="stylesheet" type="text/css" href="http://gebimai.com/css/simplelist.css" />
</head>
<body class="area{$$subPage$}"><h1 class="area">{$area$}商品列表</h1><ul class="stocks"><!--*/
for (var i in stocks) {
    /*--><li class="stock"><span class="title">{$i$}</span><span class="price">￥{$stocks[i][0]$}</span><a href="{$stocks[i][1]$}" target="_blank">点我购买</a></li><!--*/
}
/*--></ul></body></html><!--*/
//-->
