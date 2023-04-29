using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases air factory build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class PvPLongbow : PvPCruiser
    {
        public float airFactoryBuildRateBoost;
        public float aircarftBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircarftBuildRateBoost > 0);

            IPvPBoostProvider factoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(factoryBoostProvider);

            IPvPBoostProvider aircraftBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(aircarftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);
        }
    }
}