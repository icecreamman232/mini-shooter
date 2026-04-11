using UnityEngine;

namespace Shinrai.Core
{
    public static class Util
    {
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
    }
}
