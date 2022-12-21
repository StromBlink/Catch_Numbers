using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
	public class HelpBoxArea
	{
		private Action<HelpBox, bool> onFixAsked;

		private List<HelpBox> helpBoxesData = new List<HelpBox>();
		private Dictionary<string, int> idToQuantity = new Dictionary<string, int>();

		public bool IsEmpty => helpBoxesData == null || helpBoxesData.Count == 0;

		public void Add(HelpBox helpBox)
		{
			helpBoxesData.Add(helpBox);
			helpBox.onClick += OnHelpBoxClicked;
			helpBox.onHelpBoxExpand += OnHelpBoxUnfold;
			helpBox.onFixAsked += OnFixAsked;

			if (idToQuantity.ContainsKey(helpBox.Id))
			{
				idToQuantity[helpBox.Id]++;
			}
			else
			{
				idToQuantity.Add(helpBox.Id, 1);
			}
		}

		public void Remove(HelpBox helpBox)
		{
			if (helpBox == null)
				return;

			if (!helpBoxesData.Contains(helpBox))
				return;

			helpBoxesData.Remove(helpBox);
			
			helpBox.onClick -= OnHelpBoxClicked;
			helpBox.onHelpBoxExpand -= OnHelpBoxUnfold;

			idToQuantity[helpBox.Id]--;
			if (idToQuantity[helpBox.Id] == 0)
			{
				idToQuantity.Remove(helpBox.Id);
			}
		}

		public void Display()
		{
			foreach (KeyValuePair<string, int> kvp in idToQuantity)
			{
				string id = kvp.Key;
				int quantity = kvp.Value;

				if (quantity == 1)
				{
					DisplayMonoHelpBox(id);
				}
				else
				{
					DisplayMultiHelpBox(kvp);
				}
			}
		}

		private void DisplayMonoHelpBox(string id)
		{
			HelpBox helpBox = helpBoxesData.Find(x => x.Id == id);

			helpBox?.Display();
		}

		private void DisplayMultiHelpBox(KeyValuePair<string, int> kvp)
		{
			HelpBox baseHelpBox = helpBoxesData.Find(x => x.Id == kvp.Key);

			HelpBox helpBox = new HelpBox(baseHelpBox);
			helpBox.message = kvp.Value + " " + helpBox.objectType;
			helpBox.isExpandable = true;
			helpBox.onClick = OnHelpBoxClicked;
			helpBox.Display();
			baseHelpBox.isExpand = helpBox.isExpand;
		}

		private void OnHelpBoxClicked(string id)
		{
			foreach (HelpBox exporterHelpBoxData in helpBoxesData)
			{
				if (exporterHelpBoxData.Id != id)
					continue;

				//TODO Expandable?
				if (exporterHelpBoxData.isExpandable)
				{
//					Debug.Log(exporterHelpBoxData.message, exporterHelpBoxData.projectObject);
				}
				else
				{
					Selection.activeObject = exporterHelpBoxData.projectObject;
				}
			}
		}

		private void OnHelpBoxUnfold(string id)
		{
			foreach (HelpBox exporterHelpBoxData in helpBoxesData)
			{
				if (exporterHelpBoxData.Id == id)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(20);
					GUIStyle buttonStyle = new GUIStyle(EditorStyles.label) {wordWrap = true};
					if (GUILayout.Button(exporterHelpBoxData.message, buttonStyle))
					{
						Selection.activeObject = exporterHelpBoxData.projectObject;
					}
					
					EditorGUILayout.EndHorizontal();
				}
			}
		}

		private void OnFixAsked(string id)
		{
			foreach (HelpBox exporterHelpBoxData in helpBoxesData)
			{
				if (exporterHelpBoxData.Id == id)
				{
					onFixAsked?.Invoke(exporterHelpBoxData, false);
				}
			}
		}

		public void FixAll(bool silently = false)
		{
			foreach (HelpBox exporterHelpBoxData in helpBoxesData)
			{
				onFixAsked?.Invoke(exporterHelpBoxData, silently);
			}
		}

		public bool ContainsID(string id)
		{
			return idToQuantity.ContainsKey(id);
		}
	}
}