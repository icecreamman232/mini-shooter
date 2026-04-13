using Shinrai.Core;
using Shinrai.Modifiers;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerHealth : EntityHealth
    {
        [SerializeField] private StatComponent _statComponent;
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
        
        public override void Initialize()
        {
            base.Initialize();
            _playerHealthChangedEvent = new PlayerHealthChangedEvent(_currentHealth, _maxHealth);
            UpdateHealthBar();
            _statComponent.OnStatChanged += OnStatChanged;
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
