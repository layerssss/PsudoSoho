/// <reference path="/ISPReferences/admin.isp.js" />
/*<!--*/
$load('Default.htm.master.js')({
    head: function () {
    /*--><script src="js/jquery.textExt.modified.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        // fix sub nav on scroll
        var $win = $(window)
      , $nav = $('.subnav')
      , navTop = $('.subnav').length && $('.subnav').offset().top - 40
      , isFixed = 0

        processScroll()

        $win.on('scroll', processScroll)

        function processScroll() {
            var i, scrollTop = $win.scrollTop()
            if (scrollTop >= navTop && !isFixed) {
                isFixed = 1
                $nav.addClass('subnav-fixed')
            } else if (scrollTop <= navTop && isFixed) {
                isFixed = 0
                $nav.removeClass('subnav-fixed')
            }
        }
    });
</script>
<!--*/

    },
    body: function () {
        /*-->
<div class="subnav">
    <ul class="nav nav-pills">
        <li><a href="#admin-area">区域信息</a></li>
        <li><a href="#cur-status">当前待付款订单</a></li>
        <li><a href="#cur-senders">当前正在上班的送货员</a></li>
        <li><a href="#stock">商品上下架管理</a></li>
        <li><a href="#manage-senders">快递员管理</a></li>
        <li><a href="#manage-addresses">可送达楼宇管理</a></li>
        <li><a href="Admin/JumpOrdersToday">查看今天的所有订单</a></li>
    </ul>
</div>
<div id="admin-container">
    <section id="admin-area" class="well">
        <dl>
            <dt>管理区域：</dt>
            <dd>
                {$area$}</dd>
        </dl>
    </section>
    <!-- END OF SIDEBAR -->
    <section id="cur-status" class="well">
            <h4 id="cur-status-header" class="well-header">
                当前待付款订单</h4>

                
            <div class="row-fluid">
                <div class="span1">
                    ID:</div>
                <div class="span2">
                    支付宝地址:</div>
                <div class="span1">
                    总额:</div>
                <div class="span3">
                    商品:</div>
                <div class="span1">
                    数量:</div>
                <div class="span4">
                    操作:</div>
            </div>
                <!--*/
        for (var i = 0; i < payingOrders.length; i++) {
            var o = payingOrders[i]; /*-->
                <div class="row-fluid">
                <div class="span1 ">
                    <span class="badge badge-info">{$o.id$}</span></div>
                <div class="span2">
                    {$o.alipay$}
                    </div>
                <div class="span1 x-large">
                    {$o.sum$}角</div>
                <div class="span3">
                    <a target="_blank" class="thumbnail row-fluid" href="{$o.stock.barcode.url$}">
                        <div class="span2">
                            <img src="{$o.stock.barcode.imgUrl$}" class="" style="width: 100%;max-width:100px;" /></div>
                        <div class="span10">
                            {$o.stock.barcode.title$}</div>
                    </a>
                </div>
                <div class="span1 x-large">
                    {$o.num$}</div>
                <div class="span4">
                    <a href="Admin/NotifyPayment?redirect=&orderId={$o.id$}" class="btn btn-info">私信发送AA提醒</a>
                    <a href="Admin/VerifyPayment?redirect=&orderId={$o.id$}" class="btn btn-primary">确认已付款</a>
                    
                    </div>
            </div><hr />
                        

                <!--*/
        } /*-->
            </table>
        </section>
    <section id="cur-senders" class="well">
            <h4 id="cur-senders-header" class="well-header">
                当前正在上班的送货员<a href="Admin/Refresh?redirect=" class="btn btn-info"><i class="icon-refresh icon-white"></i></a></h4>
                <div class="row-fluid" class="span2">
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
                
                <div class="row-fluid" class="span2">
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
        </section>
    <!-- END OF CUR-STATUS -->
    <section id="stock" class="well">
            <h4 id="stock-header" class="well-header">
                商品上下架管理</h4>
                <form action="Admin/AddStock?redirect=" method="post" class="row-fluid">
                <div class="span3">
                        <input type="text" name="barcode" placeholder="输入条形码或者展示地址" /></div><div class="span7">
                        <input type="text" name="price" placeholder="输入单价" />
                        <input type="text" name="importprice" placeholder="输入进价" />
                        <button class="btn btn-success" type="submit">
                            上架</button></div>
                </form>
                <!--*/
        for (var i = 0; i < stocks.length; i++) {
            var s = stocks[i]; /*-->
                <div class="row-fluid">
                    <div class="span1">
                        <a target="_blank" class="thumbnail" href="{$s.barcode.url$}">
                            <img src="{$s.barcode.imgUrl$}" style="width:100%;max-width:100px;" /></a>
                    </div>
                    <div class="span4">
                        <a target="_blank" href="{$s.barcode.url$}">{$s.barcode.title$}</a>
                    </div>
                    <div class="span3">
                        <a target="_blank" href="{$s.barcode.url$}">{$s.price$} 角/件</a>/
                        
                        <a target="_blank" href="{$s.barcode.url$}">{$s.importprice$} 角/件</a>
                    </div>
                    <div class="span4">
                        <a href="Admin/DelStock?redirect=&barcode={$urlEncode(s.barcode.bc)$}" class="btn btn-primary">
                            下架</a>
                    </div>
                </div>
                <!--*/
        } /*-->
     </section>
    <!-- END OF STOCK -->
    <section id="manage-senders" class="well">
        <h4 id="manage-senders-header" class="well-header">
            送货员管理</h4>
        <div class="row">
            <div class="span4">
                <form action="Admin/AddSender?redirect=" method="post">
                <label for="alias">
                    代号（英文字母）：</label>
                <input type="text" name="alias" class="span4" />
                <label for="weiboName">
                    微博昵称</label>
                <input type="text" name="weiboName" class="span4" />
                <label for="interval">
                    不同楼宇之间时间间隔（分钟）</label><input type="text" name="interval" class="span4" />
                <label for="intervalConnected">
                    同一楼宇之间时间间隔（分钟）</label><input type="text" name="intervalConnected" class="span4" />
                <label for="addresses">
                    可送达的楼宇</label>
                <textarea name="addresses" id="addresses" class="span4"></textarea>
                <script type="text/javascript">
                    $(function () {
                        $('#addresses').textext({
                            plugins: 'tags autocomplete ajax arrow prompt',
                            prompt: 'Add one...',
                            ajax: {
                                url: 'Public/GetAddresses?area={$urlEncode(area)$}',
                                dataType: 'json',
                                cacheResults: true
                            }, autocomplete: {
                                position: 'above'
                            }
                        });
                    });
                </script>
                <br>
                <button class="btn btn-success" type="submit">
                    增加送货员</button>
                </form>
            </div>
            <div class="span5 existed-senders">
                <h5 class="well-sub-header">
                    送货员</h5>
                <!--*/
        for (var i = 0; i < senders.length; i++) {
            var s = senders[i]; /*-->
                <span class="label label-info" title="不同楼宇：{$s.interval$}分钟；同一楼宇：{$s.intervalConnected$}分钟">{$s.alias$}<a href="Admin/DelSender?redirect=&alias={$s.alias$}"
                    class="close">&times;</a></span>
                <!--*/
        } /*-->
            </div>
        </div>
    </section>
    <section id="manage-addresses" class="well">
        <h4 id="manage-addresses-header" class="well-header">
            可送达楼宇管理</h4>
        <div class="row">
            <div class="span4">
                <form action="Admin/AddAddress?redirect=" method="post">
                <label for="address">
                    楼宇名称：</label>
                <input type="text" name="address" class="span4" id="address" />
                <br>
                <button class="btn btn-success" type="submit">
                    增加可送达楼宇</button>
                </form>
            </div>
            <div class="span5 existed-senders">
                <h5 class="well-sub-header">
                    现在的可送达楼宇</h5>
                <!--*/
        for (var i = 0; i < addresses.length; i++) {
            var a = addresses[i]; /*-->
                <span class="label label-info">{$a$}<a href="Admin/DelAddress?redirect=&address={$a$}"
                    class="close">&times;</a></span>
                <!--*/
        } /*-->
            </div>
        </div>
    </section>
</div>
<!--*/ } }); //-->
