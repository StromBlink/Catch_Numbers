using UnityEditor;

namespace Voodoo.Data.BinaryPrefs
{
    public static class BinaryPrefsHelper
    {
        [MenuItem("Voodoo/BinaryPrefs/Delete All")]
        public static void DeleteAll()
        {
            BinaryPrefs.DeleteAll();
        }
    }
}
