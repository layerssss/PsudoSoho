/*<!--*/

$load('master/frame.master.js')({
    title: 'Tags',
    head: function () {
        /*-->
        <!--*/
    }, body: function () {

        /*-->
        <h2 class="page-header">
        All tags:</h2>
        <ul class="thumbnails">
        <!--*/
        for (var i in tags) {
            /*-->
            <li><a href="{$htmlEncode(i)$}.tag" class="btn btn-large"><i class="icon-tag">
            </i><span class="badge">{$tags[i]$}</span>
            <span>{$i$}</span>
            </a></li>
            <!--*/
        }
        /*-->
        </ul>
        <!--*/
    } 
});
//-->
