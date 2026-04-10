using System;
using System.Collections.Generic;

namespace Shinrai.Core
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, IGameService> _services = new Dictionary<Type, IGameService>();
        
        public static void RegisterService<T>(T service) where T : IGameService
        {
            _services.Add(typeof(T), service);
        }
        
        public static void UnregisterService<T>() where T : IGameService
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _services.Remove(typeof(T));
            }
        }
        
        public static T GetService<T>() where T : IGameService
        {
            if (_services.ContainsKey(typeof(T)))
            {
                return (T)_services[typeof(T)];
            }
            throw new Exception($"Service {typeof(T).Name} not registered");
        }
        
        public static bool HasService<T>() where T : IGameService
        {
            return _services.ContainsKey(typeof(T));
        }
        
        public static void Clear()
        {
            _services.Clear();
        }
    }
}
