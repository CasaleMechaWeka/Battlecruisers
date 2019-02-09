using BattleCruisers.Buildables.Boost;
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
        // FELIX  Use :)
        public float shipBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(navalFactoryBuildRateBoost > 0);
            Assert.IsTrue(shipBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(navalFactoryBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(factoryBoostProvider);
        }
    }
}