using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Voodoo.Data.BinaryPrefs
{
    public static class BinaryUtility
    {
        /// <summary>
        /// Returns the file path for a specific key
        /// </summary>
        /// <param name="_Key"></param>
        /// <returns></returns>
        public static string GetFilePathFromKey(string _Key)
        {
            return Application.persistentDataPath + "/" + _Key + ".voodoosave";
        }
        
        public static string GetKeyFromPath(string _Path)
        {
            string key = _Path;
            string pathToRemove = Application.persistentDataPath + "/";
            key = key.Replace(".voodoosave", "");
            key = key.Replace(pathToRemove, "");
            return key;
        }

        public static object BinaryProcessGet(string _Key)
        {
            string filePath = GetFilePathFromKey(_Key);
            if (!File.Exists(filePath))
            {
                Debug.LogError("You're trying to load data from a not exiting file");
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
        
            try
            {
                var result = formatter.Deserialize(stream);
                stream.Close();
                return result;
            }
            catch (SerializationException exception)
            {
                Debug.Log("Failed to deserialize. Reason : " + exception.Message);
                stream.Close();
                return null;
            }
        }

        public static void BinaryProcessSet(string _Key, object _Value)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetFilePathFromKey(_Key), FileMode.Create);

            try
            {
                formatter.Serialize(stream, _Value);
            }
            catch (SerializationException exception)
            {
                Debug.Log("Failed to serialize. Reason : " + exception.Message);
                throw;
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
