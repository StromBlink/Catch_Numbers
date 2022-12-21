using UnityEngine.UI;

namespace Voodoo.Visual.UI
{
    public class LastColor : ColorWitness
    {
        public Image image;

        private bool alreadySetup;
        
        public void OnClick()
        {
            colorPicker.ChangeColor(hsva);
        }
        
        protected override void UpdateHSVA(HSVA hsva)
        {
            if (alreadySetup) return;
            
            base.hsva = hsva;
            image.color = base.hsva.HSVAToColor();
            alreadySetup = true;
        }

    }
}