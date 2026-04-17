using UnityEngine;

namespace Shinrai.VFX
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private int _hitAnimTrigger = Animator.StringToHash("Hit");
        private readonly float[] _rangeAngles = {0, 30, 60, 90, 120, 150, 180};

        public void PlayHitEffect()
        {
            transform.Rotate(Vector3.forward, _rangeAngles[Random.Range(0, _rangeAngles.Length)]);
            _animator.SetTrigger(_hitAnimTrigger);
        }
    }
}
