using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.DAL.Migrations
{
	public class UpdateQuestionToleranceSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "UpdateQuestionToleranceSeeder"; }
		}

		public UpdateQuestionToleranceSeeder(WTAdventContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			UpdateQuestions();

			// add seeding method calls

			return true;
		}

		private void UpdateQuestions()
		{
			// 1. Stephansplatz
			var stephansplatz = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 1);
			stephansplatz.StartLocation = new Location() { Lat = "48.739232", Lng = "9.285549" };
			stephansplatz.ToleranceDistance = 30;
			Unit.QuestionRepository.Update(stephansplatz);
			// 2. Cafe Demel
			var demel = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 2);
			demel.ZoomLevel = 18;
			demel.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(demel);
			// 3. Spanische Hofreitschule
			var hofreitschule = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 3);
			hofreitschule.ZoomLevel = 17;
			hofreitschule.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(hofreitschule);
			// 4. Zuckerlwerkstatt
			var zuckerl = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 4);
			zuckerl.ZoomLevel = 17;
			zuckerl.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(zuckerl);
			// 5. Riesenrad
			var riesenrad = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 5);
			riesenrad.ZoomLevel = 17;
			riesenrad.ToleranceDistance = 1;
			Unit.QuestionRepository.Update(riesenrad);
			// 6. Augartenporzellan
			var augarten = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 6);
			augarten.ZoomLevel = 17;
			augarten.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(augarten);
			// 7. Rathaus / Breitenseer Lichtspiele
			var rathaus = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 7);
			rathaus.ZoomLevel = 14;
			rathaus.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(rathaus);
			// 8. Oper
			var oper = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 8);
			oper.ZoomLevel = 15;
			oper.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(oper);
			// 9. Albertina
			var albertina = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 9);
			albertina.ZoomLevel = 18;
			albertina.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(albertina);
			// 10. Weihnachtsdorf Maria-Theresien-Platz
			var mariatheresienplatz = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 10);
			mariatheresienplatz.ZoomLevel = 18;
			mariatheresienplatz.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(mariatheresienplatz);
			// 11. Ronacher Marry Poppins
			var ronacher = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 11);
			ronacher.ZoomLevel = 16;
			ronacher.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(ronacher);
			// 12. Naschmarkt
			var naschmarkt = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 12);
			Unit.QuestionRepository.Update(naschmarkt);
			// 13. Musikverein
			var musikverein = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 13);
			musikverein.ZoomLevel = 17;
			musikverein.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(musikverein);
			// 14. Kultur- & Weihnachtsmarkt Schloss Schönbrunn
			var schoenbrunn = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 14);
			schoenbrunn.ZoomLevel = 14;
			Unit.QuestionRepository.Update(schoenbrunn);
			// 15. Haus des Meeres
			var hausdesmeeres = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 15);
			hausdesmeeres.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(hausdesmeeres);
			// 16. Gschupfter Ferdl
			var ferdl = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 16);
			ferdl.ZoomLevel = 16;
			ferdl.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(ferdl);
			// 17. Weihnachtsdorf Schloss Belvedere
			var belvedere = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 17);
			belvedere.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(belvedere);
			// 18. Adventmarkt vor der Karlskirche
			var karlskirche = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 18);
			karlskirche.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(karlskirche);
			// 19. Labstelle
			var labstelle = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 19);
			labstelle.ZoomLevel = 15;
			labstelle.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(labstelle);
			// 20. MuseumsQuartier
			var mq = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 20);
			mq.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(mq);
			// 21. Altwiener Christkindlmarkt
			var freyung = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 21);
			freyung.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(freyung);
			// 22. Weihnachtsmarkt am Spittelberg
			var spittelberg = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 22);
			spittelberg.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(spittelberg);
			// 23. Stadtbahnbögen
			var guertel = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 23);
			Unit.QuestionRepository.Update(guertel);
			// 24. Schneekugelmanufaktur
			var schneekugel = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 24);
			schneekugel.ToleranceDistance = 0.3m;
			Unit.QuestionRepository.Update(schneekugel);
			
			Unit.Save();
		}
	}
}
