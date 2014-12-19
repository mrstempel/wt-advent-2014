using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WTAdvent.Core.Mvc
{
	public class RequireBasicAuthentication : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var req = filterContext.HttpContext.Request;
			bool isAuthenticated = false;

			if (!String.IsNullOrEmpty(req.Headers["Authorization"]))
			{
				var cred = Encoding.ASCII.GetString(Convert.FromBase64String(req.Headers["Authorization"].Substring(6))).Split(':');
				isAuthenticated =
					(cred[0] == ConfigurationManager.AppSettings["Authentication.Basic.Name"] &&
					cred[1] == ConfigurationManager.AppSettings["Authentication.Basic.Password"]);
			}

			if (!isAuthenticated)
			{
				var res = filterContext.HttpContext.Response;
				res.StatusCode = 401;
				res.AddHeader("WWW-Authenticate", "Basic realm=\"queez\"");
				res.End();

				filterContext.Result = new EmptyResult();
			}
		}
	}
}
