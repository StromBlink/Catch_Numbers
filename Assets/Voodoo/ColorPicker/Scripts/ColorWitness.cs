using UnityEngine;

namespace Voodoo.Visual.UI
{
	public abstract class ColorWitness : MonoBehaviour
	{
		public ColorPicker colorPicker;
		protected HSVA hsva = new HSVA(0f, 1f, 1f, 1f);

		protected virtual void OnEnable()
		{
			colorPicker.OnChangedColor += UpdateHSVA;
		}

		protected virtual void OnDisable()
		{
			colorPicker.OnChangedColor -= UpdateHSVA;
		}

		protected virtual void UpdateHSVA(HSVA hsva)
		{
			this.hsva = hsva;
		}
	}
}