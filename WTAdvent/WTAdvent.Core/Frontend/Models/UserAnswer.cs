using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.Frontend.Models
{
	public class UserAnswer
	{
		public Answer Answer { get; set; }
		public int BonusPoints { get; set; }
	}
}
