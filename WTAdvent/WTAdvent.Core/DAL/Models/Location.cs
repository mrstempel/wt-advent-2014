using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public class Location : BaseEntity
	{
		public string Lat { get; set; }
		public string Lng { get; set; }
	}

	public class LocationMap : EntityTypeConfiguration<Location>
	{
		public LocationMap()
		{
			// Table Name
			this.ToTable("Location");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
