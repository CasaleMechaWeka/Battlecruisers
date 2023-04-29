using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public static class PvPStaticSmokeStats
    {
        public static class PvPShip
        {
            public static PvPSmokeStatistics Weak { get; }
            public static PvPSmokeStatistics Normal { get; }
            public static PvPSmokeStatistics Strong { get; }

            static PvPShip()
            {
                Weak = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(3, 4),
                    StartSpeed = 0.85f,
                    RateOverTime = 6,
                    RateOverDistance = 16,
                    BurstsCount = 0,
                    EdgeRadius = 0.001f,
                    NoiseStrength = 0.01f,
                    StartSize = 0.05f
                };
                Normal = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(3, 4),
                    StartSpeed = 1,
                    RateOverTime = 6,
                    RateOverDistance = 16,
                    BurstsCount = 4,
                    EdgeRadius = 0.1f,
                    NoiseStrength = 0.05f,
                    StartSize = 0.1f
                };

                Strong = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(4, 6),
                    StartSpeed = 1,
                    RateOverTime = 4,
                    RateOverDistance = 12,
                    BurstsCount = 8,
                    EdgeRadius = 0.2f,
                    NoiseStrength = 0.1f,
                    StartSize = 0.3f
                };
            }
        }

        public static class PvPBuilding
        {
            public static PvPSmokeStatistics Weak { get; }
            public static PvPSmokeStatistics Normal { get; }
            public static PvPSmokeStatistics Strong { get; }

            static PvPBuilding()
            {
                Weak = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(2.5f, 3.5f),
                    StartSpeed = 0.25f,
                    StartSize = 0.005f,
                    NoiseStrength = 0.005f,
                    VelocityOverLifetimeY = 0.3f
                };

                Normal = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(2.5f, 3.5f),
                    StartSpeed = 0.5f,
                    StartSize = 0.02f,
                    NoiseStrength = 0.02f,
                    VelocityOverLifetimeY = 0.5f
                };

                Strong = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(3.5f, 5),
                    StartSpeed = 1,
                    StartSize = 0.05f,
                    NoiseStrength = 0.03f,
                    VelocityOverLifetimeY = 1
                };
            }
        }

        public static class PvPAircraft
        {
            public static PvPSmokeStatistics Weak { get; }
            public static PvPSmokeStatistics Normal { get; }
            public static PvPSmokeStatistics Strong { get; }

            static PvPAircraft()
            {
                Weak = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(1.1f, 1.3f),
                    StartSize = 0.05f,
                    RateOverTime = 0,
                    RateOverDistance = 39
                };

                Normal = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(1.7f, 1.9f),
                    StartSize = 0.07f,
                    RateOverTime = 0,
                    RateOverDistance = 35
                };

                Strong = new PvPSmokeStatistics()
                {
                    StartLifetime = new PvPRange<float>(2, 2.3f),
                    StartSize = 0.09f,
                    RateOverTime = 0,
                    RateOverDistance = 31
                };
            }
        }
    }
}