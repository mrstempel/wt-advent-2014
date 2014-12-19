using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTAdvent.Core.Frontend.Models
{
	public class HighscoreItem
	{
		public int Rank { get; set; }
		public int SnowieVersion { get; set; }
		public int UserId { get; set; }
		public string Name { get; set; }
		public long Points { get; set; }
	}
}
