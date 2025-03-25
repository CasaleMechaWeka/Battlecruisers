using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives (and broadsides) build rate
    /// + Increases offensives (and broadsides) fire rate
    /// </summary>
    public class Rockjaw : Cruiser
    {
        public float offensivesFireRateBoost;
        public float offensivesBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            if (ApplicationModel.SelectedLevel == 35) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                offensivesFireRateBoost = SetUltraCruiserUtility(args, offensivesFireRateBoost);
                offensivesBuildRateBoost = SetUltraCruiserUtility(args, offensivesBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(offensivesFireRateBoost > 0);
            Assert.IsTrue(offensivesBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(offensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider buildRateBoostProvider = new BoostProvider(offensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.OffensivesProviders.Add(buildRateBoostProvider);
        }
    }
}