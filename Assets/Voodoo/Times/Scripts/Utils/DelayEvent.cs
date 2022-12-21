using UnityEngine.Events;

namespace Voodoo.Events
{
    public class DelayEvent : Delay
    {
	    public UnityEvent unityEvent;
	
	    protected override void EndOfCountdown(Timer _timer)
	    {
		    unityEvent?.Invoke();
            base.EndOfCountdown(_timer);
	    }
    }
}
