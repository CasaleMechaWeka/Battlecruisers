using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increse all buildables health
    /// </summary>
    public class PvPCricket : PvPCruiser
    {
        public float buildingHealthBoost;
        public override void Initialise(PvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildingHealthBoost > 0);

            IBoostProvider buildingHealthBoostProvider = new BoostProvider(buildingHealthBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingHealth.AllBuildingsProviders.Add(buildingHealthBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}

