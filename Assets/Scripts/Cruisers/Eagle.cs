using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
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
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 36) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                droneBuildingBuildRateBoost = SetUltraCruiserUtility(args, droneBuildingBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(droneBuildingBuildRateBoost > 0);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(droneBuildingBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(boostProvider);
        }
    }
}