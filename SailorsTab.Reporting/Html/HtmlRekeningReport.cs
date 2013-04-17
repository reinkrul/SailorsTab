using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SailorsTab.Domain;

namespace SailorsTab.Reporting
{
	public class HtmlRekeningReport : IRekeningReport
	{
		#region IRekeningReport implementation
		public void Generate (Stream outputStream, Rekening rekening, IList<ConsumptieLog> consumpties)
		{
			TextWriter writer = new StreamWriter(outputStream);
			writeDocumentStart(writer, rekening);
			writeReportHeader(writer, rekening, consumpties);
            writeConsumptieTable(writer, consumpties);
			writeDocumentEnd(writer);
			writer.Flush();
		}
		#endregion
		
		void writeDocumentStart(TextWriter writer, Rekening rekening) {
			writer.WriteLine("<!DOCTYPE html>");
			writer.WriteLine("<html>");
			writer.WriteLine("	<head>");
			writer.WriteLine("		<title>Overzicht voor rekening '" + rekening.Naam + "'</title>");
			writer.WriteLine("	</head>");
			writer.WriteLine("	<body>");
		}
		
		void writeDocumentEnd(TextWriter writer) {
			writer.WriteLine("	</body>");
			writer.WriteLine("</html>");
		}
		
		void writeReportHeader(TextWriter writer, Rekening rekening, IList<ConsumptieLog> consumpties) {
			string sinds = "";
			if (consumpties.Count > 0) {
				sinds = " sinds " + ReportingUtil.FormatDate(consumpties[0].Datum);
			}
			writer.WriteLine("		<h1>Overzicht voor rekening: '" + rekening.Naam + "'" + sinds + "</h1>");
		}
		
		void writeConsumptieTable(TextWriter writer, IList<ConsumptieLog> consumpties) {
			writer.WriteLine("		<table>");
			writer.WriteLine("			<tr><th>Datum</th><th>Consumptie</th><th>Aantal</th>");
            foreach (ConsumptieLog log in consumpties.OrderByDescending(c => c.Datum))
            {
                writer.WriteLine("<tr><td>" + log.Datum + "</td><td>" + log.ConsumptieNaam + "</td><td>1</td></tr>");
            }
			writer.WriteLine("		</table>");
		}
	}
}

