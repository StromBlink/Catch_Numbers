namespace Voodoo.Utils
{
	public class SelectableCommand : ISelectable
	{
		private bool isSelected;

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				isSelected = value;
				OnSelectedChange();
			}
		}

		public virtual void OnSelectedChange()
		{

		}
	}
}