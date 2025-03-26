using BattleCruisers.Buildables;
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
        public float shieldBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            if (args.Faction == Faction.Reds && ApplicationModel.SelectedLevel == 2) //Level #2 "Jimmo" would be too hard otherwise
                shieldBuildRateBoost = 1f;

            if (ApplicationModel.SelectedLevel == 34) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                defensivesFireRateBoost = SetUltraCruiserUtility(args, defensivesFireRateBoost);
                defensivesBuildRateBoost = SetUltraCruiserUtility(args, defensivesBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(defensivesFireRateBoost > 0);
            Assert.IsTrue(defensivesBuildRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(defensivesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider buildRateBoostProvider = new BoostProvider(defensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DefensivesProviders.Add(buildRateBoostProvider);

            IBoostProvider shieldBuildRateBoostProvider = new BoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(shieldBuildRateBoostProvider);
        }
    }
}