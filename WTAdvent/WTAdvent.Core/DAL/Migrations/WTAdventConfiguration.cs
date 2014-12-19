using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;

namespace WTAdvent.Core.DAL.Migrations
{
	public sealed class WTAdventConfiguration : DbMigrationsConfiguration<WTAdventContext>
	{
		public WTAdventConfiguration()
        {
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = false;
        }

		protected override void Seed(WTAdventContext context)
		{
			List<BaseSeeder> seeder = new List<BaseSeeder>();
			seeder.Add(new InitialSeeder(context));
			seeder.Add(new TestQuestionSeeder(context));
			seeder.Add(new UpdateQuestionSeeder(context));
			seeder.Add(new UpdateBonusSeeder(context));
			seeder.Add(new UpdateQuestionZoomSeeder(context));
			seeder.Add(new UpdateQuestionToleranceSeeder(context));
			
			foreach (var baseSeeder in seeder)
			{
				if (baseSeeder.ShouldSeed && baseSeeder.Seed())
				{
					baseSeeder.FinishSeed();
				}
			}
		}
	}
}
