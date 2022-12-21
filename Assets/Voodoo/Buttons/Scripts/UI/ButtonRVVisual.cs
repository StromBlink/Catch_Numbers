using UnityEngine;

namespace Voodoo.Visual.UI.Button
{
    public class ButtonRVVisual : MonoBehaviour
    {
	    public ButtonRV rvButton;
        public IconRV rvIcon;
        
        private void Start()
        {
            if (rvButton == null)
                return;
            
#if VOODOO_SAUCE
            rvButton.onRVLoaded.AddListener(SetInteractivity);
            rvButton.onRVCompleted.AddListener(OnRVCompleted);
#endif

            OnEnable();
        }

        private void OnDestroy()
        {
#if VOODOO_SAUCE
            if (rvButton == null)
                return;
            
            rvButton.onRVLoaded.RemoveListener(SetInteractivity);
            rvButton.onRVCompleted.RemoveListener(OnRVCompleted);
#endif
        }

        private void OnEnable()
        {
#if VOODOO_SAUCE
            SetInteractivity(VoodooSauce.IsRewardedVideoAvailable());
#elif UNITY_EDITOR
            SetInteractivity(true);
#else
            SetInteractivity(false);
#endif
        }

        private void OnRVCompleted(bool _completed)
        {
#if VOODOO_SAUCE
            SetInteractivity(VoodooSauce.IsRewardedVideoAvailable());
#else
            SetInteractivity(false);
#endif
        }

        private void SetInteractivity(bool _isInteractable)
        {
            if (rvButton != null && rvButton.button != null)
                rvButton.button.interactable = _isInteractable;

            if (rvIcon != null)
            {
	            rvIcon.UpdateDisplay(_isInteractable);
            }
        }
    }
}