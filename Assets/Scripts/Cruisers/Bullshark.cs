using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases defensives build speed
    /// + Increases defensives fire rate
    /// </summary>
    public class Bullshark : Cruiser
    {
        public float defensivesFireRateBoost;
        public float defensivesBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 34) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                defensivesFireRateBoost = SetUltraCruiserUtility(args, defensivesFireRateBoost);
                defensivesBuildRateBoost = SetUltraCruiserUtility(args, defensivesBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(defensivesFireRateBoost > 0);
            Assert.IsTrue(defensivesBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(defensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider buildRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(defensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(buildRateBoostProvider);
        }
    }
}