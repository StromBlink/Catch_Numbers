using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Visual.UI.Button
{
    [CustomEditor(typeof(ButtonHub),true)]
    public class ButtonHubInspector : Editor
    {
        private ButtonHub buttonHub;
        
        //GUIStyle
        private GUIStyle buttonStyle;
        
        //Path
        private const string localPath = "Assets/Voodoo/Buttons/Sprites/Editor/";
        
        //Textures
        private Texture2D addImage;
        private Texture2D deleteImage;
        


        private void OnEnable()
        {
            buttonHub = (ButtonHub)target;
            addImage = (Texture2D)AssetDatabase.LoadAssetAtPath(localPath+"Plus.png",
                typeof(Texture2D));
            deleteImage = (Texture2D)AssetDatabase.LoadAssetAtPath(localPath+"Cross.png",
                typeof(Texture2D));
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Undo.RecordObject(buttonHub, "Undo buttonHub modifications");
            PrefabUtility.RecordPrefabInstancePropertyModifications(buttonHub);
            DrawButtonHub();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawButtonHub()
        {
            if (buttonHub == null)
                return;

            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle (GUI.skin.button);
                buttonStyle.padding = new RectOffset(4,4,2,2);
            }

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    float side = EditorGUIUtility.singleLineHeight*1.25f;
                    EditorGUILayout.PrefixLabel("States");
                    GUILayout.FlexibleSpace();
                    DrawAddAndDelete(side, AddState, ClearStates);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;
                for (var i = 0; i < buttonHub.states.Count; i++)
                {
                    ButtonHubState state = buttonHub.states[i];
                    DrawState(state);
                }

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawState(ButtonHubState state)
        {
            int stateIndex = buttonHub.states.IndexOf(state);
            SerializedProperty statesProperty = serializedObject.FindProperty("states");
            
            if (statesProperty.arraySize <= stateIndex) //FIX for adding a new state in the same frame 
                return;
            
            SerializedProperty currentStateProperty = statesProperty.GetArrayElementAtIndex(stateIndex);
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    float side = EditorGUIUtility.singleLineHeight*1.25f;
                    currentStateProperty.isExpanded = EditorGUILayout.Foldout(currentStateProperty.isExpanded, string.Empty);
                    GUILayout.Space(-50);
                    state.name = EditorGUILayout.TextField(state.name, GUILayout.Width(187));
                    GUILayout.FlexibleSpace();
                    DrawAddAndDelete(side, () => AddButtonHandler(state), () => DeleteState(state));
                }
                EditorGUILayout.EndHorizontal();
                
                if (currentStateProperty.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    for (var i = 0; i < state.buttonHandlers.Count; i++)
                    {
                        ButtonHandler buttonHandler = state.buttonHandlers[i];
                        DrawButtonHandler(state, i);
                    }
                    EditorGUI.indentLevel--;
                }

            }
            EditorGUILayout.EndVertical();
        }

        private void DrawButtonHandler(ButtonHubState state, int buttonHandlerIndex)
        {
            EditorGUILayout.BeginHorizontal();
            {
                float side = EditorGUIUtility.singleLineHeight * 1.25f;
                ButtonHandler buttonHandler = state.buttonHandlers[buttonHandlerIndex];
                state.buttonHandlers[buttonHandlerIndex] = (ButtonHandler)EditorGUILayout.ObjectField(buttonHandler, typeof(ButtonHandler), true);
                if (GUILayout.Button(deleteImage, buttonStyle, GUILayout.Width(side), GUILayout.Height(side)))
                {
                    DeleteButtonHandler(state, buttonHandlerIndex);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawAddAndDelete(float sideSize, Action onAddAction, Action onDeleteAction)
        {
            if (GUILayout.Button(addImage, buttonStyle, GUILayout.Width(sideSize), GUILayout.Height(sideSize)))
            {
                onAddAction?.Invoke();
            }
            if (GUILayout.Button(deleteImage, buttonStyle, GUILayout.Width(sideSize), GUILayout.Height(sideSize)))
            {
                onDeleteAction?.Invoke();
            }
        }
        
        private void AddState()
        {
            ButtonHubState newState = new ButtonHubState();
            newState.name = "State " + buttonHub.states.Count;
            newState.buttonHandlers = new List<ButtonHandler>();
            buttonHub.states.Add(newState);
        }
        
        private void ClearStates()
        {
            buttonHub.states.Clear();
        }

        private void AddButtonHandler(ButtonHubState state)
        {
            state.buttonHandlers.Insert(state.buttonHandlers.Count,state.buttonHandlers.LastOrDefault());
        }

        private void DeleteState(ButtonHubState state)
        {
            buttonHub.states.Remove(state);
        }

        private void DeleteButtonHandler(ButtonHubState state, int buttonHandlerIndex)
        {
            if (state?.buttonHandlers?.Count > buttonHandlerIndex)
                state.buttonHandlers.RemoveAt(buttonHandlerIndex);
        }
    }
}