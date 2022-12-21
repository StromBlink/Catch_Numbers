using UnityEngine;
using TMPro;

namespace Voodoo.Visual.UI
{
public class HexDisplayer : ColorWitness
{
    public TMP_InputField inputField;

    private bool isSelected;
    
    protected override void UpdateHSVA(HSVA _hsva)
    {
        base.UpdateHSVA(_hsva);

        UpdateInputField();
    }

    private void UpdateInputField()
    {
        if (isSelected)
        {
            return;
        }
        
        string value = $"#{ColorUtility.ToHtmlStringRGBA(hsva.HSVAToColor())}";

        if (!string.Equals(value,inputField.text))
        {
            inputField.text = value;
        }
        
    }
    
    public void ChangeInputFieldValue()
    {
        if (isSelected == false)
        {
            return;
        }

        if (!ColorUtility.TryParseHtmlString(inputField.text, out Color tmp) || hsva.HSVAToColor() == tmp) return;
        
        HSVA _hsva = new HSVA(tmp);
        colorPicker.ChangeColor(_hsva);

    }

    public void Selected()
    {
        isSelected = true;
    }
    
    public void Deselected()
    {
        isSelected = false;
    }
}
}
