using UnityEditor;
using UnityEditor.Compilation;

namespace Voodoo.Utils
{
    public abstract class AbstractEditorWindow : EditorWindow
    {
        protected const byte CompilingLock  = 0b1;
        protected const byte DisableLock    = 0b10;
        protected const byte PlayModeLock   = 0b100;

        protected const int  FilledByte     = 0xf; // == 0b1111

        protected byte       lockers        = 0;

        public bool          IsLocked => lockers != 0;

        protected virtual void Bind()
        {
            CompilationPipeline.assemblyCompilationStarted  += OnCompileStarted;
            CompilationPipeline.assemblyCompilationFinished += OnCompileEnded;
            EditorApplication.playModeStateChanged          += OnPlayModeStateChange;
        }

        protected virtual void Unbind()
        {
            CompilationPipeline.assemblyCompilationStarted  -= OnCompileStarted;
            CompilationPipeline.assemblyCompilationFinished -= OnCompileEnded;
            EditorApplication.playModeStateChanged          -= OnPlayModeStateChange;
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        protected abstract void Dispose();

        protected virtual void OnEnable()
        {
            Bind();
            lockers &= (FilledByte ^ DisableLock);
        }

        protected virtual void OnDisable()
        {
            Unbind();
            lockers |= DisableLock;
        }

        protected virtual void OnCompileStarted(string assembly)
        {
            lockers |= CompilingLock;
        }

        protected virtual void OnCompileEnded(string assembly, CompilerMessage[] messages)
        {
            lockers &= (FilledByte ^ CompilingLock);
        }

        protected virtual void OnPlayModeStateChange(PlayModeStateChange state) 
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                lockers |= PlayModeLock;
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                lockers &= (FilledByte ^ PlayModeLock);
            }
        }
    }
}