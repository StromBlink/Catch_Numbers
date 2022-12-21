using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Voodoo.Meta
{
	public class ProgressionView : View<ProgressionView>
	{
		public Image m_progressionBar;

		public TextMeshProUGUI m_levelText;
		public TextMeshProUGUI m_actualLevelText;
		public TextMeshProUGUI m_nextLevelText;

		public void UpdateProgression(float _progression, float _maxProgression)
		{
			if (_maxProgression <= 0)
			{
				Debug.LogError("_maxProgression must be > 0.");
				m_progressionBar.fillAmount = 1;
				return;
			}

			float clampedProgression = Mathf.Clamp(_progression, 0, _maxProgression);
			float fillAmount = clampedProgression / _maxProgression;

			m_progressionBar.fillAmount = fillAmount;
		}

		public void UpdateLevelTexts(int _Level)
		{
			m_levelText.text = "Level " + _Level;
			m_actualLevelText.text = _Level.ToString();
			m_nextLevelText.text = (_Level + 1).ToString();
		}
	}
}
