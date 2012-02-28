using System;
using System.Globalization;
namespace SailorsTab.Common
{
	public class CurrencyUtil
	{
		public static string FormatEuros(decimal value)
		{
            return string.Format("{0:C}", value);
		}
	}
}

