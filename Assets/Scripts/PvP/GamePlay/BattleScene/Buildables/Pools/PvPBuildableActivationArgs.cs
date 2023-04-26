using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils;



namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPBuildableActivationArgs
    {
        public IPvPCruiser ParentCruiser { get; }
        public IPvPCruiser EnemyCruiser { get; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }

        public PvPBuildableActivationArgs(
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IPvPCruiserSpecificFactories cruiserSpecificFactories)
        {
            Helper.AssertIsNotNull(parentCruiser, enemyCruiser, cruiserSpecificFactories);

            ParentCruiser = parentCruiser;
            EnemyCruiser = enemyCruiser;
            CruiserSpecificFactories = cruiserSpecificFactories;
        }
    }
}