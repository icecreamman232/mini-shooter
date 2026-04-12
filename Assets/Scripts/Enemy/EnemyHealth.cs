using System;
using Shinrai.VFX;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyHealth : EntityHealth
    {
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private SelfContainVFX _bloodParticle;
        public Action<EnemyController> OnEnemyDeath;

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
