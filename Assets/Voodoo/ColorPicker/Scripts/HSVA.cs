using UnityEngine;

namespace Voodoo.Visual.UI
{
 public class HSVA
 	{
 		public float h;
 		public float s;
 		public float v;
 		public float a;
 
 		public HSVA(Color color)
 		{
 			(h, s, v, a) = ColorToHSVA(color);
 		}
 
 		public HSVA(HSVA hsva)
 		{
 			h = hsva.h;
 			s = hsva.s;
 			v = hsva.v;
 			a = hsva.a;
 		}
 
 		public HSVA(float h, float s, float v, float a)
 		{
 			this.h = h;
 			this.s = s;
 			this.v = v;
 			this.a = a;
 		}
 
 		public HSVA(string savedString)
 		{
 			JsonUtility.FromJsonOverwrite(savedString, this);
 		}
 
 		public string ToJSon()
 		{
 			return JsonUtility.ToJson(this);
 		}
 
 		public Color HSVAToColor()
 		{
 			return Color.HSVToRGB(h, s, v);
 		}
 
 		public override string ToString()
        {
	        return $"({h},{s},{v},{a})";
        }
 		
 		private static (float h, float s, float v, float a) ColorToHSVA(Color color)
 		{
	        Color.RGBToHSV(color, out float h, out float s, out float v);
 
 			return (h, s, v, color.a);
 		}
 	}   
}
