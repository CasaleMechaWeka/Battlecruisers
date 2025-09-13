using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases ultras build speed
    /// + Increases mast/structure build speed
    /// </summary>
    public class Axiom : Cruiser
    {
        public float ultrasBuildRateBoost;
        public float mastStructureBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(ultrasBuildRateBoost > 0);
            Assert.IsTrue(mastStructureBuildRateBoost > 0);

            IBoostProvider ultrasBoost = new BoostProvider(ultrasBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders.Add(ultrasBoost);

            IBoostProvider mastBoost = new BoostProvider(mastStructureBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders.Add(mastBoost);
        }
    }
}


