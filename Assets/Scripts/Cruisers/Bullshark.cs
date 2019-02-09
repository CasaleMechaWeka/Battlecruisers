using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases defensives build speed
    /// + Increases defensives fire rate
    /// </summary>
    public class Bullshark : Cruiser
    {
        public float defensivesFireRateBoost;
        public float defensivesBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(defensivesFireRateBoost > 0);
            Assert.IsTrue(defensivesBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(defensivesFireRateBoost);
            FactoryProvider.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider buildRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(defensivesBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(buildRateBoostProvider);
        }
    }
}