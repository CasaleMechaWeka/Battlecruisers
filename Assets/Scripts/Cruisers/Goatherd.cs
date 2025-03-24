using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives fire rate
    /// </summary>
    public class Goatherd : Cruiser
    {
        public float MastStructureBuildRate;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 58) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                MastStructureBuildRate = SetUltraCruiserUtility(args, MastStructureBuildRate);
            }
            base.Initialise(args);

            Assert.IsTrue(MastStructureBuildRate > 0);

            IBoostProvider buildRateBoostProvider = new BoostProvider(MastStructureBuildRate);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders.Add(buildRateBoostProvider);
        }
    }
}

