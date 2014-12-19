using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTAdvent.Core.Frontend.Models
{
	public class FanwallItem
	{
		public string Id { get; set; }
		public string CreatedTime { get; set; }
		public string Link { get; set; }
		public string ThumbnailUrl { get; set; }
		public string ImageUrl { get; set; }
		public string ImageWidth { get; set; }
		public string ImageHeight { get; set; }
		public string Text { get; set; }
		public string VideoLowUrl { get; set; }
		public string VideoLowWidth { get; set; }
		public string VideoLowHeight { get; set; }
		public string VideoUrl { get; set; }
		public string VideoWidth { get; set; }
		public string VideoHeight { get; set; }
		public string PostingType { get; set; }
	}
}
