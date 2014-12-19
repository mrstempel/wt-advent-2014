using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public class User : BaseEntity
	{
		public Guid Guid { get; set; }
		public string Sex { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Country { get; set; }
		public int CountryId { get; set; }
		public bool HasProfileImage { get; set; }
		public bool HasDailyReminder { get; set; }
		public bool HasNewsletter { get; set; }
		public string NewsletterLanguage { get; set; }
		public DateTime InsertDate { get; set; }
		public DateTime? UpdateDate { get; set; }
		public int SnowieVersion { get; set; }
		public string FacebookId { get; set; }

		public ICollection<BonusPoint> BonusPoints { get; set; }
	}

	public class UserMap : EntityTypeConfiguration<User>
	{
		public UserMap()
		{
			this.HasKey(t => t.Id);

			this.Property(t => t.InsertDate).HasColumnName("InsertDate").HasColumnType("datetime2");
			this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").HasColumnType("datetime2");
		}
	}
}
