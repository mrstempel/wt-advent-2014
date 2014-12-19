using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using WTAdvent.Core.Frontend.Models;

namespace WTAdvent.Core.Frontend.ApiWrappers
{
	public static class Twitter
	{
		private static TwitterContext _twitterCtx;
		public static TwitterContext TwitterCtx
		{
			get
			{
				if (_twitterCtx == null)
				{
					var auth = new ApplicationOnlyAuthorizer()
					{
						Credentials = new InMemoryCredentials()
						{
							ConsumerKey = ConfigurationManager.AppSettings["Twitter.ConsumerKey"],
							ConsumerSecret = ConfigurationManager.AppSettings["Twitter.ConsumerSecret"]
						}
					};
					auth.Authorize();
					_twitterCtx = new TwitterContext(auth);
				}

				return _twitterCtx;
			}
		}

		public static FanwallContainer Search(string q, string nextUrl)
		{
			try
			{
				var srch =
					(from search in TwitterCtx.Search
						where search.Type == SearchType.Search &&
						      search.Query == q
						      && search.Count == 200
						select search)
						.SingleOrDefault();

				if (srch != null && srch.Statuses.Count > 0)
				{
					var tweetsWithImages = srch.Statuses.Where(s => s.Entities != null && s.Entities.MediaEntities != null && s.Entities.MediaEntities.Count > 0).ToList();
					var fanwallContainer = new FanwallContainer();
					var items = new List<FanwallItem>();
				}

				return null;
			}
			catch (Exception ex)
			{
				return null;
			}

			return null;
		}
	}
}
