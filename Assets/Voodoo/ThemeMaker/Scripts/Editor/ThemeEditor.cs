using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voodoo.LevelDesign
{
	[CustomEditor(typeof(Theme)), CanEditMultipleObjects]
	public class ThemeEditor : Editor
	{
		SerializedProperty m_Tags;
		SerializedProperty m_Lighting;

		SerializedProperty m_TagIterator;
		SerializedProperty m_TagEnd;

		GUIStyle m_BoxStyle;
		Color m_TagColor;

		List<SerializedProperty> m_ValidTags = new List<SerializedProperty>();

		ThemesManager m_Manager;

		private void OnEnable()
		{
			m_Tags = serializedObject.FindProperty("m_Tags");
			m_Lighting = serializedObject.FindProperty("m_Lighting");
			m_Manager = ThemeManagerEditor.GetManager;
			CheckTags();
		}

		public override void OnInspectorGUI()
		{
			if (!m_Manager)
				m_Manager = ThemeManagerEditor.GetManager;

			if (!m_Manager)
			{
				return;
			}

			if (m_Manager.m_AllTags.Count != m_ValidTags.Count)
				CheckTags();

			serializedObject.Update();

			for (int i = 0; i < m_ValidTags.Count; i++)
				TagInspector(m_ValidTags[i], i);

			GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.standardVerticalSpacing));
			TagInspector(m_Lighting, -1);
			serializedObject.ApplyModifiedProperties();

			if (Application.isPlaying && m_Manager.m_ApplyThemeChangesAtRuntime && ThemesManager.s_CurrentTheme == serializedObject.targetObject)
				ThemesManager.RefreshTheme();
		}

		public static void CheckTags(Theme _Theme)
		{
			if (!_Theme)
				return;

			ThemeEditor themeEd = CreateEditor(_Theme) as ThemeEditor;
			themeEd.CheckTags();
		}

		private void CheckTags()
		{
			if (!m_Manager)
				return;

			serializedObject.Update();
			m_ValidTags.Clear();

			bool foundTag = false;

			for (int i = 0; i < m_Manager.m_AllTags.Count; i++)
			{
				foundTag = false;
				for (int j = 0; j < m_Tags.arraySize; j++)
				{
					if (m_Tags.GetArrayElementAtIndex(j).FindPropertyRelative("m_TagName").stringValue == m_Manager.m_AllTags[i])
					{
						m_ValidTags.Add(m_Tags.GetArrayElementAtIndex(j));
						foundTag = true;
						break;
					}
				}

				if (!foundTag)
				{
					m_Tags.InsertArrayElementAtIndex(m_Tags.arraySize);
					SerializedProperty newTag = m_Tags.GetArrayElementAtIndex(m_Tags.arraySize - 1);
					newTag.FindPropertyRelative("m_TagName").stringValue = m_Manager.m_AllTags[i];
					newTag.FindPropertyRelative("m_MainColor").colorValue = Color.white;
					m_ValidTags.Add(m_Tags.GetArrayElementAtIndex(m_Tags.arraySize - 1));
				}
			}

			for (int i = 0 ; i < m_Tags.arraySize ; i++)
			{
				bool isContained = false;
				for (int j = 0 ; j < m_Manager.m_AllTags.Count ; j++)
				{
					if (m_Tags.GetArrayElementAtIndex(i).FindPropertyRelative("m_TagName").stringValue == m_Manager.m_AllTags[j])
					{
						isContained = true;
						break;
					}
				}

				if (isContained == false)
				{
					m_Tags.DeleteArrayElementAtIndex(i);
					--i;
				}
			}
			serializedObject.ApplyModifiedProperties();
		}

		private void TagInspector(SerializedProperty _Tag, int _Index)
		{
			m_TagIterator = _Tag.Copy();
			m_TagEnd = _Tag.GetEndProperty();

			Color guiColor = GUI.color;

			Random.InitState(_Index);

			m_TagColor = Random.ColorHSV(0f, 1f, 0.25f, 0.25f, 0.5f, 0.75f);
			m_TagColor.a = 0.25f;

			GUI.color = m_TagColor;

			if (m_BoxStyle == null)
			{
				m_BoxStyle = new GUIStyle("box");
				m_BoxStyle.normal.background = Texture2D.whiteTexture;
			}

			EditorGUILayout.BeginVertical(m_BoxStyle);

			GUI.color = guiColor;

			m_TagIterator.NextVisible(true);

			EditorGUILayout.BeginHorizontal();

			if (m_TagIterator.propertyType == SerializedPropertyType.String)
				GUILayout.Label(m_TagIterator.stringValue, EditorStyles.whiteLargeLabel);
			else
			{
				GUILayout.Label(_Tag.displayName, EditorStyles.whiteLargeLabel);
				_Tag.isExpanded = true;
			}
			//if (GUILayout.Button("Copy", EditorStyles.miniButton))
			//    m_CopyBuffer = _Tag.Copy();

			//EditorGUI.BeginDisabledGroup(m_CopyBuffer == null);
			//if (GUILayout.Button("Paste"))
			//    serializedObject.CopyFromSerializedProperty(m_CopyBuffer);
			//EditorGUI.EndDisabledGroup();

			EditorGUILayout.EndHorizontal();
			//_Tag.isExpanded = EditorGUILayout.Toggle( _Tag.isExpanded, EditorStyles.whiteLargeLabel);

			if (_Tag.isExpanded)
			{
				if (m_TagIterator.propertyType == SerializedPropertyType.String)
				{
					m_TagIterator.NextVisible(false);
					EditorGUILayout.PropertyField(m_TagIterator);

					_Tag.isExpanded = EditorGUILayout.Foldout(_Tag.isExpanded, new GUIContent("Advanced Options"), true);
				}
				else
					EditorGUILayout.PropertyField(m_TagIterator);

				while (m_TagIterator.NextVisible(false) && m_TagIterator.propertyPath != m_TagEnd.propertyPath)
					EditorGUILayout.PropertyField(m_TagIterator, true);
			}
			else
			{
				EditorGUILayout.PropertyField(_Tag.FindPropertyRelative("m_MainColor"));
				_Tag.isExpanded = EditorGUILayout.Foldout(_Tag.isExpanded, new GUIContent("Advanced Options"), true);
			}

			EditorGUILayout.EndVertical();
		}
	}
}