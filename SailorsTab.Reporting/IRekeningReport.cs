using System;
using System.IO;
using System.Collections.Generic;

using SailorsTab.Domain;

namespace SailorsTab.Reporting
{
	public interface IRekeningReport
	{
		void Generate(Stream outputStream, Rekening rekening, IList<ConsumptieLog> consumpties);
	}
}

