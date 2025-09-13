using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases defensives fire rate
    /// + Improves turret accuracy
    /// + Increases defensives build speed
    /// </summary>
    public class FortNova : Cruiser
    {
        public float defensivesFireRateBoost;
        public float turretAccuracyBoost;
        public float defensivesBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(defensivesFireRateBoost > 0);
            Assert.IsTrue(turretAccuracyBoost > 0);
            Assert.IsTrue(defensivesBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(defensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider accuracyBoostProvider = new BoostProvider(turretAccuracyBoost);
            CruiserSpecificFactories.GlobalBoostProviders.TurretAccuracyBoostProviders.Add(accuracyBoostProvider);

            IBoostProvider defensivesBuildRateProvider = new BoostProvider(defensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(defensivesBuildRateProvider);
        }
    }
}


