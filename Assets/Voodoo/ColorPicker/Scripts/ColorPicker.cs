using UnityEngine;
using UnityEngine.UI;

namespace Voodoo.Visual.UI
{
    public class ColorPicker : MonoBehaviour
    {
        public GraphicRaycaster graphicRaycaster;

        public HSVA hsva;

        public delegate void ColorChanged(HSVA hsva);
        public event ColorChanged OnChangedColor;
        public event ColorChanged OnEndChanged;

        private void Start()
        {
            if (graphicRaycaster != null) return;
            Debug.LogError("Can't find the graphic raycaster");
            
            graphicRaycaster = FindCanvas(gameObject).GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
            {
                Debug.LogError("Can't find the graphic raycaster");
            }
        }

        private GameObject FindCanvas(GameObject source)
        {
            while (source.transform != transform.root)
            {
                if (source.GetComponent<GraphicRaycaster>() != null)
                {
                    return source;
                }
                
                source = source.transform.parent.gameObject;
            }
            return null;
        }

        public void ChangeColor(HSVA hsva)
        {
            if (this.hsva == hsva)
            {
                return;
            }

            this.hsva = hsva;

            OnChangedColor?.Invoke(this.hsva);
        }

        public void EndChange()
        {
            OnEndChanged?.Invoke(this.hsva);
        }
    }
}