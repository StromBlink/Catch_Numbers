using UnityEngine;

namespace Voodoo.Events
{
    public abstract class Delay : MonoBehaviour
    {
        public bool     atStart = true;
        public float    delay;
        
        private Timer   timer;

        public event TimeEvent DelayTicked;
        public event TimeEvent DelayEnded;

        protected bool isInitialized;

        protected virtual void Start()
        {
            if (atStart)
                StartCountdown();
        }

        protected virtual void Initialize()
        {
            if (!isInitialized)
            {
                timer = new Timer(delay);
                timer.Ticked += Tick;
                timer.Stopped += EndOfCountdown;
                isInitialized = true;
            }
        }
        
        /// <summary>
        /// Start the countdown
        /// </summary>
        public virtual void StartCountdown()
        {
            Initialize();
            timer.Start();
        }

        public virtual void ResetCountdown()
        { 
            Initialize();
            timer.ForceStop();
        }

        protected virtual void Tick(Timer _timer) 
        {
            DelayTicked?.Invoke(_timer);
        }

        protected virtual void EndOfCountdown(Timer _timer) 
        {
            DelayEnded?.Invoke(_timer);
        }

        private void OnDestroy()
        {
            timer?.Dispose();
        }
    }
}
