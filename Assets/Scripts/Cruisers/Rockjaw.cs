using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives (and broadsides) fire rate
    /// </summary>
    public class Rockjaw : Cruiser
    {
        public float offensivesFireRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            IBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(offensivesFireRateBoost);
            FactoryProvider.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(boostProvider);
        }
    }
}