///
<reference path="/ISPReferences/sender.isp.js" />
/*<!--*/
$load('Default.htm.master.js')({
    body: function () {
        var activeState=activeInfo ? "正在上班" : "未上班";
        /*-->
<div class="row">
    <div class="span2">
        <div class="well sidebar">
            <dl>
                <dt>负责区域：</dt>
                <dd>
                    {$area$}</dd>
                <dt>负责地址：</dt>
                <dd>
                    {$addresses$}</dd>
                <dt>当前状态：</dt>
                <dd>
                    {$activeState$}<a href="Sender/Refresh?redirect=" class="btn btn-info"><i class="icon-refresh icon-white"></i></a></dd>
                <!--*/if(activeInfo){/*-->
                <dt>正在接受的地址</dt>
                <dd>
                    &nbsp;{$activeInfo.lastAddress$}</dd>
                <dt>上班时间</dt>
                <dd>
                    {$activeInfo.start$}</dd>
                <dt>下班时间</dt>
                <dd>
                    {$activeInfo.stop$}</dd>
                <!--*/}/*-->
            </dl>
        </div>
    </div>
    <div class="span10">
        <div>
            <ul class="nav nav-tabs">
                <li class="active"><a href="#feedback" data-toggle="tab" title="当你送完一个订单之后请在此反馈订单">反馈订单</a></li>
                <!--*/if(activeInfo){/*-->
                <li><a href="#send" data-toggle="tab" title="该操作会清空“正在接受的地址”，这样下一次接受的订单便会至少是在{$activeInfo.sender.interval$}分钟之后。">我要去发货了</a></li>
                <!--*/}/*-->
                <li><a href="#status" data-toggle="tab" title="查看一下当前还有哪些同事在上班，以及它们能接受哪些区域的订单，以决定你的上班时间。">查看当前区域的同事</a></li>
                <!--*/if(activeInfo){/*-->
                <li><a href="#deactivate" data-toggle="tab" title="如果你有急事需要提前下班，或者想延后下班时间，即可尝试在此处更改你的下班时间。">更改下班时间</a></li>
                <!--*/}else{/*-->
                <li><a href="#activate" data-toggle="tab" title="请按实际情况输入你的预计下班时间，到了下班时间后系统将不再自动给你分配订单">上班</a></li>
                <!--*/}/*-->
            </ul>
            <div class="tab-content">
                <div id="status" class="tab-pane">
                    <h4 id="cur-senders-header" class="well-header">
                        “{$area$}”区域当前正在上班的送货员</h4>
                    <div class="row-fluid span2">
                        <div class="span2">
                            代号
                        </div>
                        <div class="span2">
                            开始上班时间
                        </div>
                        <div class="span2">
                            下班时间
                        </div>
                        <div class="span2">
                            当前可接受订单的时间
                        </div>
                        <div class="span4">
                            可以接受的地址
                        </div>
                    </div>
                    <!--*/
        for (var i = 0; i < activeSenders.length; i++) {
            var a = activeSenders[i]; /*-->
                    <div class="row-fluid span2">
                        <div class="span2">
                            {$a.sender.alias$}
                        </div>
                        <div class="span2">
                            {$a.start$}
                        </div>
                        <div class="span2">
                            {$a.stop$}
                        </div>
                        <div class="span2">
                            {$a.cur$}
                        </div>
                        <div class="span4">
                            {$a.sender.addresses$}
                        </div>
                    </div>
                    <!--*/
        } /*-->
                </div>
                <!--*/if(activeInfo){/*-->
                <form id="deactivate" action="/Sender/Deactivate?redirect=" method="post" class="tab-pane form-inline">
                
                新的下班时间：<input type="text" style="" name="stop" class="timepicker" placeholder="示例：20:00" />
                <button class="btn btn-danger">
                    更改下班时间</button>
                </form>
                <div id="send" class="tab-pane">
                    
                    <a href="/Sender/ResetLastAddress?redirect=" class="btn btn-info">我要去送货了</a>
                </div>
                <!--*/}else{/*-->
                <form id="activate" action="/Sender/Activate?redirect=" method="post" class="tab-pane form-inline ">
                
                预计下班时间：<input type="text" style="" name="stop" class="timepicker" placeholder="示例：20:00" />
                <button class="btn btn-danger">
                    上班</button>
                </form>
                <!--*/}/*-->
                <form id="feedback" action="/Sender/Feedback?redirect=" method="post" class="active tab-pane form-inline">
                
                订单ID：<input type="text" style="" name="orderId" placeholder="" />
                <button class="btn btn-success">
                    反馈订单</button>
                </form>
            </div>
        </div>
        <div class="well">
            <h4 class="page-header">
                待发货订单</h4>
            <div class="row-fluid">
                <div class="span1">
                    ID:</div>
                <div class="span2">
                    时间:</div>
                <div class="span1">
                    总额:</div>
                <div class="span1">
                    商品:</div>
                <div class="span1">
                    数量:</div>
                <div class="span6">
                    地址:</div>
            </div>
            <!--*/for(var i=0;i<orders.length;i++){var o=orders[i];/*-->
            <div class="row-fluid">
                <div class="span1 ">
                    <span class="badge badge-info">{$o.id$}</span></div>
                <div class="span2">
                    {$o.time$}</div>
                <div class="span1 x-large">
                    {$o.sum$}角({$o.sum-o.num*o.stock.importprice$})</div>
                <div class="span1">
                    <a target="_blank" class="thumbnail" title="{$o.stock.barcode.title$}" href="{$o.stock.barcode.url$}">
                         <img src="{$o.stock.barcode.imgUrl$}" class="" style="width: 100%; max-width: 100px;" />
                    </a>
                </div>
                <div class="span1 x-large">
                    {$o.num$}</div>
                <div class="span4">
                    {$o.address$}</div>
                <div class="span2">
                    <a title="当你送完一个订单之后请点此反馈订单" href="/Sender/Feedback?redirect=&orderId={$o.id$}" class="btn btn-success"><i class="icon-question-sign icon-white">
                    </i>反馈</a>
                </div>
            </div>
            <hr />
            <!--*/}/*-->
        </div>
    </div>
</div>
<!--*/
    }
});
//-->
