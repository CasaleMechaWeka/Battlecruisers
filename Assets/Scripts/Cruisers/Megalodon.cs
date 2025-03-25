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
            if (ApplicationModel.SelectedLevel == 39) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                ultrasBuildRateBoost = SetUltraCruiserUtility(args, ultrasBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(ultrasBuildRateBoost > 0);

            IBoostProvider boostProvider = new BoostProvider(ultrasBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders.Add(boostProvider);
        }
    }
}