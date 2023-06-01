using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// </summary>
    public class Trident : Cruiser
    {
        public float tacticalsBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 32) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                tacticalsBuildRateBoost = SetUltraCruiserUtility(args, tacticalsBuildRateBoost);
            }

            Assert.IsTrue(tacticalsBuildRateBoost > 0);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(boostProvider);
        }
    }
}