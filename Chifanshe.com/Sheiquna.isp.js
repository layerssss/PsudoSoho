/// <reference path="/ISPReferences/Sheiquna.isp.js" />
/*<!--*/

$load("master/main.master.js")({
    title:'那谁去拿外卖呢？',
    stores: stores
    ,
    head:function(){
        /*-->
<script type="text/javascript">
    $(function () {
        var live = function () {
            $('.add-player').show();
            $('.player').removeClass('choosen').addClass('player-disabled');
            $('.player').live('hover', function () {
                $('.player').addClass('player-disabled');
                $(this).prevAll().andSelf().removeClass('player-disabled');
            });
            var click = function () {
                $('.player-disabled').remove();
                $('.add-player').hide();
                $('.player').die('hover');
                interval = 1;
                counter = 0;
                if (timer) {
                    clearInterval(timer);
                }
                timer = setInterval(function () {
                    counter++;
                    if (counter > interval) {
                        counter = 0;
                        var r = Math.random();
                        interval += Math.random(1);
                        var ar = $('.player');
                        ar.each(function (i, e) {
                            if ($(e).hasClass('choosen')) {
                                $(e).removeClass('choosen');
                                $(ar[(i + 1) % ar.length]).addClass('choosen');
                                return false;
                            }
                        });
                        if (interval >= 20) {
                            clearInterval(timer);
                            timer = null;
                            $('.player.choosen').siblings('.player').addClass('player-disabled');
                            $('.player').die('click');
                            $('.player').live('click', function () {
                                return false;
                            });
                            $('.reset-player').slideDown();
                        }
                    }
                }, 10);
                $('.player:not(.add-player)').die('click').live('click', function () { return false; });
                $('.player:first').addClass('choosen');
                return false;
            };
            $('.player').live('click', click);
        };
        $('.add-player').click(function () {
            for (var i = 0; i < 3; i++) {
                $(this).prev().clone().insertBefore(this);
            }
            $('.player:not(.add-player)').each(function (i, e) {
                $(e).text('少年' + (i + 1));
            });
            return false;
        });
        var interval = 1;
        var counter = 0;
        var timer;
        live();
    });
</script>
<!--*/
    },
    main: function () {

        /*-->
<div class="main-cnt">
    <div class="suscess">
        <h3 class="order-s">
            yoooooooooooooo ！<span>订餐已经提交成功，马上会在派送途中啦。</span></h3>
        <div class="s-follow">
            关注一下嘛骚年，关注是美德什么的：
            <iframe width="100%" height="24" frameborder="0" allowtransparency="true" marginwidth="0"
                marginheight="0" scrolling="no" border="0" src="http://widget.weibo.com/relationship/followbutton.php?language=zh_cn&width=100%&height=24&uid=2236633074&style=2&btn=light&dpc=1">
            </iframe>
        </div>
    </div>
    <div class="nawaimai clearfix">
        <h4 class="nwm-title">
            谁去拿外卖？<span>神圣英雄少年或许就是你！<a href="/">(我还是匿了吧)</a></span></h4>
        <div class="player-cnt">
            <a href="#" class="player player-disabled">少年1</a> 
            <a href="#" class="player player-disabled">少年2</a>
            <a href="#" class="player player-disabled">少年3</a>
             <a href="#" class="player player-disabled">少年4</a>
            <a href="#" class="player player-disabled">少年5</a> 
            <a href="#" class="add-player">加多几个</a>
        </div>
        <a href="#" onclick="location.reload();return false;" style="display:none;" class="active reset-player">英雄少年表示屈才了……</a>
    </div>
    <script type="text/javascript">
        bgImg = "/images/tbbt.jpg";
    </script>
</div>
<style type="text/css">
    .cnt-nav
    {
        display: none !important;
    }
    .main-cnt
    {
        position: relative;
        background-color: #f5f5f5;
        width: 500px;
        margin-left: auto;
        margin-right: auto;
        left: 0 !important;
        margin-top: 50px;
        border-radius: 20px;
    }
    .share-to-weibo
    {
        display: none !important;
    }
    #logo
    {
        background-color: transparent;
    }
</style>
</div>
<!--*/
    }

});
//-->
