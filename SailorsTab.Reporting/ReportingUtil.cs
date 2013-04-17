using System;

namespace SailorsTab.Reporting
{
	public class ReportingUtil
	{
		public static string FormatDate(DateTime date)
		{
			 return string.Format("{0:dd-MM-yyyy}", date);	
		}
	}
}

