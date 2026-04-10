using UnityEngine;

namespace Shinrai.Core
{
    public class BootstrapHandler : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _bootStraps;
        
        private void Awake()
        {
            foreach (var component in _bootStraps)
            {
                if (component is IBootStrap bootStrap)
                {
                    bootStrap.Install();
                }
            }
        }
    }
}