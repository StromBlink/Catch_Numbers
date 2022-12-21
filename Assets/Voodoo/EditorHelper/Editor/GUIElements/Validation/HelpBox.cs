using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Voodoo.Utils
{
	public class HelpBox
	{
		public string helpBoxType;
		public string objectType;
		public string message;
		public MessageType messageType;
		public Object projectObject;
		public bool isClickable;
		public bool isExpandable;
		public bool isFixable;
		public bool isExpand;
		
		public string Id => helpBoxType + "-" + objectType;
		public Action<string> onClick;
		public Action<string> onHelpBoxExpand;
		public Action<string> onFixAsked;

		private readonly Texture2D UIRepair;

		public HelpBox()
		{
			UIRepair = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/VoodooPackages/VoodooStore/Textures/Repair.png", typeof(Texture2D));
		}
		
		public HelpBox(HelpBox source) : this()
		{
			if (source == null)
				return;
			
			helpBoxType     = source.helpBoxType;
			objectType      = source.objectType;
			message         = source.message;
			messageType     = source.messageType;
			projectObject   = source.projectObject;
			isClickable     = source.isClickable;
			isExpandable    = source.isExpandable;
			isFixable       = source.isFixable;
			isExpand        = source.isExpand;
			
			onClick         = source.onClick;
			onHelpBoxExpand = source.onHelpBoxExpand;
			onFixAsked      = source.onFixAsked;
		}
		
//		//Example
//		helpBoxType = HelpBoxType.Data;
//		objectType = "Description";
//		message = "You need to fill the Description field";
//		messageType = MessageType.Error;
//		projectObject = null;
//		
//		//Example 2
//		helpBoxType = HelpBoxType.IncorrectPath;
//		objectType = "Script";
//		message = "Assets/VoodooPackages/EasyAnchors.cs has an incorrect path. Please put it under the following folder : Scripts";
//		messageType = MessageType.Warning;
//		projectObject = AssetDatabase.LoadAssetAtPath<Object>("Assets/VoodooPackages/EasyAnchors.cs");

		public void Display()
		{
			if (isClickable == false && isFixable == false && isExpandable == false)
			{
				EditorGUILayout.HelpBox(message, messageType);
			}
			else
			{
				DisplayClickableHelpBox();
			}
		}
		
		private void DisplayClickableHelpBox()
		{
			EditorGUILayout.BeginVertical("box");
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox(message, messageType);
			
			if (isClickable)
			{
				MakePreviousElementClickable();
			}
			
			if (isFixable)
			{
				DisplayRepairButton();
			}
			
			EditorGUILayout.EndHorizontal();
			
			if (isExpandable && isExpand)
			{
				DisplayExpandHelpBox();
			}
			
			EditorGUILayout.EndVertical();
		}

		private void MakePreviousElementClickable()
		{
			Rect previousRect = EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect());
			GUIStyle buttonStyle = new GUIStyle();
			buttonStyle.active.background = new Texture2D(1, 1);
			
			Color[] pixels = buttonStyle.active.background.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = new Color(0.804f, 0.804f, 0.804f, 0.05f);
			}
			buttonStyle.active.background.SetPixels(pixels);
			
			if (GUI.Button(previousRect, GUIContent.none, buttonStyle))
			{
				OnHelpBoxClicked();
			}
		}

		private void DisplayRepairButton()
		{
			
			if (GUILayout.Button(UIRepair, GUILayout.Height(38), GUILayout.Width(38)))
			{
				OnFixAsked();
			}
		}

		private void DisplayExpandHelpBox()
		{
			onHelpBoxExpand?.Invoke(Id);
		}

		private void OnHelpBoxClicked()
		{
			onClick?.Invoke(Id);
			if (isExpandable)
			{
				isExpand = !isExpand;
			}
		}

		private void OnFixAsked()
		{
			onFixAsked?.Invoke(Id);
		}
	}
	
}