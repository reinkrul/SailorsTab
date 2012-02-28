using System;
using System.IO;
using Gtk;
using SailorsTab.Domain;
using SailorsTab.Imaging;

namespace SailorsTab.Windows
{
	public partial class RekeningWidget : Gtk.Button
	{
		private const int FontSize = 20;
		private string[] imageExtensions = new string[] { "png", "jpg", "bmp", "jpeg", "gif" };
		private Rekening rekening;
        private Label label;
		
		public RekeningWidget (Rekening rekening) : base()
		{
			this.rekening = rekening;
			initialize();
		}
		
		private void initialize()
		{	
			VBox box = new VBox();
			
			// Label
			label = new Label(rekening.Naam);
			Pango.FontDescription fontDescription = Pango.FontDescription.FromString("Arial");
			fontDescription.Size = FontSize * 700;
			fontDescription.Weight = Pango.Weight.Bold;
			label.ModifyFont(fontDescription);
			box.PackStart(label);
			
			// Additional image
			Image image = getImage();
			if (image != null)
			{
				box.PackStart(image);
			}
			
			Add(box);
            Refresh();
		}

		private Image getImage()
		{
			foreach(var ext in imageExtensions)
			{
				string fileName = rekening.Naam + "." + ext;
				if (File.Exists(fileName))
				{
					return new Image(fileName);
				}
			}
			
			return null;
		}
		
		public Rekening Rekening {
			get { return rekening; }
		}
	}
}

