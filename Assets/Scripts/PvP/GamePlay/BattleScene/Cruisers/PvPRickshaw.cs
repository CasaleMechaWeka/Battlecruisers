using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// </summary>
    public class PvPRickshaw : PvPCruiser
    {
        public float tacticalsBuildRateBoost;
        public float droneBuildingBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(tacticalsBuildRateBoost > 0);
            Assert.IsTrue(droneBuildingBuildRateBoost > 0);

            IPvPBoostProvider tacticalsBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(tacticalsBoostProvider);
            IPvPBoostProvider droneBuildingsBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(droneBuildingBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(droneBuildingsBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}