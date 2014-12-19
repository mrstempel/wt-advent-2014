using System;
using System.Linq;
using System.Web;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.UnitOfWork;

namespace WTAdvent.Core.Mvc
{
	public class WebContext
	{
		private HttpContext context = HttpContext.Current;
		private WTAdventUnitOfWork<WTAdventContext> unitOfWork;
		public WTAdventUnitOfWork<WTAdventContext> UnitOfWork
		{
			get { return unitOfWork ?? (unitOfWork = new WTAdventUnitOfWork<WTAdventContext>()); }
			set { unitOfWork = value; }
		}

		public T GetFromContext<T>(string key)
		{
			return (T)this.context.Session[key];
		}

		public void SetInContext(string key, object value)
		{
			this.context.Session[key] = value;
		}

		private Guid? webUserGuid;
		/// <summary>
		/// Cookie for user
		/// </summary>
		public Guid? WebUserGuid
		{
			get { return webUserGuid; }
		}

		public User LoggedUser
		{
			get
			{
				if (GetFromContext<User>("LoggedUser") == null)
					SetInContext("LoggedUser", UnitOfWork.UserRepository.AsQueryable().FirstOrDefault(u => u.Guid == this.WebUserGuid));

				return GetFromContext<User>("LoggedUser");
			}
			set { SetInContext("LoggedUser", value); }
		}

		public WebContext(WTAdventUnitOfWork<WTAdventContext> unitOfWork)
		{
			this.UnitOfWork = unitOfWork;
			initGuid();
		}

		public void ClearContext()
		{
			SetInContext("LoggedUser", null);
			var cookie = new HttpCookie("IsFirstVisit", null) { Expires = DateTime.Now.AddYears(5) };
			context.Response.Cookies.Add(cookie);
			ResetWebUserGuid();
		}

		private Guid? initGuid()
		{
			if (!webUserGuid.HasValue && context.Request.Cookies["WebUserGuid"] != null)
			{
				Guid cookieGuid;
				bool isvalid = Guid.TryParse(context.Request.Cookies["WebUserGuid"].Value, out cookieGuid);
				if (isvalid && context.Session["checkGuid"] == null)
				{
					if (UnitOfWork.UserRepository.AsQueryable().Count(f => f.Guid == cookieGuid) == 1)
						webUserGuid = cookieGuid;
					
					context.Session.Add("checkGuid", true);
				}
				else if (isvalid && context.Session["checkGuid"] != null)
				{
					webUserGuid = cookieGuid;
				}
			}

			if (!webUserGuid.HasValue)
			{
				webUserGuid = Guid.NewGuid();
				//save to DB
				var user = new User { Guid = webUserGuid.Value, InsertDate = DateTime.Now };
				UnitOfWork.UserRepository.Insert(user);
				UnitOfWork.Save();
				//save to cookie
				var cookie = new HttpCookie("WebUserGuid", webUserGuid.ToString()) { Expires = DateTime.Now.AddYears(5) };
				context.Response.Cookies.Add(cookie);
			}

			return webUserGuid;
		}

		protected void ResetWebUserGuid()
		{
			webUserGuid = null;
			context.Session["checkGuid"] = null;

			var cookie = new HttpCookie("WebUserGuid", null) { Expires = DateTime.Now.AddYears(5) };
			context.Response.Cookies.Add(cookie);

			var cookieFirstVisit = new HttpCookie("IsFirstVisit", "true") { Expires = DateTime.Now.AddYears(5) };
			context.Response.Cookies.Add(cookieFirstVisit);

			context.Request.Cookies.Clear();
			webUserGuid = initGuid();
		}

		public void SetWebUser(User user)
		{
			var cookie = new HttpCookie("WebUserGuid", user.Guid.ToString());
			cookie.Expires = DateTime.Now.AddYears(5);
			context.Response.Cookies.Add(cookie);
			context.Request.Cookies["WebUserGuid"].Value = user.Guid.ToString();
			webUserGuid = user.Guid;
			LoggedUser = user;
		}
	}
}
