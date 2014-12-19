using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using KwIt.Project.Pattern.DAL.Repository;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.UnitOfWork;

namespace WTAdvent.Core.DAL.Repository
{
	public class WTAdventRepository<TEntity> : GenericRepository<TEntity> 
        where TEntity: BaseEntity
    {
		private WTAdventUnitOfWork<WTAdventContext> _unitOfWork;
		protected WTAdventUnitOfWork<WTAdventContext> UnitOfWork
		{
			get
			{
				if (_unitOfWork == null)
					_unitOfWork = new WTAdventUnitOfWork<WTAdventContext>(this.Context as WTAdventContext);
				return _unitOfWork;
			}
		}

		public WTAdventRepository(WTAdventContext context)
            : base(context)
        {
        }
    }
}
