
using System;
using Shinrai.Modifiers;

namespace Shinrai.Core
{
    public static class Constant
    {
        public const float MIN_FIRE_RATE = 80;
        public const float MAX_FIRE_RATE = 10;
        public const float MIN_DELAY = 0.1f;
        public const float MAX_DELAY = 2f;

        public static float CheckMax(StatTarget statTarget, float currentValue)
        {
            switch (statTarget)
            {
                case StatTarget.MaxHP:
                case StatTarget.CurrentHP:
                case StatTarget.MinDamage:
                case StatTarget.MaxDamage:
                case StatTarget.MoveSpeed:
                    return currentValue;
                case StatTarget.FireRate:
                    return currentValue >= MAX_FIRE_RATE ? MAX_FIRE_RATE : currentValue;
            }
            return currentValue;
        }

        public static float CheckMin(StatTarget statTarget, float currentValue)
        {
            switch (statTarget)
            {
                case StatTarget.MaxHP:
                case StatTarget.CurrentHP:
                case StatTarget.MinDamage:
                case StatTarget.MaxDamage:
                case StatTarget.MoveSpeed:
                    return currentValue <= 0 ? 0 : currentValue;
                case StatTarget.FireRate:
                    return currentValue <= MIN_FIRE_RATE ? MIN_FIRE_RATE : currentValue;
            }
            return currentValue;
        }
    }
}
