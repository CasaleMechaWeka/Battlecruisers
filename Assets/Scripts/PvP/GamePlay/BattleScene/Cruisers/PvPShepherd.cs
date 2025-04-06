using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives and defensive fire rate
    /// </summary>
    public class PvPShepherd : PvPCruiser
    {
        public float buildingFireRateBoost;

        public override void Initialise(PvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildingFireRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(buildingFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(fireRateBoostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}

