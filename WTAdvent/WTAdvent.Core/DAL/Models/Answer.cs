using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public class Answer : BaseEntity
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

		public int QuestionId { get; set; }
		private Question _question;
		public Question Question
		{
			get { return _question; }
			set
			{
				_question = value;
				if (value != null)
					QuestionId = value.Id;
			}
		}

		public virtual Location DroppedLocation { get; set; }
		public int TryCount { get; set; }
		public decimal TargetDistance { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsCorrect { get; set; }
	}

	public class AnswerMap : EntityTypeConfiguration<Answer>
	{
		public AnswerMap()
		{
			// Table Name
			this.ToTable("Answer");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
		}
	}
}
