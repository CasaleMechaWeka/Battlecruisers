using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Rocket Bonus
    /// </summary>
    public class PvPPistol : PvPCruiser
    {
        public float fireRateRocketBonus;
        public float buildSpeedForRocketBuildings;

        public override void Initialise(PvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(fireRateRocketBonus > 0);
            Assert.IsTrue(buildSpeedForRocketBuildings > 0);

            IBoostProvider rocketFireRateBoostProvider = new BoostProvider(fireRateRocketBonus);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(rocketFireRateBoostProvider);

            IBoostProvider buildSpeedForRocketProvider = new BoostProvider(buildSpeedForRocketBuildings);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders.Add(buildSpeedForRocketProvider);
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}
