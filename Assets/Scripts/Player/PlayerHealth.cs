using Shinrai.Core;
using Shinrai.Modifiers;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerHealth : EntityHealth
    {
        private StatComponent _statComponent;
        private PlayerHealthChangedEvent _playerHealthChangedEvent;

        private void OnDestroy()
        {
            _statComponent.OnStatChanged -= OnStatChanged;
        }

        [ContextMenu("Test Damage")]
        public void TestDamage()
        {
            TakeDamage(10, null);
        }
        
        public void Initialize(StatComponent statComponent)
        {
            
            _playerHealthChangedEvent = new PlayerHealthChangedEvent(_currentHealth, _maxHealth);
            UpdateHealthBar();
            _statComponent = statComponent;
            _maxHealth = _statComponent.GetBase(StatTarget.MaxHP);
            _statComponent.OnStatChanged += OnStatChanged;
            base.Initialize();
        }
        
        
        protected override void UpdateHealthBar()
        {
            base.UpdateHealthBar();
            _playerHealthChangedEvent.CurrentHealth = _currentHealth;
            _playerHealthChangedEvent.MaxHealth = _maxHealth;
            EventBus.Emit(_playerHealthChangedEvent);
        }
        
        private void OnStatChanged(StatComponent.StatChangeEvent eventArgs)
        {
            if(!(eventArgs.StatTarget is StatTarget.CurrentHP or StatTarget.MaxHP)) return;

            if (eventArgs.StatTarget == StatTarget.CurrentHP)
            {
                _currentHealth = eventArgs.NewValue;
                UpdateHealthBar();
            }
            else
            {
                var portionCurrentHP = _currentHealth / _maxHealth;
                _maxHealth = eventArgs.NewValue;
                _currentHealth = _maxHealth * portionCurrentHP;
                UpdateHealthBar();
            }
        }
    }
}
