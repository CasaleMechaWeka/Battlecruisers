using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases shield recharge rate
    /// </summary>
    public class Raptor : Cruiser
    {
        public float shieldRechargeRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(shieldRechargeRateBoost);
            FactoryProvider.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(boostProvider);
        }
    }
}