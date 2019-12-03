using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Effects.Smoke
{
    public class SmokeStatistics
    {
        public IRange<float> StartLifetime { get; }
        public float StartSpeed { get; }
        public float StartSize { get; }
        public float RateOverTime { get; }
        public float RateOverDistance { get; }
        public int BurstsCount { get; }
        public float EdgeRadius { get; }
        public float NoiseStrength { get; }

        public SmokeStatistics(
            IRange<float> startLifetime, 
            float startSpeed, 
            float rateOverTime, 
            float rateOverDistance, 
            int burstsCount, 
            float edgeRadius, 
            float noiseStrength, 
            float startSize)
        {
            StartLifetime = startLifetime;
            StartSpeed = startSpeed;
            RateOverTime = rateOverTime;
            RateOverDistance = rateOverDistance;
            BurstsCount = burstsCount;
            EdgeRadius = edgeRadius;
            NoiseStrength = noiseStrength;
            StartSize = startSize;
        }
    }
}