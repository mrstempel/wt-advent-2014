using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public class Question : BaseEntity
	{
		public int Day { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public virtual Location StartLocation { get; set; }
		public virtual Location TargetLocation { get; set; }
		public int ZoomLevel { get; set; }
		public decimal ToleranceDistance { get; set; }
	}

	public class QuestionMap : EntityTypeConfiguration<Question>
	{
		public QuestionMap()
		{
			// Table Name
			this.ToTable("Question");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
