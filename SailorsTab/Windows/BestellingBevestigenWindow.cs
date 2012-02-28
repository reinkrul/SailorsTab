using System;
using System.Linq;
using System.Collections.Generic;
using Gtk;
using SailorsTab.Domain;

namespace SailorsTab
{
	public class BestellingBevestigenWindow : Window
	{
		private VBox box;
		private TextView overzicht;
		
		public BestellingBevestigenWindow (IEnumerable<Bestelling> bestellingen)
			: base(Gtk.WindowType.Toplevel)
		{
			box = new VBox(false, 3);
			overzicht = new TextView(new TextBuffer(new TextTagTable()));
			overzicht.Buffer.Text = formatBestellingOverzicht(bestellingen);
			Add(box);
		}
		
		private string formatBestellingOverzicht(IEnumerable<Bestelling> bestellingen)
		{
			string result = string.Join(Environment.NewLine, bestellingen.Select(b => b.ToString()).ToArray());
			result += Environment.NewLine + "----------------------------------" + Environment.NewLine;
			result += ((decimal)bestellingen.Sum(b => ((decimal)b.Aantal * b.Consumptie.Prijs) * 100)) / 100;
			// TODO: Util for formatting currency
			return result;
		}
	}
}

