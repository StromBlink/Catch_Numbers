using UnityEngine;

namespace Voodoo.Meta
{
	public class PercentCircleView : PercentView<PercentCircleView>
	{
		protected override void RefreshSlot(PlayerSlot _Slot, float _Percent)
		{
			base.RefreshSlot(_Slot, _Percent);
			_Slot.m_Transform.rotation = Quaternion.AngleAxis(_Percent * 360.0f, Vector3.forward);
		}
	}
}
