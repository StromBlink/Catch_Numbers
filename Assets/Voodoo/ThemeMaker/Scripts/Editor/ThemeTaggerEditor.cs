using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace Voodoo.LevelDesign
{
    [CustomEditor(typeof(ThemeTagger))]
    public class ThemeTaggerEditor : Editor
    {
        SerializedProperty m_Tag;

        SerializedProperty m_Meshes;
        SerializedProperty m_SkinnedMeshes;
        SerializedProperty m_Sprites;

        SerializedProperty m_Lines;
        SerializedProperty m_Trails;
        SerializedProperty m_Particles;

        SerializedProperty m_Images;
        SerializedProperty m_Texts;
        SerializedProperty m_TextMeshPros;

        SerializedProperty m_Lights;

        private ThemeTagger m_TargetScript;

        private GameObject m_DraggedGO;

        private List<MeshRenderer> m_MeshesBuffer = new List<MeshRenderer>();
        private List<SkinnedMeshRenderer> m_SkinnedMeshesBuffer = new List<SkinnedMeshRenderer>();
        private List<SpriteRenderer> m_SpritesBuffer = new List<SpriteRenderer>();

        private List<LineRenderer> m_LinesBuffer = new List<LineRenderer>();
        private List<TrailRenderer> m_TrailsBuffer = new List<TrailRenderer>();
        private List<ParticleSystem> m_ParticlesBuffer = new List<ParticleSystem>();

        private List<Image> m_ImagesBuffer = new List<Image>();
        private List<Text> m_TextsBuffer = new List<Text>();
        private List<TextMeshProUGUI> m_TextMeshProsBuffer = new List<TextMeshProUGUI>();

        private List<Light> m_LightsBuffer = new List<Light>();

        private void OnEnable()
        {
            m_Tag = serializedObject.FindProperty("m_Tag");

            m_Meshes = serializedObject.FindProperty("m_Meshes");
            m_SkinnedMeshes = serializedObject.FindProperty("m_SkinnedMeshes");
            m_Sprites = serializedObject.FindProperty("m_Sprites");

            m_Lines = serializedObject.FindProperty("m_Lines");
            m_Trails = serializedObject.FindProperty("m_Trails");
            m_Particles = serializedObject.FindProperty("m_Particles");

            m_Images = serializedObject.FindProperty("m_Images");
            m_Texts = serializedObject.FindProperty("m_Texts");
            m_TextMeshPros = serializedObject.FindProperty("m_TextMeshPros");

            m_Lights = serializedObject.FindProperty("m_Lights");

            m_TargetScript = (ThemeTagger)target;
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((ThemeTagger)target), typeof(ThemeTagger), false);
            GUI.enabled = true;
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Tag);

            DragObjectsArea();

            Event e = Event.current;

            if (GUILayout.Button("Auto Select Children"))
                AutoSelect();

            if (e.type != EventType.DragPerform)
            {
                DrawList(m_Meshes);
                DrawList(m_SkinnedMeshes);
                DrawList(m_Sprites);

                DrawList(m_Lines);
                DrawList(m_Trails);
                DrawList(m_Particles);

                DrawList(m_Images);
                DrawList(m_Texts);
                DrawList(m_TextMeshPros);

                DrawList(m_Lights);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AutoSelect()
        {
            if (m_TargetScript.transform.childCount == 0)
                return;

            m_MeshesBuffer.Clear();
            m_SkinnedMeshesBuffer.Clear();
            m_SpritesBuffer.Clear();
            m_LinesBuffer.Clear();
            m_TrailsBuffer.Clear();
            m_ParticlesBuffer.Clear();
            m_ImagesBuffer.Clear();
            m_TextsBuffer.Clear();
            m_TextMeshProsBuffer.Clear();
            m_LightsBuffer.Clear();

            if (m_TargetScript.GetComponentsInChildren<MeshRenderer>().Length > 0)
                m_MeshesBuffer.AddRange(m_TargetScript.GetComponentsInChildren<MeshRenderer>());
            
            if (m_TargetScript.GetComponentsInChildren<SkinnedMeshRenderer>().Length > 0)
                m_SkinnedMeshesBuffer.AddRange(m_TargetScript.GetComponentsInChildren<SkinnedMeshRenderer>());

            if (m_TargetScript.GetComponentsInChildren<SpriteRenderer>().Length > 0)
                m_SpritesBuffer.AddRange(m_TargetScript.GetComponentsInChildren<SpriteRenderer>());


            if (m_TargetScript.GetComponentsInChildren<LineRenderer>().Length > 0)
                m_LinesBuffer.AddRange(m_TargetScript.GetComponentsInChildren<LineRenderer>());

            if (m_TargetScript.GetComponentsInChildren<TrailRenderer>().Length > 0)
                m_TrailsBuffer.AddRange(m_TargetScript.GetComponentsInChildren<TrailRenderer>());

            if (m_TargetScript.GetComponentsInChildren<ParticleSystem>().Length > 0)
                m_ParticlesBuffer.AddRange(m_TargetScript.GetComponentsInChildren<ParticleSystem>());


            if (m_TargetScript.GetComponentsInChildren<Image>().Length > 0)
                m_ImagesBuffer.AddRange(m_TargetScript.GetComponentsInChildren<Image>());

            if (m_TargetScript.GetComponentsInChildren<Text>().Length > 0)
                m_TextsBuffer.AddRange(m_TargetScript.GetComponentsInChildren<Text>());

            if (m_TargetScript.GetComponentsInChildren<TextMeshProUGUI>().Length > 0)
                m_TextMeshProsBuffer.AddRange(m_TargetScript.GetComponentsInChildren<TextMeshProUGUI>());


            if (m_TargetScript.GetComponentsInChildren<Light>().Length > 0)
                m_LightsBuffer.AddRange(m_TargetScript.GetComponentsInChildren<Light>());

            if (ValidDrag)
                ApplyDragAndDrop();
        }

        private void DragObjectsArea()
        {
            Event evt = Event.current;
            Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(drop_area, "Add Objects");

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return;

                    CheckForValidObjects();

                    if(!ValidDrag)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        return;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        ApplyDragAndDrop();
                    }
                    break;
            }
        }

        private bool ValidDrag
        {
            get
            {
                return m_MeshesBuffer.Count + m_SkinnedMeshesBuffer.Count + m_SpritesBuffer.Count + m_LinesBuffer.Count + m_TrailsBuffer.Count + m_ParticlesBuffer.Count + m_ImagesBuffer.Count + m_TextsBuffer.Count + m_TextMeshProsBuffer.Count + m_LightsBuffer.Count > 0;
            }
        }

        private void CheckForValidObjects()
        {
            m_MeshesBuffer.Clear();
            m_SkinnedMeshesBuffer.Clear();
            m_SpritesBuffer.Clear();
            m_LinesBuffer.Clear();
            m_TrailsBuffer.Clear();
            m_ParticlesBuffer.Clear();
            m_ImagesBuffer.Clear();
            m_TextsBuffer.Clear();
            m_TextMeshProsBuffer.Clear();
            m_LightsBuffer.Clear();

            foreach (Object dragged_object in DragAndDrop.objectReferences)
            {
                if (!(dragged_object is GameObject))
                    continue;

                m_DraggedGO = dragged_object as GameObject;

                if (m_DraggedGO.GetComponent<MeshRenderer>())
                    m_MeshesBuffer.Add(m_DraggedGO.GetComponent<MeshRenderer>());
                if (m_DraggedGO.GetComponent<SkinnedMeshRenderer>())
                    m_SkinnedMeshesBuffer.Add(m_DraggedGO.GetComponent<SkinnedMeshRenderer>());

                if (m_DraggedGO.GetComponent<SpriteRenderer>())
                    m_SpritesBuffer.Add(m_DraggedGO.GetComponent<SpriteRenderer>());


                if (m_DraggedGO.GetComponent<LineRenderer>())
                    m_LinesBuffer.Add(m_DraggedGO.GetComponent<LineRenderer>());

                if (m_DraggedGO.GetComponent<TrailRenderer>())
                    m_TrailsBuffer.Add(m_DraggedGO.GetComponent<TrailRenderer>());

                if (m_DraggedGO.GetComponent<ParticleSystem>())
                    m_ParticlesBuffer.Add(m_DraggedGO.GetComponent<ParticleSystem>());


                if (m_DraggedGO.GetComponent<Image>())
                    m_ImagesBuffer.Add(m_DraggedGO.GetComponent<Image>());

                if (m_DraggedGO.GetComponent<Text>())
                    m_TextsBuffer.Add(m_DraggedGO.GetComponent<Text>());

                if (m_DraggedGO.GetComponent<TextMeshProUGUI>())
                    m_TextMeshProsBuffer.Add(m_DraggedGO.GetComponent<TextMeshProUGUI>());


                if (m_DraggedGO.GetComponent<Light>())
                    m_LightsBuffer.Add(m_DraggedGO.GetComponent<Light>());
            }
        }

        private void ApplyDragAndDrop()
        {
            bool alreadyInList = false;

            //Meshes
            if (m_MeshesBuffer.Count > 0)
            {
                for (int i = 0; i < m_MeshesBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Meshes.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Meshes.Length; j++)
                        {
                            if (m_TargetScript.m_Meshes[j] == m_MeshesBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Meshes.InsertArrayElementAtIndex(m_Meshes.arraySize);
                    m_Meshes.GetArrayElementAtIndex(m_Meshes.arraySize - 1).objectReferenceValue = m_MeshesBuffer[i];
                }
            }
            
            if (m_SkinnedMeshesBuffer.Count > 0)
            {
                for (int i = 0; i < m_SkinnedMeshesBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_SkinnedMeshes.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_SkinnedMeshes.Length; j++)
                        {
                            if (m_TargetScript.m_SkinnedMeshes[j] == m_SkinnedMeshesBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_SkinnedMeshes.InsertArrayElementAtIndex(m_SkinnedMeshes.arraySize);
                    m_SkinnedMeshes.GetArrayElementAtIndex(m_SkinnedMeshes.arraySize - 1).objectReferenceValue = m_SkinnedMeshesBuffer[i];
                }
            }

            //Sprites
            if (m_SpritesBuffer.Count > 0)
            {
                for (int i = 0; i < m_SpritesBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Sprites.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Sprites.Length; j++)
                        {
                            if (m_TargetScript.m_Sprites[j] == m_SpritesBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Sprites.InsertArrayElementAtIndex(m_Sprites.arraySize);
                    m_Sprites.GetArrayElementAtIndex(m_Sprites.arraySize - 1).objectReferenceValue = m_SpritesBuffer[i];
                }
            }

            //Lines
            if (m_LinesBuffer.Count > 0)
            {
                for (int i = 0; i < m_LinesBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Lines.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Lines.Length; j++)
                        {
                            if (m_TargetScript.m_Lines[j] == m_LinesBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Lines.InsertArrayElementAtIndex(m_Lines.arraySize);
                    m_Lines.GetArrayElementAtIndex(m_Lines.arraySize - 1).objectReferenceValue = m_LinesBuffer[i];
                }
            }

            //Trails
            if (m_TrailsBuffer.Count > 0)
            {
                for (int i = 0; i < m_TrailsBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Trails.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Trails.Length; j++)
                        {
                            if (m_TargetScript.m_Trails[j] == m_TrailsBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Trails.InsertArrayElementAtIndex(m_Trails.arraySize);
                    m_Trails.GetArrayElementAtIndex(m_Trails.arraySize - 1).objectReferenceValue = m_TrailsBuffer[i];
                }
            }

            //Particles
            if (m_ParticlesBuffer.Count > 0)
            {
                for (int i = 0; i < m_ParticlesBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Particles.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Particles.Length; j++)
                        {
                            if (m_TargetScript.m_Particles[j] == m_ParticlesBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Particles.InsertArrayElementAtIndex(m_Particles.arraySize);
                    m_Particles.GetArrayElementAtIndex(m_Particles.arraySize - 1).objectReferenceValue = m_ParticlesBuffer[i];
                }
            }

            //Images
            if (m_ImagesBuffer.Count > 0)
            {
                for (int i = 0; i < m_ImagesBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Images.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Images.Length; j++)
                        {
                            if (m_TargetScript.m_Images[j] == m_ImagesBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Images.InsertArrayElementAtIndex(m_Images.arraySize);
                    m_Images.GetArrayElementAtIndex(m_Images.arraySize - 1).objectReferenceValue = m_ImagesBuffer[i];
                }
            }

            //Texts
            if (m_TextsBuffer.Count > 0)
            {
                for (int i = 0; i < m_TextsBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Texts.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Texts.Length; j++)
                        {
                            if (m_TargetScript.m_Texts[j] == m_TextsBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Texts.InsertArrayElementAtIndex(m_Texts.arraySize);
                    m_Texts.GetArrayElementAtIndex(m_Texts.arraySize - 1).objectReferenceValue = m_TextsBuffer[i];
                }
            }

            //TextMeshPros
            if (m_TextMeshProsBuffer.Count > 0)
            {
                for (int i = 0; i < m_TextMeshProsBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_TextMeshPros.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_TextMeshPros.Length; j++)
                        {
                            if (m_TargetScript.m_TextMeshPros[j] == m_TextMeshProsBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_TextMeshPros.InsertArrayElementAtIndex(m_TextMeshPros.arraySize);
                    m_TextMeshPros.GetArrayElementAtIndex(m_TextMeshPros.arraySize - 1).objectReferenceValue = m_TextMeshProsBuffer[i];
                }
            }

            //Lights
            if (m_LightsBuffer.Count > 0)
            {
                for (int i = 0; i < m_LightsBuffer.Count; i++)
                {
                    alreadyInList = false;

                    if (m_Lights.arraySize > 0)
                    {
                        for (int j = 0; j < m_TargetScript.m_Lights.Length; j++)
                        {
                            if (m_TargetScript.m_Lights[j] == m_LightsBuffer[i])
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                    }

                    if (alreadyInList)
                        continue;

                    m_Lights.InsertArrayElementAtIndex(m_Lights.arraySize);
                    m_Lights.GetArrayElementAtIndex(m_Lights.arraySize - 1).objectReferenceValue = m_LightsBuffer[i];
                }
            }
        }

        private void DrawList(SerializedProperty _Prop)
        {
            if (_Prop.arraySize <= 0)
                return;

            Event e = Event.current;

            GUILayout.Label(_Prop.displayName, EditorStyles.whiteLargeLabel);

            for (int i = 0; i < _Prop.arraySize; i++)
            {
                if (!_Prop.GetArrayElementAtIndex(i).objectReferenceValue)
                    continue;

                EditorGUILayout.BeginHorizontal();
                
                GUI.enabled = false;
                EditorGUILayout.ObjectField(GUIContent.none, _Prop.GetArrayElementAtIndex(i).objectReferenceValue, _Prop.GetArrayElementAtIndex(i).objectReferenceValue.GetType(), false);
                GUI.enabled = true;

                if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(25f)))
                {
                    if (_Prop.GetArrayElementAtIndex(i).objectReferenceValue != null)
                        _Prop.DeleteArrayElementAtIndex(i);

                    _Prop.DeleteArrayElementAtIndex(i);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
