using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Decreases naval factory build speed
    /// + Increases ship build speed
    /// + Decreases aircraft factory build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class PvPFlea : PvPCruiser
    {
        public float navalFactoryBuildRateBoost;
        public float shipBuildRateBoost;
        public float airFactoryBuildRateBoost;
        public float aircraftBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(navalFactoryBuildRateBoost > 0);
            Assert.IsTrue(shipBuildRateBoost > 0);
            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IPvPBoostProvider navalFactoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(navalFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(navalFactoryBoostProvider);

            IPvPBoostProvider shipBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(shipBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);

            IPvPBoostProvider aircraftFactoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(aircraftFactoryBoostProvider);

            IPvPBoostProvider aircraftBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}