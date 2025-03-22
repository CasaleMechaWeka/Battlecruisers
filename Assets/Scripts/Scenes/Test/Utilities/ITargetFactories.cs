using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public interface ITargetFactories
    {
        TargetFactoriesProvider TargetFactoriesProvider { get; }
        ITargetProcessorFactory TargetProcessorFactory { get; }
        ITargetTrackerFactory TargetTrackerFactory { get; }
        TargetDetectorFactory TargetDetectorFactory { get; }
        TargetProviderFactory TargetProviderFactory { get; }
    }
}