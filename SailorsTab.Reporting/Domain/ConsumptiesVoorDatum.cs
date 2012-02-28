using System;
using System.Collections.Generic;

using SailorsTab.Domain;

namespace SailorsTab.Reporting.Domain
{
	public class ConsumptiesVoorDatum : IEnumerable<ConsumptieLog>
	{
		public DateTime Datum { get; private set; }
		
		private IEnumerable<ConsumptieLog> consumpties;
		
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

