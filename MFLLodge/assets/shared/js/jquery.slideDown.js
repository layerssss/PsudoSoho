$(function(){
	$(".slideDown dt").click(function(){
		$(this).parent(".slideDown").find("dd").slideToggle(500);
	})
});