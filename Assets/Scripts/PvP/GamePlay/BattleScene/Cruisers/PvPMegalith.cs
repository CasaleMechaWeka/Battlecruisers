using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increse all buildables health
    /// </summary>
    public class PvPMegalith : PvPCruiser
    {
        public float buildingHealthBoost;
        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildingHealthBoost > 0);

            IBoostProvider buildingHealthBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(buildingHealthBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingHealth.AllBuildingsProviders.Add(buildingHealthBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}

