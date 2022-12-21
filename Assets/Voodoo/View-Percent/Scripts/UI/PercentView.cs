using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Voodoo.Meta
{
	public class PercentView<T> : View<T> where T : MonoBehaviour
	{
		private const float c_RefreshRate = 0.2f;

		public class PlayerSlot
		{
			public Color m_Color;
			public float m_Percent;
			public RectTransform m_Transform;
			public Image m_Image;

			public PlayerSlot() : this(Color.black) { }
			public PlayerSlot(Color m_Color) : this(m_Color, 0.0f, null, null) { }
			public PlayerSlot(Color m_Color, float m_Percent, RectTransform m_Transform, Image m_Image)
			{
				this.m_Color = m_Color;
				this.m_Percent = m_Percent;
				this.m_Transform = m_Transform;
				this.m_Image = m_Image;
			}
		}

		public RectTransform m_Background;
		public GameObject m_PlayerSlot;

		private List<PlayerSlot> m_PlayerSlots;
		private bool m_IsRefreshing = false;
		private float m_RefreshTime;

		public override void Show([CallerFilePath] string callerFilePath = "")
		{
			base.Show();

			m_IsRefreshing = true;
			m_RefreshTime = Time.time;
		}

		public override void Hide()
		{
			base.Hide();

			m_IsRefreshing = false;
		}

		public int RegisterPlayer(Color _Color)
		{
			if (m_PlayerSlots == null)
				m_PlayerSlots = new List<PlayerSlot>();

			PlayerSlot newPlayer = new PlayerSlot(_Color);

			GameObject slot = Instantiate(m_PlayerSlot, m_Background);
			newPlayer.m_Transform = slot.GetComponent<RectTransform>();
			newPlayer.m_Image = slot.GetComponent<Image>();
			newPlayer.m_Image.color = _Color;

			m_Background.SetAsFirstSibling();

			for (int i = 0; i < m_PlayerSlots.Count; ++i)
			{
				if (m_PlayerSlots[i] == null)
				{
					m_PlayerSlots[i] = newPlayer;
					return i;
				}
			}

			m_PlayerSlots.Add(newPlayer);
			return m_PlayerSlots.Count - 1;
		}

		public void UnregisterPlayer(int _Index)
		{
			Destroy(m_PlayerSlots[_Index].m_Transform.gameObject);
			m_PlayerSlots[_Index] = null;
		}

		public void UpdatePlayer(int _Index, float _Percent)
		{
			m_PlayerSlots[_Index].m_Percent = _Percent;
		}

		private void LateUpdate()
		{
			if (!m_IsRefreshing)
				return;

			if (m_PlayerSlots == null || m_PlayerSlots.Count == 0)
				return;

			if (Time.time - m_RefreshTime < c_RefreshRate)
				return;

			float percent = 0.0f;
			for (int i = 0; i < m_PlayerSlots.Count; ++i)
			{
				PlayerSlot slot = m_PlayerSlots[i];
				RefreshSlot(slot, percent);
				percent += slot.m_Percent;
			}
		}

		protected virtual void RefreshSlot(PlayerSlot _Slot, float _Percent)
		{
			_Slot.m_Image.fillAmount = _Slot.m_Percent;
		}
	}
}
