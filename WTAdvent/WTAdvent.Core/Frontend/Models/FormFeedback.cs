using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTAdvent.Core.Frontend.Models
{
	public class FormFeedback
	{
		public bool IsError { get; set; }
		public bool AutoClose { get; set; }
		public string Headline { get; set; }
		public string Text { get; set; }
	}
}
