/// <reference path="/ISPReferences/Dev/Default.htm.isp.js" />
/*<!--*/
$load('Default.htm.master.js')({
    body: function () {
        /*-->
            <form method="post" action="/Dev/EditCodex?id={$c.id$}&redirect=./">
            <input name="keyword" style="width:100%" value="{$c.keyword$}" /><br />eg. 草泥马|卧槽|尼玛<br />
            <input name="content" style="width:100%" value="{$c.content$}" /><br />eg. 亲请注意文明用语哦~[酷]<br />
            <input name="sort" style="width:100%" value="{$c.sort$}" /><br />eg. 1<br />
            <input type="submit" value="确定" class="btn btn-primary" />
            <a href="./" class="btn">取消</a>
            </form>
            
            <!--*/
    }
});
//-->