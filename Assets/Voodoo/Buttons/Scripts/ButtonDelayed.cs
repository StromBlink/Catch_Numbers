using Voodoo.Events;

namespace Voodoo.Visual.UI.Button
{
    public class ButtonDelayed : ButtonStandard
    {
        public Delay delay;
        
        protected virtual void OnEnable()
        {
            if (delay != null && delay.delay > 0)
            {
                delay.StartCountdown();
            }
        }
        
        protected virtual void OnDisable()
        {
            if (delay == null)
            {
                return;
            }

            delay.ResetCountdown();
        }
    }
}