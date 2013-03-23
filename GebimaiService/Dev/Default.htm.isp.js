/// <reference path="/ISPReferences/Dev/Default.htm.isp.js" />
/*<!--*/
$load('Default.htm.master.js')({
    body: function () {
        /*-->
        <form method="post" action="/Dev/TestRobot?redirect=/Error/?message={replies}" class="form-inline">
        <input name="salut" />
        <input type="submit" class="btn btn-primary" value="调戏一下" />
        </form><!--*/
    for(var i =0;i<codex.length;i++){var c=codex[i];
            /*-->
            <form class="form-inline">
            <input readonly="readonly" class="" value="{$c.keyword$}" />
            <input readonly="readonly" class="input-xxlarge" value="{$c.content$}"/>
            <input class="input-mini" readonly="readonly" name="sort" value="{$c.sort$}" />
<a target href="{$c.id$}.codex" class="btn btn-primary">更改</a>
            <!--*/if(c.locked){/*-->
            <a href="#" onclick="return false;" class="btn btn-info disabled">该词条在30秒之内被{$c.lockdev$}打开</a>
            <!--*/}/*-->
</form>
<!--*/}/*-->
<div><a href="/Dev/AddCodex?redirect={id}.codex" class="btn btn-info">增加一个</a></div>
            
            
            <!--*/
    }
});
//-->