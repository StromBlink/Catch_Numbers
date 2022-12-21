using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voodoo.Utils.Ease
{
    public class CurveInfo
    {
        public string name;
        public AnimationCurve curve;
    }
    
    public class AnimationCurves : MonoBehaviour
    {
        public List<CurveInfo> curves = new List<CurveInfo>();
        
        public void SetUpCurves()
        {
            curves = new List<CurveInfo>(); 
            
            for (int i = 0; i < Enum.GetNames(typeof(Ease)).Length; i++)
            {
                CurveInfo _curveInfo = new CurveInfo();
                Ease _ease = (Ease)i;
                _curveInfo.name = _ease.ToString();
                _curveInfo.curve = CurveEasing.GenerateAnimationCurve(_ease);

                curves.Add(_curveInfo);
            }
            
        }

    }
}
