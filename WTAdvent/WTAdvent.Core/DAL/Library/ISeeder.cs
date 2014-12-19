using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.UnitOfWork;

namespace WTAdvent.Core.DAL.Library
{
	public interface ISeeder
	{
		string Id { get; }

		WTAdventUnitOfWork<WTAdventContext> Unit { get; }
		bool ShouldSeed { get; }

		bool Seed();
		bool FinishSeed();
	}
}
