using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
   public class GUIHyperLink : GUIButton
   {
      private string url;

      public void Initialize(string name, string tooltip, string url, params GUILayoutOption[] layoutOptions)
      {
         this.url = url;
         guiContent = EditorHelper.GetIcon(name);
         guiContent.tooltip = tooltip;
         this.layoutOptions = layoutOptions;

         guiStyle = new GUIStyle(EditorStyles.label);
         guiStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
      }

      public void Initialize(string name, string tooltip, string url, GUIStyle guiStyle, params GUILayoutOption[] layoutOptions)
      {
         this.url = url;
         guiContent = EditorHelper.GetIcon(name);
         guiContent.tooltip = tooltip;
         this.layoutOptions = layoutOptions;
         this.guiStyle = guiStyle;
      }

      public override void Execute()
      {
         Application.OpenURL(url);
      }
   }
}