using BattleCruisers.Buildables.Boost;
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

        public override void Initialise(PvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(offensivesFireRateBoost > 0);
            Assert.IsTrue(offensivesBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(offensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider buildRateBoostProvider = new BoostProvider(offensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.OffensivesProviders.Add(buildRateBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}