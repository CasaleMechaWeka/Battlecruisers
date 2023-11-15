using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
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
        public float tacticalUltrasBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(tacticalsBuildRateBoost > 0);

            IPvPBoostProvider tacticalsBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(tacticalsBoostProvider);
            IPvPBoostProvider tacticalUltrasBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalUltrasProviders.Add(tacticalUltrasBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}