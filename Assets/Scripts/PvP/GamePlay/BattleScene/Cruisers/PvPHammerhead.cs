using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases naval factory build speed
    /// + Increases ship build speed
    /// </summary>
    public class PvPHammerhead : PvPCruiser
    {
        public float navalFactoryBuildRateBoost;
        public float shipBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(navalFactoryBuildRateBoost > 0);
            Assert.IsTrue(shipBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(navalFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(factoryBoostProvider);

            IBoostProvider shipBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(shipBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}