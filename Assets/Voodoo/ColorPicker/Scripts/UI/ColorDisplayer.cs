using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voodoo.Visual.UI.ColorDisplayer
{
	public class ColorDisplayer : ColorWitness
	{
		public Image image;

		protected override void UpdateHSVA(HSVA _hsva)
		{
			if (_hsva == null) return;
			
			base.UpdateHSVA(_hsva);
			image.color = _hsva.HSVAToColor();
		}
	}
}