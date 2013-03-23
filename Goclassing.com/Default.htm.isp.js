/*<!--*/
$load('master/frame.master.js')({
    head: function () {
        /*-->
<script type="text/javascript">
    $(function () {
        $('.course').click(function () {
            $('.cur-course').html('').append(
        $(this).next('.des-course').clone().fadeIn());
            return false;
        });
        $('.create-course-btn').click(function () {
            $('.cur-course').html('').append(
        $('.create-course').clone().fadeIn());
            return false;
        });
    });
    var showNewFiles = function (files) {
        $('.newCourses').removeClass('span6').addClass('span4');
        var span8 = $('<div class="well"><h6>New topics:</h6></div>').appendTo($('.hero-unit').removeClass('hero-unit').addClass('well').parent()
        .removeClass('span6').addClass('span8'));
        for (var i = 0; i < files.length; i++) {
            $('<div class="row-fluid"><img src="img/loading.gif" /></div>').appendTo(span8)
            .load('/inline/' + files[i] + '.file');
        }
    };
</script>
<!--*/
    },
    body: function () {
        /*-->
<div class="row">
    <div class="span6">
        <div class="hero-unit">
            <h2>
                Welcome to Goclassing</h2>
            <p>
                Goclassing is a place where you can:</p>
            <div>
                <a href="#" class="btn btn-large btn-success create-course-btn">Create a course</a>
                <a href="/tags" class="btn btn-info btn-large">Join a course</a>
            </div>
        </div>
        <div class="cur-course">
        </div>
        <div class="alert alert-success create-course" style="display: none;">
            <a class="close" data-dismiss="alert">×</a>
            <form class="form" action="/Course/Create?ANTICSRF=&amp;redirect=/{cid}.course" data-anticsrf="action" method="post">
            <fieldset>
                <legend>Create a course</legend>
                <div class="control-group">
                    <label class="control-label" for="course-id">
                        Course ID:</label>
                    <div class="controls">
                        <input type="text" name="id" class="input-xlarge" id="course-id">
                        <p class="help-block">
                            6(+) chars, only contains lowercase a-z, _(underscore), .(dot)</p>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="course-title">
                        Course title:</label>
                    <div class="controls">
                        <input type="text" name="title" class="input-xlarge" id="course-title">
                    </div>
                </div>
                <div class="form-actions">
                    <button type="submit" class="btn btn-primary btn-create">
                        Create</button>
                    <button type="button" onclick="$(this).parents('.alert').children('.close').trigger('click');"
                        class="btn">
                        Cancel</button>
                </div>
            </fieldset>
            </form>
        </div>
    </div>
    <div class="span6 newCourses">
        <ul class="row thumbnails hidden-phone">
            <!--*/
        var i = 0;
        for (i = 0; i < courses.length; i++) {
            var c = courses[i];
            /*-->
            <li class="span1"><a style="background-color:#{$c.color$};" class="thumbnail course" href="{$c.id$}.course" title="{$c.title$}">
                <img src="{$c.img$}" alt="{$c.title$}" style="width: 100%; max-width: 100px;" />
            </a>
                <div class="des-course alert alert-info" style="display: none;">
                    <a class="close" data-dismiss="alert">×</a>
                    <!--*/
            $load('inline/course.isp.js')({ course: c }); /*-->
                </div>
            </li>
            <!--*/
        }
        for (; i < 30; i++) {
            /*-->
            <li class="span1"><a class="thumbnail" href="#" onclick="$('.create-course-btn').trigger('click');return false;"
                title="Take over here!">
                <img src="/img/blank.gif" alt="Take over here!" style="width: 100%; max-width: 100px;" />
            </a></li>
            <!--*/
        }
        /*-->
        </ul>
    </div>
</div>
<!--*/
    }
});
//-->
