using System;
using System.Collections.Generic;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Modifiers
{
    public class StatComponent : MonoBehaviour
    {
        private Dictionary<StatTarget, float> _baseValues;
        private Dictionary<StatTarget, float> _finalValues;

        public event Action<StatChangeEvent> OnStatChanged;
        
        public readonly struct StatChangeEvent
        {
            public readonly StatTarget StatTarget;
            public readonly float OldValue;
            public readonly float NewValue;
            
            public StatChangeEvent(StatTarget statTarget, float oldValue, float newValue)
            {
                StatTarget = statTarget;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }
        
        
        public void Initialize(PlayerController player)
        {
            _baseValues = new Dictionary<StatTarget, float>
            {
                [StatTarget.CurrentHP] = player.Health.CurrentHealth,
                [StatTarget.MaxHP] = player.Health.MaxHealth,
                [StatTarget.MoveSpeed] = player.Movement.Speed,
                [StatTarget.MinDamage] = 0,
                [StatTarget.MaxDamage] = 0
            };
            _finalValues = new Dictionary<StatTarget, float>();
        }
        
        public float GetBase(StatTarget stat) => _baseValues[stat];
        public float GetFinal(StatTarget stat) => _finalValues[stat];
        
        public void SetFinal(StatTarget stat, float value)
        {
            _finalValues[stat] = value;
            OnStatChanged?.Invoke(new StatChangeEvent(stat, _baseValues[stat], value));
        }
    }
}
