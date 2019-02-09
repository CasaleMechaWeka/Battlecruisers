using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives (and broadsides) build rate
    /// + Increases offensives (and broadsides) fire rate
    /// </summary>
    public class Rockjaw : Cruiser
    {
        public float offensivesFireRateBoost;
        public float offensivesBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(offensivesFireRateBoost > 0);
            Assert.IsTrue(offensivesBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(offensivesFireRateBoost);
            FactoryProvider.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider buildRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(offensivesBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.BuildRate.OffensivesProviders.Add(buildRateBoostProvider);
        }
    }
}