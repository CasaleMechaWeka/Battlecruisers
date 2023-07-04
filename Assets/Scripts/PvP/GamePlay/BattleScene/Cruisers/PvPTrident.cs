using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// </summary>
    public class PvPTrident : PvPCruiser
    {
        public float tacticalsBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(tacticalsBuildRateBoost > 0);

            IPvPBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(boostProvider);
        }
    }
}