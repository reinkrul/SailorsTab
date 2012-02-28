using System;
using Gtk;

namespace SailorsTab.Tabmin
{
	public class PasswordWindow : Window
	{
		private HBox box;
		private Label label;
		private TextView passwordField;
		private Button button;
		
		public PasswordWindow () : base(WindowType.Toplevel)
		{
			Password = null;
			Cancelled = true;
			
			box = new HBox(true, 3);
			
			label = new Label("Wachtwoord:");
			box.PackStart(label);
			
			TextTagTable textTagTable = new TextTagTable();
			passwordField = new TextView(new TextBuffer(new TextTagTable()));
			box.PackStart(passwordField);
			
			button = new Button();
			button.Label = "Ok";
			button.Clicked += delegate {
				Password = passwordField.Buffer.Text;
				Cancelled = false;
				Hide();
			};
			box.PackStart(button);
			
			Add(box);
			ShowAll();
		}
		
		public String Password { get; private set; }
		public bool Cancelled { get; private set; }
	}
}

