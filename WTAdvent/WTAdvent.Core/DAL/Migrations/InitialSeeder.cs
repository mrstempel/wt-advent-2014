using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;

namespace WTAdvent.Core.DAL.Migrations
{
	public class InitialSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "InitialSeeder"; }
		}

		public InitialSeeder(WTAdventContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			// add seeding method calls

			return true;
		}
	}
}
