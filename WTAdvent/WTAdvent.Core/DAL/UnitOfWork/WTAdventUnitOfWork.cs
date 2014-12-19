using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Library;
using KwIt.Project.Pattern.DAL.UnitOfWork;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.DAL.Repository;

namespace WTAdvent.Core.DAL.UnitOfWork
{
	public class WTAdventUnitOfWork<TContext> : GenericUnitOfWork<TContext>
		where TContext : WTAdventContext, new()
	{
		private UserRepository _userRepository;
		public UserRepository UserRepository
		{
			get
			{
				if (_userRepository == null)
					_userRepository = new UserRepository(this.Context);

				return _userRepository;
			}
		}

		private BonusPointRepository _bonuspointRepository;
		public BonusPointRepository BonuspointRepository
		{
			get
			{
				if (_bonuspointRepository == null)
					_bonuspointRepository = new BonusPointRepository(this.Context);

				return _bonuspointRepository;
			}
		}

		private IRepository<Highscores> _highscoreRepository;
		public IRepository<Highscores> HighscoreRepository
		{
			get
			{
				if (_highscoreRepository == null)
					_highscoreRepository = new WTAdventRepository<Highscores>(this.Context);

				return _highscoreRepository;
			}
		}

		private IRepository<Location> _locationRepository;
		public IRepository<Location> LocationRepository
		{
			get
			{
				if (_locationRepository == null)
					_locationRepository = new WTAdventRepository<Location>(this.Context);

				return _locationRepository;
			}
		}

		private QuestionRepository _questionRepository;
		public QuestionRepository QuestionRepository
		{
			get
			{
				if (_questionRepository == null)
					_questionRepository = new QuestionRepository(this.Context);

				return _questionRepository;
			}
		}

		private AnswerRepository _answerRepository;
		public AnswerRepository AnswerRepository
		{
			get
			{
				if (_answerRepository == null)
					_answerRepository = new AnswerRepository(this.Context);

				return _answerRepository;
			}
		}

		private ActionTokenRepository _actionTokenRepository;
		public ActionTokenRepository ActionTokenRepository
		{
			get
			{
				if (_actionTokenRepository == null)
					_actionTokenRepository = new ActionTokenRepository(this.Context);

				return _actionTokenRepository;
			}
		}

		private IRepository<Country> _countryRepository;
		public IRepository<Country> CountryRepository
		{
			get
			{
				if (_countryRepository == null)
					_countryRepository = new WTAdventRepository<Country>(this.Context);

				return _countryRepository;
			}
		}

		private IRepository<DBMigrationHistory> _dBMigrationHistoryRepository;
		public IRepository<DBMigrationHistory> DBMigrationHistoryRepository
		{
			get
			{
				if (_dBMigrationHistoryRepository == null)
					_dBMigrationHistoryRepository = new WTAdventRepository<DBMigrationHistory>(this.Context);

				return _dBMigrationHistoryRepository;
			}
		}

		public bool ProxyCreationEnabled
		{
			get { return this.Context.Configuration.ProxyCreationEnabled; }
			set { this.Context.Configuration.ProxyCreationEnabled = value; }
		}

		public WTAdventUnitOfWork()
        {
            _context = new TContext();
        }

		public WTAdventUnitOfWork(TContext context)
        {
            _context = context;
        }
	}
}
