using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Voodoo.Visual.UI
{
    public class SwatchLibrary : MonoBehaviour
    {
        public ColorPicker colorPicker;
        public Transform content;
        
        public GameObject swatchPrefab;
        
        public delegate void ChangeMode();
        public static event ChangeMode OnDeleteMode;
        public static event ChangeMode OnNormalMode;
        
        public bool deleteMode;

        public List<Swatch> swatches = new List<Swatch>();
        
        private GraphicRaycaster raycaster;
        private PointerEventData pointerEventData;
        private EventSystem eventSystem;

        private const string swatchKey = "Swatch";
        
        private void Start()
        {
            eventSystem = EventSystem.current;
            raycaster = colorPicker.graphicRaycaster;

            foreach (Transform child in content) {
                Destroy(child.gameObject);
            }
            
            swatches = new List<Swatch>();
            
            RefreshSavedSwatches();
        }

        private void RefreshSavedSwatches()
        {
            int swatchIndex = 0;
            
            while (PlayerPrefs.HasKey(swatchKey + swatchIndex))
            {
                RefreshSavedSwatch(PlayerPrefs.GetString(swatchKey + swatchIndex));
                swatchIndex++;

                // IndexOut
                if (swatchIndex > 1000)
                {
                    break;
                }
            }
        }

        private void RefreshSavedSwatch(string savedString)
        {
            HSVA hsva = new HSVA(savedString);
            AddSwatch(hsva);
        }
        
        // Set by customs buttons
        public void AddButton()
        {
            AddSwatch(colorPicker.hsva);
        }

        private void AddSwatch(HSVA hsva)
        {
            GameObject go = Instantiate(swatchPrefab, content);
            
            Swatch swatch = go.GetComponent<Swatch>();
                
            swatch.SetUpSwatch(this, hsva);

            swatches.Add(swatch);

            swatch.SetIndex(swatches.Count - 1);
        }

        private void Update()
        {
            if (!deleteMode) return;
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                pointerEventData = new PointerEventData(eventSystem) {position = Input.mousePosition};
                //Set the Pointer Event Position to that of the mouse position

                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();
                    
                raycaster.Raycast(pointerEventData, results);

                if (CheckForSwatches(results) == false)
                {
                    SwitchMode();
                }
            }
        }

        public void SwitchMode()
        {
            SaveSwatches();
            
            if (deleteMode == false)
            {
                deleteMode = true;
                
                OnDeleteMode?.Invoke();
            }
            else
            {
                deleteMode = false;

                OnNormalMode?.Invoke();
            }
        }

        private static bool CheckForSwatches(IReadOnlyList<RaycastResult> results)
        {
            return results.Any(t => t.gameObject.GetComponent<Swatch>() != null);
        }

        private void SaveSwatches()
        {
            for (int i = 0; i < swatches.Count; i++)
            {
                swatches[i].SetIndex(i);
                PlayerPrefs.SetString(swatchKey + i, swatches[i].hsva.ToJSon());
            }
        }
        
        private void OnApplicationQuit()
        {
            SaveSwatches();
        }
    }
}
