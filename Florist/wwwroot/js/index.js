$(function () {

	$(window).scroll(function (event) {
		if ($(this).scrollTop() > 150) {
			$("#menu").fadeIn();
			$("#menu").addClass('fixed', 1000, 'linear');
		}
		else {
			$("#menu").removeClass('fixed')
		}
	});

});