using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases defensives fire rate
    /// </summary>
    public class Bullshark : Cruiser
    {
        public float defensivesFireRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(defensivesFireRateBoost);
            FactoryProvider.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(boostProvider);
        }
    }
}