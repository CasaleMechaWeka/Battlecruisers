using BattleCruisers.Buildables.Boost;
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

            IBoostProvider tacticalsBoostProvider = new BoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(tacticalsBoostProvider);
            IBoostProvider droneBuildingsBoostProvider = new BoostProvider(droneBuildingBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders.Add(droneBuildingsBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}