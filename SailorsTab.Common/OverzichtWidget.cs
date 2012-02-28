using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SailorsTab.Common
{
    public class OverzichtWidget<T> : Gtk.Box
    {
        // Fields
        private Gtk.Table table;
        private OverzichtWidgetColumn[] columns;

        public OverzichtWidget(params OverzichtWidgetColumn[] columns)
        {
            this.columns = columns;
        }

        private void addObject(T obj, uint rowIndex)
        {
            for (uint i = 0; i < columns.Length; i++)
            {
                OverzichtWidgetColumn overzichtwidgetcolumn1 = columns[i];
                this.addObjectColumn(obj, overzichtwidgetcolumn1, rowIndex, i);
            }
        }

        private void addObjectColumn(T obj, OverzichtWidgetColumn column, uint rowIndex, uint columnIndex)
        {
            object value = typeof(T).GetProperty(column.Property).GetValue(obj, null);
            Gtk.Label label1 = new Gtk.Label();
            label1.SetAlignment(0, 0.5f);
            if (string.IsNullOrEmpty(column.Format))
            {
                if (value == null)
                {
                    label1.Text = "null";
                }
                else
                {
                    label1.Text = value.ToString();
                }
            }
            else
            {
                label1.Text = string.Format(column.Format, value);
            }

            this.table.Attach(label1, columnIndex, (columnIndex + 1), rowIndex, (rowIndex + 1));
        }


        public void Refresh(T[] objects)
        {
            if (this.table != null)
            {
                this.Remove(this.table);
                this.table.Destroy();
            }
            this.table = new Gtk.Table((uint)objects.Length, (uint)this.columns.Length, false);
            this.Add(this.table);
            for (uint i = 0; i < objects.Length; i++)
            {
                this.addObject(objects[i], i);
            }
            this.ShowAll();
        }
    }
}