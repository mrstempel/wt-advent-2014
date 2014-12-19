using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WTAdvent.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//routes.MapRoute(
			//	name: "Default",
			//	url: "{controller}/{action}/{id}",
			//	defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			//);

			// Auth Route
			routes.MapRoute(
				name: "Auth",
				url: "{lng}/Auth/{action}",
				defaults: new
				{
					controller = "Auth",
					action = "Login",
					lng = "de"
				}
			);

			// Popup Route
			routes.MapRoute(
				name: "Popup",
				url: "{lng}/Popup/{action}/{id}",
				defaults: new
				{
					controller = "Popup",
					action = "Index",
					lng = "de",
					id = UrlParameter.Optional
				}
			);

			// Advent Route
			routes.MapRoute(
				name: "Advent",
				url: "{lng}/Advent/{action}/{id}",
				defaults: new
				{
					controller = "Advent",
					action = "Index",
					lng = "de",
					id = UrlParameter.Optional
				}
			);

			// Game Route
			routes.MapRoute(
				name: "Game",
				url: "{lng}/Game/{action}/{id}",
				defaults: new
				{
					controller = "Game",
					action = "Index",
					lng = "de",
					id = UrlParameter.Optional
				}
			);

			// Mobile Route
			routes.MapRoute(
				name: "Mobile",
				url: "{lng}/Mobile/{action}/{id}",
				defaults: new
				{
					controller = "Mobile",
					action = "Index",
					lng = "de",
					id = UrlParameter.Optional
				}
			);

			//Home Route
			routes.MapRoute(
				name: "Default",
				url: "{lng}",
				defaults: new
				{
					controller = "Home",
					action = "Index",
					lng = UrlParameter.Optional
				}
			);
		}
	}
}