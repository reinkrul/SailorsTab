using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using SailorsTab.Common;


namespace SailorsTab.Tabmin
{
	public class ObjectEditorWindow<T> : Window, IObjectEditor<T>
	{
		private bool initialized;
		private SortedDictionary<String, Property> properties = new SortedDictionary<String, Property>();
		
		private T currentObject;
		
		private Table table;
		private Button opslaanButton;
		private Button annulerenButton;
		
		public ObjectEditorWindow() : base(WindowType.Toplevel) { }
		
		public void EditObject(T obj, Action<T> saveObjectFunc)
		{
			lock(this)
			{
				if (initialized)
				{
					throw new InvalidOperationException("ObjectEditorWindow has already been initialized.");
				}
				
				Initialize(obj, saveObjectFunc);
				initialized = true;
			}
			
			ShowAll();
		}
		
		protected void Initialize(T obj, Action<T> saveObjectFunc)
		{
			currentObject = obj;
			loadProperties(obj);
			
			Modal = true;
			
			table = new Table((uint)properties.Count + 1, 2, false);
			
			uint index = 0;
			foreach(Property property in properties.Values)
			{
				table.Attach(property.Label, 0, 1, index, index + 1);
				table.Attach(property.GetWidget(), 1, 2, index, index + 1);
					
				index++;
			}
			
			opslaanButton = new Button("Opslaan");
			opslaanButton.Clicked += delegate {
				foreach(string name in properties.Keys)
				{
					if (!ValidateValue(name, properties[name].GetValue()))
					{
						UIHelper.ShowErrorDialog(this, "De waarde van '{0}' is ongeldig", name);
						return;
					}
				}
				
				try
				{
					updateObject();
				}
				catch(Exception ex)
				{
					UIHelper.ShowErrorDialog(this, "Kan properties niet parsen: " + ex.Message);
					return;
				}
				
				saveObjectFunc(obj);
				Destroy();
			};
			table.Attach(opslaanButton, 0, 1, index + 1, index + 2);
			
			annulerenButton = new Button("Annuleren");
			annulerenButton.Clicked += delegate {
				Destroy();
			};
			table.Attach(annulerenButton, 1, 2, index + 1, index + 2);
			
			Add(table);
		}
		
		protected virtual string[] GetIncludedProperties()
		{
			// To be overriden
			return null;
		}
		protected virtual bool ValidateValue(string property, object value)
		{
			// To be overriden
			return true;
		}
		
		private void updateObject()
		{
			foreach(string name in properties.Keys)
			{
				object value = parseValue(properties[name].GetValue().ToString(), properties[name].PropertyInfo.PropertyType);
				properties[name].PropertyInfo.SetValue(currentObject, value, null);
			}
		}
		
		private object parseValue(string value, Type requestedType) 
		{
			if (requestedType == typeof(double))
			{
				return double.Parse(value);
			}
			else if (requestedType == typeof(string))
			{
				return value;
			}
			else if (requestedType == typeof(decimal))
			{
				return decimal.Parse(value);
			}
			else if (requestedType == typeof(int))
			{
				return int.Parse(value);
			}
			else
			{
				throw new NotSupportedException("Cannot convert value to type '" + requestedType + "', not supported");
			}
		}
		
		private void loadProperties(T obj)
		{
			PropertyInfo[] propertiesInfo = obj.GetType().GetProperties();
			string[] includedProperties = GetIncludedProperties();
			foreach(var propertyInfo in propertiesInfo)
			{
				if (includedProperties != null && includedProperties.Contains(propertyInfo.Name))
				{
					Property property = new TextProperty(propertyInfo);
					property.SetValue(propertyInfo.GetValue(obj, null));
					
					properties.Add(propertyInfo.Name, property);
				}
			}
		}
		
		private abstract class Property
		{
			public PropertyInfo PropertyInfo { get; private set; }
			public Label Label { get; private set; }
			
			public Property(PropertyInfo propertyInfo)
			{
				PropertyInfo = propertyInfo;
				Label = new Label();
				Label.Text = propertyInfo.Name;
			}
			
			public String LabelText
			{
				get { return Label.Text; }
				set { Label.Text = value; }
			}
			
			public abstract void SetValue(object value);
			public abstract object GetValue();
			public abstract Widget GetWidget();
		}
		private class TextProperty : Property
		{
			public TextView Field { get; private set; }
			
			public TextProperty(PropertyInfo propertyInfo) : base(propertyInfo)
			{
				TextBuffer buffer = new TextBuffer(new TextTagTable());
				Field = new TextView(buffer);
				Field.Buffer.Text = "";
			}
			
			public override void SetValue (object value)
			{
				Field.Buffer.Text = value == null ? "" : value.ToString();
			}
			public override object GetValue ()
			{
				return Field.Buffer.Text;
			}
			public override Widget GetWidget()
			{
				return Field;
			}
		}
	}
}

