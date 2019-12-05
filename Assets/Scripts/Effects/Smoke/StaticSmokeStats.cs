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
                    StartLifetime = new Range<float>(4, 6),
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

        public static class Building
        {
            public static SmokeStatistics Weak { get; }
            public static SmokeStatistics Normal { get; }
            public static SmokeStatistics Strong { get; }

            static Building()
            {
                Weak = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(2.5f, 3.5f),
                    StartSpeed = 0.25f,
                    StartSize = 0.005f,
                    NoiseStrength = 0.005f,
                    VelocityOverLifetimeY = 0.3f
                };

                Normal = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(2.5f, 3.5f),
                    StartSpeed = 0.5f,
                    StartSize = 0.02f,
                    NoiseStrength = 0.02f,
                    VelocityOverLifetimeY = 0.5f
                };

                Strong = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(3.5f, 5),
                    StartSpeed = 1,
                    StartSize = 0.05f,
                    NoiseStrength = 0.03f,
                    VelocityOverLifetimeY = 1
                };
            }
        }

        public static class Aircraft
        {
            public static SmokeStatistics Weak { get; }
            public static SmokeStatistics Normal { get; }
            public static SmokeStatistics Strong { get; }

            static Aircraft()
            {
                Weak = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(0.3f, 0.45f),
                    StartSize = 0.02f,
                    RateOverTime = 2,
                    RateOverDistance = 24
                };

                Normal = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(1.5f, 2),
                    StartSize = 0.08f,
                    RateOverTime = 2,
                    RateOverDistance = 18
                };

                Strong = new SmokeStatistics()
                {
                    StartLifetime = new Range<float>(4, 6),
                    StartSize = 0.2f,
                    RateOverTime = 2,
                    RateOverDistance = 12
                };
            }
        }
    }
}