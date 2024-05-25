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
        public float OffenseFireRate;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 58) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                OffenseFireRate = SetUltraCruiserUtility(args, OffenseFireRate);
            }
            base.Initialise(args);

            Assert.IsTrue(OffenseFireRate > 0);

            IBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(OffenseFireRate);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
        }
    }
}

