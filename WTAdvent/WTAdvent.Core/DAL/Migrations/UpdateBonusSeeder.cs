using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.DAL.Migrations
{
	public class UpdateBonusSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "UpdateBonusSeeder"; }
		}

		public UpdateBonusSeeder(WTAdventContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			UpdateBonus();

			// add seeding method calls

			return true;
		}

		private void UpdateBonus()
		{
			// register bonus
			var registerBonus = Unit.BonuspointRepository.Get(b => b.Message == "register_bonus");
			foreach (var rb in registerBonus)
			{
				rb.Message = "punkte-wiebekommen-registrierung";
				Unit.BonuspointRepository.Update(rb);
			}

			// answer bonus
			var answerBonus = Unit.BonuspointRepository.Get(b => b.Message.StartsWith("answer"));
			foreach (var ab in answerBonus)
			{
				ab.Message = "punkte-wiebekommen-fragerichtig";
				Unit.BonuspointRepository.Update(ab);
			}

			// more info bonus
			var moreInfoBonus = Unit.BonuspointRepository.Get(b => b.Message.StartsWith("klick"));
			foreach (var mib in moreInfoBonus)
			{
				mib.Message = "punkte-wiebekommen-mehrerfahren";
				Unit.BonuspointRepository.Update(mib);
			}

			Unit.Save();
		}
	}
}
