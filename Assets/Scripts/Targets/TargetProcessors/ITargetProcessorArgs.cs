using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface ITargetProcessorArgs
    {
        ITargetsFactory TargetsFactory { get; }
        ITargetConsumer TargetConsumer { get; }
        Faction EnemyFaction { get; }
        IList<TargetType> AttackCapabilities { get; }
        float MaxRangeInM { get; }
        float MinRangeInM { get; }
        IDamagable ParentDamagable { get; }
    }
}
