using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Rocket Bonus
    /// </summary>
    public class Pistol : Cruiser
    {
        public float fireRateRocketBonus;
        public float buildSpeedForRocketBuildings;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 85) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                fireRateRocketBonus = SetUltraCruiserUtility(args, fireRateRocketBonus);
                buildSpeedForRocketBuildings = SetUltraCruiserUtility(args, buildSpeedForRocketBuildings);
            }
            base.Initialise(args);

            Assert.IsTrue(fireRateRocketBonus > 0);
            Assert.IsTrue(buildSpeedForRocketBuildings > 0);

            IBoostProvider rocketFireRateBoostProvider = new BoostProvider(fireRateRocketBonus);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(rocketFireRateBoostProvider);

            IBoostProvider buildSpeedForRocketProvider = new BoostProvider(buildSpeedForRocketBuildings);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders.Add(buildSpeedForRocketProvider);
        }
    }
}
