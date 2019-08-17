using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public interface ITargetFactories
    {
        ITargetFactoriesProvider TargetFactoriesProvider { get; }
        ITargetProcessorFactory TargetProcessorFactory { get; }
        ITargetTrackerFactory TargetTrackerFactory { get; }
        ITargetDetectorFactory TargetDetectorFactory { get; }
    }
}