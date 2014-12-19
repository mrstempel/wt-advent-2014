using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.Mvc;
using WTAdvent.Core.Utils;

namespace WTAdvent.Web.Controllers
{
    public class HomeController : BaseController
    {
		public HomeController() { }
		public HomeController(string lng) : base(lng) { }

        public ActionResult Index(string lng)
        {
	        if (!string.IsNullOrEmpty(Request["i_t"]))
	        {
		        this.InviteToken = Request["i_t"];
	        }

			if (lng == null)
			{
				try
				{
					lng = this.AvailableLanguages.First(l => l.Key == Request.UserLanguages[0].Substring(0, 2).ToLower()).Key;
					this.Lng = lng;
				}
				catch (Exception)
				{
					this.Lng = "en";
				}
			}

            return View();
        }
    }
}
