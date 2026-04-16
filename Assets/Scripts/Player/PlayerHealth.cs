using Shinrai.Core;
using Shinrai.Modifiers;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerHealth : EntityHealth
    {
        [Header("Player")]
        /// <summary>
        /// In divine mode, player will not take damage from enemies
        /// </summary>
        [SerializeField] private bool _isDivineMode = false;
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
            _statComponent = statComponent;
            _maxHealth = _statComponent.GetBase(StatTarget.MaxHP);
            _statComponent.OnStatChanged += OnStatChanged;
            base.Initialize();
            UpdateHealthBar();
        }

        public override void TakeDamage(float damage, EntityController source)
        {
            if (_isDivineMode) return;
            base.TakeDamage(damage, source);
        }


        protected override void UpdateHealthBar()
        {
            base.UpdateHealthBar();
            _playerHealthChangedEvent.CurrentHealth = _currentHealth;
            _playerHealthChangedEvent.MaxHealth = _maxHealth;
            EventBus.Emit(_playerHealthChangedEvent);
            EventBus.Emit(new ExternalStateChangeEvent(StatTarget.CurrentHP, _currentHealth));
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
