using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Effects.Smoke
{
    public static class StaticSmokeStats
    {
        public static class Ship
        {
            public static SmokeStatistics Weak { get; }
            public static SmokeStatistics Normal { get; }
            public static SmokeStatistics Strong { get; }

            static Ship()
            {
                Weak
                    = new SmokeStatistics(
                        startLifetime: new Range<float>(3, 4),
                        startSpeed: 0.85f,
                        rateOverTime: 6,
                        rateOverDistance: 16,
                        burstsCount: 0,
                        edgeRadius: 0.001f,
                        noiseStrength: 0.01f,
                        startSize: 0.05f);

                Normal
                    = new SmokeStatistics(
                        startLifetime: new Range<float>(3, 4),
                        startSpeed: 1,
                        rateOverTime: 6,
                        rateOverDistance: 16,
                        burstsCount: 30,
                        edgeRadius: 0.1f,
                        noiseStrength: 0.05f,
                        startSize: 0.1f);
                
                Strong
                    = new SmokeStatistics(
                        startLifetime: new Range<float>(6, 4),
                        startSpeed: 1,
                        rateOverTime: 4,
                        rateOverDistance: 12,
                        burstsCount: 50,
                        edgeRadius: 0.2f,
                        noiseStrength: 0.1f,
                        startSize: 0.3f);
            }
        }

        // FELIX  Remove :)
        /// <summary>
        /// For:
        /// + Airplanes
        /// + Buildings
        /// + Small ships (attack boat, frigate)
        /// </summary>
        public static class Small
        {
            public static SmokeStats WeakSmoke { get; }
            public static SmokeStats NormalSmoke { get; }
            public static SmokeStats StrongSmoke { get; }

            static Small()
            {
                WeakSmoke = new SmokeStats(startLifetimeMin: 1, startLifetimeMax: 2, emissionRatePerS: 10, maxNumberOfParticles: 100);
                NormalSmoke = new SmokeStats(startLifetimeMin: 2, startLifetimeMax: 4, emissionRatePerS: 20, maxNumberOfParticles: 100);
                StrongSmoke = new SmokeStats(startLifetimeMin: 4, startLifetimeMax: 6, emissionRatePerS: 30, maxNumberOfParticles: 200);
            }
        }

        /// <summary>
        /// For:
        /// + Big ships (destroyer, archon battleship)
        /// </summary>
        public static class Normal
        {
            public static SmokeStats WeakSmoke { get; }
            public static SmokeStats NormalSmoke { get; }
            public static SmokeStats StrongSmoke { get; }

            static Normal()
            {
                WeakSmoke = new SmokeStats(startLifetimeMin: 1, startLifetimeMax: 2, emissionRatePerS: 10, maxNumberOfParticles: 100);
                NormalSmoke = new SmokeStats(startLifetimeMin: 2, startLifetimeMax: 4, emissionRatePerS: 20, maxNumberOfParticles: 100);
                StrongSmoke = new SmokeStats(startLifetimeMin: 4, startLifetimeMax: 6, emissionRatePerS: 30, maxNumberOfParticles: 200);
            }
        }

        /// <summary>
        /// For:
        /// + Cruisers
        /// </summary>
        public static class Big
        {
            public static SmokeStats WeakSmoke { get; }
            public static SmokeStats NormalSmoke { get; }
            public static SmokeStats StrongSmoke { get; }

            static Big()
            {
                WeakSmoke = new SmokeStats(startLifetimeMin: 2, startLifetimeMax: 4, emissionRatePerS: 10, maxNumberOfParticles: 100);
                NormalSmoke = new SmokeStats(startLifetimeMin: 4, startLifetimeMax: 6, emissionRatePerS: 20, maxNumberOfParticles: 100);
                StrongSmoke = new SmokeStats(startLifetimeMin: 6, startLifetimeMax: 10, emissionRatePerS: 30, maxNumberOfParticles: 250);
            }
        }
    }
}