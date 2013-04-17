using System;
using SailorsTab.Domain;

namespace SailorsTab
{
	public class ConsumptieWidget : Gtk.Button
	{
		private Consumptie consumptie;
		
		public ConsumptieWidget (Consumptie consumptie)
		{
			this.consumptie = consumptie;
			Label = consumptie.ToStringMetPrijs();
		}
		
		public Consumptie Consumptie {
			get { return consumptie; }
		}
	}
}

