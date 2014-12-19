using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public class Country : BaseEntity
	{
		public int WienInfoId { get; set; }
		public string Name { get; set; }
		public string Lng { get; set; }
	}

	public class CountryMap : EntityTypeConfiguration<Country>
	{
		public CountryMap()
		{
			// Table Name
			this.ToTable("Country");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
