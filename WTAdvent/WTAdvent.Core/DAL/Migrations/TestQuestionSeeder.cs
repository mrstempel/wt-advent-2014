using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTAdvent.Core.DAL.Context;
using WTAdvent.Core.DAL.Models;

namespace WTAdvent.Core.DAL.Migrations
{
	public class TestQuestionSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "TestQuestionSeeder"; }
		}

		public TestQuestionSeeder(WTAdventContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			insertTestQuestions();

			// add seeding method calls

			return true;
		}

		private void insertTestQuestions()
		{
			var startLocation = new Location() {Lat = "48.209195", Lng = "16.3835774"};

			// Stephansplatz
			var q1 = new Question()
			{
				Day = 1,
				Name = "Stephansplatz",
				Address = "Stephansplatz",
				StartLocation = startLocation,
				TargetLocation = new Location() { Lat = "48.20922", Lng = "16.372988" },
				ZoomLevel = 7,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q1);

			// Oper
			var q2 = new Question()
			{
				Day = 2,
				Name = "Oper",
				Address = "Opernring 2",
				StartLocation = q1.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2027596", Lng = "16.3688546" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q2);

			// Adventmarkt vor der Karlskirche
			var q3 = new Question()
			{
				Day = 3,
				Name = "Adventmarkt vor der Karlskirche",
				Address = "Karlsplatz",
				StartLocation = q2.TargetLocation,
				TargetLocation = new Location() { Lat = "48.198763", Lng = "16.371564" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q3);

			// Albertina
			var q4 = new Question()
			{
				Day = 4,
				Name = "Albertina",
				Address = "Albertinaplatz 1",
				StartLocation = q3.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2045603", Lng = "16.3689826" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q4);

			// Zuckerlwerkstatt
			var q5 = new Question()
			{
				Day = 5,
				Name = "Zuckerlwerkstatt",
				Address = "Herrengasse 6-8",
				StartLocation = q4.TargetLocation,
				TargetLocation = new Location() { Lat = "48.208804", Lng = "16.366349" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q5);

			// MuseumsQuartier
			var q6 = new Question()
			{
				Day = 6,
				Name = "MuseumsQuartier",
				Address = "Museumsplatz 1",
				StartLocation = q5.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2034986", Lng = "16.3591175" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q6);

			// Altwiener Christkindlmarkt
			var q7 = new Question()
			{
				Day = 7,
				Name = "Altwiener Christkindlmarkt",
				Address = "Freyung",
				StartLocation = q6.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2116791", Lng = "16.3653325" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q7);

			// Riesenrad
			var q8 = new Question()
			{
				Day = 8,
				Name = "Riesenrad",
				Address = "Riesenradplatz",
				StartLocation = q7.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2171863", Lng = "16.3955191" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q8);

			// Gschupfter Ferdl
			var q9 = new Question()
			{
				Day = 9,
				Name = "Gschupfter Ferdl",
				Address = "Windmühlgasse 20",
				StartLocation = q8.TargetLocation,
				TargetLocation = new Location() { Lat = "48.199105", Lng = "16.356327" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q9);

			// Weihnachtsdorf Schloss Belvedere
			var q10 = new Question()
			{
				Day = 10,
				Name = "Weihnachtsdorf Schloss Belvedere",
				Address = "Prinz-Eugen-Straße 27",
				StartLocation = q9.TargetLocation,
				TargetLocation = new Location() { Lat = "48.191264", Lng = "16.3798096" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q10);

			// Spanische Hofreitschule
			var q11 = new Question()
			{
				Day = 11,
				Name = "Spanische Hofreitschule",
				Address = "Michaelerplatz 1",
				StartLocation = q10.TargetLocation,
				TargetLocation = new Location() { Lat = "48.207648", Lng = "16.366099" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q11);

			// Naschmarkt
			var q12 = new Question()
			{
				Day = 12,
				Name = "Naschmarkt",
				Address = "Wienzeile",
				StartLocation = q11.TargetLocation,
				TargetLocation = new Location() { Lat = "48.19902", Lng = "16.364136" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q12);

			// Augartenporzellan
			var q13 = new Question()
			{
				Day = 13,
				Name = "Augartenporzellan",
				Address = "Obere Augartenstraße 1A",
				StartLocation = q12.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2233062", Lng = "16.3776122" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q13);

			// Rathaus / Breitenseer Lichtspiele
			var q14 = new Question()
			{
				Day = 14,
				Name = "Rathaus / Breitenseer Lichtspiele",
				Address = "Rathausplatz",
				StartLocation = q13.TargetLocation,
				TargetLocation = new Location() { Lat = "48.210854", Lng = "16.359855" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q14);

			// Labstelle
			var q15 = new Question()
			{
				Day = 15,
				Name = "Labstelle",
				Address = "Lugeck 6",
				StartLocation = q14.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2096397", Lng = "16.3746946" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q15);

			// Stadtbahnbögen
			var q16 = new Question()
			{
				Day = 16,
				Name = "Stadtbahnbögen",
				Address = "Stadtbahnbögen",
				StartLocation = q15.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2177132", Lng = "16.3421805" },
				ZoomLevel = 13,
				ToleranceDistance = 4
			};
			Unit.QuestionRepository.Insert(q16);

			// Weihnachtsmarkt am Spittelberg
			var q17 = new Question()
			{
				Day = 17,
				Name = "Weihnachtsmarkt am Spittelberg",
				Address = "Spittelberggasse, Schrankgasse, Gutenberggasse",
				StartLocation = q16.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2034441", Lng = "16.3551958" },
				ZoomLevel = 13,
				ToleranceDistance = 4
			};
			Unit.QuestionRepository.Insert(q17);

			// Cafe Demel
			var q18 = new Question()
			{
				Day = 18,
				Name = "Cafe Demel",
				Address = "Kohlmarkt 14",
				StartLocation = q17.TargetLocation,
				TargetLocation = new Location() { Lat = "48.208593", Lng = "16.367216" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q18);

			// Musikverein
			var q19 = new Question()
			{
				Day = 19,
				Name = "Musikverein",
				Address = "Musikvereinsplatz 1",
				StartLocation = q18.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2008", Lng = "16.3729" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q19);

			// Haus des Meeres
			var q20 = new Question()
			{
				Day = 20,
				Name = "Haus des Meeres",
				Address = "Fritz-Grünbaum-Platz 1",
				StartLocation = q19.TargetLocation,
				TargetLocation = new Location() { Lat = "48.19766", Lng = "16.353048" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q20);

			// Kultur- & Weihnachtsmarkt Schloss Schönbrunn
			var q21 = new Question()
			{
				Day = 21,
				Name = "Kultur- & Weihnachtsmarkt Schloss Schönbrunn",
				Address = "Schönbrunner Schloßstraße",
				StartLocation = q20.TargetLocation,
				TargetLocation = new Location() { Lat = "48.184865", Lng = "16.31224" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q21);

			// Kaffeerösterei Alt Wien
			var q22 = new Question()
			{
				Day = 22,
				Name = "Kaffeerösterei Alt Wien",
				Address = "Schleifmühlgasse 23",
				StartLocation = q21.TargetLocation,
				TargetLocation = new Location() { Lat = "48.1981671", Lng = "16.3637632" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q22);

			// Weihnachtsdorf Maria-Theresien-Platz
			var q23 = new Question()
			{
				Day = 23,
				Name = "Weihnachtsdorf Maria-Theresien-Platz",
				Address = "Maria Theresien-Platz",
				StartLocation = q22.TargetLocation,
				TargetLocation = new Location() { Lat = "48.204437", Lng = "16.360717" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q23);

			// Schneekugelmanufaktur
			var q24 = new Question()
			{
				Day = 24,
				Name = "Schneekugelmanufaktur",
				Address = "Schumanngasse 87",
				StartLocation = q23.TargetLocation,
				TargetLocation = new Location() { Lat = "48.2242749", Lng = "16.3310988" },
				ZoomLevel = 13,
				ToleranceDistance = 2
			};
			Unit.QuestionRepository.Insert(q24);

			
			Unit.Save();
		}
	}
}
