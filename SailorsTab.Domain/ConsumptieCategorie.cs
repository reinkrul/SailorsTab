using System;
using System.Collections.Generic;

namespace SailorsTab.Domain
{
	public class ConsumptieCategorie
	{
		public ConsumptieCategorie()
		{
		}
		
		public ConsumptieCategorie(string naam, Consumptie[] consumpties)
		{
			this.Naam = naam;
			this.Consumpties = consumpties;
		}
		
		public string Naam { get; set; }
		public Consumptie[] Consumpties { get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[ConsumptieCategorie: Naam={0}, Consumpties={1}]", Naam, Consumpties);
		}
	}
}