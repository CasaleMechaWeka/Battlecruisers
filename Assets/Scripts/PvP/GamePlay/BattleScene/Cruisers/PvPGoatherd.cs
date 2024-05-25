using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offensives fire rate
    /// </summary>
    public class PvPGoatherd : PvPCruiser
    {
        public float OffenseFireRate;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(OffenseFireRate > 0);

            IPvPBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(OffenseFireRate);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
