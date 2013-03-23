/// <reference path="/ISPReferences/Admin/Stores.isp.js" />
/*<!--*/
$load("master/admin.master.js")({
head:function(){},
    body: function () {
    /*-->
    <div class="row thumbnails">
    <div class="store-list">
    	<ul class="clearfix">
    <!--*/
    for(var i=0;i<stores.length;i++){var s=stores[i];
        /*-->
     	<li><a href="{$s.id$}.store" class="thumbnail">{$s.title$}</a></li>
        <!--*/
    }
    /*-->
    </ul>
    </div>
    <style type="text/css">
    	.store-list {
    		margin-left:30px;
    	}
    	.store-list li {
    		display:block;
    		width:120px;
    		float:left;
    		margin-right:10px;
    	}
    	.store-list li a {
    		display:block;
    		padding:4px 8px;
    		background-color:#fff;
    	}
    	.add-store-btn {
    		margin-left:30px;
    	}
    </style>
        <div class="add-store-btn" style="margin-top:20px;"><a href="/Administrator/AddStore?title=null&redirect=" class="btn btn-primary">增加店家</a></div>
        </div>
        <!--*/}
});
//-->