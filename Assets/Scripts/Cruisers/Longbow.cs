using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases air factory build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class Longbow : Cruiser
    {
        public float airFactoryBuildRateBoost;
        // FELIX  Use :)
        public float aircarftBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircarftBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(airFactoryBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.BuildRate.AirFactoryProviders.Add(factoryBoostProvider);
        }
    }
}