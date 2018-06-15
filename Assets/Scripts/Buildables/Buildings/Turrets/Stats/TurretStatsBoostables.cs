using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    /// <summary>
    /// Keeps track of global turret boost levels, and exposes the cumulative
    /// boost in simple properties.
    /// </summary>
    public class TurretStatsBoostables : ITurretStatsBoostables
    {
        private readonly IBoostable _accuracyBoostable, _fireRateBoostable;
        private readonly IBoostableGroup _accuracyBoostableGroup, _fireRateBoostabelGroup;

        public float AccuracyMultiplier { get { return _accuracyBoostable.BoostMultiplier; } }
        public float FireRateMultiplier { get { return _fireRateBoostable.BoostMultiplier; } }

        public TurretStatsBoostables(IBoostFactory boostFactory)
        {
            Assert.IsNotNull(boostFactory);

            _accuracyBoostable = boostFactory.CreateBoostable();
            _fireRateBoostable = boostFactory.CreateBoostable();

            _accuracyBoostableGroup = boostFactory.CreateBoostableGroup();
            _accuracyBoostableGroup.AddBoostable(_accuracyBoostable);

            _fireRateBoostabelGroup = boostFactory.CreateBoostableGroup();
            _fireRateBoostabelGroup.AddBoostable(_fireRateBoostable);
        }

        public void Initialise(IGlobalBoostProviders globalBoostProviders)
        {
            _accuracyBoostableGroup.AddBoostProvidersList(globalBoostProviders.TurretAccuracyBoostProviders);
            _fireRateBoostabelGroup.AddBoostProvidersList(globalBoostProviders.TurretFireRateBoostProviders);
        }

        public void DisposeManagedState()
        {
            _accuracyBoostableGroup.CleanUp();
            _fireRateBoostabelGroup.CleanUp();
        }
    }
}
