using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Voodoo.Visual.UI.Button
{
	[RequireComponent(typeof(UnityEngine.UI.Button))]
	public abstract class ButtonHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
		public UnityEngine.UI.Button button;
		public delegate void ButtonEvent();
		public event ButtonEvent OnClicked;
		public event ButtonEvent OnDown;
		public event ButtonEvent OnUp;
		
		public virtual void OnPointerClick(PointerEventData _event)
		{
			if (button.interactable)
			{
				OnClicked?.Invoke();
			}
		}

		public virtual void OnPointerDown(PointerEventData _event)
		{
			if (button.interactable)
			{
				OnDown?.Invoke();
				//Vibration MediumImpact
			}
		}

		public virtual void OnPointerUp(PointerEventData _event)
		{
			if (button.interactable)
			{
				OnUp?.Invoke();
			}
		}
	}
}