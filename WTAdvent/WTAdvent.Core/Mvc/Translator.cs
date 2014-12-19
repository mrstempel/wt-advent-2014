using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.Frontend;

namespace WTAdvent.Core.Mvc
{
	public class Translator
	{
		protected string Language { get; set; }

		public Translator(string language)
		{
			this.Language = language.ToUpper();
		}

		public string Translate(string key)
		{
			var trans = StaticAppData.Translations.FirstOrDefault(f => f.Key.ToLower() == key.ToLower());
			//handle missing key
			if (trans == null)
			{
				return string.Format("missing translation [{0}] in {1}", key, Language);
			}
			//get property of Langauge
			if (trans.GetType().GetProperty(Language).GetValue(trans) == null)
			{
				return string.Format("missing translation value for [{0}] in {1}", key, Language);
			}
			string value = trans.GetType().GetProperty(Language).GetValue(trans).ToString();

			return value;
		}
	}
}
