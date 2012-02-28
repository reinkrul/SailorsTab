using System;
using SailorsTab.Domain;
using Gtk;
using SailorsTab.Common;

namespace SailorsTab.Tabmin
{
	public class RekeningWindow : Window, IObjectEditor<Rekening>
	{	
		private bool initialized;
		
		public void EditObject(Rekening rekening, Action<Rekening> saveObjectFunc)
		{
			if (initialized)
			{
				throw new Exception("Object editor kan maar 1x geinitialiseerd worden.");
			}
			else
			{
				initialize(rekening, saveObjectFunc);
				initialized = true;
			}
			ShowAll();
		}
		
		private void initialize(Rekening rekening, Action<Rekening> saveObjectFunc)
		{
			Modal = true;
			
			table = new Table(3, 2, false);
			Add(table);	
			
			rekeningNaamLabel = new Label("Naam:");
			table.Attach(rekeningNaamLabel, 0, 1, 0, 1);
			
			rekeningNaamBuffer = new TextBuffer(new TextTagTable());
			rekeningNaamBuffer.Text = rekening.Naam;
			rekeningNaam = new TextView(rekeningNaamBuffer);
			table.Attach(rekeningNaam, 1, 2, 0, 1);
			
			rekeningWaardeLabel = new Label("Waarde:");
			table.Attach(rekeningWaardeLabel, 0, 1, 1, 2);
			
			rekeningWaardeBuffer = new TextBuffer(new TextTagTable());
			rekeningWaardeBuffer.Text = CurrencyUtil.FormatEuros(rekening.Waarde);
			rekeningWaarde = new TextView(rekeningWaardeBuffer);
			table.Attach(rekeningWaarde, 1, 2, 1, 2);
			
			opslaanButton = new Button("Opslaan");
			opslaanButton.Clicked += delegate {
				if (parseInto(rekening))
				{
					saveObjectFunc(rekening);
					Destroy();
				}
			};
			table.Attach(opslaanButton, 0, 1, 2, 3);
			
			annulerenButton = new Button("Annuleren");
			annulerenButton.Clicked += delegate {
				Destroy();
			};
			table.Attach(annulerenButton, 1, 2, 2, 3);
		}
		
		private Table table;
		
		private Label rekeningNaamLabel;
		private TextView rekeningNaam;
		private TextBuffer rekeningNaamBuffer;
		
		private Label rekeningWaardeLabel;
		private TextView rekeningWaarde;
		private TextBuffer rekeningWaardeBuffer;
		
		private Button opslaanButton;
		private Button annulerenButton;
		
		public RekeningWindow () : base(WindowType.Toplevel)
		{
			
		}
		
		bool parseInto(Rekening rekening)
		{
			bool parsed = true;
			
			if (string.IsNullOrEmpty(rekeningNaamBuffer.Text.Trim()))
			{
				UIHelper.ShowErrorDialog(this, "Naam is niet ingevuld.");
				parsed = false;
			}
			else
			{
				rekening.Naam = rekeningNaamBuffer.Text.Trim();
			}
			
			try
			{
				rekening.Waarde = decimal.Parse(rekeningWaardeBuffer.Text.Trim());
			}
			catch(FormatException ex)
			{
				MessageDialog messageDialog = new MessageDialog(this, DialogFlags.DestroyWithParent | DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Ongeldige waarde voor 'waarde'.");
				messageDialog.Run();
				messageDialog.Destroy();
				parsed = false;
			}
			
			return parsed;
		}
	}
}

