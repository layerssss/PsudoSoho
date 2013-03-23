/*<!--*/
var c = arguments[0].course;
/*-->
<div class="row-fluid">
                        <div class="span2">
                            <a class="thumbnail" style="background-color:#{$c.color$};" href="{$c.id$}.course" title="{$c.title$}">
                                <img src="{$c.img$}" alt="{$c.title$}" style="width: 100%; max-width: 70px;" />
                            </a>
                        </div>
                        <div class="span6" style="overflow:hidden;height:75px;">
                            <h5>
                                <a href="{$c.id$}.course">{$c.title$}</a></h5>
                            
                    {$htmlEncode(c.desc)$}
                    <div class="tags">
                        <span class="smaller">Tags:</span>
                        <!--*/for(var j=0;j<c.tag.length;j++){/*-->
                        <a href="{$c.tag[j]$}.tag"><i class="icon-tag"></i><em>{$c.tag[j]$}</em></a>
                        <!--*/}/*-->
                    </div>
                        </div>
                        <div class="span4">
                            <a href="{$c.id$}.course" class="btn btn-info"><i class="icon-search icon-white">
                            </i>Visit</a>
                            <!--*/if(!c.pub){/*-->
                            <a href="/Course/Join?id={$c.id$}&amp;ANTICSRF=&amp;redirect=%20" data-anticsrf="href" class="btn btn-success"><i class="icon-plus icon-white">
                            </i>Join</a> 
                            <!--*/}else{/*-->
                            <a class="btn btn-success disabled" href="#" onclick="return false;"
                            title="This course is open for public."><i class="icon-info-sign icon-white"></i>
                            Public</a>
                            <!--*/}/*-->
                        </div>
                    </div><!--*///-->