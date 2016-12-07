using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Slash.Unity.Common.Attributes
{
    /* MinMaxRangeAttribute.cs
    * by Eddie Cameron – For the public domain
    * From http://www.grapefruitgames.com/blog/2013/11/a-min-max-range-for-unity/
    * —————————-
    * Use a MinMaxRange class to replace twin float range values (eg: float minSpeed, maxSpeed; becomes MinMaxRange speed)
    * Apply a [MinMaxRange( minLimit, maxLimit )] attribute to a MinMaxRange instance to control the limits and to show a
    * slider in the inspector
    */

    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public float MaxLimit;

        public float MinLimit;

        public MinMaxRangeAttribute(float minLimit, float maxLimit)
        {
            this.MinLimit = minLimit;
            this.MaxLimit = maxLimit;
        }
    }

    [Serializable]
    public class MinMaxRange
    {
        public float RangeEnd;

        public float RangeStart;

        public float GetRandomValue()
        {
            return Random.Range(this.RangeStart, this.RangeEnd);
        }
    }
}