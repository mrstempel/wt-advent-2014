using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.Frontend.Models
{
	public class MyPoints
	{
		public IEnumerable<BonusPoint>  LatestBonusPoints { get; set; }
		public IEnumerable<Question> OpenQuestions { get; set; }
		public IEnumerable<HighscoreItem> TopTen { get; set; }
		public HighscoreItem MyRank { get; set; }
	}
}
