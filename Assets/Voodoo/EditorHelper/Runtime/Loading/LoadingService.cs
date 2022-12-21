using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.Utils
{
    public static class LoadingService
    {
        static Dictionary<object, Loading> _loadings = new Dictionary<object, Loading>();
        
        static LoadingService() 
        {
            Application.quitting += Dispose;
        }

        public static Loading Register(object key)
        {
            if (_loadings.ContainsKey(key))
            {
                return null;
            }

            var loading = new Loading();
            _loadings.Add(key, loading);
            return loading;
        }

        public static IDisposable Subscribe(object key, Action<int> stateChanged, Action<float> progressChanged = null, Action<int> completed = null) 
        {
            if (_loadings.ContainsKey(key) == false)
            {
                return null;
            }

            return _loadings[key].Subscribe(stateChanged, progressChanged, completed);
        }

        public static void Dispose()
        {
            foreach (Loading load in _loadings.Values)
            {
                load.Dispose();
            }

            _loadings.Clear();
        }
    }

    public class Loading : IDisposable
    {
        public bool _disposed = false;

        int _state = -1;
        public int State
        {
            get => _state;
            set { _state = value; _stateChanged?.Invoke(value); }
        }

        float _progress = 0f;
        public float Progress
        {
            get => _progress;
            set { _progress = value; _progressChanged?.Invoke(value); }
        }

        int _result = -1;
        public int Result
        {
            get => _result;
            set { _result = value; _completed?.Invoke(value); }
        }

        public static event Action<int>     _stateChanged;
        public static event Action<float>   _progressChanged;
        public static event Action<int>     _completed;
                
        public IDisposable Subscribe(Action<int> stateChanged, Action<float> progressChanged = null, Action<int> completed = null)
        {
            _stateChanged += stateChanged;
            
            if (progressChanged != null)
            {
                _progressChanged += progressChanged;
            }

            if (progressChanged != null)
            {
                _completed += completed;
            }

            return new Unsubscriber(() => Unsubscribe(stateChanged, progressChanged, completed));
        }

        public void Unsubscribe(Action<int> stateChanged, Action<float> progressChanged = null, Action<int> completed = null)
        {
            if (_disposed == true)
            {
                return;
            }

            _stateChanged -= stateChanged;

            if (progressChanged != null)
            {
                _progressChanged -= progressChanged;
            }

            if (progressChanged != null)
            {
                _completed -= completed;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _stateChanged    = null;
            _progressChanged = null;
            _completed       = null;

            GC.SuppressFinalize(this);
        }
        
        sealed class Unsubscriber : IDisposable
        {
            bool _disposed = false;

            Action _unsubscribe;

            public Unsubscriber(Action unsubscribe)
            {
                _unsubscribe = unsubscribe;
            }

            public void Dispose()
            {
                if (_disposed || _unsubscribe != null)
                {
                    return;
                }

                _unsubscribe();

                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }

}
