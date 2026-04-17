using System;
using Shinrai.VFX;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyHealth : EntityHealth
    {
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private SelfContainVFX _bloodParticle;
        [SerializeField] private HitEffect _hitEffect;
        public Action<EnemyController> OnEnemyDeath;

        public override void TakeDamage(float damage, EntityController source)
        {
            base.TakeDamage(damage, source);
            if (_hitEffect != null)
            {
                _hitEffect.PlayHitEffect();
            }
        }


        protected override void Kill()
        {
            OnEnemyDeath?.Invoke(_enemyController);
            if (_bloodParticle != null)
            {
                var particle = Instantiate(_bloodParticle, transform.position, Quaternion.identity);
                particle.PlayVFX();
                
            }
            base.Kill();
        }
    }
}
