using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using KwIt.Project.Pattern.Utils;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.UnitOfWork;
using WTAdvent.Core.Frontend.Models;

namespace WTAdvent.Core.Mvc
{
	//[Authorize]
	public class BaseController : Controller
	{
		public const String WurflManagerCacheKey = "__WurflManager";
		public string Lng { get; set; }

		public User LoggedUser
		{
			get { return WebContext.LoggedUser; }
			set { WebContext.LoggedUser = value; }
		}

		private WTAdventUnitOfWork<WTAdventContext> _unitOfWork;
		public WTAdventUnitOfWork<WTAdventContext> UnitOfWork
		{
			get
			{
				if (_unitOfWork == null)
					_unitOfWork = new WTAdventUnitOfWork<WTAdventContext>();

				return _unitOfWork;
			}
		}

		public WebContext WebContext { get; set; }

		public bool IsAuthenticated
		{
			get { return User.Identity.IsAuthenticated; }
		}

		private long _bonuspoints = -1;
		public long Bonuspoints
		{
			get
			{
				if (_bonuspoints == -1)
				{
					_bonuspoints = UnitOfWork.BonuspointRepository.GetUserBonus(LoggedUser.Id);
				}

				return _bonuspoints;
			}
		}

		private Translator _translator;
		/// <summary>
		/// Translation
		/// </summary>
		public Translator Translator
		{
			get
			{
				if (_translator == null)
					_translator = new Translator(Lng);
				return _translator;
			}
		}

		private List<KeyValuePair<string, string>> _availableLanguages;
		public IEnumerable<KeyValuePair<string, string>> AvailableLanguages
		{
			get
			{
				if (_availableLanguages == null)
				{
					_availableLanguages = new List<KeyValuePair<string, string>>();
					string[] lngs = ConfigurationManager.AppSettings["Languages"].Split(",".ToCharArray());
					foreach (var lng in lngs)
					{
						string[] lngParts = lng.Split("#".ToCharArray());
						_availableLanguages.Add(new KeyValuePair<string, string>(lngParts[0], lngParts[1]));
					}
				}

				return _availableLanguages;
			}
		}
		public string LanguageName
		{
			get
			{
				var lng = ConfigurationManager.AppSettings["Languages"].Split(",".ToCharArray()).Where(l => l.StartsWith(Lng, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
				return lng != null ? lng.Substring(3) : string.Empty;
			}
		}

		public FormFeedback FormError { get; set; }
		public FormFeedback FormSuccess { get; set; }

		public bool IsAdmin
		{
			get { return LoggedUser.Id == Convert.ToInt32(ConfigurationManager.AppSettings["Admin"]); }
		}

		public string InviteToken
		{
			get
			{
				return (this.Session["i_t"] != null && !string.IsNullOrEmpty(this.Session["i_t"].ToString()))
					? this.Session["i_t"].ToString()
					: null;
			}
			set { this.Session["i_t"] = value; }
		}

		public BaseController()
		{
			WebContext = new WebContext(this.UnitOfWork);
		}

		public DateTime LocalDateTime
		{
			get { return DateTimeCorrection.GetLocalDateTime(); }
		}

		public BaseController(string lng)
			: this()
		{
			this.Lng = lng;
		}

		public string Translate(string key)
		{
			return Translator.Translate(key);
		}

		protected override void Dispose(bool disposing)
		{
			//dispose unitofwork
			if (this.UnitOfWork != null)
				this.UnitOfWork.Dispose();

			base.Dispose(disposing);
		}
	}
}
