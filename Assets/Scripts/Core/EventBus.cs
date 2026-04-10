using System;
using System.Collections.Generic;


namespace Shinrai.Core
{
    public static class EventBus
    {
        private static Dictionary<Type, Delegate> _listeners = new();

        /// <summary> Subscribe to an event </summary>
        public static void Subscribe<T>(Action<T> listener) where T : struct
        {
            var type = typeof(T);
            if (_listeners.TryGetValue(type, out var existingListener))
            {
                _listeners[type] = Delegate.Combine(existingListener, listener);
            }
            else
            {
                _listeners[type] = listener;
            }
        }
        
        public static void Unsubscribe<T>(Action<T> listener) where T : struct
        {
            var type = typeof(T);
            if (!_listeners.TryGetValue(type, out var existingListener)) return;

            var updated = Delegate.Remove(existingListener, listener);
            if (updated != null)
            {
                _listeners[type] = updated;
            }
            else
            {
                _listeners.Remove(type);
            }
        }
        
        public static void Emit<T>(T data) where T : struct
        {
            if (_listeners.TryGetValue(typeof(T), out var listener))
            {
                ((Action<T>)listener)?.Invoke(data);
            }
        }
    }
}
