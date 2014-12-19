using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.Frontend.Models
{
	public class UserQuestion
	{
		public Question Question { get; set; }
		public string TranslatedTitle { get; set; }
		public string TranslatedTargetName { get; set; }
		public string Link { get; set; }
		public IEnumerable<Answer> Answers { get; set; }
		public bool IsAnswered { get; set; }
	}
}
