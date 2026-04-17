using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shinrai.Core
{
    public static class Util
    {
        private static System.Random _random = new System.Random();

        public static float NextRandomFloat()
        {
            return (float)_random.NextDouble();
        }
        
        /// <summary>
        /// Remap a value from one range to another
        /// </summary>
        /// <param name="value">Value to remap</param>
        /// <param name="from1">Min bound of the original range</param>
        /// <param name="to1">Max bound of the original range</param>
        /// <param name="from2">Min bound of the target range</param>
        /// <param name="to2">Max bound of the target range</param>
        /// <returns></returns>
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return from2 + (value - from1)/(to1 - from1) * (to2 - from2);
        }
        
        public static bool IsInLayerMask(int layerWantToCheck, LayerMask layerMask)
        {
            if (((1 << layerWantToCheck) & layerMask) != 0)
            {
                return true;
            }
            return false;
        }

        public static T PickRandom<T>(this List<T> list)
        {
            return list.Count == 0 ? default : list[Random.Range(0, list.Count)];
        }
        
        public static T PickRandom<T>(this T[] array)
        {
            return array.Length == 0 ? default : array[Random.Range(0, array.Length)];
        }


        public static void AddToDictionaryList<TKey, TValue>(Dictionary<TKey, List<TValue>> dict, TKey key, TValue value)
        {
            if (!dict.TryGetValue(key, out var list))
            {
                list = new List<TValue>();
                dict[key] = list;
            }

            list.Add(value);
        }

        public static void DelayCall(this MonoBehaviour mb, float delay, System.Action action)
        {
            mb.StartCoroutine(DelayCoroutine(delay, action));
        }

        private static IEnumerator DelayCoroutine(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        
        /// <summary>
        /// Shuffles the elements of a list in place using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to shuffle</param>
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        /// <summary>
        /// Convert fire rate to delay (delay between 2 shots)
        /// </summary>
        public static float FireRateToDelay(float fireRate)
        {
            return Remap(fireRate, Constant.MIN_FIRE_RATE, Constant.MAX_FIRE_RATE, Constant.MIN_DELAY, Constant.MAX_DELAY);
        }
        
        
        /// <summary>
        /// Convert delay (delay between 2 shots) to fire rate
        /// </summary>
        public static float DelayToFireRate(float delay)
        {
            return Remap(delay, Constant.MIN_DELAY, Constant.MAX_DELAY, Constant.MIN_FIRE_RATE, Constant.MAX_FIRE_RATE);
        }
    }
}
