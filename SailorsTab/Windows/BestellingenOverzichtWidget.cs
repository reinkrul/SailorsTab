using System;
using System.Collections.Generic;
using Gtk;
using SailorsTab.Domain;

namespace SailorsTab
{
	public class BestellingenOverzichtWidget : TreeView
	{	
		public BestellingenOverzichtWidget()
		{
			HeightRequest = 300;
			WidthRequest = 200;
			
			// Model
			Model = new ListStore(typeof(string), typeof(Bestelling));
			
			// Renderer
			CellRendererText defaultColumnRenderer = new CellRendererText();
			
			// Columns
			TreeViewColumn defaultColumn = new TreeViewColumn();
			defaultColumn.PackStart(defaultColumnRenderer, true);
			defaultColumn.Title = "Bestelling";
			defaultColumn.AddAttribute(defaultColumnRenderer, "text", 0);
			AppendColumn(defaultColumn);
			
			// Bestellig verwijderen
			KeyPressEvent += delegate(object o, KeyPressEventArgs args) {
				if (args.Event.Key == Gdk.Key.BackSpace || args.Event.Key == Gdk.Key.Delete)
				{
					Selection.SelectedForeach((model, path, iter) => ((ListStore)Model).Remove(ref iter));
				}
			};
		}

		public void BestellingToevoegen(Rekening rekening, Consumptie consumptie)
		{
			Bestelling bestelling = findBestelling(rekening, consumptie);
			if (bestelling == null)
			{
				bestelling = new Bestelling(rekening, consumptie);
				((ListStore)Model).AppendValues(bestelling.ToString(), bestelling);
			}
			else
			{
				bestelling.Aantal++;
				Model.SetValue(find(bestelling), 0, bestelling.ToString());
			}
		}
		
		public Bestelling[] GetBestellingen()
		{
			List<Bestelling> bestellingen = new List<Bestelling>();
			iterate((bestelling) => bestellingen.Add(bestelling));
			
			return bestellingen.ToArray();
		}
		
		public void Clear()
		{
			((ListStore)Model).Clear();
		}
		
		private void iterate(Action<Bestelling, TreeIter> action)
		{
			TreeIter iter;
			Model.GetIterFirst(out iter);
			
			do
			{
				object val = Model.GetValue(iter, 1);
				if (val != null)
				{
					action((Bestelling)val, iter);
				}
			} while(Model.IterNext(ref iter));
		}
		
		private void iterate(Action<Bestelling> action)
		{
            iterate((bestelling, treeIter) => { action(bestelling); });
		}
		
		private TreeIter find(Bestelling bestelling)
		{
			TreeIter iter;
			Model.GetIterFirst(out iter);
			
			do
			{
				object val = Model.GetValue(iter, 1);
				if (val != null && val.Equals(bestelling))
				{
					return iter;
				}
			} while(Model.IterNext(ref iter));
			
			return TreeIter.Zero;
		}
		
		private Bestelling findBestelling(Rekening rekening, Consumptie consumptie)
		{	
			TreeIter iter;
			Model.GetIterFirst(out iter);
			
			do
			{
				Bestelling bestelling = (Bestelling)Model.GetValue(iter, 1);
				if (bestelling != null && bestelling.Rekening.Equals(rekening) && bestelling.Consumptie.Equals(consumptie))
				{
					return bestelling;
				}
			} while(Model.IterNext(ref iter));
			
			return null;
		}
	}
}

