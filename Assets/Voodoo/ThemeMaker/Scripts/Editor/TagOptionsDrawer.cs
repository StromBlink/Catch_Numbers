using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voodoo.LevelDesign
{
    [CustomPropertyDrawer(typeof(TagOptions), true)]
    public class TagOptionsDrawer : PropertyDrawer
    {
        SerializedProperty m_Application;
        SerializedProperty m_Iterator;
        SerializedProperty m_EndProp;

        GUIContent m_ApplicationLabel;
        Rect m_ApplicationRect;
        Rect m_PropertiesBoxRect;
        Rect m_NextPropertyRect;

        bool m_AdvancedApplication;
        float m_Height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property.isExpanded = false;
            m_Height = EditorGUIUtility.singleLineHeight;

            m_Application = property.FindPropertyRelative("m_Application");

            m_Iterator = property.Copy();
            m_Iterator.NextVisible(true);

            if (m_Application.enumValueIndex > 0)
            {
                m_EndProp = property.GetEndProperty();

                while (m_Iterator.NextVisible(false) && m_Iterator.propertyPath != m_EndProp.propertyPath)
                    m_Height += EditorGUI.GetPropertyHeight(m_Iterator, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            m_Height += EditorGUIUtility.standardVerticalSpacing * 3;

            return m_Height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = false;
            m_Application = property.FindPropertyRelative("m_Application");
            m_ApplicationRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            m_AdvancedApplication = m_Application.enumValueIndex == 1;

            GUI.Label(new Rect(m_ApplicationRect.x, m_ApplicationRect.y, EditorGUIUtility.labelWidth, m_ApplicationRect.height), label);
            if (GUI.Button(new Rect(m_ApplicationRect.x + EditorGUIUtility.labelWidth, m_ApplicationRect.y, m_ApplicationRect.width - EditorGUIUtility.labelWidth, m_ApplicationRect.height), new GUIContent(m_AdvancedApplication ? "Advanced Control" : "Use Main Color")))
                m_AdvancedApplication = !m_AdvancedApplication;

            m_Application.enumValueIndex = m_AdvancedApplication ? 1 : 0;

            m_Iterator = property.Copy();
            m_Iterator.NextVisible(true);

            if (m_Application.enumValueIndex > 0)
            {
                m_PropertiesBoxRect = new Rect(position.x, position.y + m_ApplicationRect.height, position.width, position.height - m_ApplicationRect.height - EditorGUIUtility.standardVerticalSpacing * 3);
                GUI.Box(EditorGUI.IndentedRect(m_PropertiesBoxRect), GUIContent.none);
                EditorGUI.indentLevel++;
                m_EndProp = property.GetEndProperty();
                m_Height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                while (m_Iterator.NextVisible(false) && m_Iterator.propertyPath != m_EndProp.propertyPath)
                {
                    m_NextPropertyRect = new Rect(position.x, position.y + m_Height, position.width, EditorGUI.GetPropertyHeight(m_Iterator, true));
                    EditorGUI.PropertyField(m_NextPropertyRect, m_Iterator, true);
                    m_Height += m_NextPropertyRect.height + EditorGUIUtility.standardVerticalSpacing;
                }
                EditorGUI.indentLevel--;
            }
            else
                m_PropertiesBoxRect = new Rect(position.x, position.y + m_ApplicationRect.height, position.width, 0);

            GUI.Box(EditorGUI.IndentedRect(new Rect(position.x, m_PropertiesBoxRect.yMax + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.standardVerticalSpacing)), GUIContent.none);
        }
    }
}