using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
    public static class WidgetExtension
    {
        public static WidgetWindow ToWindow<T>(this T widget) where T : IWidget
        {
            WidgetWindow window = ScriptableObject.CreateInstance<WidgetWindow>();
            window.Widget = widget;

            return window;
        }

        public static ValidationWidgetWindow ToValidationWindow<T>(this T widget) where T : IWidget
        {
            ValidationWidgetWindow window = EditorWindow.CreateWindow<ValidationWidgetWindow>();
            window.Widget = widget;

            return window;
        }

        public static T Prompt<T>(this T value)
        {
            var prompt = new PromptWidget<T> { instance = value };
            WidgetWindow window = prompt.ToWindow();
#if UNITY_2019_3_OR_NEWER
            window.ShowModal();
#else
			Type windowType = window.GetType(); 
 
			MethodInfo showModal = windowType.GetMethod("ShowModal", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
         
			if (showModal!= null)
			{
				showModal?.Invoke(window, null);
			}
#endif
            
            return prompt.instance;
        }

        public static WidgetWindow ShowAsDropDown<T>(this T widget, Rect rect, Vector2 size) where T : IWidget
        {
            var window = widget.ToWindow();

            window.ShowAsDropDown(rect, size);
            return window;
        }
    }
}