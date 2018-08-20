using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    // FELIX  Merge with TimeBC?
    public class TimeScaleIndependentProvider : IDeltaTimeProvider
    {
        public float UnscaledDeltaTime { get { return Time.unscaledDeltaTime; } }
    }
}
