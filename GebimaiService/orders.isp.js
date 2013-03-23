/// <reference path="/ISPReferences/orders.isp.js" />
/*<!--*/
$load('Default.htm.master.js')({
    head: function () {
    /*--><script src="js/jquery-ui-1.8.18.custom.min.js" type="text/javascript"></script><link
        href="css/south-street/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
<!--*/

    },
    body: function () {
        /*-->
<form class="page-header well form-inline">
显示<input type="text" id="date" value="{$date$}" />的当天订单
<script type="text/javascript">
    $(function () {
        $('#date').datepicker({
            onSelect: function (dateText, inst) {
                location.href = '{$alias$}-' + dateText + '.orders';
            },
            dateFormat: 'yymmdd'
        });
    });
</script>
</form>
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
            <div class="span3">
                地址:</div>
            <div class="span1">
                状态:</div>
            <div class="span2">
                送货员:</div>
        </div>
        <!--*/for(var i=0;i<orders.length;i++){var o=orders[i];
        var state='';
        switch(o.state){
            case 0:
                state='<span class="label label-inverse">未付款</span>';
                break;
                case 1:
                state='<span class="label label-info">待发货</span>';
                break;
                case 2:
                state='<span class="label label-success">已完成</span>';
                break;
                case 3:
                state='<span class="label label-important">已作废</span>';
                break;
                default:
                state='<span class="label">未知状态'+o.state+'</span>';
        }
        /*-->
        <div class="row-fluid">
            <div class="span1 ">
                <span class="badge badge-info">{$o.id$}</span></div>
            <div class="span2">
                {$o.time$}</div>
            <div class="span1 x-large">
                {$o.sum$}角({$o.sum-o.num*o.stock.importprice$})</div>
            <div class="span1">
                <a target="_blank" class="thumbnail" href="{$o.stock.barcode.url$}" title="{$o.stock.barcode.title$}">
                        <img src="{$o.stock.barcode.imgUrl$}" class="" style="width: 100%;max-width:100px;" />
                </a>
            </div>
            <div class="span1 x-large">
                {$o.num$}</div>
            <div class="span3">
                {$o.address$}</div>
            <div class="span1">
                {$state$}</div>
            <div class="span2">
                {$o.timespan.sender.alias$}</div>
        </div><hr />
        <!--*/}/*-->
</div>
<!--*/ } }); //-->
