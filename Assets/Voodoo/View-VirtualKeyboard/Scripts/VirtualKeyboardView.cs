using UnityEngine.UI;
using Voodoo.Pattern;

using TMPro;
using System.Runtime.CompilerServices;

namespace Voodoo.Meta
{
    public class VirtualKeyboardView : View<VirtualKeyboardView>
    {
        public TMP_Text m_InputField;
        public int m_MaxCharCount = 20;

        // Cache
        private string m_Word;

        public override void Show([CallerFilePath] string callerFilePath = "")
        {
            base.Show();

            OnClear();
        }

        public void OnLetterPressed(string _Letter)
        {
            if (string.IsNullOrEmpty(m_Word))
                m_Word = _Letter;
            else if (m_Word.Length < m_MaxCharCount)
                m_Word += _Letter;

            Refresh();
        }

        public void OnValidate()
        {
            // TODO : Validation
        }

        public void OnBackslash()
        {
            if (string.IsNullOrEmpty(m_Word))
                return;

            if (m_Word.Length == 1)
            {
                OnClear();
                return;
            }

            m_Word = m_Word.Substring(0, m_Word.Length - 1);
            Refresh();
        }

        public void OnClear()
        {
            m_Word = "";
            Refresh();
        }

        private void Refresh()
        {
            m_InputField.text = m_Word;
        }
    }
}