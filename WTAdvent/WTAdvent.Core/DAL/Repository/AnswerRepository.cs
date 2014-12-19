using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using KwIt.Project.Pattern.DAL.Repository;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.UnitOfWork;
using WTAdvent.Core.Mvc;

namespace WTAdvent.Core.DAL.Repository
{
	public class AnswerRepository : WTAdventRepository<Answer> 
    {
		public AnswerRepository(WTAdventContext context)
            : base(context)
        {
        }

		public void Insert(Answer entity, int day)
		{
			// set try count
			try
			{
				var tryCount = (from a in AsQueryable() where a.UserId == entity.UserId && a.QuestionId == entity.QuestionId select a.TryCount).Max();
				entity.TryCount = ++tryCount;
			}
			catch (Exception)
			{
				entity.TryCount = 1;
			}

			// check if trycount does not exceed max try count
			if (entity.TryCount > Convert.ToInt32(ConfigurationManager.AppSettings["Questions.MaxTry"]))
			{
				return;
			}

			// check if answer is correct
			var question = UnitOfWork.QuestionRepository.GetById(entity.QuestionId);
			entity.IsCorrect = question.ToleranceDistance >= entity.TargetDistance;

			// if answer is correct set bonuspoints
			if (entity.IsCorrect && entity.UserId != Convert.ToInt32(ConfigurationManager.AppSettings["Admin"]))
			{
				var bonusPoints = BonusPointsFor.CorrectAnswer/entity.TryCount;
				UnitOfWork.BonuspointRepository.Insert(entity.UserId, bonusPoints, "punkte-wiebekommen-fragerichtig");
			}

			entity.CreatedDate = DateTime.Now;
			base.Insert(entity);
		}
    }
}
