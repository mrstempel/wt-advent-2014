using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.Frontend.Models;

namespace WTAdvent.Core.Mvc
{
	public class BaseView<T> : WebViewPage<T>
	{
		#region User Properties

		public bool IsAuthenticated
		{
			get { return HttpContext.Current.User.Identity.IsAuthenticated; }
		}

		public User LoggedUser
		{
			get { return BaseController.LoggedUser; }
		}

		public bool IsFirstVisit
		{
			get
			{
				if (Request.Cookies["IsFirstVisit"] != null &&
					Request.Cookies["IsFirstVisit"].Value.Length > 0 &&
					!Convert.ToBoolean(Request.Cookies["IsFirstVisit"].Value))
				{
					return false;
				}

				var cookie = new HttpCookie("IsFirstVisit", "false") { Expires = DateTime.Now.AddYears(5) };
				Response.Cookies.Add(cookie);
				return true;
			}
		}

		public bool HasEmail
		{
			get { return !string.IsNullOrEmpty(LoggedUser.Email); }
		}

		public long Bonuspoints
		{
			get { return BaseController.Bonuspoints; }
		}

		#endregion

		#region Translation

		/// <summary>
		/// Translation
		/// </summary>
		public Translator Translator
		{
			get { return BaseController.Translator; }
		}

		public string Lng
		{
			get { return BaseController.Lng; }
		}

		#endregion

		#region Sharing

		public string OgTitle
		{
			get
			{
				if (!string.IsNullOrEmpty(Request.Params["sType"]))
				{
					// set share specific title
					if (Request.Params["sType"].StartsWith("day"))
					{
						return Translate("share-location-line1");
					}

					if (Request.Params["sType"] == "webcam")
					{
						return Translate("share-kugel-line1");
					}
				}

				return Translate("share-allgemein-line1");
			}
		}

		public string OgImage
		{
			get
			{
				if (!string.IsNullOrEmpty(Request.Params["sType"]))
				{
					// set share specific image
					if (Request.Params["sType"].StartsWith("day"))
					{
						var day = Request.Params["sType"].Substring(3);
						return BaseUrl + "/Images/share/share-" + day + ".png";
					}

					if (Request.Params["sType"] == "webcam")
					{
						return BaseUrl + "/Images/share/share-kugel.png";
					}
				}

				return BaseUrl + "/Images/share/share-allgemein1-" + Lng + ".png";
			}
		}

		public string OgDescription
		{
			get
			{
				if (!string.IsNullOrEmpty(Request.Params["sType"]))
				{
					// set share specific description
					if (Request.Params["sType"].StartsWith("day"))
					{
						return Translate("share-location-line2");
					}

					if (Request.Params["sType"] == "webcam")
					{
						return Translate("share-kugel-line2");
					}
				}

				return Translate("share-allgemein-line2");
			}
		}

		public string InviteToken
		{
			get { return BaseController.UnitOfWork.ActionTokenRepository.GetInviteToken(LoggedUser.Id).Token.ToString(); }
		}

		#endregion

		public DateTime LocalDateTime
		{
			get { return BaseController.LocalDateTime; }
		}

		public bool IsCompetitionExpired
		{
			get
			{
				var expiredDate = new DateTime(2014, 12, 24, 23, 59, 59).AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["DateTimeCorrection"]));
				return LocalDateTime > expiredDate;
			}
		}

		public string FacebookAppId
		{
			get { return Request.Url.Host.Contains("vienna.info") ? "408321912650804" : "366547376854630"; }
		}

		private BaseController _baseController;
		/// <summary>
		/// Reference for base controller
		/// </summary>
		protected virtual BaseController BaseController
		{
			get
			{
				if (_baseController == null)
				{
					_baseController = ((BaseController)this.ViewContext.Controller);
				}
				return _baseController;
			}
		}

		public string BaseUrl
		{
			get { return ConfigurationManager.AppSettings["BaseUrl"]; }
		}

		public FormFeedback FormError
		{
			get { return BaseController.FormError; }
		}

		public FormFeedback FormSuccess
		{
			get { return BaseController.FormSuccess; }
		}

		private int _userCount = -1;
		public int UserCount
		{
			get
			{
				if (_userCount == -1)
				{
					_userCount = BaseController.UnitOfWork.UserRepository.Get(u => u.Email != null).Count();
				}

				return _userCount;
			}
		}

		private int _countryCount = -1;
		public int CountryCount
		{
			get
			{
				if (_countryCount == -1)
				{
					_countryCount = 0;
				}

				return _countryCount;
			}
		}

		public override void Execute()
		{
		}

		public string Translate(string key)
		{
			return BaseController.Translate(key);
		}
	}
}
