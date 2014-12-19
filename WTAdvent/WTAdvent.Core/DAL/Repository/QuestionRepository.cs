using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using KwIt.Project.Pattern.DAL.Repository;
using KwIt.Project.Pattern.Utils;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Migrations;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.UnitOfWork;
using WTAdvent.Core.Frontend.Models;
using WTAdvent.Core.Mvc;

namespace WTAdvent.Core.DAL.Repository
{
	public class QuestionRepository : WTAdventRepository<Question> 
    {
		public QuestionRepository(WTAdventContext context)
            : base(context)
        {
        }

		public UserQuestion GetUserQuestion(int day, int userId, Translator translator)
		{
			var question = AsQueryable().FirstOrDefault(q => q.Day == day);
			var uq = new UserQuestion()
			{
				Question = question,
				TranslatedTargetName = translator.Translate("antwort" + question.Day + "-headline"),
				TranslatedTitle = translator.Translate("frage" + question.Day),
				Link =  translator.Translate("antwort" + question.Day + "-artikel-link"),
				Answers = UnitOfWork.AnswerRepository.AsQueryable()
							.Where(a => a.UserId == userId && a.QuestionId == question.Id)
							.OrderBy(a => a.CreatedDate)
			};

			return uq;
		}

		private bool isQuestionAnswered(UserQuestion q)
		{
			return ((q.Answers != null &&
			         q.Answers.Any() &&
			         q.Answers.ElementAt(q.Answers.Count() - 1).IsCorrect) ||
			        (q.Answers != null && q.Answers.Any() &&
					 q.Answers.ElementAt(q.Answers.Count() - 1).TryCount >= Convert.ToInt32(ConfigurationManager.AppSettings["Questions.MaxTry"])));
		}

		public IEnumerable<UserQuestion> GetQuestions(int fromDay, int userId, Translator translator)
		{
			var questions = AsQueryable().Where(q => q.Day <= fromDay).OrderBy(q => q.Day).ToList();
			var userQuestions = new Collection<UserQuestion>();
			
			foreach (var q in questions)
			{
				var uq = new UserQuestion()
				{
					Question = q,
					TranslatedTargetName = translator.Translate("antwort" + q.Day + "-headline"),
					TranslatedTitle = translator.Translate("frage" + q.Day),
					Link = translator.Translate("antwort" + q.Day + "-artikel-link"),
					Answers = UnitOfWork.AnswerRepository.AsQueryable()
								.Where(a => a.UserId == userId && a.QuestionId == q.Id)
								.OrderBy(a => a.CreatedDate).ToList()
				};
				uq.IsAnswered = isQuestionAnswered(uq);
				userQuestions.Add(uq);
			}

			return userQuestions;
		}

		public IEnumerable<Question> GetOpenQuestions(int userId)
		{
			var day = DateTimeCorrection.GetLocalDateTime().Day;
			var questions = AsQueryable().Where(q => q.Day <= day).OrderBy(q => q.Day).ToList();
			var openQuestions = new List<Question>();

			foreach (var q in questions)
			{
				var answers = UnitOfWork.AnswerRepository.AsQueryable().Where(a => a.UserId == userId && a.QuestionId == q.Id).ToList();
				if (answers.FirstOrDefault(a => a.IsCorrect) == null && answers.Count() < Convert.ToInt32(ConfigurationManager.AppSettings["Questions.MaxTry"]) )
					openQuestions.Add(q);
			}

			return openQuestions;
		}
    }
}
