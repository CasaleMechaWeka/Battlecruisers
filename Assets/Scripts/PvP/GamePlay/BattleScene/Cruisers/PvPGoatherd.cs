using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{

    class Person
    {

    }






    /// <summary>
    /// Perks:
    /// + Increases offensives fire rate
    /// </summary>
    public class PvPGoatherd : PvPCruiser
    {
        public float MastStructureBuildRate;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(MastStructureBuildRate > 0);

            IPvPBoostProvider buildRateBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(MastStructureBuildRate);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders.Add(buildRateBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
