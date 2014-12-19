using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;
using WTAdvent.Core.Mvc;

namespace WTAdvent.Web.Controllers
{
    public class AuthController : BaseController
    {
		public AuthController(string lng) : base(lng) {}

		public JsonResult Logout()
		{
			//WebSecurity.Logout();
			this.WebContext.ClearContext();
			return Json(true, JsonRequestBehavior.AllowGet);
		}

	    public JsonResult Login(string email)
	    {
		    var user = UnitOfWork.UserRepository.GetByEmail(email);
		    if (user != null)
		    {
				WebContext.SetWebUser(user);
			    return Json(true, JsonRequestBehavior.AllowGet);
		    }

		    return Json(false, JsonRequestBehavior.AllowGet);
	    }

	    public JsonResult Update(int snowieVersion, string sex, string firstname, string surname, string email)
	    {
			// return values: 1 okay, 0 e-mail not unique, -1 server error

			// check if email is unique
		    if (!UnitOfWork.UserRepository.IsEmailUnique(LoggedUser.Id, email))
		    {
			    return Json(0, JsonRequestBehavior.AllowGet);
		    }

		    try
		    {
			    LoggedUser.SnowieVersion = snowieVersion;
			    LoggedUser.Sex = sex;
			    LoggedUser.Firstname = firstname;
			    LoggedUser.Surname = surname;
			    LoggedUser.Email = email;
			    UnitOfWork.UserRepository.Update(LoggedUser);
			    UnitOfWork.Save();
				return Json(1, JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception)
		    {

				return Json(-1, JsonRequestBehavior.AllowGet);
		    }
	    }

		[RequireBasicAuthentication]
		public CsvActionResult Export()
		{
			var items = UnitOfWork.UserRepository.AsQueryable().Where(u => u.Email  != null);
			var dataTable = new DataTable();
			var obj = new User();

			Type t = obj.GetType();
			PropertyInfo[] props = t.GetProperties();


			foreach (var prop in props)
			{
				dataTable.Columns.Add(prop.Name, typeof(string));
			}

			foreach (var item in items)
			{
				var row = dataTable.NewRow();
				foreach (var prop in props)
				{
					row[prop.Name] = prop.GetValue(item, null);
				}
				dataTable.Rows.Add(row);
			}

			var result = new CsvActionResult(dataTable);
			result.FileDownloadName = "Snowie-Export.csv";
			return result;
		}

		[RequireBasicAuthentication]
	    public JsonResult InitHighscores()
		{
			try
			{
				var usersWithPoints = (from u in UnitOfWork.UserRepository.AsQueryable()
									   join b in UnitOfWork.BonuspointRepository.AsQueryable() on u.Id equals b.UserId
									   select u).Distinct().ToList();

				foreach (var u in usersWithPoints)
				{
					var id = u.Id;
					var highscore = UnitOfWork.HighscoreRepository.AsQueryable().FirstOrDefault(h => h.UserId == id);
					if (highscore == null)
					{
						UnitOfWork.BonuspointRepository.UpdateHighscores(id);
						UnitOfWork.Save();		
					}
				}

				return Json(true, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{

				return Json(ex, JsonRequestBehavior.AllowGet);
			}
		}
    }
}
