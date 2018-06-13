using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class TurretStatsBoostables : ITurretStatsBoostables
    {
        public IBoostable AccuracyBoostable { get; private set; }
        public IBoostable FireRateBoostable { get; private set; }

        public TurretStatsBoostables(IBoostFactory boostFactory)
        {
            Assert.IsNotNull(boostFactory);

            AccuracyBoostable = boostFactory.CreateBoostable();
            FireRateBoostable = boostFactory.CreateBoostable();
        }
    }
}
