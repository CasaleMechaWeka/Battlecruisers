using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increse all buildables health
    /// </summary>
    public class PvPMegalith : PvPCruiser
    {
        public float buildingFireRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildingFireRateBoost > 0);

            IPvPBoostProvider fireRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildingFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(fireRateBoostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.DefenseFireRateBoostProviders.Add(fireRateBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}

