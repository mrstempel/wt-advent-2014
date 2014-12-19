var isFirstVisit;
var hasEmail;
var maxTryCount;

function letItSnow()
{
	initBBQ();

	if ($.bbq.getState('u') || $.bbq.getState('t'))
	{
		return;
	}

	// has e-mail?
	if (!hasEmail)
	{
		debug("no e-mail - loading intro ...");
		loadIntro();
		return;
	}

	goParty();
}

function resizeMe()
{
	var maskSize = getMaskSize();
}

function bbqLink(url, target)
{
	$.bbq.pushState({ "u": url, "t": target });
}

function loadInOverlay(url)
{
	url = '/' + lng + '/Popup/' + url;
	$('#' + overlayPrefix + '-overlay-content').fadeOut('fast', function ()
	{
		$('#' + overlayPrefix + '-overlay-frame-container').fadeOut('fast', function ()
		{
			showLoadingMsg();
			debug('loading ' + url + ' in overlay ...');
			$('#' + overlayPrefix + '-overlay-content').load(url, function ()
			{
				hideLoadingMsg();
				$('#' + overlayPrefix + '-overlay-content').fadeIn('medium');
			});
		});
	});
}

function loadIntro()
{
	$('#home-btn').hide();
	showLoadingMsg();
	$('#start-overlay-container').show();
	$('#start-overlay-frame-container').hide();
	$('#start-overlay-content').load('/' + lng + '/Popup/Intro?isOnMap=false', function ()
	{
		hideLoadingMsg();
		$('#start-overlay-content').fadeIn('fast');
	});
}

function goParty()
{
	// go party!
	location.href = '/' + lng + '/Mobile/Advent';
}

function loadIntroHashtags()
{
	$('.intro-hashtag-loading').show();
	$.ajax({
		url: '/' + lng + '/Popup/HomeHashtag',
		type: 'get',
		async: true,
		success: function (data)
		{
			var fanwallContainer = $('.intro-hashtag-container');
			if (data)
			{
				$.each(data, function (k, v)
				{
					if (v.VideoLowUrl != null && v.VideoLowUrl != '')
					{
						$(fanwallContainer).html($(fanwallContainer).html() + '<a href="' + v.Link + '" target="_blank"><video class="video-thumb" src="' + v.VideoLowUrl + '" style="opacity: 1;" autoplay loop muted onmouseover="muted=false" onmouseout="muted=true"></video></a>');
					}
					else
					{
						if (v.PostingType == "Instagram" || v.PostingType == "Facebook")
						{
							var imgUrl = (v.PostingType == "Facebook") ? v.ImageUrl : v.ThumbnailUrl;
							$(fanwallContainer).html($(fanwallContainer).html() + '<a href="' + v.Link + '" target="_blank"><img src="' + imgUrl + '" class="fan-pic"/></a>');
						}
					}
				});
			}

			$('.intro-hashtag-loading').fadeOut('fast', function()
			{
				$(fanwallContainer).fadeIn();
			});
		},
		error: function (request, status, error)
		{
		}
	});
}

function loadMoreHashtags()
{
	$('#hashtags-more-btn').fadeOut('fast', function()
	{
		$('#hashtags-loading').fadeIn('fast');
	});

	$.ajax({
		url: '/' + lng + '/Popup/MoreHashtag',
		type: 'get',
		async: true,
		data: { nextUrl: $('#hashtags-nexturl').val() },
		success: function (data)
		{
			var fanwallContainer = $('#fanwall');
			if (data)
			{
				$('#fanwall-bottom').css('height', '150px');
				$.each(data.Items, function(k, v)
				{
					if (v.VideoLowUrl != null && v.VideoLowUrl != '')
					{
						//$(fanwallContainer).html($(fanwallContainer).html() + '<a href="' + v.Link + '" target="_blank"><video class="video-thumb" src="' + v.VideoLowUrl + '" style="opacity: 1;" autoplay loop muted onmouseover="muted=false" onmouseout="muted=true"></video></a>');
					}
					else
					{
						if (v.PostingType == "Instagram" || v.PostingType == "Facebook")
						{
							var imgUrl = (v.PostingType == "Facebook") ? v.ImageUrl : v.ThumbnailUrl;
							$(fanwallContainer).html($(fanwallContainer).html() + '<a href="' + v.Link + '" target="_blank"><img src="' + imgUrl + '" class="fan-pic"/></a>');
						}
					}
				});

				if (data.NextUrl != null && data.NextUrl.length > 0)
				{
					$('#hashtags-loading').fadeOut('fast', function()
					{
						$('#hashtags-nexturl').val(data.nextUrl);
						$('#hashtags-more-btn').fadeIn('fast');
					});
				}
				else
				{
					$('#hashtags-loading').fadeOut('fast');
					$('#fanwall-bottom').hide();
				}
			}
			else
			{
				$('#hashtags-loading').fadeOut('fast');
			}
		},
		error: function (request, status, error)
		{
			$('#hashtags-loading').fadeOut('fast');
		}
	});
}

// general popups
function showPopup(url)
{
	showLoadingMsg();
	var modalWindow = $('#modal-sidebar');
	$(modalWindow).load('/' + lng + '/Popup/' + url, function ()
	{
		$('.modal').modal('hide');
		hideLoadingMsg();
		$(modalWindow).modal('show');
	});
}

function mobileLogin()
{
	if ($('#login-email').val().length == 0)
	{
		$('#login-email').addClass('error');
		return;
	}
	else
	{
		$.ajax({
			url: '/' + lng + '/Auth/Login',
			type: 'get',
			async: true,
			data: { email: $('#login-email').val() },
			success: function (data)
			{
				if (data)
				{
					window.location.href = '/' + lng + '/Mobile/Advent';
				}
				else
				{
					$('#login-email').addClass('error');
				}
			},
			error: function (err, req)
			{
				$('#login-email').addClass('error');
			}
		});
	}
}

function toggleMenu()
{
	if ($('#mobile-menu').is(':visible'))
	{
		$('#mobile-menu').slideUp();
	}
	else
	{
		$('#mobile-menu').slideDown().css('display', 'inline-block');
	}
}

function goToMap(day)
{
	location.href = '/' + lng + '/Mobile/Map?day=' + day;
}