using UnityEditor;
using UnityEngine;


namespace Voodoo.Utils
{
	public abstract class ValidationEditor
	{
		public bool canFixAll = true;

		public bool IsValid
		{
			get => isValid;
		}

		private bool isValid;

		private Vector2 scrollView;
		protected HelpBoxArea helpBoxArea = new HelpBoxArea();
		
		public void ShowValidation()
		{
			if (helpBoxArea.IsEmpty)
			{
				isValid = true;
				return;
			}

			isValid = false;

			GUILayout.Space(20);
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUILayout.LabelField("Validations", EditorStyles.boldLabel);

			ValidateFields();
			
			if (canFixAll)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Fix All", GUILayout.Height(40.0f),GUILayout.Width(80.0f)))
				{
					helpBoxArea.FixAll();
				}
				
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}

			// Do not display the scroll view if there are no data to display
			scrollView = EditorGUILayout.BeginScrollView(scrollView);
			{
				helpBoxArea?.Display();
			}
			EditorGUILayout.EndScrollView();
		}

		public void ClearValidation()
		{
			helpBoxArea = new HelpBoxArea();
		}

		public virtual void ValidateFields()
		{
			// Example of validation test
			/*
			 HelpBox exporterHelpBox = new HelpBox
			{
				helpBoxType   = HelpBoxType.Data,
				messageType   = MessageType.Error,
				projectObject = null,
			};
			 
			 * if (Exporter.data.elementsToExport == null || Exporter.data.elementsToExport.Count == 0)
			{
				ExporterHelpBox tempExporterHelpBox = new ExporterHelpBox(exporterHelpBox)
				{
					objectType = "Missing Package",
					message    = "You need a content to create a package, drag and drop element(s) on the + zone"
				};
				exporterHelpBoxArea.Add(tempExporterHelpBox);
				
				res = false;
			}
			 */
		}
	}
}