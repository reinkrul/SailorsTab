using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using SailorsTab.Domain;

namespace SailorsTab.Reporting.Html
{
	[TestFixture]
	public class HtmlRekeningReportTest
	{
		[Test]
		public void TestGenerate()
		{
			MemoryStream stream = new MemoryStream();
			
			Rekening rekening = new Rekening("Foo");
			rekening.Waarde = 24.3m;
			
			List<ConsumptieLog> consumpties = new List<ConsumptieLog>();
			consumpties.Add (new ConsumptieLog("Cola", new DateTime(2012, 1, 1)));
			consumpties.Add (new ConsumptieLog("Cola", new DateTime(2012, 1, 1)));
			consumpties.Add (new ConsumptieLog("Cola", new DateTime(2012, 1, 1)));
			consumpties.Add (new ConsumptieLog("Cola", new DateTime(2012, 1, 1)));
			consumpties.Add (new ConsumptieLog("Bier", new DateTime(2012, 1, 1)));
			consumpties.Add (new ConsumptieLog("Bier", new DateTime(2012, 1, 1)));
			consumpties.Add (new ConsumptieLog("Cola", new DateTime(2012, 1, 7)));
			consumpties.Add (new ConsumptieLog("Bier", new DateTime(2012, 1, 7)));
			consumpties.Add (new ConsumptieLog("Chips", new DateTime(2012, 1, 7)));
			consumpties.Add (new ConsumptieLog("Chips", new DateTime(2012, 2, 7)));
			consumpties.Add (new ConsumptieLog("Chips", new DateTime(2012, 2, 7)));
			consumpties.Add (new ConsumptieLog("Bier", new DateTime(2012, 2, 7)));
			consumpties.Add (new ConsumptieLog("Chips", new DateTime(2012, 2, 7)));
			consumpties.Add (new ConsumptieLog("Bier", new DateTime(2012, 2, 7)));
			consumpties.Add (new ConsumptieLog("Cola", new DateTime(2012, 2, 7)));
			
			HtmlRekeningReport report = new HtmlRekeningReport();
			report.Generate(stream, rekening, consumpties);
			
			string html = UTF8Encoding.UTF8.GetString(stream.GetBuffer());
			Console.WriteLine ("HTML report:\r\n" + html);
		}
	}
}

