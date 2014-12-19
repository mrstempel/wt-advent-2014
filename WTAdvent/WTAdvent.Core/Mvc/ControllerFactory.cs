using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WTAdvent.Core.Mvc
{
	public class ControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
		{
			//create our custom controller
			if (controllerType.BaseType == typeof(BaseController))
			{
				//create controller with language or without language
				if (requestContext.RouteData.Values.ContainsKey("lng"))
					return (IController)Activator.CreateInstance(controllerType, new[] { requestContext.RouteData.Values["lng"] });
				else
					return (IController)Activator.CreateInstance(controllerType);
			}

			//base fallback
			return base.GetControllerInstance(requestContext, controllerType);
		}
	}
}
