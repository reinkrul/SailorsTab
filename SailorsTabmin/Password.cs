using System;
namespace SailorsTab.Tabmin
{
	public class Password
	{	
		const int Shifts = 1;
		
		public static Password Decode(String encodedPassword)
		{
			return new Password(shift(encodedPassword, -Shifts));
		}
		
		private static String shift(String str, int places)
		{
			String encodedPassword = "";
			foreach(var c in str)
			{
				int newChar = (int)c + places;
				if (newChar > 'z')
				{
					newChar = 'a' + (newChar - 'z') - 1;
				}
				else if (newChar < 'a')
				{
					newChar = 'z' - ('a' - newChar) + 1;
				}
				encodedPassword += (char)newChar;
			}
			
			return encodedPassword;
		}
		
		public Password (String plain)
		{
			Plain = plain;
		}
		
		public String Plain { get; private set; }
		
		public String encode()
		{
			return shift(Plain, Shifts);
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals (this, obj))
				return true;
			if (obj.GetType () != typeof(Password))
				return false;
			SailorsTab.Tabmin.Password other = (SailorsTab.Tabmin.Password)obj;
			return Plain == other.Plain;
		}


		public override int GetHashCode ()
		{
			unchecked {
				return (Plain != null ? Plain.GetHashCode () : 0);
			}
		}
	}
}

