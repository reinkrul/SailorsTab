using System;
using System.Linq;

namespace SailorsTab.Domain
{
	public class Rekening : IDomainObject
	{	
		public Rekening()
		{
			Naam = "";
			Waarde = 0;
		}
		public Rekening(string naam) : base()
		{
			this.Naam = naam;
		}
		
		public int Id { get; set; }
		public string Naam { get; set; }
		public decimal Waarde { get; set; }
		
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals (this, obj))
				return true;
			if (obj.GetType () != typeof(Rekening))
				return false;
			SailorsTab.Domain.Rekening other = (SailorsTab.Domain.Rekening)obj;
			return Id == other.Id && Naam == other.Naam && Waarde == other.Waarde;
		}


		public override int GetHashCode ()
		{
			return Id.GetHashCode () ^ (Naam != null ? Naam.GetHashCode () : 0) ^ Waarde.GetHashCode ();
		}
		
		public bool Validate()
		{
			if (string.IsNullOrEmpty(Naam))
			{
				return false;
			}
			
			return true;
		}
		
		public override string ToString ()
		{
			return string.Format ("[Rekening: Id={0}, Naam={1}, Waarde={2}]", Id, Naam, Waarde);
		}
	}
}

