using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.UnitOfWork;

namespace WTAdvent.Core.Frontend
{
	public static class StaticAppData
	{
		private static HashSet<Translation> _translations;
		public static HashSet<Translation> Translations
		{
			get
			{
				if (_translations == null)
				{
					_translations = new HashSet<Translation>();
					var uow = new WTAdventUnitOfWork<WTAdventContext>();
					using (WTAdventContext db = new WTAdventContext())
					{
						var trans = db.Translations.AsNoTracking().ToList();
						foreach (Translation item in trans)
						{
							_translations.Add(item);
						}
					}
				}

				return _translations;
			}
		}
	}
}
