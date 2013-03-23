jQuery.fn.extend({
	bgSlider:function(opt){
		var src=this
		var block=false,keeper,sCSS={position:'fixed',left:0,top:0,bottom:0,right:0,zIndex:-1},iCSS={position:'absolute',left:0,top:0,opacity:0}
		dfl={
			interval:4000,
			speed:1500,
			pags:false,
			slideshow:false,
			preload:false,
			current:0,
			bgstretch:false
		}
		opt=$.extend(dfl,opt)
		if(opt.pags)opt.pags=$(opt.pags)
		if(opt.preload){
			var tmp=[]
			for(var i=0;i<this.length;i++){
				tmp[i]=new Image()
				tmp[i].src=this[i]
			}
		}
		
		var resize=function(){
			var im=$('#bgSlider img'),
				w=im.attr('offsetWidth'),
				h=im.attr('offsetHeight'),
				k=im.attr('k')||w/h
				
			if(document.body.offsetWidth/document.body.offsetHeight<k)
				im.attr('k',k).css({width:document.body.offsetHeight*k+'px',height:document.body.offsetHeight+'px'})
			else
				im.attr('k',k).css({width:document.body.offsetWidth+'px',height:document.body.offsetWidth/k+'px'})
		}
		
		$(window).bind('resize',resize)
		
		var loadSrc=function(src){
			if(!opt.bgstretch)return loadURL(src)
			if(opt.pags)
				opt.pags.parent().eq(opt.current).addClass('current').siblings().removeClass('current')
			if(opt.slideshow)
				clearInterval(opt.slideshow),
				opt.slideshow=setInterval(function(){keeper.trigger('bgSliderNext')},opt.interval)
			keeper.append(t=$('<img>').css(iCSS))
 			t.bind('load',function(){
				resize()
				$(this).animate({opacity:1},opt.speed,function(){
					$(this).siblings().remove()
					block=false
				})
			})
			t.attr('src',src)
		}
		var loadURL=function(src){
			if(opt.pags)
				opt.pags.parent().eq(opt.current).addClass('current').siblings().removeClass('current')
			if(opt.slideshow)
				clearInterval(opt.slideshow),
				opt.slideshow=setInterval(function(){keeper.trigger('bgSliderNext')},opt.interval)
			keeper.append(t=$('<div></div>').css(iCSS).css({width:'100%',height:'100%',zIndex:-1,'background-image':'url('+src+')'}))
			t.animate({opacity:1},opt.speed,function(){
				$(this).siblings().remove()
				block=false
			})
		}
		$('body').append(keeper=$('<div id="bgSlider"></div>').css(sCSS))
		
		keeper.bind('bgSliderNext',function(){
			if(!block){
				block=true
				opt.current++
				if(!(opt.current<src.length))opt.current=0
				loadSrc(src[opt.current])
			}
		})
		keeper.bind('bgSliderPrev',function(){
			if(!block){
				block=true
				if(opt.current==0)opt.current=src.length
				opt.current--
				loadSrc(src[opt.current])
			}
		})		
		if(opt.pags)$(opt.pags).live('click',function(){
			if(!block){
				block=true
				loadSrc(src[opt.current=this.rel-1])				
			}
			return false
		})
		loadSrc(src[opt.current])
	}
})

$(window).load(function () {
    var len = bg.length;
    var offset = Math.floor(Math.random() * len);
    var newBg = new Array();
    for (var i = 0; i < len; i++) {
        newBg.push(bg[(i + offset) % len]);
    }
    $(newBg).bgSlider({ bgstretch: true, slideshow: true, preload: true })
})