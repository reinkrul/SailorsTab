using System;
using Gtk;
using System.Drawing;
using SailorsTab.Common;

namespace SailorsTab.Windows
{
	public partial class RekeningWidget : Button
	{
        public void Refresh()
        {
            label.Text = rekening.Naam + "  (" + CurrencyUtil.FormatEuros(rekening.Waarde) + ")";
        }
	}
}

