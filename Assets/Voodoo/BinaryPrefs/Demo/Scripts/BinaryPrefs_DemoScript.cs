using System;
using UnityEngine;

namespace Voodoo.Data.BinaryPrefs
{

    public class BinaryPrefs_DemoScript : MonoBehaviour
    {
        void Start()
        {
            //---------------------------------------------------------------------------------//
            //---------------------------------------------------------------------------------//
            // BinaryPrefs works like PlayerPrefs:
            // BinaryPrefs gets values as fast as PlayerPrefs and sets them 40 times faster.

            // PLAYER PREFS
            PlayerPrefs.SetInt("testInt", 5);
            int playerPrefsInt = PlayerPrefs.GetInt("testInt");

            // BINARY PREFS
            BinaryPrefs.SetInt("testInt", 5);
            int binaryPrefsInt = BinaryPrefs.GetInt("testInt");

            //---------------------------------------------------------------------------------//
            //---------------------------------------------------------------------------------//

            // BinaryPrefs handles useful types as:
            BinaryPrefs.SetBool("testBool", true);
            BinaryPrefs.SetDouble("testDouble", 2.2);

            //---------------------------------------------------------------------------------//
            //---------------------------------------------------------------------------------//

            // BinaryPrefs handles multi-dimensional array serialization:
            BinaryPrefs.SetClass("testClass", new TwoDimensionArray()
            {
                m_Value = new int[,,]
                {
                    {
                        {1, 2, 3, 4},
                        {4, 5, 6, 4}
                    },
                    {
                        {1, 2, 3, 4},
                        {4, 5, 6, 4}
                    },
                },
            });

            //---------------------------------------------------------------------------------//
            //---------------------------------------------------------------------------------//

            // BinaryPrefs is compatible with Json:
            BinaryPrefs.SetString("testJson", JsonUtility.ToJson(new DataToJson()));

            //---------------------------------------------------------------------------------//
            //---------------------------------------------------------------------------------//

            // BinaryPrefs comes along with useful tools:
            BinaryPrefs.HasKey("testJson");
            BinaryPrefs.DeleteKey("testJson");

            BinaryPrefs.ForceSave(); // Use with caution
            BinaryPrefs.DeleteAll(); // Use with caution

            //---------------------------------------------------------------------------------//
            //---------------------------------------------------------------------------------//
        }
    }

    [Serializable]
    public class TwoDimensionArray
    {
        public int[,,] m_Value;
    }

    [Serializable]
    public class DataToJson
    {
        public Vector3 m_Position;
        public int m_LifePoint;
        public string m_Name;
        public bool m_IsPowerActive;
    }
}