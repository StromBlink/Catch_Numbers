using System;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils 
{
    public interface IWidgetWindow 
    {
        IWidget Widget { get; }
    }

    public class WidgetWindow : EditorWindow, IWidgetWindow
    {
        bool _disposed = false;

        public IWidget Widget { get; set; }

        public virtual void OnEnable() 
        {
            if (Widget != null && Widget is IEnable enable)
            {
                enable.OnEnable();
            }
        }

        public virtual void OnGUI()
        {
            Widget?.OnGUI();
        }

        public virtual void OnDisable()
        {
            if (Widget != null && Widget is IEnable enable)
            {
                enable.OnDisable();
            }
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        protected virtual void Dispose() 
        {
            if (_disposed)
            {
                return;
            }

            if (Widget != null && Widget is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _disposed = true;
        }
    }

    public class PromptWidget<T> : IWidget
    {
        public T instance = default;

        public void OnGUI()
        {
            instance = DefaultElementEditor.OnGUI<T>(instance);
        }
    }

    public class ValidationWidgetWindow : WidgetWindow, IValidable
    {
        public event Action validated;

        public override void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            { 
                Widget?.OnGUI();

                if (GUILayout.Button("Confirm"))
                {
                    Close();
                }
            }
            EditorGUILayout.EndVertical();
        }

        public override void OnDisable()
        {
            OnClose();
            base.OnDisable();
        }

        public void OnClose() 
        {
            validated?.Invoke();
        }
    }
}