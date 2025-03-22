using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases drone station and ultralisk build speed.
    /// </summary>
    public class PvPEagle : PvPCruiser
    {
        public float droneBuildingBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(droneBuildingBuildRateBoost > 0);

            IBoostProvider boostProvider = new BoostProvider(droneBuildingBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(boostProvider);
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}