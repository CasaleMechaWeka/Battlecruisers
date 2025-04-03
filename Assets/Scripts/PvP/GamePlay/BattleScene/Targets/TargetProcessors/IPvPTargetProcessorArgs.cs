using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public interface IPvPTargetProcessorArgs
    {
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        Faction EnemyFaction { get; }
        IList<TargetType> AttackCapabilities { get; }
        float MaxRangeInM { get; }
        float MinRangeInM { get; }
        ITarget ParentTarget { get; }
    }
}
