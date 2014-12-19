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

function loadInOverlayIframe(url)
{
	url = '/' + lng + '/Popup/' + url;
	$('#' + overlayPrefix + '-overlay-frame-container').fadeOut('fast', function ()
	{
		$('#' + overlayPrefix + '-overlay-content').fadeOut('fast', function ()
		{
			debug('loading ' + url + ' in iframe ...');
			showLoadingMsg();
			$('#' + overlayPrefix + '-overlay-frame').attr('src', url);
			resizeMe();
			$('#' + overlayPrefix + '-overlay-frame-container').fadeIn('medium');
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

function loadIntroOnMap()
{
	$('#home-btn').hide();
	showLoadingMsg();
	$('#start-overlay-container').show();
	$('#start-overlay-frame-container').hide();
	if ($('#start-overlay-content').html().length == 0)
	{
		$('#start-overlay-content').load('/' + lng + '/Popup/Intro?isOnMap=true', function()
		{
			hideLoadingMsg();
			$('#start-overlay-content').fadeIn('fast');
		});
	}
	else
	{
		hideLoadingMsg();
	}
}

function hideIntroOnMap()
{
	$('#start-overlay-container').fadeOut('fast', function()
	{
		$('#home-btn').show();
	});
}

function goParty()
{
	// go party!
	$('#start-overlay-content').html('');
	initMap();
	initTooltipps();
}

function initTooltipps()
{
	$('#popover-points').load('/' + lng + '/Game/GetPoints', function ()
	{
		$('#sidebar').show();
		$('#sidebar').find('span').each(function ()
		{
			if (!isIpad)
				$(this).popover({ html: true, animation: false, trigger: 'hover', placement: 'left', container: "body" });

			$(this).click(function ()
			{
				if (!isIpad)
					$(this).popover('hide');

				if ($(this).attr("data-url"))
				{
					showSidebarPopup($(this).attr("data-url"));
				}
			});
		});
	});
}

// sidebar popups
function showSidebarPopup(url)
{
	showLoadingMsg();
	var modalWindow = $('#modal-sidebar');
	$(modalWindow).load('/' + lng + '/Popup/' + url, function ()
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
					} else
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
						$('#hashtags-nexturl').val(data.NextUrl);
						$('#hashtags-more-btn').fadeIn('fast');
					});
				} else
				{
					$('#hashtags-loading').fadeOut('fast');
				}
			}
			else
			{
				$('#hashtags-loading').fadeOut('fast');
				$('#fanwall-bottom').hide();
			}
		},
		error: function (request, status, error)
		{
			$('#hashtags-loading').fadeOut('fast');
		}
	});
}