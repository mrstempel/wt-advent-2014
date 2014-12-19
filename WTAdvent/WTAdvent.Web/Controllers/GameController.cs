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
    public class GameController : BaseController
    {
		public GameController(string lng) : base(lng) {}

	    public JsonResult GetQuestion(int? day, bool loadPrev)
	    {
		    if (!day.HasValue || day.Value == 0)
			    day = LocalDateTime.Day;

		    // check that day is not in the future!
			if (!IsAdmin && day > LocalDateTime.Day)
		    {
				return Json(null, JsonRequestBehavior.AllowGet);
		    }

			if (IsAdmin)
				day = 24;

		    if (!loadPrev)
		    {
				var question = UnitOfWork.QuestionRepository.GetUserQuestion(day.Value, LoggedUser.Id, Translator);
			    return Json(question, JsonRequestBehavior.AllowGet);
		    }

		    var questions = UnitOfWork.QuestionRepository.GetQuestions(day.Value, LoggedUser.Id, Translator);
		    return Json(questions, JsonRequestBehavior.AllowGet);
	    }

	    public JsonResult SetAnswer(int questionId, string locationLat, string locationLng, decimal distance, int day)
	    {
		    try
		    {
			    var answer = new Answer()
			    {
				    UserId = LoggedUser.Id,
				    QuestionId = questionId,
				    DroppedLocation = new Location() {Lat = locationLat, Lng = locationLng},
				    TargetDistance = distance
			    };
				UnitOfWork.AnswerRepository.Insert(answer, day);
			    UnitOfWork.Save();

				var userAnswer = new UserAnswer() { Answer = answer, BonusPoints = answer.IsCorrect ? Convert.ToInt32(Math.Ceiling((double)BonusPointsFor.CorrectAnswer / answer.TryCount)) : 0 };
				return Json(userAnswer, JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception ex)
		    {
				return Json(null, JsonRequestBehavior.AllowGet);
		    }
	    }

	    public JsonResult GetPoints()
	    {
		    return Json(UnitOfWork.BonuspointRepository.GetUserBonus(LoggedUser.Id), JsonRequestBehavior.AllowGet);
	    }

	    public JsonResult GetOpenQuestions()
	    {
		    var openQuestions = UnitOfWork.QuestionRepository.GetOpenQuestions(LoggedUser.Id);
		    return Json(openQuestions, JsonRequestBehavior.AllowGet);
	    }
    }
}
