using System;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    /// <summary>
    /// Allows cruisers to apply extreme modifiers to specific buildables by name.
    /// Used for signature units/buildings that build nearly instantly.
    /// </summary>
    [Serializable]
    public class SpecializedBuildableModifiers
    {
        public float buildTimeMultiplier = 1f;
        public int droneRequirementOverride = 0; // 0 means no override
        public float healthMultiplier = 1f;

        public SpecializedBuildableModifiers(float buildTimeMultiplier, int droneRequirementOverride, float healthMultiplier)
        {
            this.buildTimeMultiplier = buildTimeMultiplier;
            this.droneRequirementOverride = droneRequirementOverride;
            this.healthMultiplier = healthMultiplier;
        }
    }
}

