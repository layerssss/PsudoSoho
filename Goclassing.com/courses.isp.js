/*<!--*/
$load('master/frame.master.js')({
    title: 'Courses',
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
        <li><a href="{$alphabets[cur-1]$}.courses">«</a></li>
        <!--*/
        }
        for (var i in alphabets) {
            var a = alphabets[i];
            var active = i == cur ? "active" : "";
            /*-->
        <li class="{$active$}"><a href="{$a$}.courses">{$a$}</a></li>
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
        <li><a href="{$alphabets[Number(cur)+1]$}.courses">»</a></li>
        <!--*/
        } /*-->
    </ul>
</div>
<hr class="clearfix" />
<div class="panel">
    <div class="row">
        <!--*/
        for (var i = 0; i < courses.length; i++) {
            var c = courses[i]; /*-->
        <div class="span6">
            <!--*/
            $load('inline/course.isp.js')({ course: c }); /*-->
            <hr />
        </div>
        <!--*/
        } /*-->
    </div>
</div>
<!--*/
    }
});               //-->
