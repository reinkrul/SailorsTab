using System;
using SailorsTab.Common;

namespace SailorsTab.Domain
{
	public class Consumptie : IDomainObject
	{
		public int Id { get; set; }
		public string Naam { get; set; }
		public decimal Prijs { get; set; }
		public string Categorie { get; set; }
		
		public bool Validate()
		{
			if (string.IsNullOrEmpty(Naam))
			{
				return false;
			}
			
			return true;
		}
		
		public override string ToString()
		{
            return string.Format("[Consumptie: Id={0}, Naam={1}, Prijs={2}]", Id, Naam, CurrencyUtil.FormatEuros(Prijs));
		}
		
		public string ToStringMetPrijs()
		{
			return string.Format("{0} ({1})", Naam, CurrencyUtil.FormatEuros(Prijs));
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals (this, obj))
				return true;
			if (obj.GetType () != typeof(Consumptie))
				return false;
			Consumptie other = (Consumptie)obj;
			return Id == other.Id && Naam == other.Naam && Prijs == other.Prijs && Categorie == other.Categorie;
		}
		
		public override int GetHashCode()
		{
			return Id.GetHashCode () ^ (Naam != null ? Naam.GetHashCode () : 0) ^ Prijs.GetHashCode () ^ (Categorie != null ? Categorie.GetHashCode () : 0);
		}
	}
}

