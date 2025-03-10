using BattleCruisers.Buildables;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface ITargetProcessorArgs
    {
        ICruiserSpecificFactories CruiserSpecificFactories { get; }
        ITargetFactoriesProvider TargetFactories { get; }
        Faction EnemyFaction { get; }
        IList<TargetType> AttackCapabilities { get; }
        float MaxRangeInM { get; }
        float MinRangeInM { get; }
        ITarget ParentTarget { get; }
    }
}
