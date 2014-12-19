auto_play = false;
var video_aspect = 1.7777777777777777777777777777778;

function init_yt_player(videoSize, videoId, containerId)
{
	jQuery("#" + containerId).tubeplayer({
		width: videoSize.width, // the width of the player
		height: videoSize.height, // the height of the player
		allowFullScreen: false, // true by default, allow user to go full screen
		initialVideo: videoId, // E2JT_UzJuDs
		showControls: 1,
		modestbranding: 1,
		autoPlay: auto_play,
		iframed: true,
		isHtml5: true,
		loadSWFObject: true, // if you include swfobject, set to false
		preferredQuality: "hd720", // preferred quality: default, small, medium, large, hd720
		onPlay: function (id) { },
		onPause: function () { },
		onStop: function () { },
		onSeek: function (time) { },
		onMute: function () { },
		onUnMute: function () { },
		onPlayerEnded: function () { },
		onPlayerPaused: function () { },
		onPlayerPlaying: function () { }
	});
}

function init_yt_playerAutoPlay(videoSize, videoId, containerId)
{
	jQuery("#" + containerId).tubeplayer({
		width: videoSize.width, // the width of the player
		height: videoSize.height, // the height of the player
		allowFullScreen: false, // true by default, allow user to go full screen
		initialVideo: videoId, // E2JT_UzJuDs
		showControls: 1,
		modestbranding: 1,
		autoPlay: true,
		iframed: true,
		isHtml5: true,
		loadSWFObject: true, // if you include swfobject, set to false
		preferredQuality: "hd720", // preferred quality: default, small, medium, large, hd720
		onPlay: function (id) { },
		onPause: function () { },
		onStop: function () { },
		onSeek: function (time) { },
		onMute: function () { },
		onUnMute: function () { },
		onPlayerEnded: function () { },
		onPlayerPaused: function () { },
		onPlayerPlaying: function () { }
	});
}

function resizePlayer(containerId)
{
	var videoSize = new Object();
	videoSize.width = $("#" + containerId).innerWidth();
	videoSize.height = Math.round(videoSize.width / video_aspect);
	jQuery('#' + containerId).tubeplayer("size", { 'width': videoSize.width, 'height': videoSize.height });
}