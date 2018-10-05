namespace BattleCruisers.Effects
{
    public static class StaticSmokeStats
    {
        /// <summary>
        /// For:
        /// + Airplanes
        /// + Buildings
        /// </summary>
        public static class Small
        {
            public static SmokeStats WeakSmoke { get; private set; }
            public static SmokeStats NormalSmoke { get; private set; }
            public static SmokeStats StrongSmoke { get; private set; }

            static Small()
            {
                WeakSmoke = new SmokeStats(startLifetimeMin: 1, startLifetimeMax: 2, emissionRatePerS: 10);
                NormalSmoke = new SmokeStats(startLifetimeMin: 2, startLifetimeMax: 4, emissionRatePerS: 20);
                StrongSmoke = new SmokeStats(startLifetimeMin: 4, startLifetimeMax: 6, emissionRatePerS: 30);
            }
        }
    }
}