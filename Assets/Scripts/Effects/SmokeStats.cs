namespace BattleCruisers.Effects
{
    public class SmokeStats
    {
        public float StartLifetimeMin { get; private set; }
        public float StartLifetimeMax { get; private set; }
        public float EmissionRatePerS { get; private set; }
        public int MaxNumberOfParticles { get; private set; }

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