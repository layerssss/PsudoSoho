/*<!--*/
$load('master/frame.master.js')({
    title: 'Users',
    head: function () {
    },
    body: function () {
        /*-->
<div class="pagination pagination-centered" style="height: auto;">
    <ul>
        <!--*/
        var cur = -1;
        for (var i in alphabets) {
            cur=alphabets[i]==$subPage?i:cur;
        }
        if (cur==0) {
            /*-->
        <li class="disabled"><a href="#" onclick="return false;">«</a></li>
        <!--*/
        } else {
            /*-->
        <li><a href="{$alphabets[cur-1]$}.users">«</a></li>
        <!--*/
        }
        for (var i in alphabets) {
            var a = alphabets[i];
            var active = i == cur ? "active" : "";
            /*-->
        <li class="{$active$}"><a href="{$a$}.users">{$a$}</a></li>
        <!--*/
        }
        /*-->
        <!--*/
        if (cur == alphabets.length-1) {
            /*-->
        <li class="disabled"><a href="#" onclick="return false;">»</a></li>
        <!--*/
        } else {
            /*-->
        <li><a href="{$alphabets[Number(cur)+1]$}.users">»</a></li>
        <!--*/
        } /*-->
    </ul>
</div>
<hr class="clearfix" />
<div class="panel">
    <ul class="row thumbnails">
        <!--*/
        for (var i = 0; i < users.length; i++) {
              /*-->
        <li class="span2">
            <!--*/
            $load('inline/user.isp.js')({user:users[i]});
            /*-->
        </li>
        <!--*/
        } /*-->
    </ul>
</div>
<!--*/
    }
});          //-->
