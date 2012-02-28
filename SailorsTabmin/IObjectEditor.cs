using System;

namespace SailorsTab.Tabmin
{
	public interface IObjectEditor<T>
	{
		void EditObject(T obj, Action<T> saveObjectFunc);
	}
}

