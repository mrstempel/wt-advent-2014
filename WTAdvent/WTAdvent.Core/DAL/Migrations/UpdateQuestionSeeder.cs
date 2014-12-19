using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.DAL.Migrations
{
	public class UpdateQuestionSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "UpdateQuestionSeeder"; }
		}

		public UpdateQuestionSeeder(WTAdventContext context)
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
			var startLocation = new Location() {Lat = "48.209195", Lng = "16.3835774"};

			// Stephansplatz
			var stephansplatz = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 1);
			// Cafe Demel
			var demel = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 18);
			demel.Day = 2;
			demel.StartLocation = stephansplatz.TargetLocation;
			Unit.QuestionRepository.Update(demel);
			// Spanische Hofreitschule
			var hofreitschule = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 11);
			hofreitschule.Day = 3;
			hofreitschule.StartLocation = demel.TargetLocation;
			Unit.QuestionRepository.Update(hofreitschule);
			// Zuckerlwerkstatt
			var zuckerl = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 5);
			zuckerl.Day = 4;
			zuckerl.StartLocation = hofreitschule.TargetLocation;
			Unit.QuestionRepository.Update(zuckerl);
			// Riesenrad
			var riesenrad = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 8);
			riesenrad.Day = 5;
			riesenrad.StartLocation = zuckerl.TargetLocation;
			Unit.QuestionRepository.Update(riesenrad);
			// Augartenporzellan
			var augarten = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 13);
			augarten.Day = 6;
			augarten.StartLocation = riesenrad.TargetLocation;
			Unit.QuestionRepository.Update(augarten);
			// Rathaus / Breitenseer Lichtspiele
			var rathaus = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 14);
			rathaus.Day = 7;
			rathaus.StartLocation = augarten.TargetLocation;
			Unit.QuestionRepository.Update(rathaus);
			// Oper
			var oper = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 2);
			oper.Day = 8;
			oper.StartLocation = rathaus.TargetLocation;
			Unit.QuestionRepository.Update(oper);
			// Albertina
			var albertina = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 4);
			albertina.Day = 9;
			albertina.StartLocation = oper.TargetLocation;
			Unit.QuestionRepository.Update(albertina);
			// Weihnachtsdorf Maria-Theresien-Platz
			var mariatheresienplatz = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 23);
			mariatheresienplatz.Day = 10;
			mariatheresienplatz.StartLocation = albertina.TargetLocation;
			Unit.QuestionRepository.Update(mariatheresienplatz);
			// Ronacher Marry Poppins
			var ronacher = new Question()
			{
				Day = 11,
				Name = "Ronacher Marry Poppins",
				Address = "Seilerstätte 9",
				StartLocation = mariatheresienplatz.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2052028", Lng = "16.3750471" },
				ZoomLevel = 15,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(ronacher);
			// Naschmarkt
			var naschmarkt = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 12);
			naschmarkt.StartLocation = ronacher.TargetLocation;
			Unit.QuestionRepository.Update(naschmarkt);
			// Musikverein
			var musikverein = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 19);
			musikverein.Day = 13;
			musikverein.StartLocation = naschmarkt.TargetLocation;
			Unit.QuestionRepository.Update(musikverein);
			// Kultur- & Weihnachtsmarkt Schloss Schönbrunn
			var schoenbrunn = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 21);
			schoenbrunn.Day = 14;
			schoenbrunn.StartLocation = musikverein.TargetLocation;
			Unit.QuestionRepository.Update(schoenbrunn);
			// Haus des Meeres
			var hausdesmeeres = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 20);
			hausdesmeeres.Day = 15;
			hausdesmeeres.StartLocation = schoenbrunn.TargetLocation;
			Unit.QuestionRepository.Update(hausdesmeeres);
			// Gschupfter Ferdl
			var ferdl = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 9);
			ferdl.Day = 16;
			ferdl.StartLocation = hausdesmeeres.TargetLocation;
			Unit.QuestionRepository.Update(ferdl);
			// Weihnachtsdorf Schloss Belvedere
			var belvedere = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 10);
			belvedere.Day = 17;
			belvedere.StartLocation = ferdl.TargetLocation;
			Unit.QuestionRepository.Update(belvedere);
			// Adventmarkt vor der Karlskirche
			var karlskirche = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 3);
			karlskirche.Day = 18;
			karlskirche.StartLocation = belvedere.TargetLocation;
			Unit.QuestionRepository.Update(karlskirche);
			// Labstelle
			var labstelle = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 15);
			labstelle.Day = 19;
			labstelle.StartLocation = karlskirche.TargetLocation;
			Unit.QuestionRepository.Update(labstelle);
			// MuseumsQuartier
			var mq = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 6);
			mq.Day = 20;
			mq.StartLocation = labstelle.TargetLocation;
			Unit.QuestionRepository.Update(mq);
			// Altwiener Christkindlmarkt
			var freyung = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 7);
			freyung.Day = 21;
			freyung.StartLocation = mq.TargetLocation;
			Unit.QuestionRepository.Update(freyung);
			// Weihnachtsmarkt am Spittelberg
			var spittelberg = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 17);
			spittelberg.Day = 22;
			spittelberg.StartLocation = freyung.TargetLocation;
			Unit.QuestionRepository.Update(spittelberg);
			// Stadtbahnbögen
			var guertel = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 16);
			guertel.Day = 23;
			guertel.StartLocation = spittelberg.TargetLocation;
			Unit.QuestionRepository.Update(guertel);
			// Schneekugelmanufaktur
			var schneekugel = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 24);
			schneekugel.StartLocation = guertel.TargetLocation;
			Unit.QuestionRepository.Update(schneekugel);
			// Kaffeerösterei Alt Wien
			var altwien = Unit.QuestionRepository.AsQueryable().FirstOrDefault(q => q.Day == 22);
			Unit.QuestionRepository.Delete(altwien);

			Unit.Save();
		}
	}
}
