using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.Events
{
    public class DelayGameObjectActivation : Delay
    {
        public bool toActive = true;
        public List<GameObject> gameObjects = new List<GameObject>();
        
        protected override void Initialize()
        {
            DisplayGameObjects(!toActive);
            base.Initialize();
        }

        protected override void EndOfCountdown(Timer _timer)
        {
            DisplayGameObjects(toActive);
            base.EndOfCountdown(_timer);
        }

        /// <summary>
        /// Display/Hide referenced objects depending on _enable value
        /// </summary>
        /// <param name="_enabled">true displays objects</param>
        public void DisplayGameObjects(bool _enabled)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].SetActive(_enabled);
            }
        }
    }
}
