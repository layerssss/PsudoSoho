/// <reference path="/ISPReferences/Admin/store.isp.js" />
/*<!--*/
$load("master/admin.master.js")({
head:function(){},
    body: function () {


        /*-->
<h1 class="page-header">
    {$store.title$}</h1>

    <form action="/Administrator/ChangeBackground?redirect=&id={$store.id$}" class="form-inline" method="post" enctype="multipart/form-data">背景图片：<input type="file" name="file" />
    <button class="btn btn-primary" type="submit">上传</button>
    </form>
    <form action="/Administrator/EditStore?redirect=&id={$store.id$}" class="form-inline row msg-form" method="post">
    <div class="span4"><label for="title">标题：</label><input name="title" value="{$store.title$}" /></div>
    <div class="span4"><label for="alias">别名：</label><input name="alias" value="{$store.alias$}" /></div>
    <div class="span4"><label for="phone">电话号码：</label><input name="phone" value="{$store.phone$}" /></div>
    <div class="span4"><label for="pricelimit">起送价格：</label><input name="pricelimit" value="{$store.pricelimit$}" /></div>
    <div class="span4"><label for="speed">送货速度：</label><input name="speed" value="{$store.speed$}" /></div>
    <div class="span4"><label for="startTime">上班时间：</label><input name="startTime" value="{$timespans[0].startTime$}" /></div>
    <div class="span4"><label for="stopTime">下班时间：</label><input name="stopTime" value="{$timespans[0].stopTime$}" /></div>
    <button class="btn btn-primary msg-btn" type="submit">更改</button>
    </form>
<h2>
    菜单</h2>
<!--*/for(var i=0;i<foods.length;i++){var f=foods[i];/*-->
<form class="form-inline" action="/Administrator/EditFood?redirect=&id={$f.id$}" method="post">
菜名：<input type="text" name="title" value="{$f.title$}" />
价格：<input type="text" name="price" value="{$f.price$}" />角
<a href="#" onclick="$(this).next().modal();return false;" class="btn btn-info"><i
    class="icon-pencil icon-white"></i>选项</a>
<div class="modal" style="display: none;">
    <div class="modal-header">
        <a class="close" data-dismiss="modal">×</a>
        <h3>
            选项</h3>
    </div>
    <div class="modal-body">
        <p>
            One fine body…</p>
        <textarea name="optiondata">{$f.optiondata$}</textarea>
    </div>
    <div class="modal-footer">
        <a href="#" class="btn-primary btn" onclick="try{var obj=eval('o='+$(this).closest('.modal').find('textarea').val());}catch(e){alert('语法错误，这里应该输入json');return false;}$(this).closest('.modal').modal('hide');return false;">
            确定</a>
    </div>
</div>
<button class="btn btn-primary" type="submit">
    更改</button>


</form>
<!--*/
} /*-->
<div>
    <a href="/Administrator/AddFood?title=&redirect=&sid={$store.id$}" class="btn btn-info"><i class="icon-plus-sign icon-white"></i>增加菜</a>
</div>
<h2>
    订单统计</h2>
    <div class="row">
        <!--*/for(var day in orders){/*-->
    <div class="span3">
    {$day$}
    <span class="badge">{$orders[day].length$}</span>
    </div>
        <!--*/}/*-->
    </div>
<!--*/
    }
});
//-->
