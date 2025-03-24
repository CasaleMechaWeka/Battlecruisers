using BattleCruisers.Buildables.Boost;
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

            IBoostProvider tacticalsBoostProvider = new BoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(tacticalsBoostProvider);
            IBoostProvider tacticalUltrasBoostProvider = new BoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalUltrasProviders.Add(tacticalUltrasBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}