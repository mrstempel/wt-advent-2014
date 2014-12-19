using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.DAL.Repository
{
	public class ActionTokenRepository : WTAdventRepository<ActionToken>
	{
		public ActionTokenRepository(WTAdventContext context)
			: base(context)
		{
		}

		public ActionToken Get(Guid guid, ActionTokenType type)
		{
			return AsQueryable().FirstOrDefault(t => t.Token == guid && t.Type == type && t.ValidUntil >= DateTime.Now);
		}

		public ActionToken GetInviteToken(int userId)
		{
			var token = AsQueryable().FirstOrDefault(t => t.UserId == userId && t.Type == ActionTokenType.Invite);

			if (token == null)
			{
				token = new ActionToken()
				{
					UserId = userId,
					Type = ActionTokenType.Invite,
					CreatedDate = DateTime.Now,
					ValidUntil = DateTime.MaxValue,
					Token = Guid.NewGuid()
				};
				Insert(token);
				UnitOfWork.Save();
			}

			return token;
		}
	}
}
