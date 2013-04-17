using System;

namespace SailorsTab.Imaging
{
	public class Effects
	{
		public static void DrawBorder(Gdk.Image image, int width, Gdk.Color color)
		{
			for(int x = 0; x < image.Width; x++)
			{
				Console.WriteLine(image.GetPixel(x, 0).ToString());
				//image.PutPixel(x, 0, image.);
			}
		}
	}
}

