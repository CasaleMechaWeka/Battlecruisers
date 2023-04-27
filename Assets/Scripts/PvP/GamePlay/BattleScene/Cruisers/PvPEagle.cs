using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPEagle : PvPCruiser
    {
        public float droneBuildingBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(droneBuildingBuildRateBoost > 0);

            IPvPBoostProvider boostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(droneBuildingBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(boostProvider);
        }
    }
}

