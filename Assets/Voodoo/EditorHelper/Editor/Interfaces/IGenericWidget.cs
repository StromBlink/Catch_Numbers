using System;

namespace Voodoo.Utils
{
	public interface IGenericWidget<T>
	{
		Type Target { get; }
		void OnGUI(T target);
	}
}