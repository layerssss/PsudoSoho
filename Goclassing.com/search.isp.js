/*<!--*/
$load('master/frame.master.js')({
    title: htmlEncode($subPage),
    head: function () {
    },
    body: function () {
        if(users.length){/*-->
<h2 class="page-header">
    Users</h2>
    <!--*/
    for(var i=0;i<users.length;i++){var u=users[i];
    /*-->
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

    }}/*-->
<h2 class="page-header">
    Courses</h2>
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
});
//-->
