using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
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

            IBoostProvider buildRateBoostProvider = new BoostProvider(MastStructureBuildRate);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders.Add(buildRateBoostProvider);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
