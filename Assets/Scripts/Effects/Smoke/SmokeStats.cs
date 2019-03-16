namespace BattleCruisers.Effects.Smoke
{
    public class SmokeStats
    {
        public float StartLifetimeMin { get; }
        public float StartLifetimeMax { get; }
        public float EmissionRatePerS { get; }
        public int MaxNumberOfParticles { get; }

        public SmokeStats(
            float startLifetimeMin,
            float startLifetimeMax,
            float emissionRatePerS,
            int maxNumberOfParticles)
        {
            StartLifetimeMin = startLifetimeMin;
            StartLifetimeMax = startLifetimeMax;
            EmissionRatePerS = emissionRatePerS;
            MaxNumberOfParticles = maxNumberOfParticles;
        }
    }
}