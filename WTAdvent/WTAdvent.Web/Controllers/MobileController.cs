using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.Frontend;
using WTAdvent.Core.Frontend.Models;
using WTAdvent.Core.Mvc;
using WTAdvent.Core.Utils;

namespace WTAdvent.Web.Controllers
{
    public class MobileController : BaseController
    {
		public MobileController(string lng) : base(lng) { }
		
		// Intro
        public ActionResult Index()
        {
            return View();
        }

		// Register
	    public ActionResult Register()
	    {
			LoggedUser.SnowieVersion = 1;
		    return View(LoggedUser);
	    }

		// Register Form sent
		[HttpPost]
		public ActionResult Register(User user)
		{
			try
			{
				// check facebook login
				if (user.FacebookId != null)
				{
					var fbUser = UnitOfWork.UserRepository.GetByFacebookId(user.FacebookId);
					if (fbUser != null)
					{
						this.WebContext.SetWebUser(fbUser);
						FormSuccess = new FormFeedback() { Headline = "facebook-success" };
						return View(fbUser);
					}
					else
					{
						fbUser = UnitOfWork.UserRepository.GetByEmail(user.Email);
						if (fbUser != null)
						{
							this.WebContext.SetWebUser(fbUser);
							FormSuccess = new FormFeedback() { Headline = "facebook-success" };
							return View(fbUser);
						}
					}
				}

				if (UnitOfWork.UserRepository.IsEmailUnique(user))
				{
					UnitOfWork.UserRepository.Update(user);
					UnitOfWork.Save();
					UnitOfWork.BonuspointRepository.Insert(user.Id, BonusPointsFor.Register, "punkte-wiebekommen-registrierung");
					UnitOfWork.Save();
					// send confirmation e-mail
					var translatedContent = Translate("mail-anmeldetext");
					translatedContent = translatedContent.Replace("http://xmas.wien.info", "<a href='http://xmas.wien.info'>http://xmas.wien.info</a>");
					Email.SendRecommendation(ConfigurationManager.AppSettings["Email.From"], user.Email, Translate("mail-anmeldebetreff"), translatedContent);
					// check if newsletter register is on
					if (user.HasNewsletter)
					{
						// newsletter register params
						// 0: formularid (503 = de, 504 = en)
						// 1: anrede (1 = Herr, 2 = Frau)
						// 2: vorname
						// 3: nachname
						// 4: e-mail
						// e: land
						var nlRegisterUrlTemplate =
							"http://newsletter.wien.info/u/register.php?CID=121789499&f={0}&p=2&a=r&SID=&el=&llid=&counted=&c=&interest[]&inp_29721=xmas2014&inp_46={1}&inp_1={2}&inp_2={3}&inp_3={4}&inp_26=&inp_14={5}";
						var nlLng = (Request["newsletterLng"] == "de") ? "503" : "504";
						var nlAnrede = (user.Sex == "M") ? "1" : "2";
						var nlRegisterUrl = string.Format(nlRegisterUrlTemplate, nlLng, nlAnrede, user.Firstname, user.Surname, user.Email, user.CountryId);
						var request = WebRequest.Create(nlRegisterUrl);
						var response = (HttpWebResponse)request.GetResponse();
					}

					// check if invite token is set
					if (!string.IsNullOrEmpty(InviteToken))
					{
						var token =
							UnitOfWork.ActionTokenRepository.AsQueryable().FirstOrDefault(a => a.Token.ToString() == InviteToken);
						if (token != null)
						{
							UnitOfWork.BonuspointRepository.Insert(token.UserId, BonusPointsFor.Invite, "punkte-wiebekommen-freundeeingeladen");
							UnitOfWork.Save();
							this.InviteToken = null;
						}
					}
					LoggedUser = user;
					FormSuccess = new FormFeedback();
				}
				else
				{
					FormError = new FormFeedback() { Text = Translate("email_schon_vergeben") };
				}
			}
			catch (Exception e)
			{
				FormError = new FormFeedback();
				throw;
			}

			return View(user);
		}

		// Advent Home
	    public ActionResult Advent(int day = 0)
	    {
			// check e-mail
		    if (string.IsNullOrEmpty(LoggedUser.Email))
		    {
			    return RedirectToAction("Index");
		    }

		    ViewBag.Day = day;
			var questions = UnitOfWork.QuestionRepository.GetQuestions(LocalDateTime.Day, LoggedUser.Id, Translator);

			return View(questions);
	    }

		// MySnowie
	    public ActionResult MySnowie()
	    {
			// check e-mail
			if (string.IsNullOrEmpty(LoggedUser.Email))
			{
				return RedirectToAction("Index");
			}

			return View(LoggedUser);
	    }

		// MyPoints
	    public ActionResult MyPoints()
	    {
			// check e-mail
			if (string.IsNullOrEmpty(LoggedUser.Email))
			{
				return RedirectToAction("Index");
			}

			var highscores = UnitOfWork.BonuspointRepository.GetHighscores();
			var myscore = highscores.FirstOrDefault(h => h.UserId == LoggedUser.Id);
			if (myscore != null)
				myscore.Rank = highscores.IndexOf(myscore) + 1;
			else
			{
				myscore = new HighscoreItem()
				{
					Name = LoggedUser.Firstname + " " + LoggedUser.Surname.Substring(0, 1) + ".",
					Points = Bonuspoints,
					SnowieVersion = LoggedUser.SnowieVersion,
					UserId = LoggedUser.Id,
					Rank = 100
				};
			}
			var takeMax = highscores.Count > 9 ? 10 : highscores.Count;
			var model = new MyPoints()
			{
				LatestBonusPoints = UnitOfWork.BonuspointRepository.GetLatestBonusPoints(LoggedUser.Id),
				OpenQuestions = UnitOfWork.QuestionRepository.GetOpenQuestions(LoggedUser.Id),
				TopTen = highscores.Take(takeMax),
				MyRank = myscore //myscore
			};
			return View(model);
	    }

		// Webcam
		public ActionResult Webcam()
		{
			// check e-mail
			if (string.IsNullOrEmpty(LoggedUser.Email))
			{
				return RedirectToAction("Index");
			}

			// check if bonus for webcam should be set
			var webcamBonus =
				UnitOfWork.BonuspointRepository.AsQueryable()
					.FirstOrDefault(b => b.Message == "webcam" && b.UserId == LoggedUser.Id);
			if (webcamBonus == null)
			{
				UnitOfWork.BonuspointRepository.Insert(LoggedUser.Id, BonusPointsFor.Webcam, "webcam");
				UnitOfWork.Save();
				ViewBag.Bonus = 3;
			}
			else
			{
				ViewBag.Bonus = 0;
			}

			return View();
		}

		// Hashtags
		public ActionResult Hashtag(string nextUrl = null)
		{
			// check e-mail
			if (string.IsNullOrEmpty(LoggedUser.Email))
			{
				return RedirectToAction("Index");
			}

			var fanwallContainer = Fanwall.GetContainer("XmasinVienna", nextUrl);
			return View(fanwallContainer);
		}

		// Map
	    public ActionResult Map(int day = 0)
	    {
			// check e-mail
			if (string.IsNullOrEmpty(LoggedUser.Email))
			{
				return RedirectToAction("Index");
			}

		    ViewBag.Day = day;
		    return View();
	    }

		// More Infos
		public ActionResult MoreInfo(int day, int points)
		{
			// check e-mail
			if (string.IsNullOrEmpty(LoggedUser.Email))
			{
				return RedirectToAction("Index");
			}

			if (points > 0)
			{
				UnitOfWork.BonuspointRepository.Insert(LoggedUser.Id, points, "punkte-wiebekommen-mehrerfahren");
				UnitOfWork.Save();
			}

			ViewBag.Points = points;
			ViewBag.Day = day;

			var dayView = "Day" + day;
			return View("Days/" + dayView);
		}

		// Memory Game
		public ActionResult Memory(int day)
		{
			// check e-mail
			if (string.IsNullOrEmpty(LoggedUser.Email))
			{
				return RedirectToAction("Index");
			}

			ViewBag.Day = day;
			return View();
		}

		// Logout
	    public ActionResult Logout()
	    {
			//WebSecurity.Logout();
			this.WebContext.ClearContext();
		    return RedirectToAction("Index");
	    }
    }
}
