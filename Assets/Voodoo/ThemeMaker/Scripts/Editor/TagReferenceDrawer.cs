using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voodoo.LevelDesign
{
    [CustomPropertyDrawer(typeof(ThemeTagReferenceAttribute))]
    public class TagReferenceDrawer : PropertyDrawer
    {
        ThemeTagReferenceAttribute m_Attribute;
        GUIContent m_Label;
        Rect m_PropertyRect;
        ThemesManager m_Manager;
        int m_ChosenTag;
        string[] m_AvailableTags = new string[0];
        int m_IndentLevel;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            m_Manager = ThemeManagerEditor.GetManager;

            if (m_Manager)
            {
                if (m_Manager.m_AllTags.Count > 0)
                    return EditorGUIUtility.singleLineHeight;
            }

            return EditorGUIUtility.singleLineHeight * 2f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                m_Label = new GUIContent(label.text + " is not a string, it cannot have the ThemeTagReference attribute!");
                GUI.Label(position, m_Label);
                return;
            }

            m_Manager = ThemeManagerEditor.GetManager;

            if (!m_Manager)
            {
                EditorGUI.HelpBox(position, "The project does not have a ThemeManager, create one by clicking on 'Voodoo -> ThemeMaker -> Edit Manager'", MessageType.Error);
                return;
            }

            m_AvailableTags = m_Manager.m_AllTags.ToArray();

            if (m_AvailableTags.Length == 0)
            {
                EditorGUI.HelpBox(position, "The project does not have any Theme Tags, edit them on the ThemeManager by clicking on 'Voodoo -> ThemeMaker -> Edit Manager'", MessageType.Warning);
                return;
            }

            m_Label = EditorGUI.BeginProperty(position, label, property);
            m_PropertyRect = EditorGUI.PrefixLabel(position, m_Label);

            m_IndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            m_ChosenTag = 0;

            for (int i = 0; i < m_AvailableTags.Length; i++)
            {
                if (property.stringValue == m_AvailableTags[i])
                {
                    m_ChosenTag = i;
                    break;
                }
            }

            m_ChosenTag = EditorGUI.Popup(m_PropertyRect, m_ChosenTag, m_AvailableTags);
            property.stringValue = m_AvailableTags[m_ChosenTag];
            EditorGUI.EndProperty();

            EditorGUI.indentLevel = m_IndentLevel;
        }
    }
}