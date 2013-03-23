/*<!--*/
$load('master/frame.master.js')({
    title: 'Mobile',
    head: function () { /*-->
<style type="text/css">
    .ipad
    {
        background-image: url('/img/iphone.png');
        height: 1142px;
        width: 620px;
        margin: 0 auto;
        position: relative;
    }
    .ipad > iframe
    {
        border: medium none;
        height: 619px;
        left: 103px;
        position: absolute;
        top: 208px;
        width: 428px;
    }
</style>
<script type="text/javascript">
    $(function () {
        $('.ipad>iframe').attr('src', location.hash ? location.hash.substring(1) : '/');
    });
</script>
<!--*/},
    body: function () {
        /*-->
<div class="row">
    <div class="span4">
        <div class="hero-unit">
        <h3>Try Goclassing.com</h3>
        <h2>on your Pad/Phone!</h2>
        <p>
        Just visit <br />http://goclassing.com/<br /> on your web browser.
        </p>
        <p>Life is so simple! ☺</p>
        </div></div>
    <div class="span8" style="">
        <div class="ipad visible-desktop">
            <iframe src="about:blank"></iframe>
        </div>
    </div>
</div>
<!--*/
     }
});
//-->
