using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives and defensive fire rate
    /// </summary>
    public class Shepherd : Cruiser
    {
        public float buildingFireRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 58) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                buildingFireRateBoost = SetUltraCruiserUtility(args, buildingFireRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(buildingFireRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(buildingFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(fireRateBoostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);
        }
    }
}

