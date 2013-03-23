/// <reference path="/ISPReferences/Admin/Default.htm.isp.js" />
/*<!--*/
$load("master/admin.master.js")({
head:function(){},
    body: function () {


        /*-->
        <form class="form-inline" action="/Auth/Login?redirect=" method="post">
        密码<input name="password" type="password" />
        <button class="btn btn-primary" type="submit">登陆</button>
        </form>
        
        <!--*/
    }
});
//-->