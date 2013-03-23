/*<!--*/
var f = file;
var icon = {
    'jpg': 'picture',
    'jpeg': 'picture',
    'bmp': 'picture',
    'tif': 'picture',
    'png': 'picture',
    'rar': 'folder-close',
    'zip': 'folder-close',
    'gz': 'folder-close',
    'tar': 'folder-close',
    'pdf': 'file',
    'doc': 'file',
    'docx': 'file',
    'pdf': 'file',
    'xls': 'file',
    'avi': 'film',
    'mov': 'film',
    'mpg': 'film',
    'mp4': 'film',
    'mpeg': 'film',
    'wmv': 'film',
    'rmvb': 'film',
    'm4v': 'film',
    'wav': 'headphones',
    'mp3': 'headphones',
    'm4a': 'headphones',
    'ogg': 'headphones',
    'wma': 'headphones'
}[f.ext];
//icons from http://twitter.github.com/bootstrap/base-css.html#icons
icon = f.ext == '' ? 'comment' : (icon ? icon : 'file');
/*-->

                <div class="span6">
                    <h4>
                        <a href="{$f.id$}.file"><i class="icon-{$icon$}"></i>&nbsp;{$f.title$}</a>
                    </h4>
                    {$htmlEncode(f.desc)$}
                    <div class="smaller">
                        updated: {$f.timeString$}</div>
                </div>
                <div class="span6">
                    <ul class="thumbnails">
                        <!--*/
for (var k = 0; k < f.paticipated.length; k++) {
    var u = f.paticipated[k];
    /*-->
    <li>
    <!--*/
    $load('inline/user.isp.js')({ user: u, hideName: true });
    /*-->
    </li>
    <!--*/
}
/*-->
</ul>
</div>
<div class="span12" style="border-top:1px solid #ccc;margin-left:-1px;"></div>
<!--*/
//-->