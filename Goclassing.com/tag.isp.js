/*<!--*/
$load('master/frame.master.js')({
    title: htmlEncode($subPage),
    head: function () {
    },
    body: function () {
        /*-->
        
        <h2 class="page-header">Tag:{$htmlEncode($subPage)$}</h2>
        <div class="panel">
        <div class="row">
        <!--*/
        for (var i = 0; i < tags.length; i++) {/*-->
        <div class="span6">
            <!--*/
            $load('inline/course.isp.js')({ course: tags[i].course }); /*-->
            <hr />
        </div>
        <!--*/
        } /*-->
    </div>
</div>
<!--*/
    }
});       
//-->