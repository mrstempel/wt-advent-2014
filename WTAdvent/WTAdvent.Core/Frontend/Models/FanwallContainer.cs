using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTAdvent.Core.Frontend.Models
{
	public class FanwallContainer
	{
		public string NextUrl { get; set; }
		public IEnumerable<FanwallItem> Items { get; set; }
	}
}
