using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.Frontend.ApiWrappers;
using WTAdvent.Core.Frontend.Models;

namespace WTAdvent.Core.Frontend
{
	public static class Fanwall
	{
		public static FanwallContainer GetContainer(string q, string nextUrl)
		{
			var instagramContainer = Instagram.Search(q, nextUrl);
			//var twitterContainer = Twitter.Search(q, null);
			// shuffle
			//instagramItems.Shuffle();
			return instagramContainer;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			Random rng = new Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}
