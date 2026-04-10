using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerHealth : EntityHealth
    {
        private PlayerHealthChangedEvent _playerHealthChangedEvent;
        
        [ContextMenu("Test Damage")]
        public void TestDamage()
        {
            TakeDamage(10, null);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            _playerHealthChangedEvent = new PlayerHealthChangedEvent(_currentHealth, _maxHealth);
            UpdateHealthBar();
        }

        protected override void UpdateHealthBar()
        {
            base.UpdateHealthBar();
            _playerHealthChangedEvent.CurrentHealth = _currentHealth;
            _playerHealthChangedEvent.MaxHealth = _maxHealth;
            EventBus.Emit(_playerHealthChangedEvent);
        }
    }
}
