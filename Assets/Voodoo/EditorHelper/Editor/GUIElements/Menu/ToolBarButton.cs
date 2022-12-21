using System;
using UnityEngine;

namespace Voodoo.Utils
{
   public class ToolBarButton : GUIButton
   {
      private Color normalColor = Color.white;
      private Color selectedColor = Color.gray;

      private Type linkedWidget = null;

      public void SetLinkedWidget(Type type)
      {
         linkedWidget = type;
      }

      public void SelectWithLinkedWidget(Type type)
      {
         IsSelected = type == linkedWidget;
      }

      public override void OnGUI()
      {
         GUI.backgroundColor = IsSelected ? selectedColor : normalColor;
         
         base.OnGUI();

         GUI.backgroundColor = normalColor;
      }
   }
}