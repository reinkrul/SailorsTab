using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SailorsTab.Common
{
    public class OverzichtWidgetColumn
    {
        public OverzichtWidgetColumn(string property, float alignment)
        {
            Property = property;
            Alignment = alignment;
        }
        public OverzichtWidgetColumn(string property, float alignment, String format)
        {
            Property = property;
            Alignment = alignment;
            Format = format;
        }

        // Properties
        public string Property { get; set; }
        public float Alignment { get; set; }
        public string Format { get; set; }
    }

}
