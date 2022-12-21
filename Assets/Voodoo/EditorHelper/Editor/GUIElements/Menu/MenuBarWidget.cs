using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
    public class MenuBarWidget : ICommandKeeper, IWidget, IEnable
    {
        public List<MenuCommandWidget> menuCommands = new List<MenuCommandWidget>();

        protected virtual void CreateMenuCommand()
        {
        }
        
        public virtual void CreateMenuCommand(string iconPath, string tooltip, Action action, params GUILayoutOption[] layoutOption)
        {
            MenuCommandWidget toolBarButton = new MenuCommandWidget();
            menuCommands.Add(toolBarButton);
        }

        public virtual void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                foreach (MenuCommandWidget menuCommand in menuCommands)
                { 
                    DisplayMenuCommand(menuCommand);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        protected void DisplayMenuCommand(MenuCommandWidget toolBarButton)
        {
            toolBarButton.OnGUI();
        }

        public void OnEnable()
        {
            CreateMenuCommand();
        }

        public void OnDisable()
        {
        }
    }
}