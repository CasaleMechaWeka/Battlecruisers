using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases ultras build speed
    /// </summary>
    public class PvPMegalodon : PvPCruiser
    {
        public float ultrasBuildRateBoost;

        public override void Initialise(PvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(ultrasBuildRateBoost > 0);

            IBoostProvider boostProvider = new BoostProvider(ultrasBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders.Add(boostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}