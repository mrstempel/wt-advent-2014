using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.Frontend.Models;

namespace WTAdvent.Core.Frontend.ApiWrappers
{
	public static class Instagram
	{
		private static string baseUrl = "https://api.instagram.com/v1/";

		public static dynamic GetResult(string url)
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(baseUrl);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			HttpResponseMessage response = client.GetAsync(url).Result;

			return (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<dynamic>().Result : null;
		}

		public static FanwallContainer Search(string q, string nextUrl)
		{
			try
			{
				var searchUrl = (nextUrl == null)
					? string.Format("tags/{0}/media/recent?client_id={1}", q, ConfigurationManager.AppSettings["Instagram.ClienId"])
					: nextUrl;
				dynamic result = GetResult(searchUrl);
				var fanwallContainer = new FanwallContainer();
				var instagramItems = new List<FanwallItem>();
				
				// set next url
				try
				{
					fanwallContainer.NextUrl = result.pagination.next_url.ToString();
				}
				catch (Exception){}

				foreach (var i in result.data)
				{
					var item = new FanwallItem
					{
						PostingType = "Instagram",
						CreatedTime = i.created_time.ToString(),
						ImageHeight = i.images.standard_resolution.height.ToString(),
						ImageUrl = i.images.standard_resolution.url.ToString(),
						Link = i.link.ToString(),
						Text = (i.caption != null && i.caption.text != null) ? i.caption.text.ToString() : string.Empty,
						ThumbnailUrl = i.images.thumbnail.url.ToString(),
						ImageWidth = i.images.standard_resolution.width.ToString()
					};

					if (i.videos != null)
					{
						item.VideoLowUrl = i.videos.low_resolution.url.ToString();
						item.VideoLowWidth = i.videos.low_resolution.width.ToString();
						item.VideoLowHeight = i.videos.low_resolution.height.ToString();

						item.VideoUrl = i.videos.standard_resolution.url.ToString();
						item.VideoWidth = i.videos.standard_resolution.width.ToString();
						item.VideoHeight = i.videos.standard_resolution.height.ToString();
					}

					instagramItems.Add(item);
				}

				fanwallContainer.Items = instagramItems;

				return fanwallContainer;
			}
			catch (Exception ex)
			{
				return null;
				throw;
			}
		}
	}
}
