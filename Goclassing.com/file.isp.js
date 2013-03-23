/*<!--*/

$load("master/cframe.master.js")({
    title:f.title,
    head: function () {
        /*--><!--*/
    },
    course: c,
    body: function () {/*-->
<div class="navbar-fixed-top hidden-phone subnav" style="top: 40px;">
    <div class="container">
        <div class="container">
            <ul class="breadcrumb nav">
                <li>{$c.title$}</li>
                <!--*/
        if (f.ext.length) {/*-->
                <li><span><a href="Course/OpenFile?fid={$f.id$}&ANTICSRF=" data-file="{$f.ext$}" data-anticsrf="href" target="_blank" style="display: block;"
                    class="btn btn-primary"><i class="icon-file icon-white"></i>Open '{$f.title$}.{$f.ext$}'</a></span></li>
                <!--*/
        } /*-->
                <li class="divider-vertical"></li>
                <li class="admin" style="float: right;">
                    <div>
                        <a class="btn btn-info" data-toggle="button" href="#" onclick="$('.intro').toggle();return false;">
                            <i class="icon-edit icon-white"></i>Edit</a></div>
                </li>
                
                <li style="float: right; margin: 0 10px;">
                    <div>
                        <div class="btn-group " title="font size" data-toggle="buttons-radio" style="float: right;">
                            <button class="btn active" onclick="$('section>p').css('font-size',$(this).html())">
                                small</button>
                            <button class="btn " onclick="$('section>p').css('font-size',$(this).html())">
                                medium</button>
                            <button class="btn" onclick="$('section>p').css('font-size',$(this).html())">
                                large</button>
                        </div>
                    </div>
                </li>
                <!--*/
        for (var i = 0; i < chapters.length; i++) {
            var ch = chapters[i]; if (ch.title) {/*-->
                <li><a class="btn" href="#{$ch.safeTitle$}">{$ch.title$}</a></li>
                <!--*/
            }
        } /*-->
                <li><a class="btn" href="#Discussion" style="display: block;"><i class="icon-comment">
                </i>Discussion</a></li>
            </ul>
        </div>
    </div>
</div>
<div class="row">
    <!--*/
        if (f.ext.length) {/*-->
    <a href="Course/OpenFile?id={$f.id$}&amp;ANTICSRF=" data-anticsrf="href" data-file="{$f.ext$}"  target="_blank" class="btn btn-primary visible-phone">
        <i class="icon-file icon-white"></i>Open '{$f.title$}.{$f.ext$}'</a>
    <!--*/
        } /*-->
    <a class="btn btn-info admin visible-phone" data-toggle="button" href="#" onclick="$('.intro').toggle();return false;">
        <i class="icon-edit icon-white"></i>Edit</a>
        
    <div class="btn-group visible-phone" title="font size" data-toggle="buttons-radio">
        <button class="btn active" onclick="$('section>p').css('font-size',$(this).html())">
            small</button>
        <button class="btn " onclick="$('section>p').css('font-size',$(this).html())">
            medium</button>
        <button class="btn" onclick="$('section>p').css('font-size',$(this).html())">
            large</button>
    </div>
    
    <div class="span12 intro">
        <div class=" well">
        <section id="Title">
        <h1 class="page-header">
            {$c.title$}: {$f.title$}</h1>
            {$htmlEncode(f.desc)$}
            </section>
        <!--*/
        for (var i = 0; i < chapters.length; i++) {
            var ch = chapters[i]; /*-->
        <section id="{$ch.safeTitle$}">
        <!--*/
            if (ch.title) {/*-->
        <h2 class="page-header">{$ch.title$}</h2>
        <!--*/
            } /*-->
        {$ch.content$}
        </section>
        <!--*/
        } /*-->
        
        </div>
        <section id="Discussion">
        <div class="well">
        <h2 class="page-header">Discussion</h2>
        </div>
        <!--*/
        for (var i = 0; i < replies.length; i++) {
            var r = replies[i]; /*-->
        <div class="well">
        <div class="row-fluid">
        <div class="span2">
        <!--*/
            $load('inline/user.isp.js')({user:r.user}); /*-->
        <h6>{$r.time$}</h6>
        </div>
        <div class="span10">

        <a class="close admin" href="/Course/DelComment?cid={$r.id$}&ANTICSRF=&redirect=%20" data-anticsrf="href" title="Delete it">&times;</a>
        {$htmlEncode(r.content)$}

        </div></div></div>
        <!--*/
        } /*-->
        <div class="well">
        <div class="row-fluid">
        <div class="span2"><br />
        </div>
        <div class="span10">
        <form class="form" action="/Course/Comment?ANTICSRF=&redirect=%20&fid={$f.id$}" data-anticsrf="action" 
    method="post">
    <fieldset>
        <legend>Say Something</legend>
        <div class="control-group">
            <div class="controls">
                <textarea name="content" wrap="soft" maxlength="1000" style="width:95%;" id="textarea2" rows="10"></textarea>
            </div>
        </div>
        <div class="form-actions">
            <button type="submit" class="btn btn-primary">
                Submit</button>
        </div>
    </fieldset>
    </form>

        </div></div></div>
        </section>
    </div><section class="intro span12" style="display: none;">
    <form class="well form-horizontal" action="/Course/EditFile?ANTICSRF=&redirect=%20&fid={$f.id$}" data-anticsrf="action" 
    method="post" enctype="multipart/form-data">
    <fieldset>
        <legend>Edit File Info</legend>
        <div class="control-group">
            <label class="control-label" for="course-title">
                Title:</label>
            <div class="controls">
                <input type="text" class="input-xlarge" id="course-title" name="title" value="{$f.title$}" />
            </div>
        </div>
        <div class="control-group">
  <label class="control-label" for="up-file">Attachment(optional):</label>
            <div class="controls">
  <input class="input-xlarge" id="up-file" type="file" class="" name="file" placeholder="select a file" />
  <!--*/if(f.ext!=""){ /*-->
  <a href="/Course/DetachFile?fid={$f.id$}&redirect=&ANTICSRF=" class="btn btn-danger" data-confirm="This attachment will be DELETED, are you sure?" data-anticsrf="href">Detach '{$f.title$}.{$f.ext$}'</a>
  <!--*/}/*-->
        </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="course-d">
                Description:</label>
            <div class="controls">
                <textarea maxlength="10000" class="input-xlarge" id="course-d" rows="5" name="desc">{$f.desc$}</textarea>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="textarea">
                Content:</label>
            <div class="controls">
                <p class="help-block">
                    Support tags(<a href="/101.file" target="_blank">view all...</a>):</p>
                <pre>
<!--*/
        $load('inline/contentHelper.isp.js')(); /*--></pre>
                <textarea name="content" wrap="soft" maxlength="10000" style="width:95%;" id="textarea" rows="20">{$f.content$}</textarea>
            </div>
        </div>
        <div class="form-actions">
            <button type="submit" class="btn btn-primary">
                Save</button>
            <button type="button" onclick="$('.btn-info:visible').trigger('click');" class="btn">
                Cancel</button>
        </div>
    </fieldset>
    </form></section>
</div>
<!--*/
    }
});
//-->
