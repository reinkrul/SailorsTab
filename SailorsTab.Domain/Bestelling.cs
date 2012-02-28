using System;
using System.Collections.Generic;

namespace SailorsTab.Domain
{
	public class Bestelling
	{
		public Bestelling(Rekening rekening, Consumptie consumptie)
		{
			Rekening = rekening;
			Consumptie = consumptie;
			Aantal = 1;
		}
		
		public Rekening Rekening { get; set; }
		public Consumptie Consumptie { get; set; }
		
		public int Aantal { get; set; }
		
		public override string ToString ()
		{
			return string.Format("{0} - {1} {2}x", Rekening.Naam, Consumptie.ToStringMetPrijs(), Aantal);
		}
	}
}

