using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    // FELIX  Merge with TimeBC?
    public class TimeScaleIndependentProvider : IDeltaTimeProvider
    {
        public float DeltaTime { get { return Time.unscaledDeltaTime; } }
    }
}
