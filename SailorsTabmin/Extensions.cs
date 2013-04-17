using System;
using Gtk;
using SailorsTab.Domain;

namespace SailorsTab.Tabmin
{
	public static class Extensions
	{
		public static void Populate(this ComboBox comboBox, System.Object[] items)
		{
			comboBox.Clear();
			// types of ListStore columns are taken from alValuesList
			ListStore listStore = new Gtk.ListStore(typeof(string));
			comboBox.Model = listStore;
			CellRendererText text = new CellRendererText();
			comboBox.PackStart(text, true);
			comboBox.AddAttribute(text, "text", 0);

			foreach (var item in items)
			{
				listStore.AppendValues(item.ToString());
			}
		}
	}
}

