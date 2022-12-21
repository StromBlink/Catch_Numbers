using System;
using UnityEngine;

namespace Voodoo.Utils
{
    public class GUIButton : IWidget, ISelectable
    {
        public bool IsSelected { get; set; }
        public bool isUsable { get; set; } = true;

        private Action action;

        protected GUIContent guiContent;

        protected GUILayoutOption[] layoutOptions { get; set; }

        public GUIStyle guiStyle { get; set; }

        public void Initialize(string name, Action action, string tooltip)
        {
            guiContent = EditorHelper.GetIcon(name);
            guiContent.tooltip = tooltip;
            this.action = action;
        }

        public void Initialize(string name, Action action, string tooltip, GUIStyle guiStyle)
        {
            Initialize(name, action, tooltip, guiStyle, null);
        }

        public void Initialize(string name, Action action, string tooltip, params GUILayoutOption[] layoutOptions)
        {
            Initialize(name, action, tooltip, null, layoutOptions);
        }

        public void Initialize(string name, Action action, string tooltip, GUIStyle guiStyle = null, params GUILayoutOption[] layoutOptions)
        {
            guiContent = EditorHelper.GetIcon(name);
            guiContent.tooltip = tooltip;
            this.action = action;
            this.guiStyle = guiStyle;
            this.layoutOptions = layoutOptions;
        }


        public virtual void OnGUI()
        {
            if (guiContent == null)
            {
                return;
            }

            if (guiStyle == null)
            {
                guiStyle = GUI.skin.button;
            }

            if (GUILayout.Button(guiContent, guiStyle, layoutOptions))
            {
                Execute();
            }
        }

        public virtual void Execute()
        {
            if (isUsable == false)
            {
                return;
            }
            
            action?.Invoke();
        }
    }
}