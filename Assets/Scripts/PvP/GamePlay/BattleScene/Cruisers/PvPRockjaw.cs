using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives (and broadsides) build rate
    /// + Increases offensives (and broadsides) fire rate
    /// </summary>
    public class PvPRockjaw : PvPCruiser
    {
        public float offensivesFireRateBoost;
        public float offensivesBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(offensivesFireRateBoost > 0);
            Assert.IsTrue(offensivesBuildRateBoost > 0);

            IPvPBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(offensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IPvPBoostProvider buildRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(offensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.OffensivesProviders.Add(buildRateBoostProvider);
        }
    }
}