using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases naval factory build speed
    /// + Increases ship build speed
    /// </summary>
    public class Hammerhead : Cruiser
    {
        public float navalFactoryBuildRateBoost;
        public float shipBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            if (ApplicationModel.SelectedLevel == 37) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                navalFactoryBuildRateBoost = SetUltraCruiserUtility(args, navalFactoryBuildRateBoost);
                shipBuildRateBoost = SetUltraCruiserUtility(args, shipBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(navalFactoryBuildRateBoost > 0);
            Assert.IsTrue(shipBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = new BoostProvider(navalFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(factoryBoostProvider);

            IBoostProvider shipBoostProvider = new BoostProvider(shipBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);
        }
    }
}