using BattleCruisers.Buildables;
using BattleCruisers.Targets.Factories;
using System.Collections.Generic;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface ITargetProcessorArgs
    {
        ITargetFactoriesProvider TargetFactories { get; }
        // FELIX  Remove :P
        ITargetsFactory TargetsFactory { get; }
        Faction EnemyFaction { get; }
        IList<TargetType> AttackCapabilities { get; }
        float MaxRangeInM { get; }
        float MinRangeInM { get; }
        ITarget ParentTarget { get; }
    }
}
