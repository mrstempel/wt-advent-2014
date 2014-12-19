using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Context;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.DAL.Context
{
	public class WTAdventContext : BaseContext
	{
		// migration
		public DbSet<DBMigrationHistory> DBMigrationHistory { get; set; }
		
		// translations
		public DbSet<Translation> Translations { get; set; }
		
		// users
		public DbSet<User> Users { get; set; }
		
		// bonus
		public DbSet<BonusPoint> BonusPoints { get; set; }
		public DbSet<Highscores> Highscores { get; set; }
		
		// game
		public DbSet<Location> Locations { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Answer> Answers { get; set; }

		// utils
		public DbSet<ActionToken> ActionToken { get; set; }

		// countries
		public DbSet<Country> Countries { get; set; }

		public WTAdventContext() : base("EFConnectionString")
		{
			this.Configuration.LazyLoadingEnabled = true;
			this.Configuration.ProxyCreationEnabled = true;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			// migration
			modelBuilder.Configurations.Add(new DBMigrationHistoryMap());
			
			// translations
			modelBuilder.Configurations.Add(new TranslationMap());
			
			// users
			modelBuilder.Configurations.Add(new UserMap());
			
			// bonus
			modelBuilder.Configurations.Add(new BonusPointMap());
			modelBuilder.Configurations.Add(new HighscoresMap());

			// game
			modelBuilder.Configurations.Add(new LocationMap());
			modelBuilder.Configurations.Add(new QuestionMap());
			modelBuilder.Configurations.Add(new AnswerMap());

			// utils
			modelBuilder.Configurations.Add(new ActionTokenMap());

			// country
			modelBuilder.Configurations.Add(new CountryMap());

			base.OnModelCreating(modelBuilder);
		}
	}
}
