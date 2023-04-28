using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public interface IPvPTargetProcessorArgs
    {
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        IPvPTargetFactoriesProvider TargetFactories { get; }
        PvPFaction EnemyFaction { get; }
        IList<PvPTargetType> AttackCapabilities { get; }
        float MaxRangeInM { get; }
        float MinRangeInM { get; }
        IPvPTarget ParentTarget { get; }
    }
}
