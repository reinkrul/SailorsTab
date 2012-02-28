using System;
using System.Collections.Generic;

using SailorsTab.Domain;

namespace SailorsTab.Reporting.Domain
{
	public class ConsumptiesOpDatumGeaggregeerd : IEnumerable<ConsumptiesVoorDatum>
	{
		private List<ConsumptiesVoorDatum> consumptiesVoorDatum; 
		
		public ConsumptiesOpDatumGeaggregeerd (IEnumerable<ConsumptiesOpDatum> consumpties)
		{
			Dictionary<DateTime, List<ConsumptieLog>> map = new Dictionary<DateTime, List<ConsumptieLog>>();
			foreach(ConsumptieLog consumptie in consumpties) {
				DateTime date = consumptie.Datum.Date;
				
				List<ConsumptieLog> list;
				if (!map.TryGetValue(date, out list)) {
					list = new List<ConsumptieLog>();
					map[date] = list;
				}
				list.Add (consumptie);
			}
			
			consumptiesVoorDatum = new List<ConsumptiesVoorDatum>();
			foreach(List<ConsumptieLog> c in map.Values) {
				consumptiesVoorDatum.Add (new ConsumptiesVoorDatum(c[0].Datum.Date, c));
			}
		}

		#region IEnumerable[ConsumptiesVoorDatum] implementation
		public IEnumerator<ConsumptiesVoorDatum> GetEnumerator ()
		{
			return consumptiesVoorDatum.GetEnumerator();
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return consumptiesVoorDatum.GetEnumerator();
		}
		#endregion
	}
}

