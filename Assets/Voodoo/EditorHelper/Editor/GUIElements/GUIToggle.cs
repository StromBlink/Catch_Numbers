using UnityEngine;

namespace Voodoo.Utils
{
	public class GUIToggle : GUIButton
	{
		private readonly Color normalColor = Color.white;
		private readonly Color selectedColor = Color.gray;
		private readonly Color unselectableColor = Color.black;

		public bool unSelectable;

		public override void OnGUI()
		{
			GUI.backgroundColor = IsSelected ? selectedColor : normalColor;

			if (unSelectable)
			{
				GUI.backgroundColor = unselectableColor;
			}
			
			base.OnGUI();

			GUI.backgroundColor = normalColor;
		}
		
		public override void Execute()
		{
			if (isUsable == false)
			{
				return;
			}
			
			base.Execute();
			IsSelected = !IsSelected;
		}
	}
}