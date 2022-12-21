using System;
using UnityEngine;

namespace Voodoo.Utils
{
	public interface ICommandKeeper
	{
		void CreateMenuCommand(string iconPath, string tooltip, Action action, GUILayoutOption[] layoutOption);
	}
}