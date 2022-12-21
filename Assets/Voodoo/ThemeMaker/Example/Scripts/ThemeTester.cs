using UnityEngine;

namespace Voodoo.LevelDesign
{
    public class ThemeTester : MonoBehaviour
    {
        public ThemeTagger m_ChangeTagTarget;
        [ThemeTagReference]
        public string m_NewTag;
        private string m_TargetOriginalTag;
        private bool m_ToggleTagTarget;

        [Space]
        public SpriteRenderer m_ScriptAssignedColor;
        [ThemeTagReference]
        public string m_ScriptAssignedTagA;
        [ThemeTagReference]
        public string m_ScriptAssignedTagB;
        private bool m_ToggleColorTarget;

        // Start is called before the first frame update
        void Start()
        {
            if (m_ChangeTagTarget)
                m_TargetOriginalTag = m_ChangeTagTarget.m_Tag;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ThemesManager.ChangeTheme();

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (m_ChangeTagTarget)
                {
                    m_ChangeTagTarget.ChangeTag(m_ToggleTagTarget ? m_TargetOriginalTag : m_NewTag);
                    m_ToggleTagTarget = !m_ToggleTagTarget;
                }

                if (m_ScriptAssignedColor)
                {
                    m_ScriptAssignedColor.color = ThemesManager.GetColor(m_ToggleColorTarget ? m_ScriptAssignedTagB : m_ScriptAssignedTagA);
                    m_ToggleColorTarget = !m_ToggleColorTarget;
                }
            }
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.Scale(Vector3.one * 3.5f);
            GUILayout.BeginVertical(GUILayout.Width(Screen.width / 3.5f));
            GUILayout.Label("Spacebar: switch to a random theme\nCurrent theme: " + ThemesManager.s_CurrentTheme.name);
            GUILayout.Label("C: change the tag/color of the top objects");
            GUILayout.EndVertical();
            GUI.matrix = Matrix4x4.identity;
        }
    }
}
