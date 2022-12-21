using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Voodoo.Visual.UI
{
	public enum SliderType
	{
		H,
		S,
		V,
		A
	}

	public class ColorSlider : ColorWitness
	{
		public Slider slider;
		public TMP_InputField inputField;
		public Image image;

		public SliderType type;

		void Start()
		{
			if (type == SliderType.V)
			{
				image.material.SetColor("_LeftColor", Color.black);
			}

			if (type == SliderType.S)
			{
				image.material.SetColor("_LeftColor", Color.white);
			}
		}


		public void ChangeSliderValue()
		{
			HSVA _hsva = new HSVA(hsva);

			switch (type)
			{
				case SliderType.H:

					if (slider.value == _hsva.h)
					{
						return;
					}

					_hsva.h = slider.value;
					break;

				case SliderType.S:
					if (slider.value == _hsva.s)
					{
						return;
					}

					_hsva.s = slider.value;
					break;

				case SliderType.V:
					if (slider.value == _hsva.v)
					{
						return;
					}

					_hsva.v = slider.value;
					break;

				case SliderType.A:
					if (slider.value == _hsva.a)
					{
						return;
					}

					_hsva.a = slider.value;
					break;
			}

			colorPicker.ChangeColor(_hsva);
		}

		public void ChangeInputFieldValue()
		{

			HSVA _hsva = new HSVA(hsva);

			float _value = 0f;

			try
			{
				_value = float.Parse(inputField.text);

				if (_value < 0f)
				{
					_value = 0f;
					inputField.text = _value.ToString();
				}
				else if (_value > 100f && type != SliderType.H)
				{
					_value = 100f;
					inputField.text = _value.ToString();
				}
				else if (_value > 360f && type == SliderType.H)
				{
					_value = 360f;
					inputField.text = _value.ToString();
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return;
			}

			switch (type)
			{


				case SliderType.H:
					if (_hsva.h == _value / 360f)
					{
						return;
					}

					_hsva.h = _value / 360f;
					break;

				case SliderType.S:
					if (_hsva.s == _value / 100f)
					{
						return;
					}

					_hsva.s = _value / 100f;
					break;

				case SliderType.V:
					if (_hsva.v == _value / 100f)
					{
						return;
					}

					_hsva.v = _value / 100f;
					break;

				case SliderType.A:
					if (_hsva.a == _value / 100f)
					{
						return;
					}

					_hsva.a = _value / 100f;
					break;
			}

			colorPicker.ChangeColor(_hsva);

		}

		protected override void UpdateHSVA(HSVA _hsva)
		{
			base.UpdateHSVA(_hsva);

			UpdateSliderValue();
			UpdateInputFieldValue();

			if (type == SliderType.H)
			{
				return;
			}

			Color _colorRGB = Color.HSVToRGB(_hsva.h, 1, 1);



			image.material.SetColor("_RightColor", _colorRGB);
		}

		private void UpdateSliderValue()
		{
			HSVA _hsva = new HSVA(hsva);
			float _sliderValue = 0f;
			switch (type)
			{
				case SliderType.H:
					_sliderValue = _hsva.h;
					break;

				case SliderType.S:
					_sliderValue = _hsva.s;
					break;

				case SliderType.V:
					_sliderValue = _hsva.v;
					break;

				case SliderType.A:
					_sliderValue = _hsva.a;
					break;
			}

			if (slider.value != _sliderValue)
			{
				slider.value = _sliderValue;
			}
		}

		private void UpdateInputFieldValue()
		{
			HSVA _hsva = new HSVA(hsva);

			float _value = 0;

			switch (type)
			{
				case SliderType.H:
					_value = _hsva.h * 360f;
					break;

				case SliderType.S:
					_value = _hsva.s * 100f;
					break;

				case SliderType.V:
					_value = _hsva.v * 100f;
					break;

				case SliderType.A:
					_value = _hsva.a * 100f;
					break;
			}

			if (_value < 0)
			{
				_value = 0;
			}

			inputField.text = ((int) _value).ToString();


		}
	}
}