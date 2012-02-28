using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace SailorsTab.Common
{
    public class UIHelper
    {
        public static void ShowErrorDialog(Gtk.Widget parent, string message, params object[] parameters)
        {
            MessageDialog messagedialog1 = new MessageDialog(findParentWindow(parent), (DialogFlags)3, MessageType.Error, ButtonsType.Ok, message, parameters);
            messagedialog1.Run();
            messagedialog1.Destroy();
        }

        public static int ShowInfoDialog(Gtk.Widget parent, string message, params object[] parameters)
        {
            MessageDialog messagedialog1 = new MessageDialog(findParentWindow(parent), (DialogFlags)3, MessageType.Question, ButtonsType.YesNo, false, message, parameters);
            int num1 = messagedialog1.Run();
            messagedialog1.Destroy();
            return num1;
        }


        private static Window findParentWindow(Gtk.Widget parent)
        {
            Window window = null;
            do
            {
                if (parent is Window)
                {
                    window = (Window)parent;
                }
                else
                {
                    parent = parent.Parent;
                }
            }
            while (window == null);
            
            return window;
        }

    }

}
