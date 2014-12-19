var showDebugMessages = true;
var bbqUrl;
var bbqTarget;
var overlayPrefix = 'start';
var isQuestionVisible = true;
var isIpad = false;
var moreInfoSem = false;

function replaceAll(find, replace, str)
{
	return str.replace(new RegExp(find, 'g'), replace);
}

function getMaskSize()
{
	var size = new Object();
	size.width = $(window).innerWidth();
	size.height = $(window).innerHeight();
	return size;
}

function scrollToDiv(divId)
{
	$('html, body').animate({ scrollTop: $("#" + divId).offset().top - 100 }, 1000);
}

function scrollToDivMobile(divId)
{
	$('html, body').animate({ scrollTop: $("#" + divId).offset().top - 10 }, 1000);
}

function scrollToTop()
{
	$('html, body').animate({ scrollTop: 0 }, 1000);
}

function debug(message)
{
	if (showDebugMessages)
	{
		console.log(message);
	}
}

function closeOverlay(overlayId)
{
	$('#' + overlayId).slideUp(800, 'easeInOutQuart');
}

function showLoadingMsg()
{
	$('#loading-message').show();
}

function hideLoadingMsg()
{
	$('#loading-message').hide();
}

function showPopupBonus(points)
{
	$('#reminder').hide();
	$('#question').hide();
	$('#map-feedback-container').hide();
	$('#popup-bonus').show();
	showBonus('popup-points', points, false);
	window.setTimeout(function () { hideBonus('popup-points', true); }, 1300);
}

function showBonus(containerId, points, addToSidebar)
{
	debug("showBonus (" + containerId + ", " + points + ", " + addToSidebar + ")");
	var container = $('#' + containerId);
	var h1 = $('#' + containerId + " > h1");
	$(container).css('display', 'inline-block');
	$(h1).html('+' + points);
	var maskSize = getMaskSize();
	var fontSize = parseInt(maskSize.width / 4.0);
	$(h1).css('line-height', '270px');

	$(h1).show(0, function ()
	{
		$(h1).animate({
			fontSize: fontSize
		}, 300, "linear", function ()
		{
			$(h1).animate({
				fontSize: '270px'
			}, 180, "linear");
		});
	});
	if (addToSidebar)
	{
		addSidebarBonus(points);
	}
}

function addSidebarBonus(points)
{
	debug("addSidebarBonus(" + points + ")");
	if ($('#popover-points').length)
	{
		$('#popover-points').html(parseInt($('#popover-points').html()) + points);
		$('#popover-points').addClass('new');
		window.setTimeout(function()
		{
			$('#popover-points').removeClass('new');
		}, 2500);
	}
}

function hideBonus(containerId, doshowReminder)
{
	debug("hideBonus (" + containerId + ", " + doshowReminder + ")");
	var container = $('#' + containerId);
	var h1 = $('#' + containerId + " > h1");
	var maskSize = getMaskSize();
	var fontSize = parseInt(maskSize.width / 4.0);

	$(h1).animate({
		fontSize: fontSize
	}, 300, "linear", function ()
	{
		$(h1).animate({
			fontSize: '1px'
		}, 180, "linear", function ()
		{
			$(container).hide();
			$('#popup-bonus').fadeOut();
			if (doshowReminder)
			{
				$('#question').fadeOut();
				showReminder();
			} else
			{
				window.setTimeout("$('#question').fadeOut();", 600);
			}
		});
	});
}

function initBBQ()
{
	$(window).bind('hashchange', function (e)
	{
		var currentHashUrl = $.bbq.getState('u');
		var currentHashTarget = $.bbq.getState('t');

		debug("currentHashUrl = " + currentHashUrl);
		debug("currentHashTarget = " + currentHashTarget);

		if (currentHashUrl)
		{
			if (bbqUrl == 'undefined' || currentHashUrl != bbqUrl)
			{
				// handle special cases
				// register
				if (currentHashUrl == "Register" && hasEmail)
				{
					$.bbq.removeState();
					return;
				}

				bbqUrl = currentHashUrl;
				if (currentHashTarget && currentHashTarget == 'f')
				{
					loadInOverlayIframe(currentHashUrl);
				}
				else
				{
					loadInOverlay(currentHashUrl);
				}
			}
		}
	});

	$(window).trigger('hashchange');
}

function shareFacebook(url)
{
	window.open('https://www.facebook.com/sharer/sharer.php?u=' + encodeURIComponent(url), 'facebook-share-dialog', 'width=626,height=436');
	return false;
}

function shareGoogle(url)
{
	window.open('https://plus.google.com/share?url=' + encodeURIComponent(url), '', 'menubar=no,toolbar=no,resizable=yes,scrollbars=yes,height=600,width=600');
	return false;
}

function shareTwitter(url)
{
	window.open('https://twitter.com/share?url=' + encodeURIComponent(url) + '&text=%23XmasinVienna', '', 'menubar=no,toolbar=no,resizable=yes,scrollbars=yes,height=600,width=600');
	return false;
}

function resizeIframe(height, frameId)
{
	$('#' + frameId).css('height', height);
}

function showModal(modalId, action)
{
	showLoadingMsg();
	var modalWindow = $('#modal-' + modalId);
	$(modalWindow).load('/de/Popup/' + action, function ()
	{
		//if (isQuestionVisible)
		//{
		//	$('#question').fadeOut('fast');
		//}
		$('.modal').modal('hide');
		hideLoadingMsg();
		$(modalWindow).modal('show');
	});
}

function showMoreInfo(day, points, doShowAnsweredMarkers)
{
	if (!moreInfoSem)
	{
		moreInfoSem = true;
		// fix 3 points if points at all
		if (points > 0)
			points = 3;

		showLoadingMsg();
		var modalWindow = $('#modal-more-info');
		$(modalWindow).load('/' + lng + '/Advent/MoreInfo?day=' + day + '&points=' + points, function()
		{
			$('.modal').modal('hide');

			window.setTimeout(function()
			{
				hideLoadingMsg();
				$(modalWindow).modal('show');
			}, 300);

			window.setTimeout(function() { moreInfoSem = false; }, 500);

			if (points > 0)
			{
				addSidebarBonus(points);
			}

			if (doShowAnsweredMarkers)
			{
				window.setTimeout("showAnsweredMarkers();", 500);
			}
		});
	}
}

function showMoreInfoMobile(day, points, doShowAnsweredMarkers)
{
	// fix 3 points if points at all
	if (points > 0)
		points = 3;

	showLoadingMsg();
	var modalWindow = $('#modal-more-info');
	$(modalWindow).load('/' + lng + '/Mobile/MoreInfo?day=' + day + '&points=' + points, function ()
	{
		$('.modal').modal('hide');

		window.setTimeout(function () { hideLoadingMsg(); $(modalWindow).modal('show'); }, 300);

		if (points > 0)
		{
			addSidebarBonus(points);
		}

		if (doShowAnsweredMarkers)
		{
			window.setTimeout("showAnsweredMarkers();", 500);
		}
	});
}

function hideModal(modalId, bonus)
{
	$('#modal-' + modalId).modal('hide');

	if (bonus > 0)
	{
		showPopupBonus(bonus);
	}
	else if (modalId == "more-info")
	{
		$('#more-info-email-share').popover('hide');
		$('#my-points-email-share').popover('hide');
		$('#question').fadeOut();
		$('#map-feedback-container').fadeOut();
		showReminder();
	}
}

function fadeTransition(hideId, showId)
{
	$('#' + hideId).fadeOut('fast', function()
	{
		$('#' + showId).fadeIn('fast');
	});
}

function showLogin()
{
	fadeTransition('register-container', 'login-container');
	parent.window.scrollToTop();
}

function hideLogin()
{
	fadeTransition('login-container', 'register-container');
	parent.window.scrollToDiv('start-overlay-frame');
}

function hideTooSmallElements()
{
	debug("hideTooSmallElements()");
	$('#sidebar').addClass("too-small");
}

function showTooSmallElements()
{
	debug("showTooSmallElements()");
	$('#sidebar').removeClass("too-small");
}

function showReminder()
{
	$('#reminder-open-questions').html('');
	$.ajax({
		url: '/' + lng + '/Game/GetOpenQuestions',
		type: 'get',
		async: true,
		success: function (data)
		{
			if (data)
			{
				$.each(data, function(k, v)
				{
					$('#reminder-open-questions').html($('#reminder-open-questions').html() + '<a href="javascript:void(0);" onclick="loadSpecificQuestion(' + v["Day"] + ');" class="open-question" title="' + v["Day"] + '"><img src="/Images/eumel/' + v["Day"] + '.png" /></a>');
				});

				if ($('#reminder-open-questions').html().length > 0)
				{
					$('#reminder-open-question-container').show();
				}
				else
				{
					$('#reminder-open-question-container').hide();
				}

				$('#reminder').fadeIn('slow');
			}
			else
			{
				$('#reminder').fadeIn('slow');
			}
		},
		error: function (request, status, error)
		{
			$('#reminder').fadeIn('slow');
		}
	});
}

function initEmailShare(iconId, direction)
{
	$('#' + iconId)
	.popover({
		html: true,
		animation: false,
		placement: direction,
		//container: "#email-share-popover",
		title: function () { return $('#email-share-headline').html(); },
		content: function ()
		{
			var c = replaceAll('-template', '', $('#email-share-form-container').html());
			return replaceAll('{icon-id}', iconId, c);
		}
	});
}

function shareMail(iconId)
{
	var isValid = true;

	// validate
	if ($('#email-share-name').val() == '')
	{
		$('#email-share-name').addClass('error');
		isValid = false;
	}
	else
	{
		$('#email-share-name').removeClass('error');
	}

	if ($('#email-share-from').val() == '' || !validateEmail($('#email-share-from').val()))
	{
		$('#email-share-from').addClass('error');
		isValid = false;
	}
	else
	{
		$('#email-share-from').removeClass('error');
	}

	if ($('#email-share-to').val() == '' || !validateEmail($('#email-share-to').val()))
	{
		$('#email-share-to').addClass('error');
		isValid = false;
	}
	else
	{
		$('#email-share-to').removeClass('error');
	}

	if (isValid)
	{
		$.ajax({
			url: '/' + lng + '/Popup/SendRecommendation',
			type: 'get',
			data: {mailto: $('#email-share-to').val(), mailFrom: $('#email-share-from').val(), nameFrom: $('#email-share-name').val() },
			success: function (data)
			{
				$('.email-share-form').hide();
				$('.email-share-success').show();
				
				$('#' + iconId).find('input[type="text"]').val("");
				window.setTimeout("$('.popover').fadeOut('slow');", 1500);
				window.setTimeout("$('#' + iconId).popover('hide');", 2000);
				window.setTimeout("$('.email-share-form').show();$('.email-share-success').hide()", 2200);
			},
			error: function (err, req)
			{
			}
		});
	}
}

function setMediaPixel(id)
{
	var newSrc = "HTTP://bs.serving-sys.com/Serving/ActivityServer.bs?cn=as&ActivityID=" + id + "&Session=[SessionID]&ns=1";
	$('#media-px').attr("src", newSrc);
	debug('setMediapixel - ' + id);
}

function msieversion()
{
	var ua = window.navigator.userAgent;
	var msie = ua.indexOf("MSIE ");

	if (msie > 0)      // If Internet Explorer, return version number
		alert(parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))));
	else                 // If another browser, return 0
		alert('otherbrowser');

	return false;
}