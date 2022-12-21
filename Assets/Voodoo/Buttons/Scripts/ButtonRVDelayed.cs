using Voodoo.Events;

namespace Voodoo.Visual.UI.Button
{
    public class ButtonRVDelayed : ButtonRV
    {
        public Delay delay;
        
        protected void OnEnable()
        {
            if (delay == null)
                return;
            
            if (delay.delay > 0)
            {
#if VOODOO_SAUCE
                if (VoodooSauce.IsRewardedVideoAvailable())
#endif
                {
                    delay.StartCountdown();
                    delay.DelayEnded += OnDelayEnded;
                }
            }
            else
            {
                delay.gameObject.SetActive(false);
            }
        }

        protected void OnDisable()
        {
            if (delay == null)
                return;
            
            delay.ResetCountdown();
            delay.DelayEnded -= OnDelayEnded;
        }

#if VOODOO_SAUCE
        protected override void OnRewardedVideoLoadComplete(bool isLoaded)
        {
            base.OnRewardedVideoLoadComplete(isLoaded);
            if (isLoaded && delay != null)
            {
                delay.StartCountdown();
                delay.DelayEnded += OnDelayEnded;
            }
        }
#endif

        private void OnDelayEnded(Timer _timer)
        {
            if (button == null)
                return;

            button.interactable = false;
        }
    }
}