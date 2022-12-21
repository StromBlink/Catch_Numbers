using System;
using System.Collections.Generic;

namespace Voodoo.Utils
{
    public class Explorer<T>
    {
        bool _disposed = false;

        public List<int> Selection { get; protected set; } = new List<int>();
        public List<T> Items { get; protected set; } = new List<T>();

        public T this[int key]
        {
            get => Items[key];
            set
            {
                if (value == null)
                {
                    return;
                }

                Items[key] = value;
            }
        }

        public event Action collectionChanged;
        public event Action<T> itemAdded;
        public event Action<int> itemRemoved;

        public event Action<int[]> contextClicked;
        public event Action<int[]> selectionChanged;

        public void Select(int[] indexes)
        {
            Selection.Clear();
            if (indexes != null && indexes.Length > 0)
            {
                Selection.AddRange(indexes);
            }

            selectionChanged?.Invoke(indexes ?? new int[0] { });
        }

        public void ContextClick(int[] selection) => contextClicked?.Invoke(selection);

        public void Fill(List<T> items)
        {
            Items.Clear();
            if (items == null || items.Count <= 0) return;

            Items.AddRange(items);
            collectionChanged?.Invoke();
        }

        public void Insert(int index, T item)
        {
            if (item == null) return;

            if (index >= 0 && index < Items.Count)
            {
                Items.Insert(index, item);
            }
            else
            {
                Items.Add(item);
            }
                
            itemAdded?.Invoke(item);
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
            itemRemoved?.Invoke(index);

            Selection.Remove(index);
        }

        public void Clear() => Items.Clear();

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            Selection = null;
            Items = null;

            collectionChanged = null;
            itemAdded = null;
            itemRemoved = null;

            contextClicked = null;
            selectionChanged = null;

            GC.SuppressFinalize(this);
        }
    }
}
