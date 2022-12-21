using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Voodoo.Visual.UI.Button
{
    [AddComponentMenu("VoodooSauce/Rewarded Video Button")]
	public class ButtonRV : ButtonHandler
	{
        public UnityEventBool onRVLoaded;
        public UnityEventBool onRVCompleted;
        
        [Tooltip("this parameter will be given as a parameter for the ShowRewardedVideo")]
        public string rewardedType = "";

        //Cache
		private bool isInitialized;

		protected void Start()
		{
#if VOODOO_SAUCE
			VoodooSauce.SubscribeOnRewardedVideoLoaded(OnRewardedVideoLoadComplete);
#else
			OnRewardedVideoLoadComplete(true);
#endif
			isInitialized = true;
		}
        
        public override void OnPointerClick(PointerEventData _event)
		{
			if (button != null && button.interactable)
			{
				base.OnPointerClick(_event);
#if VOODOO_SAUCE
				VoodooSauce.ShowRewardedVideo(OnRewardedVideoCompleted, rewardedType);
#elif UNITY_EDITOR
				OnRewardedVideoCompleted(true);
#endif
            }
		}

		protected virtual void OnRewardedVideoLoadComplete(bool _isLoaded)
		{
			if (!isInitialized)
				return;
			
			onRVLoaded?.Invoke(_isLoaded);
		}

		protected virtual void OnRewardedVideoCompleted(bool completed)
		{
			if (!isInitialized)
				return;

			onRVCompleted?.Invoke(completed);
		}

        protected void OnDestroy()
        {
	        isInitialized = false;
            onRVLoaded.RemoveAllListeners();
            onRVCompleted.RemoveAllListeners();
        }
    }
	
	[Serializable]
	public class UnityEventBool : UnityEvent<bool> {}
}