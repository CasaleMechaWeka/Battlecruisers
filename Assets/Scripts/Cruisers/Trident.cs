using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// </summary>
    public class Trident : Cruiser
    {
        public float tacticalsBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(tacticalsBuildRateBoost > 0);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(tacticalsBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.TacticalsBuildRateBoostProviders.Add(boostProvider);
            FactoryProvider.GlobalBoostProviders.ShieldsBuildRateBoostProviders.Add(boostProvider);
        }
    }
}