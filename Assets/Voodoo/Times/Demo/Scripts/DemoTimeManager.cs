using System;
using UnityEngine;

namespace Voodoo.Events
{
    public class DemoTimeManager : MonoBehaviour
    {
        public UICountdown uiCountdown;

        private TimeSpan delay;

        private void OnEnable()
        {
            uiCountdown.OnCountdownEnded += OnEnd;
        }

        private void OnDisable()
        {
            uiCountdown.OnCountdownEnded -= OnEnd;
        }

        void OnEnd()
        {
            Debug.Log("End of the Countdown");
        }

        public void SetToNow()
        {
            uiCountdown.SetEndDate(DateTime.Now);
        }
    }
}