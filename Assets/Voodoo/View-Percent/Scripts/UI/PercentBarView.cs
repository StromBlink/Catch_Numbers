using System.Runtime.CompilerServices;
using UnityEngine;

namespace Voodoo.Meta
{
	public class PercentBarView : PercentView<PercentBarView>
	{
		private float m_BarSize;

		public override void Show([CallerFilePath] string callerFilePath = "")
		{
			base.Show();

			m_BarSize = m_Background.rect.width;
		}

		protected override void RefreshSlot(PlayerSlot _Slot, float _Percent)
		{
			base.RefreshSlot(_Slot,_Percent);
			_Slot.m_Transform.localPosition = Vector3.right * _Percent * m_BarSize;
		}
	}
}
