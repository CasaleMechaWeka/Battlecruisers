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
    public class Shepherd : Cruiser
    {
        public float buildablesFireRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 58) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                buildablesFireRateBoost = SetUltraCruiserUtility(args, buildablesFireRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(buildablesFireRateBoost > 0);

            IBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildablesFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
        }
    }
}

