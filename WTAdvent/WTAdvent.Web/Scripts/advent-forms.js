var showErrorMarkers = false;

function validateEmail(email)
{
	var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	return re.test(email);
}

function initMandatoryFields()
{
	$('.mandatory').keyup(function () { checkMandatoryField($(this)); });
	$('.mandatory').change(function () { checkMandatoryField($(this)); });
}

function checkMandatoryField(field)
{
	if ($(field).val().length > 0)
	{
		$(field).css('background-image', 'none');
		$(field).removeClass('error');
	}
	else
	{
		//if ($(field).nodeName && $(field).nodeName.toLowerCase() === 'input')
		//	$(field).css('background-image', 'url(/Images/mandatory.png)');

		if (showErrorMarkers)
			$(field).addClass('error');
	}
}

function initForms()
{
	debug("init form");
	$("textarea[maxlength]").bind('input propertychange', function ()
	{
		var maxLength = $(this).attr('maxlength');
		if ($(this).val().length > maxLength)
		{
			$(this).val($(this).val().substring(0, maxLength));
		}
	});

	initMandatoryFields();
}

function showErrorMsg(msgHeadline, msgText, autoclose)
{
	if (msgHeadline.length > 0)
		$('#msg-error h2').html(msgHeadline);

	if (msgText.length > 0)
		$('#msg-error p').html(msgText);

	$('#msg-error').show();
	$('#msg-container').fadeIn('medium');
	scrollToDiv('msg-container');

	if (autoclose)
	{
		window.setTimeout("$('#msg-container').fadeOut('slow');", 2000);
	}
}

function showSuccessMsg(msgHeadline, msgText, autoclose)
{
	if (msgHeadline.length > 0)
		$('#msg-success h2').html(msgHeadline);

	if (msgText.length > 0)
		$('#msg-success p').html(msgText);

	$('#msg-success').show();
	$('#msg-container').fadeIn('fast');
	scrollToDiv('msg-container');

	if (autoclose)
	{
		window.setTimeout("$('#msg-container').fadeOut('slow');", 2000);
	}
}

function validateMandatoryFields()
{
	showErrorMarkers = true;
	var isValid = true;
	$('.mandatory').each(function ()
	{
		if ($(this).val().length > 0)
		{
			$(this).removeClass('error');
		}
		else
		{
			$(this).addClass('error');
			isValid = false;
		}
	});
	return isValid;
}

function validateRegister()
{
	var isValid = true;
	var errorText = "";

	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	// check e-mail syntax
	if (!validateEmail($('#Email').val()))
	{
		$('#Email').addClass('error');
		errorText += "Bitte gib eine gültige E-Mail Adresse an.";
		isValid = false;
	}

	// check teilnahmebedingungen
	if (!$('#teilnahmebedingungen-check').prop('checked'))
	{
		$('#teilnahmebedingungen-container').addClass('error');
		isValid = false;
	}
	else
	{
		$('#teilnahmebedingungen-container').removeClass('error');
	}

	if (!isValid && errorText != "")
	{
		showErrorMsg('', errorText, false);
	}
	else
	{
		_gaq.push(['_trackEvent', 'anmelden', 'clicked']);
	}

	return isValid;
}

function login()
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
					parent.window.location.reload(false);
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

function logout()
{
	$.ajax({
		url: '/' + lng + '/Auth/Logout',
		type: 'get',
		async: true,
		success: function (data)
		{
			location.href = "/" + lng;
		},
		error: function (err, req)
		{
			// !?
		}
	});
}

function userUpdate()
{
	if (validateMandatoryFields())
	{
		var sex = $('#sex-m').is(':checked') ? "M" : "F";
		//var hasNewsletter = $('#HasNewsletter').prop('checked');
		//var hasDailyReminder = $('#HasDailyReminder').prop('checked');

		$.ajax({
			url: '/' + lng + '/Auth/Update',
			type: 'get',
			async: true,
			data: {
				snowieVersion: $('#SnowieVersion').val(), sex: sex, firstname: $('#Firstname').val(), surname: $('#Surname').val(),
				email: $('#Email').val()
			},
			success: function (data)
			{
				if (data == 1)
				{
					$('#msg-error').hide();
					$('#msg-success').show();
					$('#msg-container-success').fadeIn('medium');
					scrollToDivMobile('msg-container-success');
					$('#popover-my-snowie').removeClass('my-snowie1 my-snowie2 my-snowie3 my-snowie4');
					$('#popover-my-snowie').addClass('my-snowie' + $('#SnowieVersion').val());
					mySnowieVersion = $('#SnowieVersion').val();
				}
				else if (data == 0)
				{
					$('#msg-success').hide();
					showErrorMsg('', "E-mail schon vergeben", false);
				}
				else
				{
					$('#msg-success').hide();
					showErrorMsg('', '', false);
				}
			},
			error: function (err, req)
			{
				$('#login-email').addClass('error');
			}
		});
	}
}