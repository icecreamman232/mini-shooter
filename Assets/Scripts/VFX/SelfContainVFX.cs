using UnityEngine;
using UnityEngine.InputSystem;

namespace Shinrai.VFX
{
    public class SelfContainVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public void PlayVFX()
        {
            _particleSystem.Play();
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback; 
        }

        private void OnParticleSystemStopped()
        {
            Destroy(gameObject);
        }
    }
}
