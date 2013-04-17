using System;
using System.Collections.Generic;
using Gtk;
using SailorsTab.Common;

namespace SailorsTab.Tabmin
{
	public class ObjectManagerWidget<T> : Table
	{
		private Dictionary<string, T> objectMap;
		private ComboBox comboObjects;
		
		private Button wijzigenButton;
		private Button verwijderenButton;
		private Button toevoegenButton;
		
		private Type objectEditorType;
		
		public Func<T[]> GetObjectsFunc { get; set; }
		public Func<T, string> GetObjectTitleFunc { get; set; }
		public Action<T> DeleteObjectFunc { get; set; }
		public Action<T> UpdateObjectFunc { get; set; }
		public Action<T> AddObjectFunc { get; set; }
		
		public ObjectManagerWidget (Type objectEditorType) : base(3, 3, false)
		{
			objectMap = new Dictionary<string, T>();
			
			this.objectEditorType = objectEditorType;
			
			comboObjects = new ComboBox();
			comboObjects.SetSizeRequest(300, -1);
			comboObjects.Model = new ListStore(typeof(T));
			comboObjects.Changed += HandleComboOjectsChanged;
			Attach(comboObjects, 0, 3, 1, 2);
			
			toevoegenButton = new Button();
			toevoegenButton.Label = "Toevoegen";
			toevoegenButton.Clicked += HandleToevoegenButtonClicked;
			Attach(toevoegenButton, 0, 1, 2, 3);
			
			wijzigenButton = new Button();
			wijzigenButton.Label = "Wijzigen";
			wijzigenButton.Clicked += HandleWijzigenButtonClicked;
			Attach(wijzigenButton, 1, 2, 2, 3);
			
			verwijderenButton = new Button();
			verwijderenButton.Label = "Verwijderen";
			verwijderenButton.Clicked += HandleVerwijderenButtonClicked;
			Attach(verwijderenButton, 2, 3, 2, 3);
			
			setRekeningSelected(false);
		}

		public void Refresh()
		{
			setRekeningSelected(false);
			T[] items = GetObjectsFunc();
			
			comboObjects.Clear();
			objectMap.Clear();
			ListStore objectStore = new ListStore(typeof(string));
			comboObjects.Model = objectStore;
			CellRendererText textRenderer = new CellRendererText();
			comboObjects.PackStart(textRenderer, true);
			comboObjects.AddAttribute(textRenderer, "text", 0);
			
			foreach(var item in items)
			{
				string title = GetObjectTitleFunc(item);
				
				if (objectMap.ContainsKey(title))
				{
					throw new Exception("Kan niet 2 objecten met dezelfde titel toevoegen");
				}
				objectMap[title] = item;
				
				objectStore.AppendValues(title);
			}
			
			ShowAll();
		}

		void HandleVerwijderenButtonClicked (object sender, EventArgs e)
		{
			string title = comboObjects.ActiveText;
			
			int dialogResult = UIHelper.ShowInfoDialog(this, "Weet je zeker dat je de " + typeof(T).Name.ToLower() + " '{0}' wil verwijderen?", title);
			
			if (dialogResult == (int)ResponseType.Yes)
			{
				DeleteObjectFunc(objectMap[title]);
				Refresh();
			}
		}

		void HandleWijzigenButtonClicked (object sender, EventArgs e)
		{
			string objectTitle = comboObjects.ActiveText;
			
			IObjectEditor<T> objectEditor = (IObjectEditor<T>)Activator.CreateInstance(objectEditorType);
			objectEditor.EditObject(objectMap[objectTitle], delegate(T obj) {
				UpdateObjectFunc(obj);
				Refresh();
			});
		}

		void HandleToevoegenButtonClicked (object sender, EventArgs e)
		{
			T obj = (T)Activator.CreateInstance(typeof(T));

			IObjectEditor<T> objectEditor = (IObjectEditor<T>)Activator.CreateInstance(objectEditorType);
			objectEditor.EditObject(obj, delegate {
				AddObjectFunc(obj);
				Refresh();
			});
		}

		void HandleComboOjectsChanged (object sender, EventArgs e)
		{
			setRekeningSelected(true);
		}
		
		void setRekeningSelected(bool selected)
		{
			wijzigenButton.Sensitive = selected;
			verwijderenButton.Sensitive = selected;
		}
	}
}