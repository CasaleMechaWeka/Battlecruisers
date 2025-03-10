using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases ultras build speed
    /// </summary>
    public class Megalodon : Cruiser
    {
        public float ultrasBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 39) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                ultrasBuildRateBoost = SetUltraCruiserUtility(args, ultrasBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(ultrasBuildRateBoost > 0);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(ultrasBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders.Add(boostProvider);
        }
    }
}