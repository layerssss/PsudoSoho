/*<!--*/
$load('master/frame.master.js')({
    title: u.name,
    head: function () {
    },
    body: function () {
        /*-->
<div class="row-fluid">
    <div class="span2">
        <div class="thumbnail">
            <img src="{$u.avatarUrlLarge$}" style="width: 100%; max-width: 300px;" />
            <div class="caption">
                <h3>
                    {$u.name$}</h3>
                <h5>
                    Based on <a href="/82.file">our privacy policy</a>, we don't storage user's profile.</h5>
                <p>See {$u.genderStringThirdAcc$}
                        on:<br />
                    <a href="{$u.providerProfileLink$}" class="btn"><i class="icon-user"></i>{$u.providerFriendlyName$}</a></p>
            </div>
        </div>
    </div>
    <div class="span10">
        <div class="row-fluid">
            <div class="span6">
                <div class="page-header">
                    <h4>
                        Teaching:</h4>
                </div>
                <!--*/
        for (var i = 0; i < teachings.length; i++) {
            var c = teachings[i];
            $load('inline/course.isp.js')({course:c});

            /*-->
                <hr />
                <!--*/
        } /*-->
            </div>
            <div class="span6">
                <div class="page-header">
                    <h4>
                        Learning:</h4>
                </div>
                <!--*/
        for (var i = 0; i < learnings.length; i++) {
            var c = learnings[i];
            $load('inline/course.isp.js')({ course: c });
            /*-->
                <hr />
                <!--*/
        } /*-->
            </div>
        </div>
    </div>
</div>
<!--*/
    }
});   //-->
