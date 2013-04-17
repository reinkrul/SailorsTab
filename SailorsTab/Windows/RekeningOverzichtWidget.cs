using System;
using Gtk;
using SailorsTab.Domain;

namespace SailorsTab.Windows
{
	public class RekeningOverzichtWidget : Box
	{
		private Table table;
		
		public event Action<object, Rekening> RekeningClicked;

        public void RefreshChildren()
        {
            foreach (RekeningWidget widget in table.AllChildren)
            {
                widget.Refresh();
            }
        }

		public void Refresh(Rekening[] rekeningen)
		{
			if (table != null)
			{
				Remove(table);
			}
			
			// Bereken afmetingen van de tabel
			int aantalRekeningen = rekeningen.Length;
			uint columns = (uint)Math.Ceiling(Math.Sqrt((double)aantalRekeningen));
			uint rows = (uint)Math.Floor(Math.Sqrt((double)aantalRekeningen));
			table = new Table(rows, columns, true);
			
			uint index = 0;
			foreach(var rekening in rekeningen)
			{
				RekeningWidget rekeningWidget = new RekeningWidget(rekening);
				rekeningWidget.Clicked += handleRekeningWidgetClicked;
				
				// Bereken rekening widget posities
				uint thisColumn = index % columns;
				uint thisRow = (index - thisColumn) / columns;
				table.Attach(rekeningWidget, thisColumn, thisColumn + 1, thisRow, thisRow + 1); 
				                  
				index++;
			}
			
			Add(table);
		}
		
		void handleRekeningWidgetClicked (object sender, EventArgs e)
		{
			if (!(sender is RekeningWidget)) {
				throw new Exception("Expected sender to be of type RekeningWidget.");
			}
			
			if (RekeningClicked != null)
			{
				RekeningClicked(this,  ((RekeningWidget)sender).Rekening);
			}
		}
	}
}

