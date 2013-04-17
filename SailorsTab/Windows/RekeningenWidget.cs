using System;
using System.Collections.Generic;
using Gtk;
using SailorsTab.Repositories;
using SailorsTab.Domain;

namespace SailorsTab.Windows
{
	public class RekeningenWidget : Widget
	{
		private IRekeningRepository rekeningRepository;
		private List<RekeningWidget> rekeningWidgets;
		
		public RekeningenWidget (IRekeningRepository rekeningRepository)
		{
			this.rekeningRepository = rekeningRepository;
			this.rekeningWidgets = new List<RekeningWidget>();
		}
		
		public void Refresh()
		{
			foreach(var rekening in rekeningRepository.GetAll())
			{
				//RekeningWidget rekeningWidget = new RekeningWidget(rekening);
				//rekeningWidgets.Add(rekeningWidget);
			}
		}
	}
}

