using System;
namespace SailorsTab
{
	public class ConsumptieLog
	{
		public int RekeningId { get; set; }
		public string RekeningNaam { get; set; }
		public int ConsumptieId { get; set; }
		public string ConsumptieNaam { get; set; }
		public DateTime Datum { get; set; }

		// Voor testdoeleinden
		public ConsumptieLog (string consumptieNaam, DateTime datum)
		{
			this.ConsumptieNaam = consumptieNaam;
			this.Datum = datum;
		}
		
		public ConsumptieLog ()
		{
		}
		
		public override string ToString ()
		{
			return string.Format ("[ConsumptieLog: RekeningId={0}, RekeningNaam={1}, ConsumptieId={2}, ConsumptieNaam={3}, Datum={4}]", RekeningId, RekeningNaam, ConsumptieId, ConsumptieNaam, Datum);
		}
	}
}

