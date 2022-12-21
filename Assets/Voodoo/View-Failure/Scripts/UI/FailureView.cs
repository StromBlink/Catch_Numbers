using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Voodoo.Visual.UI.Button;

namespace Voodoo.Meta
{
	public class FailureView : View<FailureView>
	{
		public ButtonHub buttonHub;
		public Image icon;
		public Sprite spriteRevive;
		public Sprite spriteNextLevel;
		
		public void OnContinueButton()
		{
			Debug.Log("Write your code here");
//			Hide();
        }

		public void OnRewardedVideoCompleted(bool _completed)
		{
			Debug.Log("Write your code here");
			if (_completed)
			{
				//TODO : Give Reward to the user
			}
			else
			{
				//TODO : Specific action when the Rewarded Video is canceled
			}
		}

		public override void Show([CallerFilePath] string callerFilePath = "")
		{
			base.Show(callerFilePath);
			
			if (buttonHub != null)
			{
				buttonHub.Hide();

				if (icon != null)
				{
					icon.sprite = null;
					icon.color = new Color(0,0,0,0);
				}
				
				float rnd = UnityEngine.Random.value;
				if (rnd >= 0.5f)
				{
					buttonHub.Show("Revive");
					if (icon != null && spriteRevive != null)
					{
						icon.sprite = spriteRevive;
						icon.color = Color.white;
					}
				}
				else
				{
					buttonHub.Show("Skip Level");
					if (icon != null && spriteNextLevel != null)
					{
						icon.sprite = spriteNextLevel;
						icon.color = Color.white;
					}
				}
			}
		}
	}
}