using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
    //TODO : make it DataGUIExecutor or IWidget so that it can be called similarly to the other inspectors
    public static class DefaultElementEditor
    {
        //TODO later on offer the possibility to check for known predefine specific editors
        public static T OnGUI<T>(T value, GUIContent label) => (T)OnGUI(typeof(T), value, label);
        public static T OnGUI<T>(T value, string label = null) => (T)OnGUI(typeof(T), value, label);

        public static object OnGUI(Type type, object value, GUIContent label)
        {
            if (type.IsValueType) // value type primitive + custom structs
            {
                if (type.IsPrimitive)
                {
                    return DrawPrimitives(type, value, label);
                }
                else if (type.IsEnum)
                {
                    return EditorGUILayout.Popup(label, (int)value, Enum.GetNames(type));
                }

                var unityValueType = DrawUnityValueTypes(type, value, label);
                if (unityValueType != null)
                {
                    return unityValueType;
                }

                // if it's a custom struct do nothing we want to run through all fields just like a class
            }

            if (type.Equals(typeof(string)))
            {
                return EditorGUILayout.TextField(label, value as string);
            }

            object valueCopy = value?.DeepCopy();

            //Past this point value can be null therefore check for nullity and instantiate them if need be
            if (valueCopy == null)
            {
                valueCopy = Instantiate(type);
            }

            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                return DrawDictionary(type, valueCopy as IDictionary, label);
            }

            if (typeof(ICollection).IsAssignableFrom(type))
            {
                // All kind of Collection
                return DrawCollection(type, valueCopy as IList, label);
            }

            // reaching this point it is most certainly a class 
            /*
             * we will browse each and individual fields to give tha adapted editor for each one 
             */
            return DrawReference(valueCopy, label);
        }

        public static object OnGUI(Type type, object value, string label = null) => OnGUI(type, value, new GUIContent(label));

        static object DrawPrimitives(Type type, object value, GUIContent label = null)
        {
            if (type.Equals(typeof(int)))
            {
                return EditorGUILayout.IntField(label, (int)value);
            }

            if (type.Equals(typeof(long)))
            {
                return EditorGUILayout.LongField(label, (long)value);
            }

            if (type.Equals(typeof(float)))
            {
                return EditorGUILayout.FloatField(label, (float)value);
            }

            if (type.Equals(typeof(double)))
            {
                return EditorGUILayout.DoubleField(label, (double)value);
            }

            if (type.Equals(typeof(bool)))
            {
                return EditorGUILayout.Toggle(label, (bool)value);
            }

            EditorGUILayout.LabelField(label + " of type " + (value?.GetType().FullName ?? "unknown") + " is not know to default editor");

            return null;
        }

        static object DrawUnityValueTypes(Type type, object value, GUIContent label = null)
        {
            if (type.Equals(typeof(Color)))
            {
                return EditorGUILayout.ColorField(label, (Color)value);
            }

            if (type.Equals(typeof(Color32)))
            {
                return EditorGUILayout.ColorField(label, (Color32)value);
            }

            if (type.Equals(typeof(Vector2)))
            {
                return EditorGUILayout.Vector2Field(label, (Vector2)value);
            }

            if (type.Equals(typeof(Vector3)))
            {
                return EditorGUILayout.Vector3Field(label, (Vector3)value);
            }

            return null;
        }

        static object Instantiate(Type type)
        {
            if (type.IsArray)
            {
                Type itemsType = type.GetIEnumerableElementType();
                return Array.CreateInstance(itemsType, 0);
            }

            return type.CreateInstance();
        }

        static List<FieldInfo> GetFieldsRecursively(Type type)
        {
            Type baseType = type;
            List<FieldInfo> fields = new List<FieldInfo>();

            fields.AddRange(baseType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            while (baseType.BaseType != null && baseType.BaseType != typeof(UnityEngine.Object))
            {
                baseType = baseType.BaseType;
                fields.InsertRange(0, baseType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            }

            return fields;
        }

        static object DrawDictionary(Type type, IDictionary dictionary, GUIContent label = null)
        {
            Type keyType = dictionary.Keys.GetType().GetIEnumerableElementType();
            Type valueType = dictionary.Values.GetType().GetIEnumerableElementType();

            DrawDictionaryHeader(keyType, valueType, ref dictionary, label);

            return dictionary;
        }
        
        static void DrawDictionaryHeader(Type keyType, Type valueType, ref IDictionary dictionary, GUIContent label = null)
        {
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            {
                if (label != null && string.IsNullOrEmpty(label.text) == false)
                {
                    string tooltip = "Dictionary aren't serializable. Thus they cannot be exported using the Remote Class Exporter";
                    label.tooltip = tooltip;
                    EditorGUILayout.LabelField(new GUIContent(label));
                }
                
                // if (GUILayout.Button(EditorHelper.GetIcon("add"), GUIStyle.none, GUILayout.Width(16f), GUILayout.Height(16f)))
                // {
                //     var keyInstance = keyType.CreateInstance();
                //     var valueInstance = valueType.CreateInstance();
                //
                //     dictionary.Add(keyInstance, valueInstance);
                // }
        
            }
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
        }

        static object DrawCollection(Type type, IList list, GUIContent label = null)
        {
            Type itemsType = type.GetIEnumerableElementType();

            DrawCollectionHeader(itemsType, ref list, label);

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();
            {
                DrawItems(itemsType, ref list);
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;

            return list;
        }
        
        static void DrawCollectionHeader(Type itemsType, ref IList list, GUIContent label = null)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (label != null && string.IsNullOrEmpty(label.text) == false)
                {
                    EditorGUILayout.LabelField(label);
                }

                if (GUILayout.Button(EditorHelper.GetIcon("add"), GUIStyle.none, GUILayout.Width(16f), GUILayout.Height(16f)))
                {
                    if (list.IsFixedSize)
                    {
                        var array = Array.CreateInstance(itemsType, list.Count + 1);
                        list.CopyTo(array, 0);
                        list = array;
                    }
                    else
                    {
                        list.Add(Activator.CreateInstance(itemsType));
                    }
                }

            }
            EditorGUILayout.EndHorizontal();
        }

        static void DrawItems(Type itemsType, ref IList list)
        {
            if (list == null)
            {
                return;
            }
            
            int indexToRemove = -1;
            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("Box");
                {
                    if (GUILayout.Button(EditorHelper.GetIcon("delete"), GUIStyle.none, GUILayout.Width(16f), GUILayout.Height(16f)))
                    {
                        indexToRemove = i;
                    }

                    EditorGUIUtility.labelWidth -= 23f;
                    EditorGUILayout.BeginVertical();
                    list[i] = OnGUI(itemsType, list[i], "Item " + i);
                    EditorGUILayout.EndVertical();
                    EditorGUIUtility.labelWidth += 23f;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (indexToRemove < 0)
            {
                return;
            }
            
            if (list.IsFixedSize)
            {
                var array = Array.CreateInstance(itemsType, list.Count - 1);
                int offset = 0;
                for (int i = 0; i + offset < array.Length; i++)
                {
                    if (i == indexToRemove)
                    {
                        offset++;
                    }

                    array.SetValue(list[i + offset], i);
                }

                list = array;
            }
            else
            {
                list.RemoveAt(indexToRemove);
            }
        }

        static object DrawReference(object value, GUIContent label = null)
        {
            if (label != null && string.IsNullOrEmpty(label.text) == false)
            {
                EditorGUILayout.LabelField(label);
            }
            
            EditorGUI.indentLevel++;
            List<FieldInfo> fields = GetFieldsRecursively(value.GetType());
            for (int i = 0; i < fields.Count; i++)
            {
                object fieldValue = OnGUI(fields[i].FieldType, fields[i].GetValue(value), fields[i].Name);
                fields[i].SetValue(value, fieldValue);
            }
            EditorGUI.indentLevel--;

            return value;
        }
        static object DrawReference(object value, string label = null) => DrawReference(value, new GUIContent(label));
    }
}