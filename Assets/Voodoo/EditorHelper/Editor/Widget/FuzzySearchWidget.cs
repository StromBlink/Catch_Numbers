using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
	public class FuzzySearchWidget : IWidget
    {
        private struct Candidate 
        {
            public int    index;
            public string name;
            public double distance;
        }

        private const float ElementSize = 15f;
        private const int   ElementToDisplay = 10;

        //Data
        private Candidate[]     _candidates;
        private List<Candidate> _bestMatch;
        private int             _hoveredIndex;
        private string          _searchInput;
        private readonly double _threshold;
        
        //UI
        private Vector2       _scrollPos;
        private GUIStyle      _buttonStyle;
        private readonly bool _keepValueDisplayed;
        
        //Events
        public event Action<int> valueChanged;
        public event Action<int> selected;
        public event Action      canceled;


        public FuzzySearchWidget(string searchInput = "", double threshold = 0.5d, bool keepDisplayedValue = true)
        {
            _hoveredIndex = -1;
            _searchInput = searchInput;
            _threshold   = threshold;
            _keepValueDisplayed = keepDisplayedValue;
            
            SetButtonStyle();
        }
        
        public FuzzySearchWidget(string[] items, Action<int> onValueChanged, Action<int> onClick, Action onCancel, string searchInput = "", double threshold = 0.5d, bool keepDisplayedValue = true) : this(searchInput,threshold,keepDisplayedValue)
        {
            Setup(items, onValueChanged, onClick, onCancel);
        }

        private void SetButtonStyle()
        {
            Texture2D texture = new Texture2D(1, 1);
            
            _buttonStyle = new GUIStyle
            {
                active = {background = texture, textColor = Color.black},
                normal = {background = texture, textColor = Color.white}
            };
        }

        public void Setup(string[] items, Action<int> onValueChanged, Action<int> onClick, Action onCancel)
        {
            valueChanged = onValueChanged;
            canceled = onCancel;
            selected = onClick;

            UpdateCandidates(items);
        }

        public void UpdateCandidates(string[] items)
        {
            _candidates = new Candidate[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                _candidates[i] = new Candidate {index = i, name = items[i], distance = 0d };
            }
        }
        
        public void ShowAsSearchBar(string label, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            {
                EditorGUILayout.PrefixLabel(label);
                Rect rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.FindStyle("ToolbarSeachTextField"));
                ShowAsSearchBar(rect);
            }
            GUILayout.EndHorizontal();
        }
        
        public void ShowAsSearchBar(Rect rect)
        {
            if (GUI.Button(rect, new GUIContent(_searchInput), GUI.skin.FindStyle("ToolbarSeachTextField")))
            {
                Show(rect);
            }

            if (GUILayout.Button(string.Empty, GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                _searchInput = String.Empty;

                GUI.FocusControl(null);
            }
        }

        public void ShowAsPopup(string value)
        {
            Rect searchRect = EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(new GUIContent(value)))
                {
                    Show(searchRect);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public void Show(Rect rect, Vector2 size)
        {
            Rect final = GUIUtility.GUIToScreenRect(rect);
            final.height -= EditorGUIUtility.singleLineHeight;

            this.ShowAsDropDown(final, size);
            
            _hoveredIndex = -1;
            UpdateDistance();
        }
        
        public void Show(Rect rect)
        {
            Rect final = GUIUtility.GUIToScreenRect(rect);
            final.height -= EditorGUIUtility.singleLineHeight;

            this.ShowAsDropDown(final, new Vector2(final.width, 200f));
			
            _hoveredIndex = -1;
            UpdateDistance();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUI.BeginChangeCheck();
                {
                    GUI.SetNextControlName("SearchInput");

                    _searchInput = EditorGUILayout.TextField(_searchInput, GUI.skin.FindStyle("ToolbarSeachTextField"));
                }
                
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateDistance();
                }

                if (_buttonStyle.active.background == null)
                {
                    SetButtonStyle();
                }
                
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
                for (int i = 0; i < _bestMatch.Count; i++)
                {
                    GUI.backgroundColor = _hoveredIndex == i ? GUI.skin.settings.selectionColor : Color.clear;

                    if (GUILayout.Button(_bestMatch[i].name, _buttonStyle))
                    {
                        Select(_bestMatch[i].index);
                    }
                        
                    if (_hoveredIndex == i)
                    {
                        GUI.backgroundColor = Color.clear;
                    }
                }
                EditorGUILayout.EndScrollView();

            }
            EditorGUILayout.EndVertical();
            
            Controls();
            
            GUI.FocusControl("SearchInput");
        }

        private void Controls()
        {
            Event e = Event.current;

            if (e.type != EventType.KeyUp)
            {
                return;
            }
            
            switch (e.keyCode)
            {
                case KeyCode.Escape:
                    canceled?.Invoke();
                    break;
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    if (_bestMatch.Count == 1)
                    {
                        Select(_bestMatch[0].index);
                    }
                    else 
                    {
                        if (_bestMatch.Count == 0 || _hoveredIndex == -1)
                        {
                            _searchInput = string.Empty;
                            UpdateDistance();
                            canceled?.Invoke();
                        }
                        else
                        {
                            Select(_bestMatch[_hoveredIndex].index);
                        }
                    }
                    break;
                case KeyCode.DownArrow when _hoveredIndex + 1 < _bestMatch.Count:
                    _hoveredIndex++;
                    valueChanged?.Invoke(_hoveredIndex);
                    break;
                case KeyCode.UpArrow when _hoveredIndex > 0:
                    _hoveredIndex--;
                    valueChanged?.Invoke(_hoveredIndex);
                    break;
            }
            
            e.Use();
            EditorWindow.focusedWindow.Repaint();
        }
        
        void Select(int index)
        {
            _searchInput = String.Empty;
            UpdateDistance();
            
            if (_keepValueDisplayed)
            {
                _searchInput = _candidates[index].name;
            }
            
            selected?.Invoke(index);
        }
        
        private void UpdateDistance() 
        {
            if (_bestMatch == null)
            {
                _bestMatch = new List<Candidate>();
            }
            else
            {
                _bestMatch.Clear();
            }

            if (string.IsNullOrEmpty(_searchInput))
            {
                _bestMatch = new List<Candidate>(_candidates.ToArray()).OrderBy(o => o.name).ToList();
            }
            else
            {
                for (int i = 0; i < _candidates.Length; i++)
                {
                    int index = _candidates[i].name.ToLower().IndexOf(_searchInput.ToLower());                    
                    _candidates[i].distance = index < 0 ? _searchInput.ToLower().JaroWinklerProximity(_candidates[i].name.ToLower()) : 1d;

                    if (_candidates[i].distance > _threshold)
                    {
                        _bestMatch.Add(_candidates[i]);
                    }
                }

                _bestMatch.Sort((a, b) => -a.distance.CompareTo(b.distance));
            }

            UpdateWindowSize();
        }
        
        private void UpdateWindowSize()
        {
            Rect rect = EditorWindow.focusedWindow.position;
            rect.height = 25f + ElementSize * Math.Min(_bestMatch.Count, ElementToDisplay);

            EditorWindow.focusedWindow.maxSize = rect.size;
            EditorWindow.focusedWindow.minSize = EditorWindow.focusedWindow.maxSize;
        }
	}
}