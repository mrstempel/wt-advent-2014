using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public static class BonusPointsFor
	{
		public const int Register = 3;
		public const int Share = 1;
		public const int CorrectAnswer = 3;
		public const int Invite = 3;
		public const int Webcam = 3;
	}

	public class BonusPoint : BaseEntity
	{
		public int UserId { get; set; }
		private User _user;
		public User User
		{
			get { return _user; }
			set
			{
				_user = value;
				if (value != null)
					UserId = value.Id;
			}
		}

		public int Points { get; set; }
		public string Message { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Day { get; set; }
	}

	public class BonusPointMap : EntityTypeConfiguration<BonusPoint>
	{
		public BonusPointMap()
		{
			// Table Name
			this.ToTable("BonusPoint");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
		}
	}
}
