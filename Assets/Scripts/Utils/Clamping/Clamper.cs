using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Utils.Clamping
{
    public class Clamper : IClamper
    {
        public float Clamp(float value, IRange<float> validRange)
        {
            return Mathf.Clamp(value, validRange.Min, validRange.Max);
        }
    }
}