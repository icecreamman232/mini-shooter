using System;
using System.Collections.Generic;
using Shinrai.Core;
using Shinrai.Data;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Modifiers
{
    public class StatComponent : MonoBehaviour
    {
        [SerializeField] private CharacterData _characterData;
        private Dictionary<StatTarget, float> _baseValues;
        private Dictionary<StatTarget, float> _finalValues;
        
        public IEnumerable<StatTarget> AllStats => _baseValues.Keys;

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
                [StatTarget.CurrentHP] = _characterData.DefaultHealth,
                [StatTarget.MaxHP] = _characterData.DefaultHealth,
                [StatTarget.MoveSpeed] = _characterData.DefaultSpeed,
                [StatTarget.MinDamage] = _characterData.DefaultMinDamage,
                [StatTarget.MaxDamage] = _characterData.DefaultMaxDamage,
                [StatTarget.FireRate] = _characterData.DefaultFireRate,
                [StatTarget.NumberOfShot] = 1,
                [StatTarget.ProjectileSize] = 1,
                [StatTarget.ProjectileSpeed] = _characterData.ProjectileSpeed,
            };
            _finalValues = new Dictionary<StatTarget, float>()
            {
                [StatTarget.CurrentHP] = _characterData.DefaultHealth,
                [StatTarget.MaxHP] = _characterData.DefaultHealth,
                [StatTarget.MoveSpeed] = _characterData.DefaultSpeed,
                [StatTarget.MinDamage] = _characterData.DefaultMinDamage,
                [StatTarget.MaxDamage] = _characterData.DefaultMaxDamage,
                [StatTarget.FireRate] = _characterData.DefaultFireRate,
                [StatTarget.NumberOfShot] = 1,
                [StatTarget.ProjectileSize] = 1,
                [StatTarget.ProjectileSpeed] = _characterData.ProjectileSpeed,
            };
            
            EventBus.Subscribe<ExternalStateChangeEvent>(OnExternalStateChanged);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<ExternalStateChangeEvent>(OnExternalStateChanged);
        }

        private void OnExternalStateChanged(ExternalStateChangeEvent eventArgs)
        {
            eventArgs.Value = Constant.CheckMax(eventArgs.StatTarget, eventArgs.Value);
            eventArgs.Value = Constant.CheckMin(eventArgs.StatTarget, eventArgs.Value);
            if (eventArgs.StatTarget == StatTarget.CurrentHP)
            {
                eventArgs.Value = Mathf.Clamp(eventArgs.Value, 0, _finalValues[StatTarget.MaxHP]);
            }
            _baseValues[eventArgs.StatTarget] = eventArgs.Value;
        }

        public float GetBase(StatTarget stat) => _baseValues[stat];
        public float GetFinal(StatTarget stat) => _finalValues[stat];
        
        public void SetFinal(StatTarget stat, float value, bool isRecalculated = false)
        {
            value = Constant.CheckMax(stat, value);
            value = Constant.CheckMin(stat, value);
            if (stat == StatTarget.CurrentHP)
            {
                value = Mathf.Clamp(value, 0, _finalValues[StatTarget.MaxHP]);
            }
            _finalValues[stat] = value;
            OnStatChanged?.Invoke(new StatChangeEvent(stat, _baseValues[stat], _finalValues[stat]));
        }
    }
}
