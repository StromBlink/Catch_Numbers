using UnityEngine;

namespace Voodoo.Events
{
    public class Timer : System.IDisposable
    {
        private float duration  = 0f;
        public  float tickRate  = 0.1f;
        private bool  loop      = false;
        private int   context   = 0;
                      
        private float startTime = 0f;
        private float past      = 0f;
        private int   tickCount = 0;
        private int   loopCount = 0;
        private bool  isRunning = false;
        
        public event TimeEvent Started;
        public event TimeEvent Ticked;
        public event TimeEvent Paused;
        public event TimeEvent Resumed;
        public event TimeEvent Looped;
        public event TimeEvent Stopped;


        public float StartTime      => startTime;

        public float Past           => past;
        public float PastNormalized => Mathf.Max(Mathf.Min(past / duration, 1.0f), 0.0f);

        public float Left           => duration - past;
        public float LeftNormalized => 1 - PastNormalized;
        
        public float Duration       => duration;

        public bool  IsOver         => past >= duration;

        public bool IsRunning
        {
            get => isRunning;

            set
            {
                if (value == IsRunning) { return; }
                
                isRunning = value;
                if (value == false) { Paused?.Invoke(this); } else { Resumed?.Invoke(this); }
            }
        }

        public Timer(float duration, float past = 0f, float tickRate = 0f, bool loop = false, int context = 0)
        {
            this.duration   = duration;
            this.past       = past;
            this.tickRate   = tickRate;
            this.loop       = loop;
            this.context    = context;

            isRunning       = false;
        }
        
        public Timer(float duration, float past, bool loop) : this(duration, past, 0f, loop) {}
        public Timer(float duration, float tickRate) : this(duration, 0f, tickRate) {}
        public Timer(float duration, bool loop) : this(duration, 0f, loop) {}
        public Timer() : this(0f) {}


        /// <summary>
        /// Attach the timer to it's manager update callback, reset past time and set it to isRunning = true
        /// </summary>
        /// <param name="duration">if value > 0 change timer duration, only needed in case you wish to chenge duration between to usage</param>
        public void Start(float duration = 0f)
        {
            if(isRunning)
            { 
                ForceStop();
            }
            
            if (duration > 0f)
            {
                this.duration = duration;
            }

            if (this.duration <= 0f)
            {
                Stop();
            }
            else
            {
                startTime   = Time.realtimeSinceStartup;
                isRunning   = true;

                TimeManager.AddTimer(this);

                Started?.Invoke(this);
            }
        }

        /// <summary>
        /// If timer is running increase its past time of the given delta, then check if Ticked/Looped/Stopped are to be triggered.
        /// </summary>
        /// <param name="delta">time past between two frame</param>
        public void Update(float delta)
        {
            if (isRunning == false)
            {
                return;
            }

            past += delta;

            if (tickRate == 0f || (past / tickRate) > (tickCount + 1))
            {
                tickCount++;
                Ticked?.Invoke(this);
            }

            if (IsOver == false)
            {
                return;
            }

            if (loop == false)
            {
                Stop();
            }
            else if ((int)(past / duration) > loopCount)
            {
                loopCount++;
                Looped?.Invoke(this);
            }
        }

        /// <summary>
        /// Reset all members value to default, dettach timer from its manager update, then trigger Stopped callback
        /// </summary>
        public void Stop()
        {
            ForceStop();
            Stopped?.Invoke(this);
        }

        public void ForceStop()
        {
            Reset();
            
            TimeManager.RemoveTimer(this);
        }

        /// <summary>
        /// Reset all members value ti default
        /// </summary>
        public void Reset()
        {
            startTime   = 0f;
            past        = 0f;
            tickCount   = 0;
            loopCount   = 0;
            isRunning   = false;
        }

        /// <summary>
        /// Nullify all events refs
        /// </summary>
        public void Unbind() 
        {
            Started = null;
            Ticked = null;
            Paused = null;
            Resumed = null;
            Looped = null;
            Stopped = null;
        }

        /// <summary>
        /// Free object
        /// </summary>
        public void Dispose()
        {
            Reset();
            Unbind();

            System.GC.SuppressFinalize(this);
        }
    }

    public delegate void TimeEvent(Timer timer);
}