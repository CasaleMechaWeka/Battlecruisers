using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases ultras build speed
    /// </summary>
    public class Megalodon : Cruiser
    {
        public float ultrasBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(ultrasBuildRateBoost > 0);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(ultrasBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.UltrasBuildRateBoostProviders.Add(boostProvider);
        }
    }
}