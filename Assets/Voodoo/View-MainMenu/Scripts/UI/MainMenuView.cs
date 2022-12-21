using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Voodoo.Meta
{
	public class MainMenuView : View<MainMenuView>
	{
		public Image m_Title;
		public TextMeshProUGUI m_LevelText;
		public TextMeshProUGUI m_BestScoreText;

		public void OnPlayButton()
		{
			// TODO : Go to game mode
		}

		public void Show(int _Level, int _BestScore)
		{
			base.Show();

			m_BestScoreText.enabled = (_BestScore > 0);
			m_BestScoreText.text = "Best : " + _BestScore.ToString();
			m_LevelText.text = "Level " + _Level.ToString();
		}

		public void SetTitleColor(Color _Color)
		{
			m_Title.color = _Color;
		}
	}
}
