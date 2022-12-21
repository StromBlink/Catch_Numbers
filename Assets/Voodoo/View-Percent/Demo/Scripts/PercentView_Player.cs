using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voodoo.Meta;

namespace Voodoo.Pattern
{
	public class PercentView_Player : MonoBehaviour
	{
		public Color m_Color;

		private int m_BarViewIndex;
		private PercentBarView m_BarView;

		private int m_CircleViewIndex;
		private PercentCircleView m_CircleView;

		void Awake()
		{
			m_BarView = PercentBarView.Instance;
			m_BarViewIndex = m_BarView.RegisterPlayer(m_Color);

			m_CircleView = PercentCircleView.Instance;
			m_CircleViewIndex = m_CircleView.RegisterPlayer(m_Color);
		}

		void Update()
		{
			float percent = Mathf.Abs(Mathf.Sin(Time.time)) * 0.25f;
			m_BarView.UpdatePlayer(m_BarViewIndex, percent);
			m_CircleView.UpdatePlayer(m_CircleViewIndex, percent);
		}

		void OnDestroy()
		{
			if (m_BarView != null)
				m_BarView.UnregisterPlayer(m_BarViewIndex);

			if (m_CircleView != null)
				m_CircleView.UnregisterPlayer(m_CircleViewIndex);
		}
	}
}
