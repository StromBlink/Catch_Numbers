using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Voodoo.LevelDesign
{
	[CustomEditor(typeof(ThemesManager))]
	public class ThemeManagerEditor : Editor
	{
		private const string c_ResourcesPath = "Assets/Voodoo/ThemeMaker/Resources";
		private const string c_ManageFileName = "ThemesManager";
		private const string c_ManagerPath = c_ResourcesPath + "/" + c_ManageFileName + ".asset";
		private const string c_ThemesPath = "Assets/Voodoo/ThemeMaker/Resources/GameThemes";

		private static ThemesManager s_Manager;

		SerializedProperty m_Iterator;
		SerializedProperty m_AllTags;

		ThemesManager m_TargetScript;

		private string m_NewTagString;
		private int m_DeleteConfirmIndex = -1;

		private List<Theme> m_AllThemes = new List<Theme>();

		public static ThemesManager GetManager
		{
			get
			{
				if (!s_Manager)
					s_Manager = Resources.Load<ThemesManager>(c_ManageFileName);

				return s_Manager;
			}
		}

		[MenuItem("Voodoo/ThemeMaker/Edit Manager")]
		private static void CreateManager()
		{
			if (s_Manager)
			{
				Selection.activeObject = s_Manager;
				return;
			}

			s_Manager = Resources.Load<ThemesManager>(c_ManageFileName);

			if (s_Manager)
			{
				Selection.activeObject = s_Manager;
				return;
			}

			s_Manager = CreateInstance<ThemesManager>();

			if (!Directory.Exists(c_ResourcesPath))
				Directory.CreateDirectory(c_ResourcesPath);

			AssetDatabase.CreateAsset(s_Manager, c_ManagerPath);
			AssetDatabase.SaveAssets();
			Selection.activeObject = s_Manager;
		}

		[MenuItem("Voodoo/ThemeMaker/Create New Theme")]
		private static void CreateTheme()
		{
			Theme asset = CreateInstance<Theme>();

			string path = c_ThemesPath;

			if (!Directory.Exists(c_ThemesPath))
				Directory.CreateDirectory(c_ThemesPath);

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/Theme.asset");

			AssetDatabase.CreateAsset(asset, assetPathAndName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		[MenuItem("Voodoo/ThemeMaker/Edit Themes")]
		private static void OpenThemesFolder()
		{
			Object[] themes = Resources.LoadAll<Theme>("GameThemes");

			if (themes.Length == 0)
				CreateTheme();
			else
				Selection.activeObject = themes[0];
		}

		private void OnEnable()
		{
			m_AllTags = serializedObject.FindProperty("m_AllTags");
			m_TargetScript = (ThemesManager)target;
			m_AllThemes.Clear();
			m_AllThemes.AddRange(Resources.LoadAll<Theme>("GameThemes"));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			m_Iterator = serializedObject.GetIterator();
			m_Iterator.NextVisible(true);

			while (m_Iterator.NextVisible(false))
			{
				if (m_Iterator.propertyPath == m_AllTags.propertyPath)
					EditTags();
				else
					EditorGUILayout.PropertyField(m_Iterator, true);
			}

			serializedObject.ApplyModifiedProperties();

			GUILayout.Label(new GUIContent("Themes"), EditorStyles.largeLabel);

			if (m_AllThemes != null)
			{
				if (m_AllThemes.Count > 0)
				{
					for (int i = 0; i < m_AllThemes.Count; i++)
					{
						if (!m_AllThemes[i])
							continue;

						if (GUILayout.Button(m_AllThemes[i].name, GUILayout.Height(25)))
							Selection.activeObject = m_AllThemes[i];
					}
				}
			}

			if (GUILayout.Button("Create New"))
				CreateTheme();
		}

		private void EditTags()
		{
			for (int i = 0; i < m_AllTags.arraySize; i++)
			{
				EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginDisabledGroup(i == 0);
				if (GUILayout.Button("^", EditorStyles.miniButtonLeft, GUILayout.Width(20f)))
					m_AllTags.MoveArrayElement(i, i - 1);
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup(i == m_AllTags.arraySize - 1);
				if (GUILayout.Button("v", EditorStyles.miniButtonRight, GUILayout.Width(20f)))
					m_AllTags.MoveArrayElement(i, i + 1);
				EditorGUI.EndDisabledGroup();

				GUILayout.Label(m_AllTags.GetArrayElementAtIndex(i).stringValue);

				if (m_DeleteConfirmIndex == i)
				{
					if (GUILayout.Button("Confirm", EditorStyles.miniButton, GUILayout.Width(50f)))
					{
						m_AllTags.DeleteArrayElementAtIndex(i);
						if (m_AllThemes?.Count >= 0)
						{
							for (int t = 0 ; t < m_AllThemes.Count ; t++)
							{
								if (m_AllThemes[t])
									ThemeEditor.CheckTags(m_AllThemes[t]);
							}
						}
						m_DeleteConfirmIndex = -1;
					}

					if (GUILayout.Button("Cancel", EditorStyles.miniButton, GUILayout.Width(50f)))
						m_DeleteConfirmIndex = -1;
				}
				else
				{
					if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(25f)))
						m_DeleteConfirmIndex = i;
				}

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.BeginHorizontal();
			m_NewTagString = EditorGUILayout.TextField(m_NewTagString);

			EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_NewTagString) || m_TargetScript.m_AllTags.Contains(m_NewTagString));
			if (GUILayout.Button("Create New Tag"))
				CreateTag();
			EditorGUI.EndDisabledGroup();

			Event e = Event.current;

			if (e.isKey)
			{
				if (e.keyCode == KeyCode.Return)
				{
					e.Use();
					CreateTag();
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		void CreateTag()
		{
			if (string.IsNullOrEmpty(m_NewTagString) || m_TargetScript.m_AllTags.Contains(m_NewTagString))
				return;

			m_AllTags.InsertArrayElementAtIndex(m_AllTags.arraySize);
			m_AllTags.GetArrayElementAtIndex(m_AllTags.arraySize - 1).stringValue = m_NewTagString;
			m_NewTagString = "";

			serializedObject.ApplyModifiedProperties();
			serializedObject.Update();

			if (m_AllThemes != null)
			{
				if (m_AllThemes.Count >= 0)
				{
					for (int i = 0; i < m_AllThemes.Count; i++)
					{
						if (m_AllThemes[i])
							ThemeEditor.CheckTags(m_AllThemes[i]);
					}
				}
			}
		}
	}
}