/// <reference path="/ISPReferences/Admin/ChangePassword.isp.js" />
/*<!--*/
$load("master/admin.master.js")({
    head: function () { },
    body: function () {


        /*-->
        <form action="/Auth/ChangePassword?redirect=" method="post">
        新密码<input name="password" type="password" />
        <button type="submit">更改密码</button>
        </form>
        
        <!--*/
    }
});
//-->