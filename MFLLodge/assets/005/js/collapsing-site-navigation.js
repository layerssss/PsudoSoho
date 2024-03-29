 $(function() {
				//all the menu items
				var $items 		= $('#cc_menu .cc_item');
				//number of menu items
				var cnt_items	= $items.length;
				//if menu is expanded then folded is true
				var folded		= false;
				//timeout to trigger the mouseenter event on the menu items
				var menu_time;
				/**
				bind the mouseenter, mouseleave to each item:
				- shows / hides image and submenu
				bind the click event to the list elements (submenu):
				- hides all items except the clicked one, 
				and shows the content for that item
				*/
				$items.unbind('mouseenter')
					  .bind('mouseenter',m_enter)
				      .unbind('mouseleave')
					  .bind('mouseleave',m_leave)
					  .find('article')
					  .bind('click',function(){
					var $li_e = $(this);
						  //if the menu is already folded,
						  //just replace the content
					if(folded){
						hideContent();
						showContent($li_e.attr('class'));
					}	
					      else //fold and show the content
						fold($li_e);
				});
				
				/**
				mouseenter function for the items
				the timeout is used to prevent this event 
				to trigger if the user moves the mouse with 
				a considerable speed through the menu items
				*/
				
				$('#logo').bind('click',unfold);
				$('.read_more').bind('click', function(){
						$('.cc_back').animate({'top':'230px'},800, function(){$('.cc_back_page').animate({'top':'180'},800);}); 
						$('.read_more_content.'+$(this).attr('href').substring(1)).animate({'right':'0px'},800)
				});
				$('.cc_back_page').bind('click', function(){
						$('.read_more_content').animate({'right':'-980px'},800)
						$('.cc_back_page').animate({'top':'230'},800, function(){$('.cc_back').animate({'top':'180'},800);});
				});
				function m_enter(){
					var $this 	= $(this);
					clearTimeout(menu_time);
					menu_time 	= setTimeout(function(){
					//img
					$this.find('img').stop().animate({'top':'0px'},800);
					//cc_submenu ul
					$this.find('.cc_submenu').stop().animate({'height':'156px'},800);
					},0);
				}
				
				//mouseleave function for the items
				function m_leave(){
					var $this = $(this);
					clearTimeout(menu_time);
					//img
					$this.find('img').stop().animate({'top':'-600px'},800);
					//cc_submenu ul
					$this.find('.cc_submenu').stop().animate({'height':'0px'},800);
					$this.find('.cc_title').fadeIn();
				}
				
				//back to menu button - unfolds the menu
				$('#cc_back').bind('click',unfold);
				
				/**
				hides all the menu items except the clicked one
				after that, the content is shown
				*/
				function fold($li_e){
					
					var $item		= $li_e.closest('.cc_item');
					
					var d = 100;
					var step = 0;
					$items.unbind('mouseenter mouseleave');
					$items.not($item).each(function(){
						var $item = $(this);
						$item.stop().animate({
							'marginLeft':'-150px'
						},d += 200,function(){
							++step;
							if(step == cnt_items-1){
								folded = true;
								showContent($li_e.attr('class'));
							}	
						});
					});
				}
				
				/**
				shows all the menu items 
				also hides any item's image / submenu 
				that might be displayed
				*/
				function unfold(){
					$('#cont_privacy').animate({'right':'-980px'},800)
					$('.read_more_content').animate({'right':'-980px'},800)
					$('.cc_back').animate({'top':'230px'},800)
					$('#cc_content').stop().animate({'left':'-517px'},800,function(){
						var d = 100;
						var step = 0;
					$items.each(function(){
							var $item = $(this);
							$('.cc_title').fadeIn();
							$item.find('img')
								 .stop()
								 .animate({'top':'-600px'},800)
								 .andSelf()
								 .find('.cc_submenu')
								 .stop()
								 .animate({'height':'0px'},800);
								 
							$item.stop().animate({
							'marginLeft':'0px'
							},d += 200,function(){
								++step;
								
								if(step == cnt_items-1){
									folded = false;
									$items.unbind('mouseenter')
										  .bind('mouseenter',m_enter)
										  .unbind('mouseleave')
										  .bind('mouseleave',m_leave);
									
									hideContent();
									
								} 
							});
							
						});
					});
				}				
				//function to show the content
				function showContent(idx){
					$('#cc_content').stop().animate({'left':'180px'},800,function(){
						$('#cc_content').find('.'+idx).fadeIn(function(){
							$('div.gallery img').each(function(){
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
						});
					});
					
					$('.cc_back').animate({'top':'170px'},800)
					$('.cc_back').animate({'top':'180px'},800)
				}
				
				//function to hide the content
				function hideContent(){
					 $('#cc_content').find('section').hide();
					 				
				}
            });
				