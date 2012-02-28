using System;
using SailorsTab.Domain;

namespace SailorsTab.Tabmin
{
	public class ConsumptieEditorWindow : ObjectEditorWindow<Consumptie>
	{
		protected override string[] GetIncludedProperties ()
		{
			return new string[] { "Naam", "Prijs", "Categorie" };
		}
		
		protected override bool ValidateValue (string property, object value)
		{
			bool result = true;
			
			switch(property.ToLower())
			{
				case "prijs":
					double d;
					result = double.TryParse(value.ToString(), out d);
					break;
				case "naam":
					result = !string.IsNullOrEmpty(value.ToString().Trim());
					break;
			}
			
			return result;
		}
	}
}

