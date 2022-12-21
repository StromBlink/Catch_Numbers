using UnityEditor;
using Voodoo.Data.BinaryPrefs;

namespace Voodoo.Events
{ 
    public static class TimeManagerHelper
    {
        [MenuItem("Voodoo/Times/Next Day")]
        static void NextDay()
        {
            TimeManager.IncreaseDayCountExternal();
            BinaryPrefs.ForceSave();
        }
        
        [MenuItem("Voodoo/Times/Reset")]
        static void ResetDay()
        {
            TimeManager.ResetValues();
        }
    }
}