using BattleCruisers.Buildables.Boost;
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

        public override void Initialise(PvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildRateBoost > 0);

            IBoostProvider boostProvider = new BoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.OffensivesProviders.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders.Add(boostProvider);

            IBoostProvider aircraftBoostProvider = new BoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);

            IBoostProvider shipBoostProvider = new BoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}