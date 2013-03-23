<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MFLAdmin.Lodge.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>寻旅.筑家-旅馆配置面板</title>
    <link href="../Style/mfl.css" rel="stylesheet" type="text/css" />
    <link href="../Style/mfl-lodge.css" rel="stylesheet" type="text/css" />
    <link href="../Style/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Style/mfl-lodge-style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyTooltip.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/mfl.js" type="text/javascript"></script>
    <script src="../Scripts/mfl-lodge.js" type="text/javascript"></script>
    <script src="../Scripts/mfl-lodge-icon.js" type="text/javascript"></script>
    <script src="../Scripts/mfl-lodge-pages.js" type="text/javascript"></script>
    <script src="../Scripts/mfl-lodge-wizard.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        MFLLodgeIdent = '<%=Request.QueryString["lodge"]??"test" %>';
    </script>
    <%--<script type="text/javascript" src="http://api.map.baidu.com/api?v=1.2"></script>
    <script language="javascript" type="text/javascript" src="http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js"></script>--%>
</head>
<body>
    <div class="topbar">
        <div class="topbar-inner">
            <div class="container">
                <a href="<%=MFL.SharedConfig.MFLBaseUrl %>" class="brand">MFL</a>
                <ul class="nav">
                <li><a href="#root" title="给您的旅馆选择模板、修改模版中的特定选项" class="topNavLink">我的旅馆</a></li>
                    <li><a href="#root,template" title="给您的旅馆选择模板、修改模版中的特定选项" class="topNavLink">模版设定</a></li>
                    <li><a href="#root,properties" title="修改旅馆的基本设置" class="topNavLink">旅馆设定</a></li>
                    <li><a href="#root,traffic" title="告诉访客您的旅馆在什么地方" class="topNavLink">地理位置</a></li>
                    <li><a href="#root,attributes" title="特性是旅馆中不同房间之间用来对比的属性" class="topNavLink">特性列表</a></li>
                    <li><a href="#root,rooms" title="修改您的房间设置" class="topNavLink">房间列表</a></li>
                    <li><a href="#root,lodgeAlbum" title="用图文来向访客介绍您的旅馆" class="topNavLink">旅馆相册</a></li>
                    <li><a href="#root,admin" title="修改前台管理员登陆所需要使用的密码" class="topNavLink">管理密码</a></li>
                </ul>
                <ul id="wizard" class="secondary-nav">
                    <li><a class="mfl-refresh-btn" href="#" title="对旅馆做了修改之后，点此按钮进行应用，才能在服务器上生效。" style="position: relative; z-index: 10;">应用更改</a></li>
                    <li><a href="#" class="wizardSwitcher" title="“旅馆设置向导”<br/>将领您一步一步修改并完善您的旅馆配置。" onclick="return false;">设置向导</a></li>
                    <li><a href="<%=MFL.SharedConfig.MFLBaseUrl %>Account/Lodges.aspx" title="返回您的旅馆列表查看其他旅馆和可用操作">
                        返回旅馆列表</a></li>
                </ul>
            </div>
        </div>
    </div>
    <!-- 面包屑 -->
    <div id="nav">
        <div class="container">
            <ul class="breadcrumb">
                <li>当前位置：</li>
            </ul>
        </div>
    </div>
    <!-- 面包屑 -->
    <div id="main">
        <div class="mfl-currentPage">
        </div>
        <div class="mfl-lodge-root mfl-lodge-page">
            <div class="mfl-lodge-title">
                我的旅馆</div>
            <div  class="menu clearfix">
                <a class="template" href="#root,template">模版设定</a> <a class="properties" href="#root,properties">
                    旅馆设定</a> <a class="traffic" href="#root,traffic">地理位置</a> <a class="attributes" href="#root,attributes">
                        特性列表</a> <a class="rooms" href="#root,rooms">房间列表</a> <a class="lodgeAlbum" href="#root,lodgeAlbum">
                            旅馆相册</a> <a class="admin" href="#root,admin">管理密码</a>
            </div>
            <div class="wrapper clearfix">
                <div class="info clearfix">
                    <ul>
                        <li title="您的旅馆产品类型">旅馆类型：<p class="mfl-productType">
                            加载中...</p>
                        </li>
                        <li title="您的旅馆的中文名称，您可以在“旅馆设置”中更改它。">旅馆名称：<p class="mfl-lodgeName">
                            加载中...</p>
                        </li>
                        <li title="你的旅馆从激活到现在的总访问页面数">历史访问量：<p class="mfl-staHistory">
                            加载中...</p>
                        </li>
                        <li title="您的旅馆在今天之内的访问页面数">今天访问量：<p class="mfl-staToday">
                            加载中...</p>
                        </li>
                        <li title="当前房间的总数目">房间总数目：<p class="mfl-numRooms">
                            加载中...</p>
                        </li>
                        <li title="您的相册总共的空间">相册总空间：<p class="mfl-capPhoto">
                            加载中...</p>
                        </li>
                        <li title="您的相册已使用的空间">相册已使用空间：<p class="mfl-avaPhoto">
                            加载中...</p>
                        </li>
                        <li title="你的旅馆首页地址">首页地址：<p class="mfl-link" style="letter-spacing: -1px;">
                            <a target="_blank" href="<%= MFL.SharedConfig.LodgeBaseUrl +Request["lodge"]+"/" %>"><%= MFL.SharedConfig.LodgeBaseUrl +Request["lodge"]+"/" %></a></p>
                        </li>
                        <li><a class="mfl-refresh-refresh save" href="#">刷新</a></li>
                    </ul>
                </div>
                <div class="preview clearfix">
                    <p>
                        未来三周的客房预订情况（注：数字表示已预订房间数）</p>
                    <br />
                    <table>
                        <tr>
                            <td>
                                周一
                            </td>
                            <td>
                                周二
                            </td>
                            <td>
                                周三
                            </td>
                            <td>
                                周四
                            </td>
                            <td>
                                周五
                            </td>
                            <td>
                                周六
                            </td>
                            <td>
                                周日
                            </td>
                        </tr>
                        <tr>
                            <td class="passed">
                                11-27
                            </td>
                            <td class="passed">
                                11-27
                            </td>
                            <td>
                                10-28
                            </td>
                            <td>
                                10-29
                            </td>
                            <td>
                                10-30
                            </td>
                            <td>
                                10-31
                            </td>
                            <td>
                                11-01
                            </td>
                        </tr>
                        <tr>
                            <td class="passed">
                                11
                            </td>
                            <td class="passed">
                                11
                            </td>
                            <td>
                                10
                            </td>
                            <td>
                                1
                            </td>
                            <td>
                                3
                            </td>
                            <td>
                                10
                            </td>
                            <td>
                                11
                            </td>
                        </tr>
                        <tr>
                            <td>
                                11-27
                            </td>
                            <td>
                                11-27
                            </td>
                            <td>
                                10-28
                            </td>
                            <td>
                                10-29
                            </td>
                            <td>
                                10-30
                            </td>
                            <td>
                                10-31
                            </td>
                            <td>
                                11-01
                            </td>
                        </tr>
                        <tr>
                            <td class="passed">
                                11
                            </td>
                            <td class="passed">
                                11
                            </td>
                            <td>
                                10
                            </td>
                            <td>
                                1
                            </td>
                            <td>
                                3
                            </td>
                            <td>
                                10
                            </td>
                            <td>
                                11
                            </td>
                        </tr>
                        <tr>
                            <td>
                                11-27
                            </td>
                            <td>
                                11-27
                            </td>
                            <td>
                                10-28
                            </td>
                            <td>
                                10-29
                            </td>
                            <td>
                                10-30
                            </td>
                            <td>
                                10-31
                            </td>
                            <td>
                                11-01
                            </td>
                        </tr>
                        <tr>
                            <td class="passed">
                                11
                            </td>
                            <td class="passed">
                                11
                            </td>
                            <td>
                                10
                            </td>
                            <td>
                                1
                            </td>
                            <td>
                                3
                            </td>
                            <td>
                                10
                            </td>
                            <td>
                                11
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="mfl-lodge-attributes mfl-lodge-page ">
            <div class="mfl-lodge-title">
                特性列表</div>
            <h1>
                特性列表</h1>
            <div class="mfl-lodge-attributes-list clearfix">
                <div class="mfl-list-itemTemplate">
                    <span class="icon">
                        <img class="spiritIcon mfl-style-attributeIcon mfl-alt-attributeName" src="../Style/blank.gif" /></span>
                    <span class="mfl-attributeName"></span><span class="mfl-html-attributeOptionNameTags mfl-attribute-options">
                    </span><span class="edit"><a href="#" title="删除" onclick="return false;" class="mfl-data-attributeId">
                        <img src="../Style/lodge/cross_circle.png" /></a> <a href="#root,attributes,attribute|$attributeId$"
                            title="编辑" class="mfl-href-attributeId">
                            <img src="../Style/lodge/pencil.png" /></a> </span>
                </div>
            </div>
            <a href="#root,attributes,newAttribute" class="add">添加特性</a>
        </div>
        <div class="mfl-lodge-attribute mfl-lodge-page">
            <div class="mfl-lodge-title">
                修改特性
            </div>
            <h1>
                修改特性</h1>
            <div class="mfl-lodge-attribute-list clearfix">
                <div class="mfl-list-itemTemplate">
                    <form action="#" onsubmit="return false;" class="attributeForm">
                    <input type="hidden" class="mfl-val-attributeId" name="id" />
                    <label>
                        特性名称：</label><input type="text" name="name" class="mfl-val-attributeName small-input" />
                    <label>
                        选择图标：</label><input type="hidden" name="icon" class="mfl-val-attributeIcon" />
                    <a href="#" onclick="MFLSubmit(this);return false;" class="save AttributeSave">保存修改</a>
                    </form>
                    <div class="divWrapper">
                        <p>
                            该特性的选项列表：</p>
                        <div class="mfl-lodge-options-list clearfix">
                            <div class="mfl-list-itemTemplate mfl-class-optionIcon optionIcon$optionIcon$">
                                <form action="#" class="" onsubmit="return false;">
                                <input type="hidden" class="mfl-val-optionId " name="id" />
                                <input type="hidden" class="mfl-val-optionIcon mfl-data-optionIcon" name="icon" />
                                <span class="edit"><a href="#" title="删除" onclick="return false;" class="mfl-data-optionId">
                                    <img src="../Style/lodge/cross.png" /></a></span> <span class="option-content">
                                        <input type="text" class="mfl-val-optionName mfl-data-optionName" name="name" /></span>
                                </form>
                            </div>
                        </div>
                    </div>
                    <form action="#" onsubmit="return false;" class="newOptionForm">
                    <input type="hidden" name="attributeId" class="mfl-val-attributeId" />
                    <input type="hidden" name="name" value="新选项" />
                    <input type="hidden" name="icon" value="/icons/blank.gif" />
                    <a href="#" onclick="MFLSubmit(this);return false;" class="newOption add">添加选项</a>
                    </form>
                </div>
            </div>
        </div>
        <div class="mfl-lodge-newAttribute mfl-lodge-page clearfix">
            <div class="mfl-lodge-title">
                添加特性
            </div>
            <h1>
                添加特性</h1>
            <form action="#" onsubmit="return false;">
            <label>
                特性名称：</label><input type="text" name="name" class="small-input mfl-val-attributeName" />
            <label>
                选择图标：</label><input type="hidden" name="icon" class="mfl-val-attributeIcon" value="background-position:-0px -96px;" />
            <a href="#" onclick="MFLSubmit(this);return false;" class="save">保存修改</a>
            </form>
        </div>
        <div class="mfl-lodge-rooms mfl-lodge-page">
            <div class="mfl-lodge-title">
                房间列表
            </div>
            <h1>
                房间列表</h1>
            <div class="mfl-roomList clearfix">
                <div class="mfl-list-itemTemplate">
                    <span class="mfl-roomName"></span><span class="mfl-list-roomAttributes"><span class="mfl-list-itemTemplate">
                        <img class="spiritIcon mfl-alt-optionName mfl-style-attributeIcon mfl-class-attributeEnabled mfl-attribute-enabled-$attributeEnabled$"
                            src="../Style/blank.gif" />
                    </span></span><span class="edit"><a href="#" title="删除" onclick="return false;" class="mfl-data-roomId">
                        <img src="../Style/lodge/cross.png" /></a> <a href="#root,rooms,room|$roomId$" title="编辑"
                            class="mfl-href-roomId">
                            <img src="../Style/lodge/pencil.png" /></a> </span>
                </div>
            </div>
            <a href="#root,rooms,newRoom" class="add">添加房间</a>
        </div>
        <div class="mfl-lodge-room mfl-lodge-page">
            <div class="mfl-lodge-title">
                修改房间
            </div>
            <h1>
                修改房间</h1>
            <div class="mfl-roomList clearfix">
                <div class="mfl-list-itemTemplate">
                    <form action="#" onsubmit="return false;">
                    <label>
                        房间名称：</label><input type="text" name="name" class="mfl-val-roomName" class="small-input" />
                    <label>
                        房间价格：</label><input type="text" name="prize" class="mfl-val-roomPrize" class="small-input" />
                    <a href="#" onclick="MFLSubmit(this);return false;" class="save RoomSave">保存修改</a>
                    <br />
                    房间特性：
                    <div class="mfl-roomAttributesList clearfix">
                        <div class="mfl-list-itemTemplate">
                            <span class="icon">
                                <img class="mfl-style-attributeIcon mfl-alt-attributeName spiritIcon" src="/style/blank.gif" /></span>
                            <span class="mfl-attributeName"></span>
                            <input type="hidden" class="mfl-data-attributeId mfl-val-optionId mfl-data-optionIcon mfl-data-optionName" />
                        </div>
                    </div>
                    房间相册列表：
                    <div class="mfl-roomAlbumList clearfix">
                        <div class="mfl-list-itemTemplate">
                            <div class="photo">
                                <a href="#root,rooms,room|$roomId$,roomPhoto|$roomPhotoId$|$roomId$" class="mfl-href-roomPhotoId mfl-href-roomId">
                                    <img class="mfl-src-roomPhotoUrlThumb mfl-alt-roomPhotoTitle" />
                                </a><a href="#" onclick="return false;" class="mfl-data-roomPhotoId mfl-data-roomId"
                                    title="删除">
                                    <img src="../Style/lodge/cross_circle.png" />
                                </a>
                            </div>
                        </div>
                    </div>
                    <a href="#root,rooms,room|$roomId$,roomNewPhoto|$roomId$" class="mfl-href-roomId add">
                        添加照片</a><br />
                    <div class="progressbar">
                    </div>
                    <h3>相册已使用用空间：<span class="avaPhoto"></span>/<span class="capPhoto"></span></h3>
                    </form>
                </div>
            </div>
        </div>
        <div class="mfl-lodge-newRoom mfl-lodge-page">
            <div class="mfl-lodge-title">
                添加房间
            </div>
            <h1>
                添加房间</h1>
            <form action="#" onsubmit="return false;">
            <label>
                房间名称：</label><input type="text" name="name" class="small-input" />
            <label>
                房间价格：</label><input type="text" name="prize" class="small-input" />
            <a href="#" onclick="MFLSubmit(this);return false;" class="add">添加房间</a>
            </form>
        </div>
        <div class="mfl-lodge-properties mfl-lodge-page">
            <div class="mfl-lodge-title">
                旅馆设定
            </div>
            <div class="mfl-propertiesList clearfix">
                <div class="mfl-list-itemTemplate">
                    <form action="#" onsubmit="return false;">
                    <h1>
                        旅馆设定：</h1>
                    <label>
                        旅馆名称：</label><input type="text" class="mfl-val-lodgeName" name="lodgeName" class="small-input" /><br />
                    <label>
                        旅馆简介：</label><textarea class="mfl-val-lodgeDescription" rows="7" cols="50" name="lodgeDescription"></textarea><br />
                    <label>
                        联系方式：</label><textarea class="mfl-val-lodgeContact" rows="7" cols="50" name="lodgeContact"></textarea><br />
                        <label>订房方式：</label>
                        <textarea class="mfl-val-lodgeOrder" rows="7" cols="50" name="lodgeOrder"></textarea><br />
                    <a href="#" onclick="MFLSubmit(this);return false;" class="save">保存修改</a><br />
                    </form>
                    <br />
                </div>
            </div>
        </div>
        <div class="mfl-lodge-traffic mfl-lodge-page">
            <div class="mfl-lodge-title">
                地理交通
            </div>
            <h1>
                地理交通</h1>
            <div class="mfl-trafficList clearfix">
                <div class="mfl-list-itemTemplate">
                    <form action="#" class="trafficForm" onsubmit="return false;">
                    <label>
                        交通方式：</label><br />
                    <textarea class="mfl-val-lodgeTraffic" rows="5" cols="50" name="lodgeTraffic"></textarea>
                    <input type="hidden" class="mfl-val-lodgeLongtitude" name="lodgeLongtitude" />
                    <input type="hidden" class="mfl-val-lodgeLatitude" name="lodgeLatitude" />
                    </form>
                    <form action="#" class="mapForm" onsubmit="return false;">
                    <!--提示信息开始标签-->
                    <div class="notification attention">
                        <div>
                            提示：拖动地图选定旅馆所在地或者输入城市（可选）
                        </div>
                    </div>
                    <!--提示信息结束标签-->
                    <input type="text" class="city small-input" />
                    <label>
                        地址：</label>
                    <input type="text" class="address medium-input" /><a href="#" class="save locate"
                        onclick="MFLSubmit(this);return false;"> 定位 </a>
                    <div class="mapContainer" style="width: 820px; height: 500px; border: 1px solid #E5E5E5;
                        margin: 10px auto;">
                    </div>
                    <a href="#" onclick="MFLSubmit($('.mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.trafficForm .mfl-val-lodgeLongtitude'));return false;"
                        class="save">保存修改</a>
                    </form>
                </div>
            </div>
        </div>
        <div class="mfl-lodge-admin mfl-lodge-page">
            <div class="mfl-lodge-title">
                管理密码
            </div>
            <h1>
                管理密码</h1>
            <form action="#" onsubmit="return false;">
            <label>
                新旅馆管理密码：</label><input type="password" name="lodgeAdminPwd" class="small-input" />
            <a href="#" class="save PasswordSave" onclick="MFLSubmit(this);return false;">设定密码</a>
            <div class="notification attention">
				<div>
				<p>管理员平常负责更新旅馆各房间的入住状态以供访问者查询；</p>
				<p>管理员使用以上密码可以直接登陆“旅馆管理中心”进行每天的日常操作。</p>
				</div>
			</div>
            </form>
        </div>
        <div class="mfl-lodge-lodgeAlbum mfl-lodge-page">
            <div class="mfl-lodge-title">
                旅馆相册
            </div>
            <h1>
                旅馆相册</h1>
            <div class="mfl-lodgeAlbumList clearfix">
                <div class="mfl-list-itemTemplate">
                    <div class="photo">
                        <a href="#root,lodgeAlbum,lodgePhoto|$lodgePhotoId$" class="mfl-href-lodgePhotoId">
                            <img class="mfl-src-lodgePhotoUrlThumb mfl-alt-lodgePhotoTitle" />
                        </a><a href="#" title="删除" onclick="return false;" class="mfl-data-lodgePhotoId">
                            <img src="../Style/lodge/cross_circle.png" />
                        </a>
                        <div class="mfl-lodgePhotoType lodgePhotoType">
                        </div>
                    </div>
                </div>
            </div>
            <a href="#root,lodgeAlbum,lodgeNewPhoto" class="add">添加照片</a><br />
            <h2>当前相册分类：</h2>
            <table>
            <tbody class="mfl-photoTypeList">
                <tr class="mfl-list-itemTemplate">
                    <td class="col1"><div class="mfl-not-progressbar-percentage"></div>
                    </td>
                    <td class="mfl-html-type">
                    </td>
                </tr>
                </tbody>
            </table>
            <div class="progressbar">
            </div>
            <h3>相册已使用用空间：<span class="avaPhoto"></span>/<span class="capPhoto"></span></h3>
        </div>
        <div class="mfl-lodge-lodgeNewPhoto mfl-lodge-page">
            <div class="mfl-lodge-title">
                添加照片
            </div>
            <h1>
                添加照片</h1>
            <form action="#" onsubmit="return false;">
            <label>
                照片标题：</label>
            <input type="text" name="photoTitle" class="small-input" /><br />
            <label>
                放置位置：</label>
            <input type="text" name="photoType" class="mfl-val-lodgePhotoType small-input" /><br />
            <label>
                照片文件：</label>
            <input type="file" name="photoFile" class="small-input" /><br />
            <label>
                照片说明：</label><textarea name="photoContent"></textarea><br />
            <a href="#" onclick="MFLSubmit(this);return false;" class="add">添加照片</a><br />
            </form>
        </div>
        <div class="mfl-lodge-lodgePhoto mfl-lodge-page">
            <div class="mfl-lodge-title">
                修改照片
            </div>
            <h1>
                修改照片</h1>
            <div class="mfl-lodgePhotoList clearfix">
                <div class="mfl-list-itemTemplate">
                    <form action="#" onsubmit="return false;">
                    <input type="hidden" name="photoId" class="mfl-val-lodgePhotoId" />
                    <img class="mfl-src-lodgePhotoUrlSmall" />
                    <div class="photoInfo">
                        <label>
                            照片标题：</label>
                        <input type="text" name="photoTitle" class="mfl-val-lodgePhotoTitle small-input" /><br />
                        <label>
                            放置位置：</label>
                        <input type="text" name="photoType" class="mfl-val-lodgePhotoType small-input" /><br />
                        <label>
                            照片说明：</label>
                        <textarea name="photoContent" class="mfl-val-lodgePhotoContent"></textarea><br />
                        <a href="#" onclick="MFLSubmit(this);return false;" class="save">保存修改</a><br />
                    </div>
                </div>
                </form>
            </div>
        </div>
        <div class="mfl-lodge-roomNewPhoto mfl-lodge-page">
            <div class="mfl-lodge-title">
                添加照片
            </div>
            <h1>
                添加照片</h1>
            <form action="#" onsubmit="return false">
            <input type="hidden" name="roomId" class="roomId" />
            <label>
                照片标题：</label><input type="text" name="photoTitle" class="small-input" /><br />
            <label>
                照片文件：</label><input type="file" name="photoFile" class="small-input" /><br />
            <label>
                照片说明：</label><textarea name="photoContent"></textarea><br />
            <a href="#" onclick="MFLSubmit(this);return false;" class="add">添加照片</a><br />
            </form>
        </div>
        <div class="mfl-lodge-roomPhoto mfl-lodge-page">
            <div class="mfl-lodge-title">
            </div>
            <h1>
                修改照片</h1>
            <div class="mfl-roomPhotoList clearfix">
                <div class="mfl-list-itemTemplate">
                    <form action="#" onsubmit="return false;">
                    <input type="hidden" name="photoId" class="mfl-val-roomPhotoId" />
                    <img class="mfl-src-roomPhotoUrlSmall" />
                    <div class="photoInfo">
                        <label>
                            照片标题：</label>
                        <input type="text" name="photoTitle" class="mfl-val-roomPhotoTitle small-input" /><br />
                        <label>
                            照片说明：</label>
                        <textarea name="photoContent" class="mfl-val-roomPhotoContent"></textarea><br />
                        <a href="#" onclick="MFLSubmit(this);return false;" class="save">保存修改</a><br />
                    </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="mfl-lodge-template mfl-lodge-page">
            <div class="mfl-lodge-title">
                选择模板
            </div>
            <div class="mfl-templateList clearfix">
                <div class="mfl-list-itemTemplate">
                    <form action="#" onsubmit="return false;">
                    <h1>
                        选择模版：</h1>
                    <div class="mfl-templatesList clearfix " style="height:65px;">
                        <div class="mfl-list-itemTemplate">
                            <label>
                                模版名称：</label><span class="mfl-templateName"></span><br />
                            <label>
                                模版预览地址:</label><a href="$templatePreviewUrl$" onclick="MFLOpenIt(this);" target="_blank"
                                    class="mfl-href-templatePreviewUrl">预览</a><br />
                        </div>
                    </div>
                    <input class="mfl-val-lodgeTemplateName" type="hidden" name="templateName" />
                    <h1>
                        模版选项：</h1>
                    <div class="mfl-templatePropertiesList">
                        <div class="mfl-list-itemTemplate">
                            <label class="mfl-templatePropertyName">
                            </label>
                            :
                            <textarea class="mfl-templatePropertyValue mfl-name-templatePropertyKey"></textarea>
                        </div>
                    </div>
                    <a href="#" onclick="MFLSubmit(this);return false;" class="save">保存修改</a><br />
                    </form>
                    <br />
                </div>
            </div>
        </div>
        <iframe src="about:blank" id="mfl-hidden-iframe" class="mfl-hidden" name="mfl-hidden-iframe">
        </iframe>
    </div>
    <!--Begin #footer -->
    <div id="footer">
        <div class="container">
            &#169; Copyright 2011 xunnlv.com | Powered by <a href="<%=MFL.SharedConfig.MFLBaseUrl %>">
                寻旅.筑家</a>
        </div>
    </div>
    <!-- End #footer -->
</body>
</html>
