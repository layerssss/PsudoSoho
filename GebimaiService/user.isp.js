/// <reference path="/ISPReferences/user.isp.js" />
/*<!--*/
$load('Default.htm.master.js')({
    body: function () {
        /*-->
        <div class="hero-unit user-hero">
        <h1 class="page-header">
        <img src="<!--*/
        $(userInfo.avatarUrl); /*-->" alt="" class="thumbnail" width="50" height="50">
      Welcome,
      <span class="label"><!--*/
        $(userInfo.name); /*--></span></h1>
    <div class="links">
    <!--*/
        if (adminInfo) {
            $('<a href="/' + adminInfo.alias + '.admin" title="管理员首页" class="btn btn-primary btn-large" >管理员首页</a>');
        }
        if (devInfo) {
            $('<a href="/Dev" title="开发者首页" class="btn btn-primary btn-large" >开发者首页</a>');
        }
        if (senderInfo) {
            $('<a href="/' + senderInfo.alias + '.sender" title="送货员首页" class="btn btn-primary btn-large" >送货员首页</a>');
        }
        /*--> 
        </div>
        </div>
        <!--*/
    }
});
//-->