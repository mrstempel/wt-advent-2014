using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public class Translation
	{
		public string Key { get; set; }
		public string DE { get; set; }
		public string EN { get; set; }
		public string ES { get; set; }
		public string FR { get; set; }
		public string IT { get; set; }
	}

	public class TranslationMap : EntityTypeConfiguration<Translation>
	{
		public TranslationMap()
		{
			this.HasKey(t => t.Key);
		}
	}
}
