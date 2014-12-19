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

namespace WTAdvent.Core.DAL.Repository
{
	public class UserRepository : WTAdventRepository<User> 
    {
		public UserRepository(WTAdventContext context)
            : base(context)
        {
        }

		public override void Update(User user)
		{
			user.UpdateDate = DateTime.Now;
			base.Update(user);
		}

		public bool IsEmailUnique(User user)
		{
			var otherUser = AsQueryable().FirstOrDefault(u => u.Email == user.Email && u.Id != user.Id);
			return otherUser == null;
		}

		public bool IsEmailUnique(int userId, string email)
		{
			var otherUser = AsQueryable().FirstOrDefault(u => u.Email == email && u.Id != userId);
			return otherUser == null;
		}

		public User GetByEmail(string email)
		{
			return AsQueryable().FirstOrDefault(u => u.Email == email);
		}

		public User GetByFacebookId(string facebookId)
		{
			return AsQueryable().FirstOrDefault(u => u.FacebookId == facebookId);
		}

		public void AddBonuspoints(User user, int points, string message)
		{
			AddBonuspoints(user.Id, points, message);
		}

		public void AddBonuspoints(int userId, int points, string message)
		{
			var bonuspoints = new BonusPoint()
			{
				UserId = userId,
				CreatedDate = DateTime.Now,
				Message = message,
				Points = points
			};

			UnitOfWork.BonuspointRepository.Insert(bonuspoints);
		}
    }
}
