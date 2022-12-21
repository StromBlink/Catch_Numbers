using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.Visual.UI.Button
{
    public abstract class AbstractButtonHub<T> : MonoBehaviour where T : AbstractButtonHubState
    {
        public List<T> states = new List<T>();
        private T curState;

        /// <summary>
        /// Activate all the ButtonHandler's gameObject of the state stateName and hide the previous ones
        /// </summary>
        /// <param name="stateName">The name of the state as it is written in the ButtonHub</param>
        public void Show(string stateName)
        {
            if (states == null || states.Count == 0)
                return;

            T newState = states.Find(x => string.Equals(x.StateName, stateName, StringComparison.CurrentCultureIgnoreCase));
            if (newState == null || newState == curState)
                return;

            curState?.Hide();
            curState = newState;
            curState?.Show();
        }

        /// <summary>
        /// Hide all the content displayed
        /// </summary>
        public void Hide()
        {
            if (states == null || states.Count == 0)
                return;
            
            curState?.Hide();
            curState = null;
        }

        /// <summary>
        /// Returns the list of ButtonHandler contained in the currently shown state
        /// </summary>
        /// <returns></returns>
        public List<ButtonHandler> GetCurrentStateButtons()
        {
            return curState != null ? curState.buttonHandlers : new List<ButtonHandler>();
        }
    }

    [Serializable]
    public abstract class AbstractButtonHubState
    {
        public abstract string StateName { get;}
        public List<ButtonHandler> buttonHandlers;

        public void Show()
        {
            foreach (ButtonHandler buttonHandler in buttonHandlers)
            {
                if (buttonHandler == null)
                    continue;
                
                buttonHandler.gameObject.SetActive(true);
            }
        }
        
        public void Hide()
        {
            foreach (ButtonHandler buttonHandler in buttonHandlers)
            {
                if (buttonHandler == null)
                    continue;
                
                buttonHandler.gameObject.SetActive(false);
            }
        }
    }
}
