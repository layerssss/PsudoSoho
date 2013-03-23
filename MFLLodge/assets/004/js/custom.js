$(document).ready(function(){
	$("a[rel^='prettyPhoto']").prettyPhoto({
		animation_speed: 'fast', /* fast/slow/normal */
		slideshow: 5000, /* false OR interval time in ms */
		autoplay_slideshow: false, /* true/false */
		opacity: 0.80, /* Value between 0 and 1 */
		show_title: true, /* true/false */
		counter_separator_label: '/', /* The separator for the gallery counter 1 "of" 2 */
		theme: 'light_square', /* light_rounded / dark_rounded / light_square / dark_square / facebook */
		autoplay: true, /* Automatically start videos: True/False */
		modal: false, /* If set to true, only the close button will close the window */
		deeplinking: true, /* Allow prettyPhoto to update the url to enable deeplinking. */
		overlay_gallery: true, /* If set to true, a gallery will overlay the fullscreen image on mouse over */
		keyboard_shortcuts: true, /* Set to false if you open forms inside prettyPhoto */
		ie6_fallback: true,
		social_tools: false
	});
	$(window).load(function(){
		$('.thumbs img').each(function(){
			var x = 168; //填入目标图片宽度
			var y = 168; //填入目标图片高度
			var w=$(this).width(), h=$(this).height();//获取图片宽度、高度
			if (w > x) { //图片宽度大于目标宽度时
				var w_original=w, h_original=h;
				h = h * (x / w); //根据目标宽度按比例算出高度
				w = x; //宽度等于预定宽度
				if (h < y) { //如果按比例缩小后的高度小于预定高度时
					w = w_original * (y / h_original); //按目标高度重新计算宽度
					h = y; //高度等于预定高度
				}
			}
			$(this).attr({width:w,height:h});
		});
	
		$('div.typePhotoList img').each(function(){
			var x = 100; 
			var y = 100; 
			var w=$(this).width(), h=$(this).height();
			if (w > x) { 
				var w_original=w, h_original=h;
				h = h * (x / w); 
				w = x; 
				if (h < y) { 
					w = w_original * (y / h_original);
					h = y;
				}
			}
			$(this).attr({width:w,height:h});
		});
	})
	$(".lightbox-image").append("<span></span>");		
	$(".lightbox-image").hover(function(){
		$(this).find("img").stop().animate({opacity:0.5}, "normal")
	}, function(){
		$(this).find("img").stop().animate({opacity:1}, "normal")
	});
});