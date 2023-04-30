using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases defensives build speed
    /// + Increases defensives fire rate
    /// </summary>
    public class PvPBullshark : PvPCruiser
    {
        public float defensivesFireRateBoost;
        public float defensivesBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(defensivesFireRateBoost > 0);
            Assert.IsTrue(defensivesBuildRateBoost > 0);

            IPvPBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(defensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IPvPBoostProvider buildRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(defensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(buildRateBoostProvider);
        }
    }
}