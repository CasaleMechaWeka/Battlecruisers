using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increse all buildables health
    /// </summary>
    public class Megalith : Cruiser //Change scripts so it boosts all buildables health
    {
        public float buildingHealthBoost;

        public override void Initialise(ICruiserArgs args)
        {
            if (ApplicationModel.SelectedLevel == 39) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                buildingHealthBoost = SetUltraCruiserUtility(args, buildingHealthBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(buildingHealthBoost > 0);

            IBoostProvider buildingHealthBoostProvider = new BoostProvider(buildingHealthBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingHealth.AllBuildingsProviders.Add(buildingHealthBoostProvider);
        }
    }
}