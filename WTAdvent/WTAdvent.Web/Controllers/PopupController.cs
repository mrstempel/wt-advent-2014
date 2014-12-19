using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.Frontend;
using WTAdvent.Core.Frontend.Models;
using WTAdvent.Core.Mvc;
using WTAdvent.Core.Utils;

namespace WTAdvent.Web.Controllers
{
    public class PopupController : BaseController
    {
		public PopupController(string lng) : base(lng) { }

		public ActionResult Intro(bool isOnMap = false)
		{
			ViewBag.IsOnMap = isOnMap;
			return View();
		}

	    public ActionResult Register()
	    {
		    this.LoggedUser.SnowieVersion = 1;
			return View(this.LoggedUser);
	    }

		[HttpPost]
	    public ActionResult Register(User user)
		{
			try
			{
				// check facebook login
				if (user.FacebookId != null && user.Email != null)
				{
					var fbUser = UnitOfWork.UserRepository.GetByFacebookId(user.FacebookId);
					if (fbUser != null)
					{
						this.WebContext.SetWebUser(fbUser);
						FormSuccess = new FormFeedback() {Headline = "facebook-success"};
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
					FormError = new FormFeedback() { Text = Translate("email_schon_vergeben")};
				}
			}
			catch (Exception e)
			{
				FormError = new FormFeedback();
				throw;
			}
			
			return View(user);
		}

	    public ActionResult MySnowie()
	    {
		    return View(LoggedUser);
	    }

		public ActionResult MyPoints()
		{
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

		public JsonResult Test()
		{
			return Json(UnitOfWork.BonuspointRepository.GetHighscores(), JsonRequestBehavior.AllowGet);
			
		}

	    public ActionResult Webcam()
	    {
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

		public ActionResult Hashtag(string nextUrl = null)
		{
			var fanwallContainer = Fanwall.GetContainer("XmasinVienna", nextUrl);
			return View(fanwallContainer);
		}

		public JsonResult MoreHashtag(string nextUrl = null)
		{
			var fanwallContainer = Fanwall.GetContainer("XmasinVienna", nextUrl);
			return Json(fanwallContainer, JsonRequestBehavior.AllowGet);
		}

	    public JsonResult HomeHashtag()
	    {
			var fanwallContainer = Fanwall.GetContainer("XmasinVienna", null);
		    IEnumerable<FanwallItem> items = null;
			if (fanwallContainer.Items != null)
				items = fanwallContainer.Items.Take(6);
		    return Json(items, JsonRequestBehavior.AllowGet);
	    }

	    public ActionResult Teilnahmebedingungen()
	    {
		    return View();
	    }

		public JsonResult SendRecommendation(string mailTo, string mailFrom, string nameFrom)
		{
			var inviteToken = UnitOfWork.ActionTokenRepository.GetInviteToken(LoggedUser.Id);

			// create msg-content
			var translatedContent = Translate("share-mail-text");
			translatedContent = translatedContent.Replace("http://xmas.wien.info", "<a href='http://xmas.wien.info?i_t=" + inviteToken.Token.ToString() + "'>http://xmas.wien.info</a>");
			translatedContent = translatedContent + "<br/><br/>" + nameFrom;
			Email.SendRecommendation(mailFrom, mailTo, Translate("share-mail-betreff"), translatedContent);
			return Json(true, JsonRequestBehavior.AllowGet);
		}
    }
}
