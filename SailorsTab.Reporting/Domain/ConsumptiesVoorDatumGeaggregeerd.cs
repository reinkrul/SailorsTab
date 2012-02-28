using System;
using System.Collections.Generic;

using SailorsTab.Domain;

namespace SailorsTab.Reporting.Domain
{
	public class ConsumptiesVoorDatumGeaggregeerd : IEnumerable<Item>
	{
		public class Item
		{
			public String Consumptie {
				get;
				set;
			}
			public int Aantal {
				get;
				set;
			}
		}
		
		public DateTime Datum { get; private set; }
		
		private IEnumerable<Item> consumpties;
		
		public ConsumptiesVoorDatum (DateTime datum, IEnumerable<ConsumptieLog> consumpties)
		{
			Datum = datum;
			this.consumpties = consumpties;
		}
		
		
		#region IEnumerable[ConsumptieLog] implementation
		public IEnumerator<ConsumptieLog> GetEnumerator ()
		{
			return consumpties.GetEnumerator();
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return consumpties.GetEnumerator();
		}
		#endregion
	}
}

