using System;
using System.Collections.Generic;
using Gtk;
using SailorsTab.Repositories;
using SailorsTab.Common;
using SailorsTab.Domain;

namespace SailorsTab.Windows
{
	public class MainWindow : Window
	{
		private readonly Log log = new Log(Program.LogFile, typeof(MainWindow));
		
		private IRekeningRepository rekeningRepository;
		private IConsumptieRepository consumptieRepository;
		private VBox vbox;
		private HBox box;
		private RekeningOverzichtWidget rekeningOverzichtWidget;
		
		private BestellingenOverzichtWidget bestellingenOverzichtWidget;
		
		private Rekening huidigeRekening;
		private Button afrekenenButton;
		
		public MainWindow (IRepositoryFactory repositoryFactory)
			: base(Gtk.WindowType.Toplevel)
		{
			WindowPosition = WindowPosition.Center;
			
			rekeningRepository = repositoryFactory.CreateRepository<IRekeningRepository>();
			consumptieRepository = repositoryFactory.CreateRepository<IConsumptieRepository>();
			
			// Rekeningen overzicht
			rekeningOverzichtWidget = new RekeningOverzichtWidget();
			rekeningOverzichtWidget.Refresh(rekeningRepository.GetAll());
			rekeningOverzichtWidget.RekeningClicked += handleRekeningClicked;
			
			// Afrekenen button
            Pango.FontDescription fontDescription = Pango.FontDescription.FromString("Arial");
			fontDescription.Size = 13000;
			fontDescription.Weight = Pango.Weight.Bold;
            Label afrekenenLabel = new Label("Afrekenen");
            afrekenenLabel.ModifyFont(fontDescription);
			afrekenenButton = new Button();
            afrekenenButton.Add(afrekenenLabel);
			afrekenenButton.Clicked += handleAfrekenenButtonClicked;
			
			// Rechter panel (bestellingen)
			bestellingenOverzichtWidget = new BestellingenOverzichtWidget();
			
			box = new HBox(false, 3);
			box.PackStart(rekeningOverzichtWidget);
			box.PackStart(bestellingenOverzichtWidget);
			
			// Main box
			vbox = new VBox(false, 0);
			vbox.PackStart(box);
			vbox.PackEnd(afrekenenButton);
			Add(vbox);
			
			ShowAll();
			
			this.DeleteEvent += OnDeleteEvent;
		}

		void handleAfrekenenButtonClicked (object sender, EventArgs e)
		{
			foreach(var bestelling in bestellingenOverzichtWidget.GetBestellingen())
			{
				for(int i = 0; i < bestelling.Aantal; i++)
				{
					try
					{
						rekeningRepository.ConsumptieAfrekenen(bestelling.Rekening, bestelling.Consumptie);
					}
					catch(RepositoryException ex)
					{
						log.Error(ex);
						UIHelper.ShowErrorDialog(this, "Kan de volgende bestelling niet afronden: {0}, {1}: {2}", bestelling.Rekening.Naam, bestelling.Consumptie.ToStringMetPrijs(), ex.Message);
					}
				}
			}
			
			bestellingenOverzichtWidget.Clear();
            rekeningOverzichtWidget.RefreshChildren();
		}

		void handleRekeningClicked (object sender, Rekening rekening)
		{
			huidigeRekening = rekening;	
			
			ConsumptieOverzichtWidget consumptieOverzichtWidget = new ConsumptieOverzichtWidget(this);
			consumptieOverzichtWidget.Refresh(consumptieRepository.GetAll());
			consumptieOverzichtWidget.Destroyed += handleConsumptieOverzichtWidgetClosed;
			consumptieOverzichtWidget.ShowAll();
		}

		void handleConsumptieOverzichtWidgetClosed (object sender, EventArgs e)
		{
			Consumptie consumptie = ((ConsumptieOverzichtWidget)sender).GekozenConsumptie;
			if (consumptie != null && huidigeRekening != null)
			{
				bestellingenOverzichtWidget.BestellingToevoegen(huidigeRekening, consumptie);
			}
		}
		
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
	}
}