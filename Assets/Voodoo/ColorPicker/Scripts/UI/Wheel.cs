using System;
using UnityEngine;

namespace Voodoo.Visual.UI
{
    public class Wheel : HueColorSelector
    {
        public GameObject rotator;

        protected override void UpdateHSVA(HSVA hsva)
        {
            base.UpdateHSVA(hsva);

            ChangeHandlePosition(hsva.h * 360f);
        }

        protected override void ChangeHandlePosition(Vector2 mousePosition)
        {
            float xToZero = mousePosition.x - rectPosition.x;
            float yToZero = mousePosition.y - rectPosition.y;

            ChangeHandlePosition((float) (Math.Atan2(yToZero, xToZero) / (float) Math.PI) * 180f);
        }

        protected override void ChangeHandlePosition(float angle)
        {
            var eulerAngles = rotator.transform.eulerAngles;
            
            eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, angle);
            rotator.transform.eulerAngles = eulerAngles;
        }
    }
}