using System;
using TMPro;
using UnityEngine;

namespace Voodoo.Events
{
    public class ActualTime : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private Timer timer;
        
        private void OnEnable()
        {
            timer = new Timer(1f, true);            
            timer.Looped += NewSecond;
            timer.Start();
        }
        
        private void OnDisable()
        {
            timer?.Stop();
        }

        private void NewSecond(Timer _timer)
        {
            text.text = DateTime.Now.ToLongTimeString();
        }
        
        private void OnDestroy()
        {
            timer?.Dispose();
        }
    }
}