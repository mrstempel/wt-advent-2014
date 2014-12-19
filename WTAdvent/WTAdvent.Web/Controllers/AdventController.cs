using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.Frontend.Models;
using WTAdvent.Core.Mvc;

namespace WTAdvent.Web.Controllers
{
    public class AdventController : BaseController
    {
		public AdventController(string lng) : base(lng) { }

		public ActionResult MoreInfo(int day, int points)
	    {
			if (points > 0 && !IsAdmin)
			{
				var existingBonus =
					UnitOfWork.BonuspointRepository.AsQueryable().FirstOrDefault(b => b.UserId == LoggedUser.Id && b.Day == day);
				if (existingBonus == null)
				{
					UnitOfWork.BonuspointRepository.Insert(LoggedUser.Id, points, "punkte-wiebekommen-mehrerfahren", day);
					UnitOfWork.Save();
				}
			}

			ViewBag.Points = points;
			ViewBag.Day = day;

			var dayView = "Day" + day;
		    return View(dayView);
	    }

	    public ActionResult Memory(int day)
	    {
		    ViewBag.Day = day;
		    return View();
	    }
    }
}
