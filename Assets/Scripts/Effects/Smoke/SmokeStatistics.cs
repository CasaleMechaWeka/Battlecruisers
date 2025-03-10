using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Effects.Smoke
{
    public class SmokeStatistics
    {
        public IRange<float> StartLifetime { get; set; }
        public float? StartSpeed { get; set; }
        public float? StartSize { get; set; }
        public float? RateOverTime { get; set; }
        public float? RateOverDistance { get; set; }
        public int? BurstsCount { get; set; }
        public float? EdgeRadius { get; set; }
        public float? NoiseStrength { get; set; }
        public float? VelocityOverLifetimeY { get; set; }
    }
}