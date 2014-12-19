using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Library;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.UnitOfWork;

namespace WTAdvent.Core.DAL.Migrations
{
	public abstract class BaseSeeder : ISeeder
	{
		/// <summary>
		/// ID of Seeder, idenfies also if allready done
		/// </summary>
		public virtual string Id
		{
			get
			{
				throw new MissingMemberException("Specifiy Id of Seeder");
			}
		}

		public WTAdventUnitOfWork<WTAdventContext> Unit { get; private set; }

		private bool? shouldSeed;
		/// <summary>
		/// if seed should be done or not
		/// </summary>
		public bool ShouldSeed
		{
			get
			{
				if (!shouldSeed.HasValue)
				{
					shouldSeed = Unit.DBMigrationHistoryRepository.Get(f => f.Name == this.Id && f.IsDone).FirstOrDefault() == null;
				}
				return shouldSeed.Value;
			}
		}

		public BaseSeeder(WTAdventContext context)
		{
			Unit = new WTAdventUnitOfWork<WTAdventContext>(context);
		}

		/// <summary>
		/// Do the work
		/// </summary>
		/// <returns></returns>
		public virtual bool Seed()
		{
			throw new MissingMethodException("Implement Seed!");
		}

		public bool FinishSeed()
		{
			if (this.ShouldSeed)
			{
				Unit.DBMigrationHistoryRepository.Insert(new DBMigrationHistory() { CreatedOn = DateTime.Now, Name = this.Id, IsDone = true });
				Unit.Save();
			}
			return true;
		}
	}
}
