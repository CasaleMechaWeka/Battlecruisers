using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases drone station and ultralisk build speed.
    /// </summary>
    public class PvPYeti : PvPCruiser
    {
        public float buildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildRateBoost > 0);

            IPvPBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.OffensivesProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders.Add(boostProvider);

            IPvPBoostProvider aircraftBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);

            IPvPBoostProvider shipBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}