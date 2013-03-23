/*<!--*/
$load("master/cframe.master.js")({
    head: function () {
    /*-->
<script src="/js/jquery.textExt.min.js" type="text/javascript"></script>
<script type="text/javascript" src="/js/colorpicker.js"></script>
<link href="/css/colorpicker.css" rel="stylesheet" type="text/css" />
<!--*/
    },
    title:c.title,
    course:c,
    body: function () {
        /*-->
<div class="navbar-fixed-top hidden-phone subnav" style="top: 40px;">
    <div class="container">
        <div class="container">
            <ul class="breadcrumb nav clearfix">
                <li class="admin" style="float: right;">
                    <div>
                        <a class="btn btn-info" data-toggle="button" href="#" onclick="$('.intro').toggle();return false;">
                            <i class="icon-edit icon-white"></i>Edit</a></div>
                </li>
                <!--*/if(!c.pub){/*-->
                <li class="not admin" style="float: right;">
                    <div>
                        <a class="btn btn-success" href="/Course/Join?ANTICSRF=&amp;redirect=%20&amp;id={$c.id$}" data-anticsrf="href"><i class="icon-plus-sign icon-white">
                        </i>Join</a> <a class="btn btn-danger" href="/Course/Exit?ANTICSRF=&amp;redirect=%20&amp;id={$c.id$}" data-anticsrf="href">
                            <i class="icon-remove-sign icon-white"></i>Exit</a></div>
                </li>
                <!--*/}else{/*-->
                <li class="not admin" style="float: right;">
                    <div>
                        <a class="btn btn-success disabled" href="#" onclick="return false;" title="This course is open for public.">
                            <i class="icon-info-sign icon-white"></i>Public Course</a></div>
                </li>
                <!--*/}/*-->
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
                <!--*/for(var i=0;i<chapters.length;i++){var ch=chapters[i];if(ch.title){/*-->
                <li><a class="btn" href="#{$ch.safeTitle$}">{$ch.title$}</a></li>
                <!--*/}}/*-->
            </ul>
        </div>
    </div>
</div>
<div class="row">
    <!--*/if(!c.pub){/*-->
    <a href="#" class="btn visible-phone" data-toggle="button" onclick="$(this).next().children().toggleClass('hidden-phone');return false;">
        See members...</a>
    <!--*/}/*-->
    <div class="span2">
    <div class="well hidden-phone">
        <section class="">
        <h5>Teacher:</h5>
        <!--*/
        $load('inline/user.isp.js')({user:c.user});
        /*-->
        <hr />
        <h5>Tags</h5>
        <div class="tags">
                        <!--*/for(var i=0;i<c.tag.length;i++){/*-->
                        <a href="{$c.tag[i]$}.tag"><i class="icon-tag icon-grey"></i><em>{$c.tag[i]$}</em></a>
                        <!--*/}/*-->
                    </div>
                    <hr />
                    
    <!--*/if(!c.pub){/*-->
        <div class="admin">
        <h5>New students:</h5>
    <ul class="row thumbnails">
        <!--*/for(var i =0;i<joinings.length;i++){/*-->
        <li class="span2">
        <div class="row-fluid">
        <div class="span10"><!--*/
        $load('inline/user.isp.js')({user:joinings[i]});/*--></div>
        <div class="span2 admin"><a href="Course/Aproove?id={$urlEncode(c.id)$}&amp;uid={$joinings[i].id$}&amp;ANTICSRF=&amp;redirect=%20" data-anticsrf="href" class="btn btn-success btn-mini"><i class="icon-ok icon-white"></i></a>
                    <a title="Reject" data-confirm="Are you sure?" href="Course/Disaproove?id={$urlEncode(c.id)$}&amp;uid={$joinings[i].id$}&amp;ANTICSRF=&amp;redirect=%20" data-anticsrf="href" class="btn btn-danger btn-mini" style="margin-right:5px;"><i class="icon-remove icon-white"></i></a>
                    </div>
        </div>
            
        </li>
        <!--*/
} /*-->
    </ul>
        <hr /></div>
        <h5>Students</h5>
    <ul class="row thumbnails">
        <!--*/for(var i =0;i<learnings.length;i++){var u=learnings[i];/*-->
        <li class="span2"><div class="row-fluid">
        <div class="span10"><!--*/
        $load('inline/user.isp.js')({user:learnings[i]});/*--></div>
        <div class="span2 admin">
                    <a title="Remove" data-confirm="Are you sure?" href="Course/Disaproove?id={$urlEncode(c.id)$}&amp;uid={$learnings[i].id$}&amp;ANTICSRF=&amp;redirect=%20" data-anticsrf="href" class="btn btn-danger btn-mini" style="margin-right:5px;"><i class="icon-remove icon-white"></i></a>
                    </div>
        </div></li>
        <!--*/
} /*-->
    </ul>
    <!--*/}/*-->
        </section></div>
    </div>
    <a class="btn btn-info admin visible-phone" data-toggle="button" href="#" onclick="$('.intro').toggle();return false;">
        <i class="icon-edit icon-white"></i>Edit</a>
    <!--*/if(!c.pub){/*-->
    <a class="btn btn-success not admin visible-phone" href="/Course/Join?ANTICSRF=&amp;redirect=&amp;id={$c.id$}" data-anticsrf="href">
        <i class="icon-plus-sign icon-white"></i>Join</a> <a class="btn btn-danger not admin visible-phone"
            href="/Course/Exit?ANTICSRF=&amp;redirect=&amp;id={$c.id$}" data-anticsrf="href"><i class="icon-remove-sign icon-white">
            </i>Exit</a>
    <!--*/}else{/*-->
    <a class="btn btn-success disabled not admin visible-phone" href="#" onclick="return false;"
        title="This course is open for public."><i class="icon-info-sign icon-white"></i>
        Public Course</a>
    <!--*/}/*-->
    <div class="btn-group visible-phone" title="font size" data-toggle="buttons-radio">
        <button class="btn active" onclick="$('section>p').css('font-size',$(this).html())">
            small</button>
        <button class="btn " onclick="$('section>p').css('font-size',$(this).html())">
            medium</button>
        <button class="btn" onclick="$('section>p').css('font-size',$(this).html())">
            large</button>
    </div>
    <div class="span10 intro">
        <div class=" well">
            <!--*/for(var i=0;i<chapters.length;i++){var ch=chapters[i];/*-->
            <section id="{$ch.safeTitle$}">
        <!--*/if(ch.title){/*-->
        <h2 class="page-header">{$ch.title$}</h2>
        <!--*/}/*-->
        {$ch.content$}
        </section>
            <!--*/}/*-->
        </div>
    </div>
    <section class="intro span10" style="display: none;">
    
    <form class="well form-horizontal" action="/Course/Edit?ANTICSRF=&amp;redirect=" data-anticsrf="action" enctype="multipart/form-data" 
    method="post">
    <fieldset>
        <legend>Edit Course Info</legend>
        <div class="control-group">
            <label class="control-label" for="course-id">
                ID:</label>
            <div class="controls">
                <input readonly="readonly" name="id" type="text" value="{$c.id$}" class="input-xlarge" id="course-id" />
            </div>
        </div>
        <div class="control-group"><label class="control-label" for="course-id">
                Preview:</label>
               <div class="controls">
               
               <div class="span6"><hr /><!--*/$load('inline/course.isp.js')({course:c});/*--><hr /></div>
               </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="course-color">
                Color:</label>
            <div class="controls">
                <input type="text" value="{$c.color$}" name="color" class="input-xlarge" id="course-color" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="course-title">
                Title:</label>
            <div class="controls">
                <input type="text" value="{$c.title$}" name="title" class="input-xlarge" id="course-title" />
            </div>
        </div>
        
        <div class="control-group">
            <label class="control-label" for="course-d">
                Description:</label>
            <div class="controls">
                <textarea maxlength="10000" class="input-xlarge" id="course-d" rows="5" name="desc">{$c.desc$}</textarea>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="course-tags">
                Tags:</label>
            <div class="controls">
                <input type="text" value="" name="tags" class="input-xlarge" id="course-tags" />
            </div>
        </div>
        
                <script type="text/javascript">
                    $('#course-tags').textext({
                        plugins: 'tags  autocomplete ajax arrow prompt',
                        prompt: 'Add one...',
                        tagsItems :{$c.tagString$},
                        ajax: {
                            url: 'Course/GetAllTags',
                            dataType: 'json',
                            cacheResults: true
                        }, autocomplete: {
                            position: 'above'
                        }
                    });

                   $('#course-color').ColorPicker({
	onSubmit: function(hsb, hex, rgb, el) {
		$(el).val(hex);
        $(el).css('backgroundColor','#'+hex);
		$(el).ColorPickerHide();
	},
	onBeforeShow: function () {
		$(this).ColorPickerSetColor(this.value);
	}
}).css('backgroundColor','#'+$('#course-color').val());
                    $('.text-position-below').css('top',24);
                    </script>
        <div class="control-group">
            <label class="control-label" for="course-icon">
                Icon:</label>
            <div class="controls">
                <input class="input-file" name="icon" id="course-icon" type="file" />
                <p class="help-block">
                    Image size: 70*70</p>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="course-bg">
                Background:</label>
            <div class="controls">
                <input class="input-file" name="bg" id="course-bg" type="file" />
                <p class="help-block">
                    Image size(recommended): 600*???-1440*???</p>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="textarea">
                Home page content:</label>
            <div class="controls">
                <p class="help-block">
                    Support tags(<a href="/101.file" target="_blank">view all...</a>):</p>
                <pre>
<!--*/$load('inline/contentHelper.isp.js')();/*--></pre>
                <textarea wrap="soft" name="home" class="" maxlength="10000" style="width:95%;" id="textarea" rows="20">{$c.home$}</textarea>
            </div>
        </div>
        <div class="form-actions">
            <button type="submit" class="btn btn-primary">
                Save (Ctrl + Enter)</button>
            <button type="button" onclick="$('.btn-info:visible').trigger('click');" class="btn">
                Cancel</button>
            <a href="/Course/Delete?id={$c.id$}&amp;redirect={$urlEncode('/')$}&amp;ANTICSRF=" class="btn btn-danger" data-confirm="All topics, students in this course will also be DELETED, are you sure?" data-anticsrf="href" >
                Delete</a>
        </div>
    </fieldset>
    </form></section>
</div>
<!--*/
    }
});
//-->
