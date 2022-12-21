using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
	public class ScrollWidget : IWidget
	{
		private const float rowHeight = 18.0f;
		private readonly string elementDefaultName = "ScrollWidget.";

		protected readonly ICollection collection;

		private Vector2 scrollPosition = Vector2.zero;
		private Vector2 lastScrollPosition = Vector2.zero;
		private int selectedIndex = -1;

		private Action<int> onIndexDisplayed;

		public ScrollWidget(ICollection collection, Action<int> onIndexDisplayed)
		{
			selectedIndex = -1;
			this.collection = collection;
			this.onIndexDisplayed = onIndexDisplayed;

			elementDefaultName = string.Concat(elementDefaultName, GetHashCode(),".element-");
		}

		public void OnGUI()
		{
			// Record the last scroll position so we can calculate if the user has scrolled this frame
			lastScrollPosition = scrollPosition;

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
			scrollPosition.y = Mathf.Max(0, scrollPosition.y);

			int visibleCount = Mathf.CeilToInt(Screen.height / rowHeight);
			int firstShownIndex = Mathf.FloorToInt(scrollPosition.y / rowHeight);
			int shownIndexLimit = firstShownIndex + visibleCount;

			if (collection.Count < shownIndexLimit)
			{
				shownIndexLimit = collection.Count;
			}

			if (shownIndexLimit - firstShownIndex < visibleCount)
			{
				firstShownIndex -= visibleCount - (shownIndexLimit - firstShownIndex);
			}

			firstShownIndex = Mathf.Max(0, firstShownIndex);
            
			GUILayout.Space(firstShownIndex * rowHeight);

			for (int i = firstShownIndex; i < shownIndexLimit; i++)
			{
				GUI.SetNextControlName(elementDefaultName + i.ToString());
				onIndexDisplayed?.Invoke(i);
			}

			float bottomPadding = (collection.Count - shownIndexLimit) * rowHeight;

			if (bottomPadding > 0)
			{
				GUILayout.Space(bottomPadding);
			}
            
			EditorGUILayout.EndScrollView();

			// If the user has scrolled, deselect - this is because control IDs within carousel will change when scrolled
			// so we'd end up with the wrong box selected.
			if (scrollPosition != lastScrollPosition)
			{
				if (firstShownIndex < selectedIndex && selectedIndex < shownIndexLimit && (GUI.GetNameOfFocusedControl().StartsWith(elementDefaultName) || string.IsNullOrEmpty(GUI.GetNameOfFocusedControl())))
				{
					GUI.FocusControl(elementDefaultName + selectedIndex.ToString());
				}
				else
				{
					GUI.FocusControl("");
				}
			}

			if (GUI.GetNameOfFocusedControl().StartsWith(elementDefaultName))
			{
				int focusControlIndex = Int32.Parse(GUI.GetNameOfFocusedControl().Split('-').Last());
				selectedIndex = focusControlIndex;
			}
		}
	}

	public class ScrollWidget<T> : ScrollWidget
	{
		public T this[int key]
		{
			get => (collection as List<T>)[key];
			set => (collection as List<T>)[key] = value;
		}

		public ScrollWidget(List<T> list, Action<int> onIndexDisplayed) : base(new List<T>(list), onIndexDisplayed) { }
	}
}