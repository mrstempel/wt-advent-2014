function doFacebookLogin()
{
	FB.getLoginStatus(function (response)
	{
		if (response.status === 'connected')
		{
			snowieFacebookRegister();
		}
		else
		{
			FB.login(function () {  }, { scope: 'email' });
		}
	});
}

window.fbAsyncInit = function ()
{
	FB.init({
		appId: fbApp,
		cookie: true,  // enable cookies to allow the server to access 
		// the session
		xfbml: true,  // parse social plugins on this page
		version: 'v2.1' // use version 2.1
	});

	FB.Event.subscribe('auth.authResponseChange', function (response)
	{
		if (response.status === 'connected') { snowieFacebookRegister(); }
		else if (response.status === 'not_authorized') { FB.login(function () {  }, { scope: 'email' }); }
		else { FB.login(function () {  }, { scope: 'email' }); }
	});
};

// Load the SDK asynchronously
(function (d, s, id)
{
	var js, fjs = d.getElementsByTagName(s)[0];
	if (d.getElementById(id)) return;
	js = d.createElement(s); js.id = id;
	js.src = "//connect.facebook.net/en_US/sdk.js";
	fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function snowieFacebookRegister()
{
	FB.api('/me', function (response)
	{
		console.log(response);
		$('#fb-firstname').val(response.first_name);
		$('#fb-surname').val(response.last_name);
		$('#fb-email').val(response.email);
		$('#fb-id').val(response.id);
		if (response.gender && response.gender != 'male')
		{
			$('#fb-sex').val('F');
		}
		else
		{
			$('#fb-sex').val('M');
		}
		$('#fb-login-form').submit();
	});
}