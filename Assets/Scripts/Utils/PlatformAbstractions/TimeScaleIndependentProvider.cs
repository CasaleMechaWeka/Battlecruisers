using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class TimeScaleIndependentProvider : IDeltaTimeProvider
    {
        public float DeltaTime { get { return Time.unscaledDeltaTime; } }
    }
}
