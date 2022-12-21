using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Voodoo.Visual.UI
{
	public class Swatch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public HSVA hsva;
		public Image image;
		public Image deletedImage;

		private int index;

		private SwatchLibrary swatchLibrary;
		private bool mouseDown;
		private float startTime;

		private const string swatchKey = "Swatch";

		public void OnEnable()
		{
			SwatchLibrary.OnNormalMode += HideDeletedImage;
			SwatchLibrary.OnDeleteMode += ShowDeletedImage;
		}

		public void OnDisable()
		{
			SwatchLibrary.OnNormalMode -= HideDeletedImage;
			SwatchLibrary.OnDeleteMode -= ShowDeletedImage;
		}
		
		public void SetUpSwatch(SwatchLibrary swatchLibrary, HSVA hsva)
		{
			this.swatchLibrary = swatchLibrary;
			
			this.hsva = hsva;

			image.color = this.hsva.HSVAToColor();
		}

		public void SetIndex(int index)
		{
			string value = $"{swatchKey}{this.index}";
			
			if(PlayerPrefs.HasKey(value))
			{
				PlayerPrefs.DeleteKey(value);
			}
			
			this.index = index;
			
			PlayerPrefs.SetString(value, hsva.ToJSon());
		}
	
		public void OnPointerDown(PointerEventData ped)
		{
			if (mouseDown)
			{
				return;
			}
			
			startTime = Time.time;

			mouseDown = true;
		}

		public void OnPointerUp(PointerEventData ped)
		{
			if (mouseDown == false)
			{
				return;
			}

			if (swatchLibrary.deleteMode)
			{
				OnDisable();

				string value = $"{swatchKey}{index}";

				if(PlayerPrefs.HasKey(value))
				{
					PlayerPrefs.DeleteKey(value);
				}

				swatchLibrary.swatches.Remove(this);
            
				Destroy(gameObject);
			}
			else
			{
				ChangeColor();
			}
			
			mouseDown = false;
		}
		
		private void ChangeColor()
		{
			swatchLibrary.colorPicker.ChangeColor(hsva);
		}

		private void ShowDeletedImage()
		{
			deletedImage.gameObject.SetActive(true);
		}

		private void HideDeletedImage()
		{
			deletedImage.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (!mouseDown || !(startTime + 1f < Time.time)) return;
			
			mouseDown = false;
			swatchLibrary.SwitchMode();
		}

	}
}