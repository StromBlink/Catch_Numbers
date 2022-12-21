using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Voodoo.Data.BinaryPrefs
{
    public static class BinaryPrefs
    {
        [RuntimeInitializeOnLoadMethod]
        static void RunOnStart()
        {
            Application.focusChanged += OnFocusChanged;
        }

        static void OnFocusChanged(bool _IsFocusOn)
        {
            if (!_IsFocusOn && DataToSave.Count > 0)
                ForceSave();
        }

        static Dictionary<string, object> s_DataToSave;

        static Dictionary<string, object> DataToSave
        {
            get => s_DataToSave ?? (s_DataToSave = GetAll());
            set => s_DataToSave = value;
        }

        static void AddKey(string _Key, object _Value)
        {
            if (DataToSave.ContainsKey(_Key))
            {
                DataToSave[_Key] = _Value;
            }
            else
            {
                DataToSave.Add(_Key, _Value);
            }
        }

        #region Getter/Setter
        
        #region String

        /// <summary>
        /// Sets the value of the preference identified by key.
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_Value"></param>
        public static void SetString(string _Key, string _Value)
        {
            AddKey(_Key, _Value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <param name="_Key"></param>
        /// <returns></returns>
        public static string GetString(string _Key, string _DefaultValue = "")
        {
            if (!DataToSave.ContainsKey(_Key))
                return _DefaultValue;

            if (DataToSave[_Key] is string)
                return (string) DataToSave[_Key];

            Debug.LogError("The type saved on this key doesn't match.");
            return _DefaultValue;

        }
        
        #endregion

        #region Int

        /// <summary>
        /// Sets the value of the preference identified by key.
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_Value"></param>
        public static void SetInt(string _Key, int _Value)
        {
            AddKey(_Key, _Value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <param name="_Key"></param>
        /// <returns></returns>
        public static int GetInt(string _Key, int _DefaultValue = 0)
        {
            if (!DataToSave.ContainsKey(_Key))
                return _DefaultValue;

            if (DataToSave[_Key] is int)
                return (int) DataToSave[_Key];

            Debug.LogError("The type saved on this key doesn't match.");
            return _DefaultValue;
        }

        #endregion

        #region Bool

        /// <summary>
        /// Sets the value of the preference identified by key.
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_Value"></param>
        public static void SetBool(string _Key, bool _Value)
        {
            AddKey(_Key, _Value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <param name="_Key"></param>
        /// <returns></returns>
        public static bool GetBool(string _Key, bool _DefaultValue = false)
        {
            if (!DataToSave.ContainsKey(_Key))
                return _DefaultValue;

            if (DataToSave[_Key] is bool)
                return (bool) DataToSave[_Key];

            Debug.LogError("The type saved on this key doesn't match.");
            return _DefaultValue;
        }

        #endregion

        #region Float

        /// <summary>
        /// Sets the value of the preference identified by key.
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_Value"></param>
        public static void SetFloat(string _Key, float _Value)
        {
            AddKey(_Key, _Value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <param name="_Key"></param>
        /// <returns></returns>
        public static float GetFloat(string _Key, float _DefaultValue = 0f)
        {
            if (!DataToSave.ContainsKey(_Key))
                return _DefaultValue;

            if (DataToSave[_Key] is float)
                return (float) DataToSave[_Key];

            Debug.LogError("The type saved on this key doesn't match.");
            return _DefaultValue;
        }

        #endregion

        #region Double

        /// <summary>
        /// Sets the value of the preference identified by key.
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_Value"></param>
        public static void SetDouble(string _Key, double _Value)
        {
            AddKey(_Key, _Value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <param name="_Key"></param>
        /// <returns></returns>
        public static double GetDouble(string _Key, double _DefaultValue = 0.0)
        {
            if (!DataToSave.ContainsKey(_Key))
                return _DefaultValue;

            if (DataToSave[_Key] is double)
                return (double) DataToSave[_Key];

            Debug.LogError("The type saved on this key doesn't match.");
            return _DefaultValue;
        }

        #endregion

        #region Class
        
        /// <summary>
        /// Sets the value of the preference identified by key.
        /// The class value must be Serializable. If class definition modified, the class will return default value for new variables.
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_Value"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetClass<T>(string _Key, T _Value) where T : class
        {
            if (!_Value.GetType().IsSerializable)
            {
                Debug.LogError("You're class must be serializable to be saved");
                return;
            }

            AddKey(_Key, _Value);
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// A class value must be Serializable. If class definition modified, the class will return default value for new variables.
        /// </summary>
        /// <param name="_Key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetClass<T>(string _Key, T _DefaultValue = default) where T : class
        {
            if (!DataToSave.ContainsKey(_Key))
                return null;

            if (DataToSave[_Key] is T)
                return (T) DataToSave[_Key];

            Debug.LogError("The type saved on this key doesn't match.");
            return null;
        }
        
        #endregion
        
        #endregion
        
        /// <summary>
        /// Returns true if key exists in the preferences.
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_HardCheck"></param>
        /// <returns></returns>
        public static bool HasKey(string _Key, bool _HardCheck = false)
        {
            return _HardCheck
                ? File.Exists(BinaryUtility.GetFilePathFromKey(_Key))
                : DataToSave.ContainsKey(_Key);
        }

        /// <summary>
        ///   <para>Removes key and its corresponding value from the preferences.</para>
        /// </summary>
        /// <param name="_Key"></param>
        public static void DeleteKey(string _Key)
        {
            if (HasKey(_Key, true))
                File.Delete(BinaryUtility.GetFilePathFromKey(_Key));

            if (HasKey(_Key))
                DataToSave.Remove(_Key);
        }

        /// <summary>
        ///   <para>Removes all keys and values from the preferences. Use with caution.</para>
        /// </summary>
        public static void DeleteAll()
        {
            string path = Application.persistentDataPath + "/";
            string[] binarySaves = Directory.GetFiles(path, "*.voodoosave");
            for (int i = 0; i < binarySaves.Length; i++)
            {
                File.Delete(binarySaves[i]);
            }

            DataToSave.Clear();
            Debug.Log("Binary Prefs deleted");
        }

        /// <summary>
        ///   <para>Get all keys and values from the preferences.</para>
        /// </summary>
        /// <returns></returns>
        static Dictionary<string, object> GetAll()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            string path = Application.persistentDataPath + "/";
            string[] paths = Directory.GetFiles(path, "*.voodoosave");

            for (int i = 0; i < paths.Length; i++)
            {
                string key = BinaryUtility.GetKeyFromPath(paths[i]);
                object value = GetObject(key);

                if (value != null)
                    result.Add(key, value);
            }

            return result;
        }

        /// <summary>
        ///   <para>Writes all modified preferences to disk. (Use with caution)</para>
        ///   <para>By default, ForceSave is called automatically at Application.OnFocusChanged.</para>
        /// </summary>
        public static void ForceSave()
        {
            foreach (KeyValuePair<string, object> entry in DataToSave)
            {
                SetObject(entry.Key, entry.Value);
            }

            DataToSave.Clear();
            DataToSave = null;
        }

        static void SetObject(string _Key, object _Value)
        {
            BinaryUtility.BinaryProcessSet(_Key, _Value);
        }

        static object GetObject(string _Key)
        {
            if (s_DataToSave != null)
                return s_DataToSave.ContainsKey(_Key) ? s_DataToSave[_Key] : BinaryUtility.BinaryProcessGet(_Key);

            return BinaryUtility.BinaryProcessGet(_Key);
        }
    }
}