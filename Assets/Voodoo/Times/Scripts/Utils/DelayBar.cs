using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Voodoo.Events
{
    public class DelayBar : Delay
    {
	    private List<Image> images;
	    public bool isInversed = false;

	    protected override void Initialize()
	    {
		    if (!isInitialized)
		    {
			    images = GetComponentsInChildren<Image>().ToList();
			    images.ForEach(x => x.enabled = false);
		    }
		    
		    base.Initialize();
	    }

	    public override void StartCountdown()
        {
            base.StartCountdown();
            
            if (images.Count > 0)
				images.ForEach(x => x.enabled = delay > 0);
        }

        protected override void Tick(Timer _timer)
	    {
		    UpdateImage(_timer.PastNormalized);

            base.Tick(_timer);
	    }

        private void UpdateImage(float _pourcentage)
        {
	        if (isInversed)
	        {
		        _pourcentage = 1f - _pourcentage;
	        }
	    
	        if (images.Count > 0)
		        images.ForEach(x => x.fillAmount = _pourcentage);
        }
    }
}