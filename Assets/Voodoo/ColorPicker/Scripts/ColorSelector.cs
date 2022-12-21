namespace Voodoo.Visual.UI
{
    public abstract class ColorSelector : ColorWitness
    {
        protected void NewColor(HSVA hsva)
        {
            base.hsva = hsva;
            colorPicker.ChangeColor(base.hsva);
        }

    }
}