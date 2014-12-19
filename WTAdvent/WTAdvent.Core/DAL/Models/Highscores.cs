using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace WTAdvent.Core.DAL.Models
{
	public class Highscores : BaseEntity
	{
		public int UserId { get; set; }
		public int Points { get; set; }
	}

	public class HighscoresMap : EntityTypeConfiguration<Highscores>
	{
		public HighscoresMap()
		{
			// Table Name
			this.ToTable("Highscores");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
