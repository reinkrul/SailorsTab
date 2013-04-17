using System;
using System.Linq;
using Gtk;
using SailorsTab.Common;
using SailorsTab.Domain;
using SailorsTab.Repositories;

namespace SailorsTab.Tabmin
{
	public class MainWindow : Gtk.Window
	{
		private readonly Log log = new Log(Program.LogFile, typeof(MainWindow));
		
		private IRekeningRepository rekeningRepository;
		private IConsumptieRepository consumptieRepository;
		
		private VPaned windowPaned;
		
		private Table rekeningenTable;
		private ObjectManagerWidget<Rekening> rekeningenManager;
		private OverzichtWidget<Rekening> rekeningenOverzicht;
		
		private Table consumptiesTable;
		private ObjectManagerWidget<Consumptie> consumptiesManager;
		private OverzichtWidget<Consumptie> consumptiesOverzicht;

		public MainWindow (IRepositoryFactory repositoryFactory) : base(Gtk.WindowType.Toplevel)
		{
			Resizable = true;
			
			this.rekeningRepository = repositoryFactory.CreateRepository<IRekeningRepository>();
			this.consumptieRepository = repositoryFactory.CreateRepository<IConsumptieRepository>();
			
			this.DeleteEvent += OnDeleteEvent;
			
			windowPaned = new VPaned();
			Add(windowPaned);
			
			initializeRekeningen(rekeningRepository);
			initializeConsumpties(consumptieRepository);
			
			ShowAll();
		}
		
		void initializeRekeningen(IRekeningRepository rekeningRepository)
		{
			rekeningenTable = new Table(2, 1, false);
			windowPaned.Add1(rekeningenTable);			
			
			rekeningenManager = new ObjectManagerWidget<Rekening>(typeof(RekeningWindow));
			rekeningenManager.GetObjectsFunc = delegate {
				return rekeningRepository.GetAll();
			};
			rekeningenManager.GetObjectTitleFunc = delegate(Rekening rekening) {
				return rekening.Naam;
			};
			rekeningenManager.DeleteObjectFunc = delegate(Rekening rekening) {
				try
				{
					rekeningRepository.Delete(rekening.Naam);
				}
				catch(Exception ex)
				{
					log.Error(ex);
					UIHelper.ShowErrorDialog(this, "Kan rekening '{0}' niet verwijderen: " + ex.Message, rekening.Naam);
				}
				OnRefreshRekeningen();
			};
			rekeningenManager.UpdateObjectFunc = delegate(Rekening rekening) {
				try
				{
					rekeningRepository.Update(rekening);
				}
				catch(Exception ex)
				{
					log.Error(ex);
					UIHelper.ShowErrorDialog(this, "Kan rekening '{0}' niet wijzigen: " + ex.Message, rekening.Naam);
				}
				OnRefreshRekeningen();
			};
			rekeningenManager.AddObjectFunc = delegate(Rekening rekening) {
				try
				{
					rekeningRepository.Create(rekening);
				}
				catch(Exception ex)
				{
					log.Error(ex);
					UIHelper.ShowErrorDialog(this, "Kan rekening '{0}' niet toevoegen: " + ex.Message, rekening.Naam);
				}
				OnRefreshRekeningen();
			};
			rekeningenTable.Attach(rekeningenManager, 0, 1, 0, 1);

            rekeningenOverzicht = new OverzichtWidget<Rekening>(new OverzichtWidgetColumn("Naam", 0, "{0}:"), new OverzichtWidgetColumn("Waarde", 1, "{0:C}"));
			rekeningenTable.Attach(rekeningenOverzicht, 0, 1, 1, 2);
			OnRefreshRekeningen();
			rekeningenManager.Refresh();
		}
		
		void initializeConsumpties(IConsumptieRepository consumptieRepository)
		{
			consumptiesTable = new Table(2, 1, false);
			windowPaned.Add2(consumptiesTable);
			
			consumptiesManager = new ObjectManagerWidget<Consumptie>(typeof(ConsumptieEditorWindow));
			consumptiesManager.GetObjectsFunc = delegate {
				return consumptieRepository.GetAll();
			};
			consumptiesManager.GetObjectTitleFunc = delegate(Consumptie consumptie) {
				return consumptie.Naam;
			};
			consumptiesManager.DeleteObjectFunc = delegate(Consumptie consumptie) {
				try
				{
					consumptieRepository.Delete(consumptie);
				}
				catch(Exception ex)
				{
					log.Error(ex);
					UIHelper.ShowErrorDialog(this, "Kan consumptie '{0}' niet verwijderen: " + ex.Message, consumptie.Naam);
				}
				OnRefreshConsumpties();
			};
			consumptiesManager.UpdateObjectFunc = delegate(Consumptie consumptie) {
				try
				{
					consumptieRepository.Update(consumptie);
				}
				catch(Exception ex)
				{
					log.Error(ex);
					UIHelper.ShowErrorDialog(this, "Kan consumptie '{0}' niet wijzigen: " + ex.Message, consumptie.Naam);
				}
				OnRefreshConsumpties();
			};
			consumptiesManager.AddObjectFunc = delegate(Consumptie consumptie) {
				try
				{
					consumptieRepository.Create(consumptie);
				}
				catch(Exception ex)
				{
					log.Error(ex);
					UIHelper.ShowErrorDialog(this, "Kan consumptie '{0}' niet toevoegen: " + ex.Message, consumptie.Naam);
				}
				OnRefreshConsumpties();
			};
			consumptiesTable.Attach(consumptiesManager, 0, 1, 0, 1);

            consumptiesOverzicht = new OverzichtWidget<Consumptie>(new OverzichtWidgetColumn("Naam", 0, "{0}:"), new OverzichtWidgetColumn("Prijs", 1, "{0:C}"));
			consumptiesTable.Attach(consumptiesOverzicht, 0, 1, 1, 2);
			
			OnRefreshConsumpties();
			consumptiesManager.Refresh();
		}
		
		protected void OnRefreshRekeningen()
		{
			try
			{
				rekeningenOverzicht.Refresh(rekeningRepository.GetAll());
			}
			catch(Exception ex)
			{
				log.Error(ex);
				UIHelper.ShowErrorDialog(this, "Kan rekeningen niet ophalen: " + ex.Message);
			}
		}
		
		protected void OnRefreshConsumpties()
		{
			try
			{
				consumptiesOverzicht.Refresh(consumptieRepository.GetAll());
			}
			catch(Exception ex)
			{
				log.Error(ex);
				UIHelper.ShowErrorDialog(this, "Kan consumpties niet ophalen: " + ex.Message);
			}
		}
		
		// Quit the app when the user clicks on the window close button
		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}
	}
}