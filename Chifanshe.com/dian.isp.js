/// <reference path="/ISPReferences/dian.isp.js" />
/*<!--*/


$load("master/main.master.js")({
    title: store.title,
    stores: stores
    ,
    main: function () {
        /*-->
        <div class="main-cnt">
        <script type="text/javascript" >
        bgImg="/storeImg/{$store.id$}.jpg";
        </script>
        <div class="shop-cnt">
        <h3 class="shop-title">
        {$store.title$}</h3>
        <div class="shop-des">
        <span class="de-time">送餐时间：{$store.speed$}</span> <span class="up-time">营业时间：
        <!--*/
        for (var i = 0; i < timespans.length; i++) {
            var t = timespans[i]; /*-->
        {$t.startTime$} - {$t.stopTime$}
            <!--*/
        } /*-->
        </span>
        <span class="de-price">最低起送：{$store.pricelimit$}元</span>
    </div>
    <form action="/Actions/SubmitOrder?sid={$store.id$}&redirect=/Sheiquna" class="cashier clearfix" method="post">
    <input type="hidden" name="data" />
        <div class="cashier-cnt clearfix">
            <select name="building" id="building" class="select-loc iselect" style="display: none;">
                <option value="0" selected="selected">-- 请选择宿舍楼 --</option>
                <optgroup label="暨南大学广州本部" id="guangzhouLoc">
                    <option value="真茹A1北">真茹A1北</option>
                    <option value="真茹A2南">真茹A2南</option>
                    <option value="真茹B1北">真茹B1北</option>
                    <option value="真茹B2南">真茹B2南</option>
                    <option value="真茹21B">真茹21B</option>
                    <option value="真茹21A">真茹21A</option>
                    <option value="真茹23">真茹23</option>
                    <option value="真茹24">真茹24</option>
                    <option value="真茹25">真茹25</option>
                    <option value="真茹26">真茹26</option>
                    <option value="金陵4栋1-5层">金陵4栋</option>
                    <option value="金陵1栋">金陵1栋</option>
                    <option value="金陵2栋">金陵2栋</option>
                    <option value="金陵3栋">金陵3栋</option>
                    <option value="老建阳1栋">老建阳1栋</option>
                    <option value="老建阳2栋">老建阳2栋</option>
                    <option value="新建阳5栋">新建阳5栋</option>
                    <option value="新建阳4栋">新建阳4栋</option>
                    <option value="新建阳3栋">新建阳3栋</option>
                    <option value="新建阳2栋">新建阳2栋</option>
                    <option value="新建阳1栋">新建阳1栋</option>
                    <option value="周转房B">周转房B</option>
                    <option value="周转房C">周转房C</option>
                </optgroup>
            </select>
            <!--							<input type="text" name="roomNo" id="roomNo" class="form-text room-no" placeholder="房间号"/>-->
            <input type="text" name="phNo" id="phNo" class="form-text ph-no" placeholder="联系电话" />
            <input type="text" name="addNote" class="form-text note-input" id="addNote" placeholder="留个备注给店家？如少辣、不要葱花、香菜" />
        </div>
        <!--*/
        if (isOpen) {/*-->
        <div class="done">
            <div class="whole-price">
                &yen; <span class="wp-cnt">0</span> 元</div>
            <input value="选好了" type="submit" class="next-btn sexybutton sexysimple sexygreen"/>
            <div id="wrongPhNo" class='hidden-text'><h4 class='alart-text'>亲，您电话号码好像填错了的说</h4></div>
            <div id="underLimit" class='hidden-text'><h4 class='limit-text'>亲，您选的外卖总价不到起送价呀</h4></div>
        </div>
        <!--*/
        }
        else { /*-->
        <div class="closed">我们打烊了亲……</div>
        <!--*/
        } /*-->
    </form>
    <script type="text/javascript">
    	function isNumber( s ){ 
    		var regu = "^[0-9]+$"; 
    		var re = new RegExp(regu); 
    		if (s.search(re) != -1) { return true;} else { return false;} 
    	}
    	function isUnderLimit() {
    		var priceLimit = {$store.pricelimit$};
    		var wprice = $(".wp-cnt").text();
    		wprice = Number(wprice);
    		return wprice<priceLimit;
    	}
    	$(".cashier").submit(function () {
    		var thisNum = $("#phNo").val();
    		if (isNumber(thisNum) == false) {
    			$(".hidden-text").fadeOut();
    			$("#wrongPhNo").fadeIn();
    			return false;
    		} else {
    			$("#wrongPhNo").fadeOut();
    		}
    		if(isUnderLimit()){
    			$(".hidden-text").fadeOut();
    			$("#underLimit").fadeIn();
    			return false;
    		}else{
    			$("#underLimit").fadeOut();
    		}
    	});
    </script>
</div>
<div class="goods-cnt">
    
        <!--*/
        for (var i = 0; i < foods.length; i++) {
            var f = foods[i];
            if (f.title == '') {
                continue;
            }
            var options = eval('a=' + f.optiondata + ';'); /*-->
        <div class="fd fd-set-wrap clearfix" foodId="{$f.id$}">
        <div class="fd-title">
            <div class="price">
                <span class="single-price" price="{$f.price$}">{$f.priceString$}</span>
            </div>
            <span class="fd-title-cnt">{$f.title$}</span>
            <!--*/
            for (var j in options) {
                /*-->
                <div class="opt drink">
                <span class="opt-default">选择{$j$}</span>
                <div class="sub-opt">
                <ul class="opt-list">
                <!--*/
                for (var k = 0; k < options[j].length; k++) {
                    var o = options[j][k];
                    /*-->
                    <li><a href="#">{$o$}</a></li>
                    <!--*/
                } /*-->
                </ul>
                </div>
                </div>
                <!--*/
            } /*-->
            <div class="order-num">
                0</div>
        </div>
        <a href="#" class="rm cal">-</a> <a href="#" class="add cal">+</a>
        </div>
        <!--*/
        } /*-->
</div>
<!--[if IE]>
	<script type="text/javascript">
		var zIndexNumber = 1000;
		// Put your target element(s) in the selector below!
		$(".fd").each(function() {
		        $(this).css('zIndex', zIndexNumber);
		        zIndexNumber -= 5;
		});
	</script>
<![endif]-->
</div>
<!--*/
    }

});

//-->
