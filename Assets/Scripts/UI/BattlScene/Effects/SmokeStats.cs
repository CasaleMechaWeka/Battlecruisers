namespace BattleCruisers.UI.BattleScene.Effects
{
    public class SmokeStats
    {
        public float SizeMultiplier { get; private set; }
        public float StartLifetimeMin { get; private set; }
        public float StartLifetimeMax { get; private set; }
        public float EmissionRatePerS { get; private set; }

        public SmokeStats(
            float sizeMultiplier,
            float startLifetimeMin,
            float startLifetimeMax,
            float emissionRatePerS)
        {
            SizeMultiplier = sizeMultiplier;
            StartLifetimeMin = startLifetimeMin;
            StartLifetimeMax = startLifetimeMax;
            EmissionRatePerS = emissionRatePerS;
        }
    }
}