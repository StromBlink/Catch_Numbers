using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils.Ease
{
	[CustomEditor(typeof(AnimationCurves))]
	public class AnimationCurvesEditor : Editor
	{
		private AnimationCurves myTarget;

		private bool isLoaded;

		private string[] curveTypes;

		public override void OnInspectorGUI()
		{
			if (myTarget == null)
			{
				myTarget = (AnimationCurves) target;
			}

			if (isLoaded == false)
			{
				isLoaded = true;
				myTarget.SetUpCurves();
			}

			if (myTarget.curves==null || myTarget.curves.Count == 0)
			{
				return;
			}

			for (int i = 0; i < myTarget.curves.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField(myTarget.curves[i].name, GUILayout.Width(100));
					myTarget.curves[i].curve = EditorGUILayout.CurveField(myTarget.curves[i].curve);
				}
				EditorGUILayout.EndHorizontal();
			}
			
			
		}
	}

}

