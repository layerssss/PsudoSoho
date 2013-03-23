var bgImg="./images/bg.jpg";
var total = 0;
$(document).ready(function () {
    $.backstretch(bgImg, { speed: 150 });
    var wholePrice;
    $('.iselect').iSimulateSelect();
    // click animate
    $(".i_selectoption dt").hover(function () {
        $(this).addClass("dt-hover");
    }, function () {
        $(this).removeClass("dt-hover");
    });
    $(".i_selectoption dt").toggle(function () {
        $(".i_selectoption dt").removeClass("cur-dt");
        $(this).addClass("cur-dt");
        $(this).nextUntil("dt").fadeIn();
    }, function () {
        $(".i_selectoption dt").removeClass("cur-dt");
        $(this).remove("cur-dt");
        $(this).nextUntil("dt").fadeOut()
    });
    // set placeholder for ie
    $('input[placeholder]').placeholder();
    // submenu hover
    $(".sub-opt").parent().addClass("sub-parent");
    $(".sub-parent").hover(function () {
        $(this).addClass("sub-parent-cur");
        $(this).find(".sub-opt").show();
    }, function () {
        $(this).removeClass("sub-parent-cur");
        $(this).find(".sub-opt").hide();
    });
    $(".opt-list a").click(function () {
        $(this).parent().parent().parent().parent().find(".opt-default").text($(this).text());
    });
    function sum(arguments) {
        var r = 0;
        for (var i = 0; i < arguments.length; i++) {
            r = arguments[i] + r;
        }
        wholePrice = r;
    };
    // cal
    $(".order-num").hide();
    $(".cal").click(function () {
        var singleWholePrice = 0;
        var $button = $(this);
        var jNum = $button.parent().find(".order-num");
        var num = jNum.text();
        if (!num) {
            num = 0;
        }
        num = Number(num);
        var singlePrice = Number($button.parent().find(".single-price").attr('price'));
        if ($button.text() == "+") {
            num++;
            total += singlePrice;
        } else {
            if (num) {
                num--;
                total -= singlePrice;
            }
        }
        jNum.html(String(num));
        if (num) {
            jNum.show();
        } else {
            jNum.hide();
        }
        $('.whole-price .wp-cnt').text((total / 10).toFixed(2));
    });
    $('.cashier').submit(function () {
        var data = '';
        $('.fd').each(function (i,e) {
            var num = Number($(e).find('.order-num').text());
            if (num) {
                data += $(e).attr('foodId') + '|';
                $(e).find('.opt-default').each(function (i2,e2) {
                    data += $(e2).text();
                });
                data += '|';
                data += num + '|';
            }
        });
        $(this).find('[name="data"]').val(data);
    });
    $('.whole-price .wp-cnt').text((total / 10).toFixed(2));
});