using System;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyHealth : EntityHealth
    {
        [SerializeField] private EnemyController _enemyController;
        public Action<EnemyController> OnEnemyDeath;

        protected override void Kill()
        {
            OnEnemyDeath?.Invoke(_enemyController);
            base.Kill();
        }
    }
}
