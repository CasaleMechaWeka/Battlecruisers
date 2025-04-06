using BattleCruisers.Buildables.Boost;
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
        public float shieldBuildRateBoost;

        public override void Initialise(PvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(defensivesFireRateBoost > 0);
            Assert.IsTrue(defensivesBuildRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(defensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider buildRateBoostProvider = new BoostProvider(defensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(buildRateBoostProvider);

            IBoostProvider shieldBuildRateBoostProvider = new BoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(shieldBuildRateBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}