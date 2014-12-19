using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.Utils
{
	public static class DateTimeCorrection
	{
		public static DateTime GetLocalDateTime()
		{
			try
			{
				var correction = Convert.ToInt32(ConfigurationManager.AppSettings["DateTimeCorrection"]);
				return DateTime.Now.AddHours(correction);
			}
			catch (Exception)
			{
				return DateTime.Now;
				throw;
			}
		}
	}
}
