using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// </summary>
    public class Rickshaw : Cruiser
    {
        public float tacticalsBuildRateBoost;
        public float droneBuildingBuildRateBoost;
        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 32) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                tacticalsBuildRateBoost = SetUltraCruiserUtility(args, tacticalsBuildRateBoost);
            }
            base.Initialise(args);
            Assert.IsTrue(tacticalsBuildRateBoost > 0);
            Assert.IsTrue(droneBuildingBuildRateBoost > 0);

            IBoostProvider tacticalsBoostProvider = new BoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(tacticalsBoostProvider);

            IBoostProvider boostProvider = new BoostProvider(droneBuildingBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(boostProvider);
        }
    }
}