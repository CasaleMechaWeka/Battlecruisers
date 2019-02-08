using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases drone station and ultralisk build speed.
    /// </summary>
    public class Eagle : Cruiser
    {
        public float droneBuildingBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(droneBuildingBuildRateBoost > 0);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(droneBuildingBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.DroneStationBuildRateBoostProviders.Add(boostProvider);
        }
    }
}