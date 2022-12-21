using System;
using UnityEngine;

namespace Voodoo.Utils
{
   public class MenuCommandWidget : IWidget
   {
      private GUIContent guiContent;
      private Action action;
      
      public void SetGUIContent (string icon, string tooltip, Action action)
      {
         guiContent = EditorHelper.GetIcon(icon);
         guiContent.tooltip = tooltip;
         this.action = action;
      }
      public void OnGUI()
      {
         if (GUILayout.Button(guiContent, GUILayout.Width(38), GUILayout.Height(38)))
         {
            Execute();
         }
      }

      private void Execute()
      {
         action.Invoke();
      }
   }
}