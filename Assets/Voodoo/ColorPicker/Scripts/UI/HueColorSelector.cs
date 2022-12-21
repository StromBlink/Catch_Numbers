using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Voodoo.Visual.UI
{
	public abstract class HueColorSelector : ColorSelector, IDragHandler
	{
		public RectTransform rectTransform;
		public Image image;
		public GameObject handler;

		protected Vector2 rectPosition;
		
		private Vector2 position;
		private Vector2 rectSize;
		private Texture2D texture;
		private PointerEventData pointerEventData;

		protected void Start()
		{
			SetUpHueSelector();
		}

		private void SetUpHueSelector()
		{
			position = transform.position;
			rectPosition = rectTransform.position;

			Rect _rect = rectTransform.rect;

			rectSize = new Vector2(_rect.width, _rect.height);
			texture = image.sprite.texture;
		}

		public void OnDrag(PointerEventData _pointerEventData)
		{
			ChangeFromHandler();
		}

		public void OnClickChange()
		{
			Color _newColor = GetPixelColor(Input.mousePosition);

			if (_newColor.a >= 1)
			{
				_newColor = NormalizeColor(_newColor);

				HSVA _hsva = new HSVA(_newColor);
				NewColor(_hsva);
				ChangeHandlePosition(Input.mousePosition);
			}

		}

		private void ChangeFromHandler()
		{
			ChangeHandlePosition(Input.mousePosition);

			Vector2 _handlerPosition = handler.transform.position;

			Color _newColor = GetPixelColor(_handlerPosition);

			HSVA _hsva = new HSVA(NormalizeColor(_newColor));
			NewColor(_hsva);

			// To fix offset
			ChangeHandlePosition(Input.mousePosition);
		}

		private Color NormalizeColor(Color _newColor)
		{

			HSVA _currentHSVA = new HSVA(hsva);
			HSVA _newHSVA = new HSVA(_newColor);

			_newHSVA.s = _currentHSVA.s;
			_newHSVA.v = _currentHSVA.v;
			_newColor.a = _currentHSVA.a;

			return _newHSVA.HSVAToColor();
		}


		private Color GetPixelColor(Vector2 _coor)
		{
			Vector2 _textureOrigin = GetTextureOrigin();
			Vector2 _pixelFromWorldPos = GetPixelOnTextureFromWorldPos(_coor, _textureOrigin);

			return GetColorAtPos(_pixelFromWorldPos);
		}

		private Vector2 GetTextureOrigin()
		{
			float _x = position.x - (rectSize.x / 2f);
			float _y = position.y - (rectSize.y / 2f);

			return new Vector2(_x, _y);
		}

		private Vector2 GetPixelOnTextureFromWorldPos(Vector2 _position, Vector2 _textureOrigin)
		{
			float _x = _position.x - _textureOrigin.x;
			float _y = _position.y - _textureOrigin.y;

			return new Vector2(_x, _y);
		}

		private Color GetColorAtPos(Vector2 _pos)
		{
			int _x = Mathf.FloorToInt(_pos.x * texture.height / rectSize.x);
			int _y = Mathf.FloorToInt(_pos.y * texture.width / rectSize.y);

			return texture.GetPixel(_x, _y);
		}

		protected virtual void ChangeHandlePosition(Vector2 _mousePosition)
		{

		}

		protected virtual void ChangeHandlePosition(float angle)
		{

		}
	}
}
