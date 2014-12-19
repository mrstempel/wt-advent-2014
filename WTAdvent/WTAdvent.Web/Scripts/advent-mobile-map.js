var map;
var geocoder;
var currentZoom = 14;
var targetPosition = null;
var targetMarker = null;
var questionMarker = null;
var questionFeedbackBubble = new InfoBubble({ shadowStyle: 0, backgroundColor: 'transparent', padding: 0, borderRadius: 0, borderWidth: 0, arrowStyle: 3, hideCloseButton: true, disableAutoPan: true });
var answerFeedbackBubble = new InfoBubble({ shadowStyle: 0, backgroundColor: 'transparent', padding: 0, borderRadius: 0, borderWidth: 0, arrowStyle: 3, hideCloseButton: true, disableAutoPan: true });
//var questionFeedbackBubble = new google.maps.InfoWindow();
var currentFlightPath = null;
var currentFlightPathShadow = null;
//var questions = null;
var currentQuestion = null;
var questionNewLat = null;
var answerNewLat = null;
var answerMarkers = new Array();

var markerShadow = new google.maps.MarkerImage(
        'https://www.google.com/intl/en_ALL/mapfiles/shadow50.png',
        new google.maps.Size(37, 34),  // size   - for sprite clipping
        new google.maps.Point(0, 0),   // origin - ditto
        new google.maps.Point(10, 34)  // anchor - where to meet map location
      );

var lineSymbol = { path: 'M 0,-1 0,1', strokeOpacity: 1.0, strokeWeight: 1.5, scale: 4 };

var mapStylesArray = [
	{
		featureType: 'all',
		elementType: 'geometry',
		stylers:
		[
			{ visibility: "simplified" },
			{ saturation: -70 },
			{ gamma: 4.90 }
		]
	},
	{
		featureType: 'road',
		elementType: 'geometry',
		stylers:
		[
			{ visibility: "simplified" },
			{ lightness: 100 }
		]
	},
	{
		featureType: 'road',
		elementType: 'labels.icon',
		stylers:
		[
			{ visibility: "off" }
		]
	},
	{
		featureType: 'geometry',
		elementType: 'labels.text',
		stylers:
		[
			{ color: '#adadad' },
			{ weight: 0.1 }
		]
	},
	{
		featureType: "administrative",
		elementType: "geometry.fill",
		stylers: [
		   { visibility: "off" }
		]
	},
	{
		featureType: "poi",
		stylers: [
		   { visibility: "on" }
		]
	},
	{
		featureType: "transit",
		stylers: [
		   { visibility: "off" }
		]
	}
];

google.maps.LatLng.prototype.kmTo = function (a)
{
	var e = Math, ra = e.PI / 180;
	var b = this.lat() * ra, c = a.lat() * ra, d = b - c;
	var g = this.lng() * ra - a.lng() * ra;
	var f = 2 * e.asin(e.sqrt(e.pow(e.sin(d / 2), 2) + e.cos(b) * e.cos(c) * e.pow(e.sin(g / 2), 2)));
	return f * 6378.137;
}

google.maps.Polyline.prototype.inKm = function (n)
{
	var a = this.getPath(n), len = a.getLength(), dist = 0;
	for (var i = 0; i < len - 1; i++)
	{
		dist += a.getAt(i).kmTo(a.getAt(i + 1));
	}
	return parseFloat(dist.toFixed(3));
}

function map_recenter(latlng, offsetx, offsety)
{
	var point1 = map.getProjection().fromLatLngToPoint(
        (latlng instanceof google.maps.LatLng) ? latlng : map.getCenter()
    );
	var point2 = new google.maps.Point(
        ((typeof (offsetx) == 'number' ? offsetx : 0) / Math.pow(2, map.getZoom())) || 0,
        ((typeof (offsety) == 'number' ? offsety : 0) / Math.pow(2, map.getZoom())) || 0
    );
	map.setCenter(map.getProjection().fromPointToLatLng(new google.maps.Point(
        point1.x - point2.x,
        point1.y + point2.y
    )));
	console.log("map recentered");
}

function setZoom(zoomLevel)
{
	debug("setZoom: " + zoomLevel);
	map.setZoom(zoomLevel);
	currentZoom = zoomLevel;
}

function zoomIn()
{
	setZoom(currentZoom + 1);
}

function zoomOut()
{
	setZoom(currentZoom - 1);
}

function initMap()
{
	overlayPrefix = 'map';
	if (map == null)
	{
		showLoadingMsg();
		geocoder = new google.maps.Geocoder();

		var centerLatlng = new google.maps.LatLng(48.175348, 16.347250);
		var styledMap = new google.maps.StyledMapType(mapStylesArray, { name: "Snowie Map" });
		var mapOptions =
		{
			backgroundColor: 'transparent',
			minZoom: 3,
			zoom: currentZoom,
			center: centerLatlng,
			mapTypeId: google.maps.MapTypeId.ROADMAP,
			mapTypeControl: false,
			overviewMapControl: false,
			streetViewControl: false,
			scaleControl: false,
			panControl: false,
			rotateControl: false,
			zoomControl: false,
			mapTypeControlOptions: {
				mapTypeIds: [google.maps.MapTypeId.ROADMAP, 'map_style']
			}
		};

		map = new google.maps.Map(document.getElementById('map-container'), mapOptions);
		map.mapTypes.set('map_style', styledMap);
		map.setMapTypeId('map_style');

		google.maps.event.addListener(map, "zoom_changed", function ()
		{
			currentZoom = map.getZoom();
			if (questionMarker != null && questionMarker.getMap() != null)
			{
				questionNewLat = questionMarker.getPosition().lat() - (0.00003 * Math.pow(2, (21 - map.getZoom())));
				questionFeedbackBubble.setPosition(new google.maps.LatLng(questionNewLat, questionMarker.getPosition().lng()));
			}
		});

		google.maps.event.addListenerOnce(map, "projection_changed", function()
		{
			loadQuestion(day, true);
			$('#map-container').css('opacity', 1);
			$('#start-overlay-container').slideUp('fast', 'easeInOutQuart');
			debug("google-maps: projection_changed");
		});
	}
	else
	{
		$('#map-container').css('opacity', 1);
		$('#start-overlay-container').slideUp('fast', 'easeInOutQuart');
	}
}

function isQuestionAnswered(q)
{
	return ((q.Answers != null &&
			q.Answers.length > 0 &&
			q.Answers[q.Answers.length - 1].IsCorrect) ||
			(q.Answers.length > 0 && q.Answers[q.Answers.length - 1].TryCount >= maxTryCount));
}

function resetQuestionElements()
{
	if (questionMarker != null)
	{
		questionMarker.setMap(null);
		questionMarker = null;
	}

	if (currentFlightPath != null)
		currentFlightPath.setMap(null);

	if (currentFlightPathShadow != null)
		currentFlightPathShadow.setMap(null);

	answerFeedbackBubble.setMap(null);

	$('.question-how-to').hide();
}

function loadSpecificQuestion(day)
{
	showLoadingMsg();
	loadQuestion(day, false);
}

function loadQuestion(day, loadPrev)
{
	debug("loadQuestion(" + day + ", " + loadPrev + ") called");

	$.ajax({
		url: '/' + lng + '/Game/GetQuestion',
		type: 'get',
		async: true,
		data: { day: day, loadPrev: loadPrev },
		success: function (data)
		{
			if (data)
			{
				debug(data);

				resetQuestionElements();

				// set current question
				if (loadPrev)
				{
					currentQuestion = data[data.length - 1];
					writeAnsweredMarkers(data);
				}
				else
				{
					//setCurrentQuestion(data);
					currentQuestion = data;
				}

				if (targetMarker != null)
					targetMarker.setMap(null);

				// set target position and marker
				targetPosition = new google.maps.LatLng(currentQuestion.Question.TargetLocation.Lat, currentQuestion.Question.TargetLocation.Lng);
				targetMarker = new google.maps.Marker({
					position: targetPosition,
					//map: map,
					title: currentQuestion.TranslatedTargetName,
					icon: new google.maps.MarkerImage("/images/eumel/" + currentQuestion.Question.Day + ".png", null, null, null, new google.maps.Size(55, 55)),
					shadow: markerShadow
				});

				// check if current question was already answered
				if (isQuestionAnswered(currentQuestion))
				{
					targetMarker.setMap(map);
					setZoom(currentQuestion.Question.ZoomLevel);
					map.setCenter(targetMarker.position);
					google.maps.event.addListener(targetMarker, 'click', function () { showMoreInfoMobile(currentQuestion.Question.Day, 0, false); });

					showAnsweredMarkers();
				}
				else
				{
					hideAnsweredMarkers();
					var questionPosition = new google.maps.LatLng(currentQuestion.Question.StartLocation.Lat, currentQuestion.Question.StartLocation.Lng);
					questionMarker = new google.maps.Marker({
						position: questionPosition,
						title: currentQuestion.TranslatedTitle,
						icon: new google.maps.MarkerImage("/images/eumel/dragger-rot-" + mySnowieVersion + ".png", null, null, null, new google.maps.Size(55, 55)),
						//icon: "/images/eumel/dragger-rot-" + mySnowieVersion + "-small.png",
						map: map,
						shadow: markerShadow,
						draggable: true
					});
					google.maps.event.addListener(questionMarker, 'dragend', function () { dropConfirm(); });

					setZoom(currentQuestion.Question.ZoomLevel);
					map_recenter(questionMarker.position, 0, 0);

					// set tipp links
					$('.btn-tipp').attr('href', currentQuestion.Link);
				}
			}
			else
			{
				alert("No Questions found");
			}

			hideLoadingMsg();
		},
		error: function (request, status, error)
		{
			debug("/Game/GetQuestion has thrown an exception!");
			hideLoadingMsg();
		}
	});
}

function writeAnsweredMarkers(questions)
{
	answerMarkers = new Array();
	$.each(questions, function(k, v)
	{
		// skip current question
		if (v.Question.Id != currentQuestion.Question.Id &&
			isQuestionAnswered(v))
		{
			addAnsweredMarker(v);
		}
	});
}

function addAnsweredMarker(q)
{
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(q.Question.TargetLocation.Lat, q.Question.TargetLocation.Lng),
		title: q.TranslatedTargetName,
		icon: new google.maps.MarkerImage("/images/eumel/" + q.Question.Day + ".png", null, null, null, new google.maps.Size(55, 55))
	});
	google.maps.event.addListener(marker, 'click', function ()
	{
		setMediaPixel('568030'); _gaq.push(['_trackEvent', 'mehr-lesen-tag-' + q.Question.Day, 'clicked']); showMoreInfoMobile(q.Question.Day, 0, false);
	});
	answerMarkers.push(marker);
}

function showAnsweredMarkers()
{
	debug("showAnsweredMarkers()");
	resetQuestionElements();
	showTooSmallElements();
	$.each(answerMarkers, function(k, v)
	{
		v.setMap(map);
	});
}

function hideAnsweredMarkers()
{
	debug("hideAnsweredMarkers()");
	hideTooSmallElements();
	$.each(answerMarkers, function (k, v)
	{
		v.setMap(null);
	});
}

function dropConfirm()
{
	var markerBounds = new google.maps.LatLngBounds();
	markerBounds.extend(questionMarker.position);
	map_recenter(questionMarker.position, 0, 0);
	questionFeedbackBubble.setContent($('#drop-confirm').html());
	questionNewLat = questionMarker.getPosition().lat() - (0.00003 * Math.pow(2, (21 - map.getZoom())));
	questionFeedbackBubble.open(map);
	questionFeedbackBubble.setPosition(new google.maps.LatLng(questionNewLat, questionMarker.getPosition().lng()));
}

function setAnswer()
{
	showLoadingMsg();
	debug("setAnswer() called");

	if (currentFlightPath != null)
		currentFlightPath.setMap(null);

	if (currentFlightPathShadow != null)
		currentFlightPathShadow.setMap(null);

	var flightPlanCoordinates = new Array();
	flightPlanCoordinates.push(questionMarker.position);
	flightPlanCoordinates.push(targetMarker.position);
	
	currentFlightPathShadow = new google.maps.Polyline({
		path: flightPlanCoordinates,
		strokeColor: '#EFEFEF',
		strokeOpacity: 1.9,
		strokeWeight: 8
	});

	currentFlightPath = new google.maps.Polyline({
		path: flightPlanCoordinates,
		geodesic: true,
		strokeColor: '#7d7d7d',
		strokeOpacity: 1.0,
		strokeWeight: 1
	});

	var distance = currentFlightPath.inKm();

	$.ajax({
		url: '/' + lng + '/Game/SetAnswer',
		type: 'get',
		async: true,
		data: { questionId: currentQuestion.Question.Id, locationLat: targetMarker.position.lat(), locationLng: targetMarker.position.lng(), distance: distance, day: currentQuestion.Question.Day },
		success: function(data)
		{
			debug("max: " + maxTryCount + ", trycount: " + data.Answer.TryCount);
			if (data && data.Answer.IsCorrect)
			{
				showCorrectAnswer(data.Answer.TryCount, data.BonusPoints);
			}
			else
			{
				if (data.Answer.TryCount >= maxTryCount)
				{
					showCorrectAnswerAfterMaxTrys(distance);
				}
				else
				{
					showWrongAnswer(distance, data.Answer.TryCount);
				}
			}

			hideLoadingMsg();
		},
		error: function (request, status, error)
		{
			debug("/Game/SetAnswer has thrown an exception!");
			hideLoadingMsg();
		}
	});
}

function showCorrectAnswer(tryCount, bonusPoints)
{
	debug("showCorrectAnswer called");
	questionFeedbackBubble.setMap(null);
	//showBonus('answer-points', bonusPoints, true);
	$('#answer-points').html('+' + bonusPoints);
	showQuestionFeedback(tryCount, 5);
	targetMarker.setMap(map);
	//var markerBounds = new google.maps.LatLngBounds();
	//markerBounds.extend(targetMarker.position);
	//markerBounds.extend(questionMarker.position);
	//map.fitBounds(markerBounds);
	map_recenter(targetMarker.position, 0, 0);
	currentFlightPath.setMap(map);
	showAnswerMoreInfo(3);
}

function showAnswerMoreInfo(points)
{
	debug("showAnswerMoreInfo called");
	var content = $('#answer-confirm').html().replace("{more-info-link}", "_gaq.push(['_trackEvent', 'mehr-lesen-tag-" + currentQuestion.Question.Day + "', 'clicked']);showMoreInfoMobile(" + currentQuestion.Question.Day + ", " + points + ", true);");
	content = content.replace("{more-info-bonus-message}", moreInfoBonusMsg);
	answerFeedbackBubble.setContent(content);
	answerNewLat = targetMarker.getPosition().lat() - (0.000035 * Math.pow(2, (21 - map.getZoom())));
	answerFeedbackBubble.open(map);
	answerFeedbackBubble.setPosition(new google.maps.LatLng(answerNewLat, targetMarker.getPosition().lng()));

	google.maps.event.addListener(map, "zoom_changed", function ()
	{
		answerNewLat = targetMarker.getPosition().lat() - (0.00003 * Math.pow(2, (21 - map.getZoom())));
		answerFeedbackBubble.setPosition(new google.maps.LatLng(answerNewLat, targetMarker.getPosition().lng()));
	});

	addAnsweredMarker(currentQuestion);
}

function showCorrectAnswerAfterMaxTrys(distance, points)
{
	debug("showCorrectAnswerAfterMaxTrys called");
	$('#question-how-to4 > span').html($('#question-how-to4 > span').html().replace("{km}", distance + " km"));
	questionFeedbackBubble.setMap(null);
	showQuestionFeedback(3, 4);
	questionMarker.setDraggable(false);
	targetMarker.setMap(map);
	//var markerBounds = new google.maps.LatLngBounds();
	//markerBounds.extend(targetMarker.position);
	//markerBounds.extend(questionMarker.position);
	////map_recenter(targetMarker.position, 0, 0);
	//map.fitBounds(markerBounds);
	map_recenter(targetMarker.position, 0, 0);
	//currentFlightPathShadow.setMap(map);
	currentFlightPath.setMap(map);
	showAnswerMoreInfo(3);
}

function showWrongAnswer(distance, tryCount)
{
	debug("showWrongAnswer called");
	questionFeedbackBubble.setMap(null);
	showQuestionFeedback(tryCount, ++tryCount);
}

function showQuestionFeedback(hideIndex, showIndex)
{
	debug("showQuestionFeedback: " + hideIndex + ", " + showIndex);
	var howToHide = $('#question-how-to' + hideIndex);
	var howToShow = $('#question-how-to' + showIndex);
	$(howToHide).hide();
	$(howToShow).show();
	$('#map-feedback-container').fadeIn();
}