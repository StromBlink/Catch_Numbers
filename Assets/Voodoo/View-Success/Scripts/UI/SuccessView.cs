using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Voodoo.Visual.UI.Button;

namespace Voodoo.Meta
{
	public class SuccessView : View<SuccessView>
	{
		public ButtonHub buttonHub;
		public Image icon;
		public Sprite spriteNoAds;
		public Sprite spriteReward;


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
				
#if VOODOO_SAUCE
				float rnd = UnityEngine.Random.value;
				if (rnd >= 0.33 && !VoodooSauce.IsPremium())
				{
					buttonHub.Show("NoAds");
					if (icon != null && spriteNoAds != null)
					{
						icon.sprite = spriteNoAds;
						icon.color = Color.white;
					}
				}
				else
#endif
				{
					buttonHub.Show("RewardedVideo");
					if (icon != null && spriteReward != null)
					{
						icon.sprite = spriteReward;
						icon.color = Color.white;
					}
				}
			}
		}
	}
}