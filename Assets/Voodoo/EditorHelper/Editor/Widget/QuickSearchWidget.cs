using System;
using UnityEditor;
using UnityEngine;

namespace Voodoo.Utils
{
    public class QuickSearchWidget : IWidget
    {
        bool _disposed = false;
        
        FuzzySearchWidget _searchWidget;
        
        bool         _canCancel;
        GUIContent   _close;
        GUIContent   _dropdown;
        GUIStyle _customStyle;

        private Explorer<string> _explorer;
        public Explorer<string> Explorer
        {
            get => _explorer;

            set
            {
                if (_explorer != null)
                {
                    _explorer.collectionChanged -= OnCollectionChanged;
                    _explorer.itemAdded -= OnItemAdded;
                    _explorer.itemRemoved -= OnItemRemoved;
                }

                _explorer = value;

                if (_explorer == null) return;

                _explorer.collectionChanged += OnCollectionChanged;
                _explorer.itemAdded += OnItemAdded;
                _explorer.itemRemoved += OnItemRemoved;
            }
        }

        public QuickSearchWidget(Explorer<string> explorer, bool canCancel = true)
        {
            Explorer = explorer;

            _dropdown = EditorGUIUtility.IconContent("icon dropdown@2x");
            _close = EditorGUIUtility.IconContent("winbtn_win_close@2x");

            _canCancel = canCancel;

            _customStyle = GUIStyle.none;
            _customStyle.normal.textColor = Color.white;
            
            _searchWidget = new FuzzySearchWidget(explorer.Items.ToArray(), null, OnItemSelected, CloseFuzzySearch);
        }
        
        void OnItemAdded(string label) => OnCollectionChanged();
        void OnItemRemoved(int index) => OnCollectionChanged();

        void OnCollectionChanged()
        {
            _searchWidget.Setup(_explorer.Items.ToArray(), null, OnItemSelected, CloseFuzzySearch);
        }

        public void OnGUI()
        {
            if (_explorer.Items == null || _explorer.Items.Count <= 0)
            {
                EditorGUILayout.BeginHorizontal();
                { 
                    GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                    EditorGUILayout.LabelField("None", labelStyle, GUILayout.Width(100f));
                } 
                EditorGUILayout.EndHorizontal();
                return;
            }

            Rect searchRect = EditorGUILayout.BeginHorizontal();
            {
                int index = _explorer.Selection?.Count > 0 ? _explorer.Selection[0] : -1;
                string selected = index < 0 ? "None" : _explorer.Items[index];
                
                if (_canCancel && index >= 0 && GUILayout.Button(_close, GUIStyle.none, GUILayout.Width(16f), GUILayout.Height(16f)))
                {
                    _explorer.Select(null);
                }

                EditorGUI.BeginDisabledGroup(_explorer.Items.Count <= 1);
                if (GUILayout.Button(selected, _customStyle) ||
                    GUILayout.Button(_dropdown, GUIStyle.none, GUILayout.Width(16f), GUILayout.Height(16f)))
                {
                    _searchWidget.Show(searchRect);
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }

        void OnItemSelected(int index)
        {
            if (index != int.MinValue)
            {
                _explorer.Select(new int[1] { index });
            }

            CloseFuzzySearch();
        }

        private void CloseFuzzySearch()
        {
            EditorWindow.focusedWindow.Close();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _searchWidget = null;
            Explorer = null;

            GC.SuppressFinalize(this);
        }
    }
}
