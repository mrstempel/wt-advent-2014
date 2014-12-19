using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using KwIt.Project.Pattern.DAL.Repository;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.UnitOfWork;
using WTAdvent.Core.Frontend.Models;
using WTAdvent.Core.Mvc;

namespace WTAdvent.Core.DAL.Repository
{
	public class BonusPointRepository : WTAdventRepository<BonusPoint> 
    {
		public BonusPointRepository(WTAdventContext context)
            : base(context)
        {
        }

		public override void Insert(BonusPoint entity)
		{
			base.Insert(entity);
			UnitOfWork.Save();

			// update highscores
			UpdateHighscores(entity.UserId);
		}

		public void Insert(int userId, int points, string message)
		{
			var bonus = new BonusPoint()
			{
				UserId = userId,
				Points = points,
				Message = message,
				CreatedDate = DateTime.Now
			};

			Insert(bonus);
		}

		public void Insert(int userId, int points, string message, int day)
		{
			var bonus = new BonusPoint()
			{
				UserId = userId,
				Points = points,
				Message = message,
				CreatedDate = DateTime.Now,
				Day = day
			};

			Insert(bonus);
		}

		public IEnumerable<BonusPoint> GetLatestBonusPoints(int userId)
		{
			return AsQueryable().Where(b => b.UserId == userId).OrderByDescending(b => b.CreatedDate).Take(3);
		}

		public long GetUserBonus(int userId)
		{
			try
			{
				return (from b in UnitOfWork.BonuspointRepository.AsQueryable()
						where
							b.UserId == userId
						select (long)b.Points).Sum();
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public void UpdateHighscores(int userId)
		{
			var myScores = UnitOfWork.HighscoreRepository.AsQueryable().FirstOrDefault(h => h.UserId == userId);
			if (myScores == null)
			{
				UnitOfWork.HighscoreRepository.Insert(new Highscores() { UserId = userId, Points = Convert.ToInt32(GetUserBonus(userId)) });
			}
			else
			{
				myScores.Points = Convert.ToInt32(GetUserBonus(userId));
				UnitOfWork.HighscoreRepository.Update(myScores);
			}
		}

		public List<HighscoreItem> GetHighscores()
		{
			//var usersWithPoints = (from u in UnitOfWork.UserRepository.AsQueryable()
			//	join b in UnitOfWork.BonuspointRepository.AsQueryable() on u.Id equals b.UserId
			//	select u).Distinct();

			var usersWithPoints = (from u in UnitOfWork.UserRepository.AsQueryable()
								   join h in UnitOfWork.HighscoreRepository.AsQueryable() on u.Id equals h.UserId select new { user = u, points = h.Points}).Distinct().OrderByDescending(h => h.points).Take(10);

			var topTen = new List<HighscoreItem>();

			foreach (var u in usersWithPoints)
			{
				var highScore = new HighscoreItem()
				{
					Name = u.user.Firstname + " " + u.user.Surname.Substring(0, 1) + ".",
					Points = u.points,
					SnowieVersion = u.user.SnowieVersion,
					UserId = u.user.Id
				};
				topTen.Add(highScore);
			}

			return topTen.OrderByDescending(t => t.Points).ToList();
		}
    }
}
