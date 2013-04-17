using System;
using System.Collections.Generic;
using Gtk;
using SailorsTab.Domain;
using SailorsTab.Repositories;

namespace SailorsTab
{
	public class ConsumptieOverzichtWidget : Window
	{	
		private Table table;
		
		public ConsumptieOverzichtWidget(Window parent)
			: base("Consumpties") {
			TransientFor = parent;
			base.WindowPosition = WindowPosition.CenterOnParent;
			Modal = true;
		}
		
		public void Refresh(Consumptie[] consumpties) {
			uint columns = (uint)Math.Ceiling(Math.Sqrt((double)consumpties.Length));
			uint rows = (uint)Math.Floor(Math.Sqrt((double)consumpties.Length));
			table = new Table(rows, columns, true);
			Add(table);
			
			uint index = 0;
			foreach(var consumptie in consumpties) {
				ConsumptieWidget consumptieWidget = new ConsumptieWidget(consumptie);
				consumptieWidget.Clicked += handleConsumptieWidgetClicked;
				
				// Bereken consumptie widget posities
				uint thisColumn = index % columns;
				uint thisRow = (index - thisColumn) / columns;
				table.Attach(consumptieWidget, thisColumn, thisColumn + 1, thisRow, thisRow + 1);
				
				index++;
			}
		}

		void handleConsumptieWidgetClicked (object sender, EventArgs e)
		{
			GekozenConsumptie = ((ConsumptieWidget)sender).Consumptie;
			Destroy();
		}
		
		public Consumptie GekozenConsumptie{ get; private set; }
	}
}

