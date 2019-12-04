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
                Weak = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(3, 4),
                    StartSpeed = 0.85f,
                    RateOverTime = 6,
                    RateOverDistance = 16,
                    BurstsCount = 0,
                    EdgeRadius = 0.001f,
                    NoiseStrength = 0.01f,
                    StartSize = 0.05f
                };
                Normal = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(3, 4),
                    StartSpeed = 1,
                    RateOverTime = 6,
                    RateOverDistance = 16,
                    BurstsCount = 30,
                    EdgeRadius = 0.1f,
                    NoiseStrength = 0.05f,
                    StartSize = 0.1f
                };

                Strong = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(6, 4),
                    StartSpeed = 1,
                    RateOverTime = 4,
                    RateOverDistance = 12,
                    BurstsCount = 50,
                    EdgeRadius = 0.2f,
                    NoiseStrength = 0.1f,
                    StartSize = 0.3f
                };
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