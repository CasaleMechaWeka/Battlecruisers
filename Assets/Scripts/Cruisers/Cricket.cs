using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increse all buildables health
    /// </summary>
    public class Cricket : Cruiser //Change scripts so it boosts all buildables health
    {
        public float buildingHealthBoost;

        public override void Initialise(CruiserArgs args)
        {
            if (ApplicationModel.SelectedLevel == 50) //This is where UltraCruiser Level is designated
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