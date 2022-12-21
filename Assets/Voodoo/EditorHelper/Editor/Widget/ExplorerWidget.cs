using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
    public class ExplorerWidget<T> : IWidget, IDisposable
    {
        bool    _isMac = SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX;
        Event   _currentEvent;

        protected bool  _disposed = false;

        //Filter
        protected string _filter;
        protected List<int>   _displayedItems = new List<int>();

        //Explorer display
        protected ScrollWidget _scrollWidget;
        List<int>   _selection = new List<int>();

        protected Func<T, bool> filterChecked;
        protected Action<int>   indexDisplayed;
        protected Action        filterDisplayed;

        public Explorer<T> _explorer;
        public Explorer<T> Explorer 
        { 
            get => _explorer; 

            set 
            {
                if (_explorer != null)
                {
                    _explorer.collectionChanged -= UpdateFilter;
                    _explorer.itemAdded         -= OnItemAdded;
                    _explorer.itemRemoved       -= OnItemRemoved;
                }

                _explorer = value;
                UpdateFilter();

                if (_explorer == null) return;

                _explorer.collectionChanged += UpdateFilter;
                _explorer.itemAdded         += OnItemAdded;
                _explorer.itemRemoved       += OnItemRemoved;
            }
        }

        public ExplorerWidget(Action<int> onIndexDisplay, Action onfilterDisplay, Func<T, bool> onFilterCheck)
        {
            _scrollWidget = new ScrollWidget(_displayedItems, DisplayItem);

            indexDisplayed = onIndexDisplay;
            filterDisplayed = onfilterDisplay;
            filterChecked = onFilterCheck;
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                OnFilter();
                OnExplorerList();
            }
            EditorGUILayout.EndVertical();
        }

        void OnFilter()
        {
            if (filterDisplayed == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            filterDisplayed();
            if (EditorGUI.EndChangeCheck())
            {
                UpdateFilter();
            }
        }

        void UpdateFilter()
        {
            _displayedItems.Clear();
            if (Explorer == null || Explorer.Items == null)
            {
                return;
            }

            int count = Explorer.Items.Count;
            
            RefreshSelection();
            
            for (int i = 0; i < count; i++)
            {
                if (filterChecked(Explorer[i]))
                {
                    _displayedItems.Add(i);
                }
                else
                {
                    _selection.Remove(i);
                }
            }

        }

        public void RefreshSelection()
        {
            for (int i = _selection.Count-1; i >= 0; i--)
            {
                if (_selection[i] < 0 || _selection[i] >= _explorer.Items.Count)
                {
                    _selection.RemoveAt(i);
                    continue;
                }
            }

            _explorer?.Select(_selection?.ToArray());
        }

        public void Select(int[] indexes)
        {
            _selection.Clear();
            for (int i = 0; i < indexes?.Length; i++)
            {
                if (_displayedItems.IndexOf(indexes[i]) < 0)
                {
                    continue;
                }

                _selection.Add(indexes[i]);
            }

            _explorer?.Select(_selection?.ToArray());
        }

        void OnItemRemoved(int index) => UpdateFilter();

        void OnItemAdded(T item) 
        {
            _selection.Clear();
            if (filterChecked(item))
            {
                _displayedItems.Add(Explorer.Items.Count - 1);
                Select(new int[] { Explorer.Items.Count - 1 });
            }
        }

        void OnExplorerList()
        {
            _currentEvent = Event.current;
            _scrollWidget.OnGUI();
        }

        void DisplayItem(int index)
        {
            GUI.backgroundColor = EditorHelper.backgrounds[index % 2];
            if (_selection.Contains(index))
            {
                GUI.backgroundColor = Color.white;
            }

            Rect rect = EditorGUILayout.BeginHorizontal("Box");

            GUI.backgroundColor = Color.white;

            indexDisplayed(_displayedItems[index]);
            
            OnItemEvent(rect, _displayedItems[index]);

            EditorGUILayout.EndHorizontal();
        }

        void OnItemEvent(Rect rect, int index)
        {
            if (rect.Contains(_currentEvent.mousePosition) == false || _currentEvent.type != EventType.MouseUp)
            {
                return;
            }

            if (Event.current.button == 1)
            {
                Explorer.ContextClick(new int[1] { index });
                _currentEvent.Use();
            }

            if (Event.current.button == 0)
            {
                int[] selection = GetSelection(index);
                _selection.Clear();
                _selection.AddRange(selection);

                Explorer.Select(_selection.ToArray());
                _currentEvent.Use();
            }
        }

        int[] GetSelection(int index)
        {
            bool controlPressed = _isMac && Event.current.command || _isMac == false && Event.current.control;

            int indexof = _selection.IndexOf(index);
            if (_selection.Count <= 0 ||
                ((_selection.Count > 1 || indexof < 0) && controlPressed == false && Event.current.shift == false))
            {
                return new int[1] { index };
            }
            else if (controlPressed || Event.current.shift == false)
            {
                List<int> result = new List<int>(_selection);
                if (indexof < 0)
                {
                    result.Add(index);
                }
                else if (_selection.Count > indexof)
                {
                    result.RemoveAt(indexof);
                }

                return result.ToArray();
            }
            else if (Event.current.shift)
            {
                int start = _displayedItems.IndexOf(_selection[0]);
                int end = _displayedItems.IndexOf(index);

                List<int> result = new List<int>();
                int current = start;
                int step = start < end ? 1 : -1;
                while (current != end)
                {
                    result.Add(_displayedItems[current]);
                    current += step;
                }

                result.Add(index);

                return result.ToArray();
            }

            return null;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            Explorer        = null;
            indexDisplayed  = null;
            filterChecked   = null;
            _currentEvent   = null;

            _selection?.Clear();
            _displayedItems?.Clear();

            GC.SuppressFinalize(this);
        }
    }

    public class LabelExplorer : ExplorerWidget<string>
    {
        public LabelExplorer() : base(null, null, null)
        {
            indexDisplayed = OnIndexDisplay;
            filterDisplayed = OnFilterDisplay;
            filterChecked = OnFilterCheck;
        }

        void OnIndexDisplay(int index)
        {
            EditorGUILayout.LabelField(Explorer.Items[_displayedItems[index]], GUILayout.MaxWidth(150f));
            GUILayout.FlexibleSpace();
        }

        void OnFilterDisplay()
        {
            _filter = EditorGUILayout.TextField(_filter);
        }

        bool OnFilterCheck(string label)
        {
            string loweredFilter = _filter?.ToLower();

            return string.IsNullOrEmpty(loweredFilter) || label.ToLower().IndexOf(loweredFilter) >= 0;
        }

        public void OverrideDisplay(Action<int> OnIndexDisplay) 
        {
            if (OnIndexDisplay == null)
            {
                return;
            }

            indexDisplayed = OnIndexDisplay;
        }
    }
}
