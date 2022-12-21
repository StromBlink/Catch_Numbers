using System;
using System.Threading.Tasks;
using UnityEngine;
using Voodoo.Events;


namespace Voodoo.Visual.UI.Button
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFading : MonoBehaviour
    {
        public Delay delay;
        public float fadeInDelay = 1;
        public float fadeOutDelay = 0.5f;

        private bool isFadingIn;
        private CanvasGroup canvasGroup;
        
        void OnEnable()
        {
            if (delay==null)
            {
                FadeIn(null);
            }
            else
            {
                delay.DelayEnded += FadeIn;
            }
        }
        

        
        // Start is called before the first frame update
        async void FadeIn(Timer _timer)
        {
            isFadingIn = true;
            ComponentChecking();
            canvasGroup.alpha = 0f;

            float _sensibility = fadeInDelay / 100;
            
            while (canvasGroup.alpha < 1f)
            {
                if (isFadingIn == false)
                {
                    break;
                }

                canvasGroup.alpha += _sensibility;
                await Task.Delay(TimeSpan.FromMilliseconds(1));

                if (canvasGroup == null)
                {
                    break;
                }
            }
        }
        
        void OnDisable()
        {
            FadeOut();
        }


        // Update is called once per frame
        async void FadeOut()
        {
            isFadingIn = false;
            ComponentChecking();
            canvasGroup.alpha = 1f;
            
            float _sensibility = fadeOutDelay / 100;
            while (canvasGroup.alpha > 0f)
            {
                if (isFadingIn)
                {
                    break;
                }

                canvasGroup.alpha -= _sensibility;
                await Task.Delay(TimeSpan.FromMilliseconds(1));

                if (canvasGroup == null)
                {
                    break;
                }
            }
        }


        void ComponentChecking()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

    }
}